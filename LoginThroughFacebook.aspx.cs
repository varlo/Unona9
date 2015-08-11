using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using Newtonsoft.Json.Linq;

namespace AspNetDating
{
    public partial class LoginThroughFacebook : PageBase
    {
        public LoginThroughFacebook()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Config.Misc.EnableFacebookIntegration)
                {
                    var oAuth = new oAuthFacebook
                                    {
                                        CallBackUrl = Config.Urls.Home.Trim('/') + "/LoginThroughFacebook.aspx",
                                        Scope = "user_birthday,email,publish_stream"
                                    };
                    
                    if (Request["code"] == null)
                    {
                        //Redirect the user back to Facebook for authorization.
                        Response.Redirect(oAuth.AuthorizationLinkGet());
                    }
                    else
                    {
                        //Get the access token and secret.
                        oAuth.AccessTokenGet(Request["code"]);

                        if (oAuth.Token.Length > 0)
                        {
                            string url = string.Format("https://graph.facebook.com/me?access_token={0}", oAuth.Token);
                            string json = oAuth.WebRequest(oAuthFacebook.Method.GET, url, String.Empty);
                            var userInfo = (JContainer)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                            var userId = Convert.ToInt64(userInfo["id"].Value<string>());

                            string[] usernames = Classes.User.FetchUsernamesByFacebookID(new[] { userId });

                            if (usernames.Length == 0)
                            {
                                Response.Redirect("Register.aspx?facebook=1&login=1");
                                return;
                            }

                            UserSession userSession;
                            try
                            {
                                userSession = new UserSession(usernames[0]);
                                Classes.User.AuthorizeByFacebookID(userId);
                                userSession.Authorize(Session.SessionID);

                                Facebook.SaveCredentials(usernames[0], oAuth.Token);
                            }
                            catch (NotFoundException)
                            {
                                Response.Redirect("Register.aspx?facebook=1&login=1");
                                return;
                            }
                            catch (AccessDeniedException err)
                            {
                                StatusPageMessage = err.Message;
                                Response.Redirect(Config.Urls.Home + "/ShowStatus.aspx");
                                return;
                            }
                            catch (SmsNotConfirmedException)
                            {
                                Response.Redirect("SmsConfirm.aspx?username=" + usernames[0]);
                                return;
                            }
                            catch (ArgumentException err)
                            {
                                StatusPageMessage = err.Message;
                                Response.Redirect(Config.Urls.Home + "/ShowStatus.aspx");
                                return;
                            }
                            catch (Exception err)
                            {
                                Global.Logger.LogWarning(err);
                                StatusPageMessage = err.Message;
                                Response.Redirect(Config.Urls.Home + "/ShowStatus.aspx");
                                return;
                            }

                            CurrentUserSession = userSession;
                            CurrentUserSession.LoggedInThroughFacebook = true;
                            Response.Redirect("Home.aspx");
                        }
                    }
                }
            }
        }
    }
}
