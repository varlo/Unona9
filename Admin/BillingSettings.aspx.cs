﻿/* ASPnetDating 
 * Copyright (C) 2003-2009 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BillingSettings : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.billingSettings;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterJQuery();

            Title = "Payment Management".TranslateA();
            Subtitle = "Billing Settings".TranslateA();
            Description = "Use this section to choose and configure your billing plans...".TranslateA();

            btnSave.OnClientClick = "window.scrollTo(0,0);";

            if (!IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddNewPlan.Enabled = false;
                    btnSave.Enabled = false;
                }

                LoadStrings();
            }

            CreateTable();
        }

        private void LoadStrings()
        {
            btnSave.Text = "Save".TranslateA();
            btnAddNewPlan.Text = "<i class=\"fa fa-plus\"></i>&nbsp;" + "Add new plan".TranslateA(); ;
        }

        private void CreateTable()
        {
            List<BillingPlanOptions> lOptions = new List<BillingPlanOptions>();
            var unpaidOptions = Config.Users.GetNonPayingMembersOptions();
            Reflection.GetBillingPlanOptionsData(unpaidOptions, -1);
            lOptions.Add(unpaidOptions);

            var billingplans = BillingPlan.Fetch();

            foreach (var plan in billingplans)
            {
                Reflection.GetBillingPlanOptionsData(plan.Options, plan.ID);
                lOptions.Add(plan.Options);
            }

            Reflection.GenerateBillingPlanOptionsTable(phBillingPlans, lOptions.ToArray(), RecreateTable, !HasWriteAccess);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
            {
                return;
            }

            var unpaidOptions = Config.Users.GetNonPayingMembersOptions();
            Reflection.GetBillingPlanOptionsData(unpaidOptions, -1);

            var billingplans = BillingPlan.Fetch();
            foreach (var plan in billingplans)
            {
                Reflection.GetBillingPlanOptionsData(plan.Options, plan.ID);
            }

            var lOptions = new List<BillingPlanOptions>();
            lOptions.Add(unpaidOptions);
            lOptions.AddRange(billingplans.Select(bp => bp.Options));

            try
            {
                Reflection.UpdateBillingPlanOptionsFromUI(phBillingPlans, lOptions.ToArray());
                Reflection.UpdateBillingPlansFromUI(phBillingPlans, billingplans);
            }
            catch (ArgumentException ex)
            {
                MessageBox1.Show(ex.Message, Misc.MessageType.Error);
                return;
            }

            Config.Users.SetNonPayingMembersOptions(unpaidOptions);

            foreach (var plan in billingplans)
            {
                plan.Update();
            }

            MessageBox1.Show(Lang.TransA("Billing plans have been successfully updated!"), Misc.MessageType.Success);
        }

        protected void btnAddNewPlan_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
            {
                return;
            }

            BillingPlan plan = new BillingPlan
            {
                Title = "New BillingPlan",
                Amount = 1,
                Cycle = 1,
                CycleUnit = CycleUnits.Days,
                Options = new BillingPlanOptions()
            };

            BillingPlan.Create(plan);

            RecreateTable();
        }

        private void RecreateTable()
        {
            phBillingPlans.Controls.Clear();
            CreateTable();
        }
    }
}
