/* ASPnetDating 
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
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for PayPal.
    /// </summary>
    public partial class BillingConfig : AdminPageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddNewPlan.Enabled = false;
                    btnCreateUpdate.Enabled = false;
                }
                
                LoadStrings();
                PopulateDataGrid();
                pnlBillingPlanInfo.Visible = false;
            }

            Reflection.GenerateSettingsTable(phBillingPlanOptions, typeof(BillingPlanOptions));

        }

        protected string HomeURL
        {
            get
            {
                return
                    Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath +
                    ("/PayPal/IpnHandler.aspx");
            }
        }

        private void LoadStrings()
        {
            btnAddNewPlan.Text = Lang.TransA("Add New Plan");

            dgBillingPlans.Columns[0].HeaderText = Lang.TransA("Title");
            dgBillingPlans.Columns[1].HeaderText = Lang.TransA("Amount");
            dgBillingPlans.Columns[2].HeaderText = Lang.TransA("Cycle");
            dgBillingPlans.Columns[3].HeaderText = Lang.TransA("Cycle Unit");
            dgBillingPlans.Columns[4].HeaderText = Lang.TransA("Actions");
        }

        private void LoadPlanInfoStrings()
        {
            lblBillingPlanInfo.Text = Lang.TransA("Billing Plan Details");
            lblTitle.Text = Lang.TransA("Title");
            lblAmount.Text = Lang.TransA("Amount");
            lblCycle.Text = Lang.TransA("Billing Cycle");
            lblCycleUnit.Text = Lang.TransA("Billing Cycle Units");
        }

        private void PopulateDataGrid()
        {
            DataTable dtBillingPlans = new DataTable("BillingPlans");

            dtBillingPlans.Columns.Add("PlanID");
            dtBillingPlans.Columns.Add("Title");
            dtBillingPlans.Columns.Add("Amount");
            dtBillingPlans.Columns.Add("Cycle");
            dtBillingPlans.Columns.Add("CycleUnit");

            dtBillingPlans.Rows.Add(new object[]
                                            {
                                                -1,
                                                Lang.TransA("Non-Paying Members"),
                                                String.Empty,
                                                String.Empty,
                                                String.Empty
                                            });

            BillingPlan[] plans = BillingPlan.Fetch();

            foreach (BillingPlan plan in plans)
            {
                dtBillingPlans.Rows.Add(new object[]
                                            {
                                                plan.ID,
                                                plan.Title,
                                                plan.Amount,
                                                plan.Cycle,
                                                plan.CycleUnit
                                            });
            }

            dgBillingPlans.DataSource = dtBillingPlans;
            dgBillingPlans.DataBind();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.billingSettings;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgBillingPlans.ItemCreated +=
                new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgBillingPlans_ItemCreated);
            this.dgBillingPlans.ItemCommand +=
                new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgBillingPlans_ItemCommand);
            this.dgBillingPlans.ItemDataBound += new DataGridItemEventHandler(dgBillingPlans_ItemDataBound);
        }

        #endregion

        protected void btnAddNewPlan_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            pnlBillingPlanInfo.Visible = true;
            tblBillingPlan.Visible = true;
            hidCurrentPlanID.Value = "";

            LoadPlanInfoStrings();
            PopulateCycleUnits();
            ddCycleUnit.SelectedIndex = 2;
            txtTitle.Text = "";
            txtAmount.Text = "";
            txtCycle.Text = "";
            btnCreateUpdate.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");

            phBillingPlanOptions.Controls.Clear();
            Reflection.GenerateSettingsTable(phBillingPlanOptions, typeof(BillingPlanOptions));
        }

        private void PopulateCycleUnits()
        {
            ddCycleUnit.Items.Clear();
            ddCycleUnit.Items.Add(new ListItem(Lang.TransA(CycleUnits.Days.ToString()), CycleUnits.Days.ToString()));
            ddCycleUnit.Items.Add(new ListItem(Lang.TransA(CycleUnits.Weeks.ToString()), CycleUnits.Weeks.ToString()));
            ddCycleUnit.Items.Add(new ListItem(Lang.TransA(CycleUnits.Months.ToString()), CycleUnits.Months.ToString()));
            ddCycleUnit.Items.Add(new ListItem(Lang.TransA(CycleUnits.Years.ToString()), CycleUnits.Years.ToString()));
        }

        protected void btnCreateUpdate_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            if (hidCurrentPlanID.Value != (-1).ToString() && !ValidatePlanInfo())
                return;

            BillingPlanOptions options = (BillingPlanOptions)Reflection.SaveTableSettings(phBillingPlanOptions, typeof(BillingPlanOptions));

            if (hidCurrentPlanID.Value == "")
            {
                //create new plan
                BillingPlan plan = new BillingPlan();
                plan.Title = txtTitle.Text;
                plan.Amount = Convert.ToSingle(txtAmount.Text);
                plan.Cycle = Convert.ToInt32(txtCycle.Text);
                plan.CycleUnit = (CycleUnits) Enum.Parse(typeof (CycleUnits), ddCycleUnit.SelectedItem.Value);
                plan.Options = options;
                BillingPlan.Create(plan);
            }
            else
            {
                //update current one
                if (hidCurrentPlanID.Value == (-1).ToString())
                {
                    Config.Users.SetNonPayingMembersOptions(options);
                }
                else
                {
                    BillingPlan plan = BillingPlan.Fetch(Convert.ToInt32(hidCurrentPlanID.Value));

                    plan.Title = txtTitle.Text;
                    plan.Amount = Convert.ToSingle(txtAmount.Text);
                    plan.Cycle = Convert.ToInt32(txtCycle.Text);
                    plan.CycleUnit = (CycleUnits) Enum.Parse(typeof (CycleUnits), ddCycleUnit.SelectedItem.Value);
                    plan.Options = options;
                    plan.Update();
                }
            }

            hidCurrentPlanID.Value = "";
            pnlBillingPlanInfo.Visible = false;
            PopulateDataGrid();
        }

        private bool ValidatePlanInfo()
        {
            if (txtTitle.Text.Trim() == "")
            {
                MessageBox1.Show(Lang.TransA("Title field cannot be empty!"), Misc.MessageType.Error);
                return false;
            }

            try
            {
                Decimal amount = Convert.ToDecimal(txtAmount.Text);
                if (amount < 0)
                {
                    MessageBox1.Show(Lang.TransA("The amount of money can't be negative!"), Misc.MessageType.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox1.Show(Lang.TransA("Given amount of money is invalid"), Misc.MessageType.Error);
                return false;
            }

            try
            {
                int cycle = Convert.ToInt32(txtCycle.Text);
                if (cycle < 1)
                {
                    MessageBox1.Show(Lang.TransA("The billing cycle length must be positive number!"), Misc.MessageType.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox1.Show(Lang.TransA("Given billing cycle length is invalid"), Misc.MessageType.Error);
                return false;
            }

            return true;
        }

        private void dgBillingPlans_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int planID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditPlan")
            {
                BillingPlanOptions options;
                pnlBillingPlanInfo.Visible = true;

                hidCurrentPlanID.Value = planID.ToString();
                btnCreateUpdate.Text = Lang.TransA("Update");
                btnCancel.Text = Lang.TransA("Cancel");

                if (planID == -1)
                {
                    tblBillingPlan.Visible = false;
                    options = Config.Users.GetNonPayingMembersOptions();
                }
                else
                {
                    tblBillingPlan.Visible = true;
                    LoadPlanInfoStrings();
                    PopulateCycleUnits();

                    BillingPlan plan = BillingPlan.Fetch(planID);
                    txtTitle.Text = plan.Title;
                    txtAmount.Text = plan.Amount.ToString("F");
                    txtCycle.Text = plan.Cycle.ToString();
                    ddCycleUnit.SelectedValue = plan.CycleUnit.ToString();
                    options = plan.Options;
                }

                phBillingPlanOptions.Controls.Clear();
                Reflection.GenerateSettingsTableFromObject(phBillingPlanOptions, options);
            }
            else if (e.CommandName == "DeletePlan")
            {
                if (!HasWriteAccess || planID == -1)
                    return;

                if (hidCurrentPlanID.Value != "" && Convert.ToInt32(hidCurrentPlanID.Value) == planID)
                {
                    hidCurrentPlanID.Value = "";
                    pnlBillingPlanInfo.Visible = false;
                }

                BillingPlan.Delete(planID);
                PopulateDataGrid();
            }
        }

        private void dgBillingPlans_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");

            lnkDelete.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to delete this plan?")));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hidCurrentPlanID.Value = "";
            pnlBillingPlanInfo.Visible = false;
        }

        private void dgBillingPlans_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
        }
    }
}