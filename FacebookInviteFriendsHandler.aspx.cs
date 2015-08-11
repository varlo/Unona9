using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class FacebookInviteFriendsHandler : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var requestID = Request.QueryString["request"];
            var userIDs = Request.QueryString["to"];
            var errorCode = Request.QueryString["error_code"];
            var errorMessage = Request.QueryString["error_msg"];

            if (requestID == null && errorCode == null)
            {
                Response.Redirect("Home.aspx");
                return;
            }

            if (String.IsNullOrEmpty(errorCode))
            {
                ((PageBase)Page).StatusPageMessage =
                    "Invitations to your Facebook friends have been sent successfully!".Translate();
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            else
            {
                Global.Logger.Log(BitFactory.Logging.LogSeverity.Error, errorMessage);
                ((PageBase)Page).StatusPageMessage = errorMessage;
                Response.Redirect("ShowStatus.aspx");
            }
        }
    }
}
