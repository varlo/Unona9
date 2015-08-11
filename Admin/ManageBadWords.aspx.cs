using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ManageBadWords : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Edit Bad Words".TranslateA();
            Description = "In this section you can enter bad words or modify existing ones...".TranslateA();

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageBadWords;

            base.OnInit(e);
        }

        private void loadStrings()
        {
            if (!HasWriteAccess)
                btnSave.Enabled = false;

            btnSave.Text = Lang.TransA("Save");
            txtBadWords.Text = Config.Misc.BadWords;
            cbUserRegularExpressions.Checked = Config.Misc.EnableBadWordsRegularExpressions;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            string badWords = txtBadWords.Text.Trim().Replace("\r", "");
            Config.Misc.EnableBadWordsRegularExpressions = cbUserRegularExpressions.Checked;

            if (cbUserRegularExpressions.Checked)
            {
                foreach (string badword in badWords.Split('\n'))
                {
                    try
                    {
                        new Regex(badword);
                    }
                    catch (ArgumentException)
                    {
                        Master.MessageBox.Show(String.Format(Lang.TransA("{0} is invalid regular expression"), badword), Misc.MessageType.Error);
                        return;
                    }
                }
            }

            Config.Misc.BadWords = badWords;

            Master.MessageBox.Show(Lang.TransA("Bad words has been saved successfully"), Misc.MessageType.Success);
        }
    }
}
