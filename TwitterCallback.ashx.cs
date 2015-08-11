using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using oAuth;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class TwitterCallback : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            oAuthTwitter oAuth = new oAuthTwitter();

             oAuth.AccessTokenGet(Request["oauth_token"], Request["oauth_verifier"]);
             if (oAuth.TokenSecret.Length > 0)
             {
                 var username = PageBase.GetCurrentUserSession().Username;
                 Twitter.SaveCredentials(username, oAuth.Token, oAuth.TokenSecret);

                 context.Response.Redirect("Home.aspx");
             }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
