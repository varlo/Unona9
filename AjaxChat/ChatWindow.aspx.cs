#if !AJAXCHAT_INTEGRATION
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using AjaxChat.Classes;
using AspNetDating;
using AspNetDating.Classes;

namespace AjaxChat
{
    /// <summary>
    /// The Ajax Chat Window
    /// </summary>
    public partial class ChatWindowPage : Page
    {
        private UserSession CurrentUserSession
        {
            get
            {
                return Session["UserSession"] as UserSession;
            }
        }
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
            {
                RedirectToURL("Login.aspx");
                return;
            }

            var permissionCheckResult = CurrentUserSession.CanUseChat();

            if (permissionCheckResult == PermissionCheckResult.No)
            {
                RedirectToURL("Home.aspx");
                return;
            }

            if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.UserCanUseChat;
                RedirectToURL("Profile.aspx?sel=payment");
                return;
            }       

            ClientScript.RegisterClientScriptBlock(typeof (ChatWindowPage), "ParseSmilies", CreateParseSmiliesJS());
        }

        private void RedirectToURL(string url)
        {
            Response.Clear();

            Response.Write("<script type=\"text/javascript\">");
            Response.Write(String.Format("window.opener.window.location.href = '{0}/{1}';", Config.Urls.Home, url));
            Response.Write("window.close();");
            Response.Write("</script>");

            Response.End();
            return;
        }

        /// <summary>
        /// Creates the parse smilies JS.
        /// </summary>
        /// <returns></returns>
        public string CreateParseSmiliesJS()
        {
            if (!(HttpContext.Current.ApplicationInstance is IHttpApplicationSupportSmilies))
                return null;

            string smiliesUrl = ((IHttpApplicationSupportSmilies)
                                 HttpContext.Current.ApplicationInstance).GetSmiliesUrl();

            string jsInner = "";

            foreach (string key in AjaxChat.Classes.Smilies.AlSmiliesKeysSorted)
            {
                AjaxChat.Classes.Smiley smiley = (AjaxChat.Classes.Smiley) AjaxChat.Classes.Smilies.HtSmileys[key];
                string smileyImageTag = String.Format("<img src=\"{0}\" alt=\"{1}\" />",
                                                      smiliesUrl + "/" +
                                                      smiley.Image,
                                                      smiley.Description);
                jsInner += String.Format("text = text.replace(/{0}/g, \"{1}\");", Regex.Escape(key),
                                         smileyImageTag.Replace("\"", "\\\""));
            }

            jsInner += "return text;";

            string jsFunction = "<script> function parseSmilies(text) { " + jsInner + " } </script>";
            return jsFunction;
        }
    }
}
#endif