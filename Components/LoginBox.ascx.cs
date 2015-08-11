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
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class LoginBox : UserControl
    {
        protected Label lblNotRegisteredYet;
        protected SmallBoxStart SmallBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareStrings();
        }

        private void PrepareStrings()
        {
            pnlLogin.DefaultButton = "fbLogin";

            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("Already registered?");

            fbForgotPassword.Text = Lang.Trans("Forgot password?");
            fbForgotPassword.ToolTip = Lang.Trans("Click here to retrieve your password.");


            fbLogin.Text = Lang.Trans("Log In");

            if (Config.Users.ShowStealthMode)
            {
                cbStealthMode.Visible = true;
                cbStealthMode.Text = Lang.Trans("stealth mode");
            }
            else
                cbStealthMode.Visible = false;
            divFacebook.Visible = Config.Misc.EnableFacebookIntegration;

            pnlLoginButtons.Visible =  divFacebook.Visible;
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void fbLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                showError(Lang.Trans("Please specify username!"));
                return;
            }
            if (txtPassword.Text.Length == 0)
            {
                showError(Lang.Trans("Please specify password!"));
                return;
            }

            UserSession user;
            try
            {
                user = new UserSession(txtUsername.Text);
                user.StealthMode = cbStealthMode.Checked;
                user.Authorize(txtPassword.Text, Session.SessionID);
            }
            catch (NotFoundException err)
            {
                showError(err.Message);
                return;
            }
            catch (AccessDeniedException err)
            {
                showError(err.Message);
                return;
            }
            catch (SmsNotConfirmedException)
            {
                Response.Redirect("SmsConfirm.aspx?username=" + txtUsername.Text);
                return;
            }
            catch (ArgumentException err)
            {
                showError(err.Message);
                return;
            }
            catch (Exception err)
            {
                Global.Logger.LogWarning(err);
                showError(err.Message);
                return;
            }

            ((PageBase)Page).CurrentUserSession = user;

            //if (Config.Users.NewMessageNotification)
            //    Message.ClearNewMessageFlags(user.Username);

            if (user.PrevLogin.Date != DateTime.Now.Date)
                User.AddScore(user.Username, Config.UserScores.DailyLogin, "Login");

            if (cbRememberMe.Checked)
            {
                string guid = User.CreatePendingGuid(user.Username);
                Response.Cookies["rememberMe"].Value = guid;
                Response.Cookies["rememberMe"].Expires = DateTime.Now.AddDays(7);
            }

            try
            {
                IPLogger.Log(user.Username, Request.UserHostAddress, IPLogger.ActionType.Login);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("LoginBox IP Logger", err);
            }

            if (!string.IsNullOrEmpty(Request.Params["back_url"]))
                Response.Redirect(Request.Params["back_url"]);
            else
                Response.Redirect("Home.aspx");
        }

        private void showError(string error)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "alert text-danger",
                                                    String.Format("alert('{0}');",
                                                    error.Replace("'", "\\'")), true);
        }

        protected void fbForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("LostPassword.aspx");
        }

        protected void btnUseFacebook_Click(object sender, EventArgs e)
        {
            var oFB = new oAuthFacebook
                          {
                              CallBackUrl = Config.Urls.Home.Trim('/') + "/LoginThroughFacebook.aspx",
                              Scope = "publish_stream"
                          };
            Response.Redirect(oFB.AuthorizationLinkGet());
        }
    }
}