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
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitFactory.Logging;
using AspNetDating.Classes;
using System.Web.Hosting;
#if !AJAXCHAT_INTEGRATION
using AjaxChat.Classes;
#endif

namespace AspNetDating
{
    public class Global : HttpApplication
#if !AJAXCHAT_INTEGRATION
                        , IHttpApplicationSupportSmilies,
                          IHttpApplicationUserAdapter, IHttpApplicationSupportLogin,
                          IHttpApplicationSupportAvatars, IHttpApplicationSupportTranslations,
                          IHttpApplicationSupportProfiles, IHttpApplicationChatRoomProvider,
                          IHttpApplicationConnectionStringProvider
#endif
    {
        private static readonly CompositeLogger logger = new CompositeLogger();

        public static CompositeLogger Logger
        {
            get { return logger; }
        }

        public static void InitLogger()
        {
            // create the file logger
// ReSharper disable UseObjectOrCollectionInitializer
            Logger fileLogger = new FileLogger(
// ReSharper restore UseObjectOrCollectionInitializer
                HostingEnvironment.MapPath(@"~\Logs\logfile.log"));
            
            #if DEBUG            
            fileLogger.SeverityThreshold = LogSeverity.Info;
            #else
            fileLogger.SeverityThreshold = LogSeverity.Status;     
            #endif
            
            Logger.AddLogger("File", fileLogger);
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        public Global()
        {
            InitializeComponent();
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            InitLogger();
            Logger.LogStatus("Application Started");

            if (Config.AdminSettings.ExecuteTimers)
            {
                EmailQueue.InitializeMailerTimer();
                Payments.InitializePaymentsTimer();
                BirthdayEmails.InitializeMailerTimer();
                FriendBirthdayEmails.InitializeMailerTimer();
                VisitSiteReminderEmails.InitializeMailerTimer();
                Maintenance.InitializeTimers();
                SavedSearchesEmailMatches.InitializeMailerTimer();
                InactiveGroups.InitializeTimer();
                HourlyStats.InitializeMailerTimer();
                GoogleSitemaps.InitializeTimers();
                MessagesSandBox.InitializeMailerTimer();
                Event.InitializeEventsCleanupTimer();
                IPToCountry.InitializeUpdateIpDefinitionFilesTimer();
                ScheduledAnnouncementEmails.InitializeMailerTimer();
            }

            Classes.Plugins.InitializePlugins();
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Config.Directories.Home = Server.MapPath("~");
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Path.IndexOf("/admin/", StringComparison.OrdinalIgnoreCase) == -1 &&
                Config.SEO.EnableUrlRewriting)
                UrlRewrite.Process();

            Context.Items.Add("PerformanceLogGuid", PerformanceTimers.LogStartRequest());
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            var index = Request.Url.Segments.Length - 1;
            var filename = Request.Url.Segments[index].ToLower();
            if (filename.Contains("?"))
                filename = filename.Split('?')[0];
            if (Context.Items.Contains("PerformanceLogGuid"))
                PerformanceTimers.LogEndRequest((Guid) Context.Items["PerformanceLogGuid"],
                    filename);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex == null)
                return;

            if (ex.GetType() == typeof(HttpException) &&
                ex.InnerException != null &&
                ex.InnerException.GetType() == typeof(ViewStateException))
                return;

            if (ex is HttpUnhandledException && ex.InnerException is COMException)
            {
                switch (((COMException)ex.InnerException).ErrorCode)
                {
                    case unchecked((int)0x80070040): //The specified network name is no longer available.
                    case unchecked((int)0x800703E3): //The I/O operation has been aborted because of either a thread exit or an application request. 
                    case unchecked((int)0x800704CD): //An operation was attempted on a nonexistent network connection
                    case unchecked((int)0x80070001): //Incorrect function.
                        return;
                }
            }

            try
            {
                ExceptionLogger.Log(Request, ex);
            }
            catch
            {
                ExceptionLogger.Log("Global_Application_Error", ex);
            }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
        }

        protected void Application_End(Object sender, EventArgs e)
        {
            PerformanceTimers.WriteDataToLogFile();
            Logger.LogWarning("Application Ending");
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // Global
            // 
            this.BeginRequest += new EventHandler(this.Global_BeginRequest);
        }

        #endregion

        private void Global_BeginRequest(object sender, EventArgs e)
        {
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            string vary = null;
            foreach (string key in arg.Split(';'))
            {
                if (vary == null) vary = ""; else vary += ";";

                UserSession userSession = null;
                switch (key)
                {
                    case "InterestedIn" :
                        userSession = PageBase.GetCurrentUserSession();
                        if (userSession == null) continue;
                        if (Config.Users.InterestedInFieldEnabled)
                        {
                            vary += userSession.InterestedIn.ToString();
                        }
                        else
                        {
                            switch (userSession.Gender)
                            {
                                case Classes.User.eGender.Male:
                                    vary += Classes.User.eGender.Female.ToString();
                                    break;
                                case Classes.User.eGender.Female:
                                    vary += Classes.User.eGender.Male.ToString();
                                    break;
                            }
                        }
                        break;
                    case "CurrentUsername" :
                        userSession = PageBase.GetCurrentUserSession();
                        if (userSession == null) continue;
                        vary += userSession.Username;
                        break;
                    case "Language" :
                        vary += PageBase.GetLanguageId();
                        break;
                }
            }

            return vary;
        }

        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname;

            ScriptManager scriptManager = ScriptManager.GetCurrent(page);
            if (page.IsPostBack && scriptManager != null && scriptManager.IsInAsyncPostBack)
            {
                ctrlname = ScriptManager.GetCurrent(page).AsyncPostBackSourceElementID;

                control = page.FindControl(ctrlname);
            }
            else
            {
                ctrlname = page.Request.Params["__EVENTTARGET"];
                if (!string.IsNullOrEmpty(ctrlname))
                {
                    control = page.FindControl(ctrlname);
                }
                    // if __EVENTTARGET is null, the control is a button type and we need to 
                    // iterate over the form collection to find it
                else
                {
                    string ctrlStr = String.Empty;
                    Control c = null;
                    foreach (string ctl in page.Request.Form)
                    {
                        if (ctl == null)
                            continue;

                        // handle ImageButton controls ...
                        if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                        {
                            ctrlStr = ctl.Substring(0, ctl.Length - 2);
                            c = page.FindControl(ctrlStr);
                        }
                        else
                        {
                            c = page.FindControl(ctl);
                        }
                        if (c is Button ||
                            c is ImageButton)
                        {
                            control = c;
                            break;
                        }
                    }
                }
            }
            return control;
        }
#if !AJAXCHAT_INTEGRATION
        #region IHttpApplicationConnectionStringProvider Members

        public string GetConnectionString()
        {
            return Config.DB.ConnectionString;
        }

        public string GetLoginUrl()
        {
            return Config.Urls.Home;
        }

        #endregion

        #region IHttpApplicationUserAdapter Members

        public string GetCurrentlyLoggedUsername()
        {
            if (PageBase.GetCurrentUserSession() != null)
            {
                UserSession sess = PageBase.GetCurrentUserSession();
                return sess.Username;
            }
            else
            {
                return null;
            }
        }

        public bool IsRoomAdmin(string username, int chatRoomId)
        {
            return username == "admin";
        }

        public bool HasChatAccess(string username, int chatRoomId)
        {
            if (chatRoomId == 0)
                return true;

            if (Config.Groups.EnableGroups && Config.Groups.EnableAjaxChat)
            {
                GroupMember groupMember = GroupMember.Fetch(chatRoomId, username);

                if (groupMember == null || !groupMember.Active)
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }

        public string GetUserDisplayName(string username)
        {
            return username;
        }

        public bool IsAdministrator(string username)
        {
            return username == "admin";
        }

        #endregion

        #region IHttpApplicationSupportAvatars Members

        public string GetUserAvatar(string username)
        {
            try
            {
                return ImageHandler.CreateImageUrl(Photo.GetPrimary(username).Id, 30, 30, false, true, true);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region IHttpApplicationSupportSmilies Members
        public string GetSmiliesDirectory()
        {
            return Config.Directories.Smilies;
        }

        public string GetSmiliesUrl()
        {
            return Config.Urls.Home + "/Smilies";
        }
        #endregion
        
        #region IHttpApplicationSupportTranslations
        public string Translate(string text)
        {
            return Lang.Trans(text);
        }
        #endregion

        #region IHttpApplicationSupportProfiles Members

        public string GetUserProfileUrl(string username)
        {
            return Config.Urls.Home + "/ShowUser.aspx?uid=" + username;
        }

        #endregion

        #region IHttpApplicationChatRoomProvider Members

        public ChatRoom GetChatRoom(int chatRoomId)
        {
            if (!Config.Groups.EnableGroups || !Config.Groups.EnableAjaxChat)
                return null;

            Group group = Group.Fetch(chatRoomId);
            if (group == null) return null;

            ChatRoom room = new ChatRoom();
            room.Id = chatRoomId;
            room.Name = group.Name;

            return room;
        }

        #endregion
#endif
    }
}