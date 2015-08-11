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
using System.Drawing;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for Settings.
    /// </summary>
    public partial class Settings : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Settings".TranslateA();
            Description = "Here you can customize your website by modifying its settings...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }

                LoadStrings();
            }
            //do not move this function
            LoadTable();
        }

        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
        }

        private bool ValidatePasswords()
        {
            bool AllBlank = (txtCurrentPassword.Text.Trim() == "" &&
                             txtNewPassword.Text.Trim() == "" &&
                             txtConfirmNewPassword.Text.Trim() == "");

            bool AllFilledIn = (txtCurrentPassword.Text.Trim() != "" &&
                                txtNewPassword.Text.Trim() != "" &&
                                txtConfirmNewPassword.Text.Trim() != "");

            if (!(AllBlank || AllFilledIn))
            {
                Master.MessageBox.Show(Lang.TransA("Please fill in all password fields or leave them blank!"), Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                Master.MessageBox.Show(Lang.TransA("New password fields do not match!"), Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && !CurrentAdminSession.IsPasswordIdentical(txtCurrentPassword.Text))
            {
                Master.MessageBox.Show(Lang.TransA("The specified current password is wrong!"), Misc.MessageType.Error);
                return false;
            }

            return true;
        }

        private void LoadTable()
        {
            Reflection.GenerateSettingsTable(phSettings, typeof(Config));
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.generalSettings;
            base.OnInit(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            try
            {
                Reflection.SaveTableSettings(phSettings, typeof(Config));
            }
            catch (FormatException)
            {
                Master.MessageBox.Show(Lang.TransA("Invalid values!"), Misc.MessageType.Error);
                return;
            }

            if (ValidatePasswords())
            {
                if (txtNewPassword.Text != "")
                {
                    CurrentAdminSession.Password = txtNewPassword.Text;
                    CurrentAdminSession.Update();
                }

                Master.MessageBox.Show(Lang.TransA("Settings have been successfully updated!"), Misc.MessageType.Success);
            }
        }
    }
}