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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using eStreamBG.Dating;

namespace AspNetDating
{
    /// <summary>
    /// This class is the base of all AspNetDating pages
    /// </summary>
    public class PageBase : InternalPage
    {
        private DateTime initTime = DateTime.Now;

        /// <summary>
        /// Set to false if the page will be visible to non-logged users
        /// </summary>
        public bool RequiresAuthorization = true;
        /// <summary>
        /// Set to true if you do not want IM available on the page
        /// </summary>
        public bool DoNotRegisterIMJavascript = false;

        /// <summary>
        /// Gets the current user session.
        /// </summary>
        /// <returns></returns>
        public static UserSession GetCurrentUserSession()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Session["UserSession"] as UserSession;
            }

            return null;
        }
        /// <summary>
        /// Sets the current user session.
        /// </summary>
        /// <param name="newUserSession">The new user session.</param>
        public static void SetCurrentUserSession(UserSession newUserSession)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session["UserSession"] = newUserSession;
            }
        }
        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        /// <value>The current user session.</value>
        public new UserSession CurrentUserSession
        {
            get { return base.CurrentUserSession as UserSession; }
            set
            {
                base.CurrentUserSession = value;

                if (value != null) // it will be null if the user has been logout
                {
                    if (Session["LanguageIdExplicit"] == null
                    || (bool)Session["LanguageIdExplicit"] == false)
                    {
                        LanguageId = value.LanguageId;
                    }
                    else
                    {
                        CurrentUserSession.LanguageId = LanguageId;
                        CurrentUserSession.Update();
                    }        
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreInit"/> event at the beginning of page initialization.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            if (Request.Params["theme"] != null)
            {
                Theme = Request.Params["theme"];
                Session["theme"] = Request.Params["theme"];
            }
            else if (Session["theme"] != null && !string.IsNullOrEmpty(Theme))
            {
                Theme = (string)Session["theme"];
            }
            else
            {
                Language language = Language.Fetch(LanguageId);
                if (language != null && !String.IsNullOrEmpty(language.Theme))
                {
                    if (Directory.Exists(Server.MapPath("~/App_Themes/" + language.Theme)))
                        Theme = language.Theme;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Config.Misc.SiteTheme))
                    {
                        Theme = Config.Misc.SiteTheme;
                    }
                }
            }

            Classes.Plugins.Events.OnPreInitInvoke(this, e);
            base.OnPreInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (RequiresAuthorization)
            {
                ProcessValidation();
            }

            CommunityFaceControlValidation();

            /*
            if (Config.Misc.EnableIntegratedIM && IsUserAuthorized() && !DoNotRegisterIMJavascript)
            {
                ClientScript.RegisterClientScriptInclude("IntegratedIM", "IM/functions.js");
            }
            */

            Classes.Plugins.Events.OnInitInvoke(this, e);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.InitComplete"/> event after page initialization.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInitComplete(EventArgs e)
        {
            Classes.Plugins.Events.OnInitInvoke(this, e);
            base.OnInitComplete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreLoad"/> event after postback data is loaded into the page server controls but before the <see cref="M:System.Web.UI.Control.OnLoad(System.EventArgs)"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreLoad(EventArgs e)
        {
            Classes.Plugins.Events.OnPreLoadInvoke(this, e);
            base.OnPreLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Classes.Plugins.Events.OnLoadInvoke(this, e);
            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete"/> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            Classes.Plugins.Events.OnLoadInvoke(this, e);
            base.OnLoadComplete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            Classes.Plugins.Events.OnPreRenderInvoke(this, e);
            base.OnPreRender(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreRenderComplete"/> event after the <see cref="M:System.Web.UI.Page.OnPreRenderComplete(System.EventArgs)"/> event and before the page is rendered.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            Classes.Plugins.Events.OnPreRenderCompleteInvoke(this, e);
            base.OnPreRenderComplete(e);
        }

        /// <summary>
        /// Gets or sets the status page message.
        /// </summary>
        /// <value>The status page message.</value>
        public string StatusPageMessage
        {
            set { Session["ShowStatus_Message"] = value; }
            get
            {
                if (Session["ShowStatus_Message"] == null)
                    return null;
                else
                    return (string) Session["ShowStatus_Message"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link URL.
        /// </summary>
        /// <value>The status page link URL.</value>
        public string StatusPageLinkURL
        {
            set { Session["ShowStatus_LinkURL"] = value; }
            get
            {
                if (Session["ShowStatus_LinkURL"] == null)
                    return null;
                else
                    return (string) Session["ShowStatus_LinkURL"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link text.
        /// </summary>
        /// <value>The status page link text.</value>
        public string StatusPageLinkText
        {
            set { Session["ShowStatus_LinkText"] = value; }
            get
            {
                if (Session["ShowStatus_LinkText"] == null)
                    return null;
                else
                    return (string) Session["ShowStatus_LinkText"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link skind id.
        /// </summary>
        /// <value>The status page link skind id.</value>
        public string StatusPageLinkSkindId
        {
            set { Session["ShowStatus_LinkSkindId"] = value; }
            get
            {
                if (Session["ShowStatus_LinkSkindId"] == null)
                    return null;
                else
                    return (string) Session["ShowStatus_LinkSkindId"];
            }
        }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>The language id.</value>
        public int LanguageId
        {
            set
            {
                Session["LanguageId"] = value;

                HttpCookie cookie = new HttpCookie("LanguageId", value.ToString());
                cookie.Expires = DateTime.Now.AddMonths(6);
                Response.Cookies.Add(cookie);

                if (IsUserAuthorized())
                {
                    if (CurrentUserSession.LanguageId != value)
                    {
                        CurrentUserSession.LanguageId = value;
                        CurrentUserSession.Update();
                    }
                }
                else
                {
                    Session["LanguageIdExplicit"] = true;
                }
            }
            get
            {
                return GetLanguageId();
            }
        }

        /// <summary>
        /// Gets the language id.
        /// </summary>
        /// <returns></returns>
        public static int GetLanguageId()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session["LanguageId"] == null)
            {
                int languageId = Config.Misc.DefaultLanguageId;
                if (ctx.Request.Cookies["LanguageId"] != null)
                {
                    try
                    {
                        languageId = Convert.ToInt32(ctx.Request.Cookies["LanguageId"].Value);
                    }catch(FormatException){}
                }
                else
                {
                    var languages = new List<Language>(Language.FetchAll());

                    string userCountry = IPToCountry.GetCountry(ctx.Request.UserHostAddress) ?? String.Empty;
                    string userCountryByCode = Config.Users.GetCountryByCode(userCountry) ?? String.Empty;
                    string[] userLanguages = ctx.Request.UserLanguages ?? new string[0];

                    var found = (from l in languages
                                 where l.Active &&
                                 (
                                    (!String.IsNullOrEmpty(l.BrowserLanguages) &&
                                    Array.Exists(l.BrowserLanguages.Split(','),
                                        browserLanguage => userLanguages.FirstOrDefault(
                                            userLanguage =>
                                                CreateSpecificCultureNameOrDefault(browserLanguage)
                                                == CreateSpecificCultureNameOrDefault(userLanguage.Split(';')[0])
                                            ) != null))
                                    ||
                                    (!String.IsNullOrEmpty(l.IpCountries) &&
                                    Array.Exists(l.IpCountries.Split(','),
                                        ipCountry =>
                                            ipCountry.ToLowerInvariant() == userCountryByCode.ToLowerInvariant()
                                        || ipCountry.ToLowerInvariant() == userCountry.ToLowerInvariant()))
                                 )
                                 select new { ID = l.Id }).FirstOrDefault();

                    if (found != null && found.ID != 0) languageId = found.ID;
                }
                
                ctx.Session["LanguageId"] = languageId;
                return languageId;
            }
            return (int)ctx.Session["LanguageId"];
        }

        private static string CreateSpecificCultureNameOrDefault(string name)
        {
            try
            {
                return CultureInfo.CreateSpecificCulture(name.ToLowerInvariant()).Name;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Processes the validation.
        /// </summary>
        protected void ProcessValidation()
        {
            if (!IsUserAuthorized())
            {
                Response.Redirect("~/login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }
        }

        private void CommunityFaceControlValidation()
        {
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.IsAdmin())
                {
                    return;
                }

                if (Config.CommunityFaceControlSystem.EnableCommunityFaceControl
                    && !CurrentUserSession.FaceControlApproved
                    && !(Page is MemberProfile) && !(Page is EditSalutePhoto) && !(Page is ShowStatus))
                {
                    if (!CurrentUserSession.HasProfile)
                    {
                        StatusPageLinkSkindId = "";
                        StatusPageLinkText = Config.Urls.Home + "/profile.aspx";
                        StatusPageLinkURL = Config.Urls.Home + "/profile.aspx";
                        StatusPageMessage = Lang.Trans("You must complete your profile in order to continue!");
                    }
                    else if (Photo.Search(-1, CurrentUserSession.Username, -1, null, null, null, null).Length < Config.CommunityFaceControlSystem.MinPhotosRequired)
                    {
                        StatusPageLinkSkindId = "UploadPhotos";
                        StatusPageLinkText = Lang.Trans("UPLOAD PHOTOS NOW");
                        StatusPageLinkURL = "~/profile.aspx?sel=photos";
                        StatusPageMessage =
                            String.Format(Lang.Trans("You must upload at least {0} photos in order to continue!"),
                                          Config.CommunityFaceControlSystem.MinPhotosRequired);
                    }
                    else
                    {
                        StatusPageMessage = Lang.Trans("Your profile is completed and is awaiting approval from the rest of the users!");
                    }

                    Response.Redirect("~/Showstatus.aspx");
                }    
            }
        }

        /// <summary>
        /// Determines whether the user is authorized.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the user is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserAuthorized()
        {
            if (CurrentUserSession == null)
                return false;

            return CurrentUserSession.IsAuthorized;
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void Log(Exception ex)
        {
            ExceptionLogger.Log(Request, ex);
        }
    }
}