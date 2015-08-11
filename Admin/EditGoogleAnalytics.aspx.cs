using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditGoogleAnalytics : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editGoogleAnalytics;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Edit Google Analytics".TranslateA();
            Description = "Use this section to edit google anlytics".TranslateA();

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            if (!HasWriteAccess)
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
            }

            txtGoogleAnalyticsCode.Text = Config.Misc.GoogleAnalyticsTrackingCode;
            btnSave.Text = "Save".TranslateA();
            btnClear.Text = "Clear".TranslateA();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            Config.Misc.GoogleAnalyticsTrackingCode = txtGoogleAnalyticsCode.Text.Trim();
            Master.MessageBox.Show("Tracking code has been successfully updated!", Misc.MessageType.Success);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtGoogleAnalyticsCode.Text = String.Empty;
        }
    }
}
