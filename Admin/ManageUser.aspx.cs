﻿/* ASPnetDating 
 * Copyright (C) 2003-2014 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for ManageUser.
    /// </summary>
    public partial class ManageUser : AdminPageBase
    {
        public ManageUser()
        {
            RequiresAuthorization = true;
        }


        private User managedUser = null;

        private User ManagedUser
        {
            get
            {
                if (managedUser == null)
                    managedUser = Classes.User.Load(Request.Params["uid"]);
                return managedUser;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Manage User".TranslateA();
            Description = "Here you can manage the settings for a particular user".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSubscribeUpdate.Enabled = false;
                    btnActivateUser.Enabled = false;
                    btnUpdateCredits.Enabled = false;
                }

                LoadStrings();
                PopulateFields();
                ShowHideSubscriptionInfo();

                if (Request.Params["uid"] == "admin")
                {
                    cbAllRelatedData.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
        }

        private void PopulateFields()
        {
            txtVerificationsLeft.Text = ManagedUser.MessageVerificationsLeft.ToString();

            //if (Config.Credits.Required)
            //{
            txtCredits.Text = ManagedUser.Credits.ToString();
            //}
        }

        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
            cbAllRelatedData.Text = Lang.TransA("Delete all user-related data");
            if (ManagedUser.Deleted)
            {
                txtDeleteReason.Text = ManagedUser.DeleteReason;
                txtDeleteReason.Enabled = false;
                btnDelete.Text = Lang.TransA("Undelete");
                cbAllRelatedData.Visible = false;
            }
            else
            {
                txtDeleteReason.Enabled = true;
                btnDelete.Text = Lang.TransA("Delete");
                btnDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Are you sure you want to delete this user?")));
                cbAllRelatedData.Visible = true;
            }
            btnActivateUser.Text = Lang.TransA("Activate User");

            if (Config.Users.MessageVerificationEnabled)
                pnlVerificationsLeft.Visible = true;
            else
                pnlVerificationsLeft.Visible = false;

            if (ManagedUser.Active)
                pnlActivateUser.Visible = false;
            else
                pnlActivateUser.Visible = true;

            pnlAffiliateCommission.Visible = ManagedUser.AffiliateID != null;

            pnlCredits.Visible = Config.Misc.SiteIsPaid;
            btnUpdateCredits.Text = Lang.TransA("Update");
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.userAccounts;
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ManagedUser.MessageVerificationsLeft = Int32.Parse(txtVerificationsLeft.Text);
            ManagedUser.Update();

            Response.Redirect("BrowseUsers.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (Request.Params["uid"] == "admin")
            {
                return;
            }

            if (ManagedUser.Deleted)
            {
                ManagedUser.Deleted = false;
                ManagedUser.Update();
            }
            else
            {
                if (cbAllRelatedData.Checked)
                    Classes.User.Purge(ManagedUser.Username);
                else
                    Classes.User.Delete(ManagedUser.Username, txtDeleteReason.Text);

                IPLogger.Log(ManagedUser.Username, Request.UserHostAddress, IPLogger.ActionType.AdminDeleteUser);
            }
            Response.Redirect("BrowseUsers.aspx");
        }

        private void ShowHideSubscriptionInfo()
        {
            var plans = BillingPlan.Fetch();

            if (plans.Length > 0)
            {
                Subscription subscription = Subscription.FetchActiveSubscription(ManagedUser.Username);

                if (subscription == null)
                {
                    ddPlans.Visible = true;
                    btnSubscribeUpdate.Text = Lang.TransA("Subscribe");
                    //BillingPlan[] plans = BillingPlan.Fetch();
                    BillingPlan.ApplyDiscounts(ManagedUser, plans);
                    foreach (BillingPlan plan in plans)
                    {
                        ListItem item =
                            new ListItem(
                                plan.Title + " - " + plan.Amount.ToString("c") + " / " + plan.Cycle + " " +
                                Lang.TransA(plan.CycleUnit.ToString().ToLower()), plan.ID.ToString());
                        ddPlans.Items.Add(item);
                    }

                    //administrator has not set billing plans yet 
                    if (plans.Length == 0)
                    {
                        Master.MessageBox.Show(Lang.TransA("Billing plans are not set yet!"), Misc.MessageType.Error);
                        pnlManualSubscription.Visible = false;
                        return;
                    }

                    pnlExpirationDate.Visible = false;
                    return;
                }
                else
                {
                    ddPlans.Visible = false;
                    btnSubscribeUpdate.Text = Lang.TransA("Update");
                    if (subscription.PaymentProcessor != "PayPal")
                    {
                        BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);

                        string planName = plan.Title + " - " + plan.Amount.ToString("c") + " / " + plan.Cycle + " " +
                                          Lang.TransA(plan.CycleUnit.ToString().ToLower());

                        lblCurrentPlan.Text = Lang.TransA("User's current plan is ") + planName;
                        try
                        {
                            dpExpirationDate.MinYear = DateTime.Now.Year;
                            dpExpirationDate.MaxYear = DateTime.Now.AddYears(10).Year;
                            dpExpirationDate.SelectedDate = subscription.RenewDate;
                        }
                        catch (ArgumentOutOfRangeException ex) { Master.MessageBox.Show(ex.Message, Misc.MessageType.Error); return; }

                        pnlExpirationDate.Visible = true;

                        return;
                    }
                }
            }

            //mvManualSubscription.SetActiveView(vHidden);
            pnlManualSubscription.Visible = false;
        }

        protected void btnSubscribeUpdate_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            Subscription subscription = Subscription.FetchActiveSubscription(ManagedUser.Username);

            if (subscription == null)
            {
                if (ddPlans.SelectedValue != String.Empty)
                {
                    int id =
                        Subscription.Create(ManagedUser.Username, Int32.Parse(ddPlans.SelectedValue), "Check");
                    subscription = Subscription.Fetch(id);
                    BillingPlan plan = BillingPlan.Fetch(Int32.Parse(ddPlans.SelectedValue));
                    subscription.Activate(DateTime.Now, plan);

                    if (!cbAffiliateCommission.Checked)
                    {
                        string description = String.Format("Subscription fee ({0}, {1})", plan.Amount, DateTime.Now);

                        AffiliateCommission.ApplyCommission(subscription.Username, null, (decimal)plan.Amount, description);
                    }

                    ShowHideSubscriptionInfo();
                }
            }
            else
            {
                if (ddPlans.SelectedValue != String.Empty)
                    subscription.PlanID = Int32.Parse(ddPlans.SelectedValue);

                try
                {
                    subscription.RenewDate = dpExpirationDate.SelectedDate;
                    subscription.Update();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Master.MessageBox.Show(ex.Message, Misc.MessageType.Error);
                    return;
                }
            }
        }

        protected void btnActivateUser_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ManagedUser.Active = true;
            ManagedUser.Update();

            pnlActivateUser.Visible = false;
        }

        protected void btnUpdateCredits_Click(object sender, EventArgs e)
        {
            int credits = 0;

            if (Int32.TryParse(txtCredits.Text.Trim(), out credits))
            {
                ManagedUser.Credits = credits;

                ManagedUser.Update(true);
            }
            else
            {
                Master.MessageBox.Show(Lang.TransA("Please enter valid value for credits!"), Misc.MessageType.Error);
            }
        }
    }
}