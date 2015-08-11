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
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    public partial class SmsConfirm : PageBase
    {
        public SmsConfirm()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LargeBoxStart1.Title = Lang.Trans("Sms Confirmation");
            btnConfirm.Text = Lang.Trans("Confirm");

            if (Request.Params["username"] == null)
            {
                Response.Redirect("default.aspx");
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string username = Request.Params["username"];

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(
                String.Format(Properties.Settings.Default.SmsCheckUrl, txtConfirmationCode.Text));
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            string result;
            using (StreamReader sr =
                new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }

            Global.Logger.LogInfo("SmsConfirm", "User " + username + " tried activating with code " + txtConfirmationCode.Text + ". Result: " + result);

            if (result.Trim() == "PAYBG=OK")
            {
                IPLogger.Log(username, Request.UserHostAddress, IPLogger.ActionType.SmsConfirmed);
                User user = Classes.User.Load(username);
                user.SmsConfirmed = true;
                user.Active = true;
                user.Update();

                if (Config.Users.SendWelcomeMessage)
                {
                    Message.SendWelcomeMessage(user);
                }

                StatusPageMessage = Lang.Trans
                    ("<b>Your account has been confirmed successfully!</b>");
            }
            else
            {
                StatusPageMessage = Lang.Trans
                    ("<b>The confirmation code is invalid!</b>");
            }

            Response.Redirect("ShowStatus.aspx");
        }
    }
}