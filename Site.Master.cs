using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;
using System.Web.UI.WebControls;

namespace AspNetDating
{
    public partial class Site : MasterPage
    {
        public ScriptManager ScriptManager
        {
            get
            {
                return ScriptManagerMaster;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            linkLess.Href = string.Format("App_Themes/{0}/style.less", Page.Theme);

            Page.RegisterJQuery();

            bool isAuthorized = Page is PageBase && ((PageBase)Page).IsUserAuthorized();
            // Add script reference for online status checking
            if (isAuthorized)
            {
                var service = new ServiceReference("Services/OnlineCheck.asmx");
                ScriptManagerMaster.Services.Add(service); 

                ScriptManagerMaster.CompositeScript.Scripts.Add(
                    new ScriptReference("scripts/OnlineCheck.js"));

                Page.ClientScript.RegisterStartupScript(GetType(), "InitializeOnlineCheck",
                    String.Format("InitializeOnlineCheck({0});", Config.Users.OnlineCheckTime * 60000), 
                    true);

                if (Config.Misc.EnableIntegratedIM)
                {
                    ScriptManagerMaster.CompositeScript.Scripts.Add(
                        new ScriptReference("scripts/jquery-ui-1.8.16.custom.min.js"));
                    ScriptManagerMaster.CompositeScript.Scripts.Add(
                        new ScriptReference("scripts/Messenger.js"));

                    var timestamp = DateTime.Now.ToFileTimeUtc();
                    Page.ClientScript.RegisterStartupScript(GetType(), "InitializeMessenger",
                        String.Format("Messenger.initialize('{0}', '{1}', '{2}', '{3}', {4});",
                        //chathomeurl
                        Config.Urls.ChatHome,
                        //userid
                        ((PageBase)Page).CurrentUserSession.Username,
                        //timestamp
                        timestamp,
                        //hash
                        Misc.CalculateChatAuthHash(((PageBase)Page).CurrentUserSession.Username, String.Empty, timestamp.ToString()),
                        //updateonlinefrequency
                        Config.Misc.MessengerPresenceUpdateInterval * 1000),
                        true);
                }
            }

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "body",
                String.Format("var bodyId = '{0}';", body.ClientID), true);

            // Add script reference for user preview popup
            ScriptManagerMaster.CompositeScript.Scripts.Add(
                    new ScriptReference("scripts/UserPreview.js"));
            Page.ClientScript.RegisterStartupScript(GetType(), "InitializeMouseTracking",
                "InitializeMouseTracking();", true);

//            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SelectLinks", 
//                String.Format(@"var id = '{0}';alert(id);
//            if (id != '')
//                $('#' + id).parent().addClass('active');
//            else if ($('.SideMenuLink').length > 0)
//                $('.SideMenuLink').first().parent().addClass('active');"
//                , SelectedLinkClientID), true);


            // Add script for detecting ad blockers
            if (Config.Misc.StopUsersWithAdBlocker && !(Page is ShowStatus)
                && Request.Browser.Browser != "IE") // Adblock detector doesn't work with IE
            {
                ScriptManagerMaster.CompositeScript.Scripts.Add(
                    new ScriptReference("scripts/adblock_detector.js"));
            }

            // Add the common.js
            ScriptManagerMaster.CompositeScript.Scripts.Add(
                    new ScriptReference("Images/common.js"));

            // Load pie.css for older IE browsers
            if (Request.Browser.IsBrowser("IE") && Request.Browser.MajorVersion < 9)
            {
                var pieCss = new HtmlLink();
                pieCss.Attributes.Add("rel", "stylesheet");
                pieCss.Attributes.Add("type", "text/css");
                pieCss.Href = ResolveUrl("~/images/pie.css");
                Page.Header.Controls.Add(pieCss);
            }

            // Add the images lazy loading script
            if (Config.Photos.UseLazyImagesLoading)
            {
                //Page.RegisterJQuery();
                ScriptManagerMaster.CompositeScript.Scripts.Add(
                        new ScriptReference("scripts/jquery.lazyload.js"));
                Page.ClientScript.RegisterStartupScript(GetType(), "ImagesLazyLoad",
                    "$(function() { $('img').lazyload(); });", true);
            }


            if (Page is Ads)
            {
                SetMetaTags(Config.SEO.AdsTitleTemplate, Config.SEO.AdsMetaDescriptionTemplate,
                            Config.SEO.AdsMetaKeywordsTemplate);
            }
            else if (Page is ChangeLostPassword)
            {
                SetMetaTags(Config.SEO.ChangeLostPasswordTitleTemplate, Config.SEO.ChangeLostPasswordMetaDescriptionTemplate,
                            Config.SEO.ChangeLostPasswordMetaKeywordsTemplate);
            }
            else if (Page is _default)
            {
                SetMetaTags(Config.SEO.DefaultPageTitleTemplate, Config.SEO.DefaultPageMetaDescriptionTemplate,
                            Config.SEO.DefaultPageMetaKeywordsTemplate);
            }
            else if (Page is Groups)
            {
                SetMetaTags(Config.SEO.GroupsTitleTemplate, Config.SEO.GroupsMetaDescriptionTemplate,
                            Config.SEO.GroupsMetaKeywordsTemplate);
            }
            else if (Page is Login)
            {
                SetMetaTags(Config.SEO.LoginTitleTemplate, Config.SEO.LoginMetaDescriptionTemplate,
                            Config.SEO.LoginMetaKeywordsTemplate);
            }
            else if (Page is LostPassword)
            {
                SetMetaTags(Config.SEO.LostPasswordTitleTemplate, Config.SEO.LostPasswordMetaDescriptionTemplate,
                            Config.SEO.LostPasswordMetaKeywordsTemplate);
            }
            else if (Page is NewsPage)
            {
                SetMetaTags(Config.SEO.NewsTitleTemplate, Config.SEO.NewsMetaDescriptionTemplate,
                            Config.SEO.NewsMetaKeywordsTemplate);
            }
            else if (Page is Register)
            {
                SetMetaTags(Config.SEO.RegisterTitleTemplate, Config.SEO.RegisterMetaDescriptionTemplate,
                            Config.SEO.RegisterMetaKeywordsTemplate);
            }
            else if (Page is Search || Page is Search2)
            {
                SetMetaTags(Config.SEO.SearchTitleTemplate, Config.SEO.SearchMetaDescriptionTemplate,
                            Config.SEO.SearchMetaKeywordsTemplate);
            }
            else if (Page is SendProfile)
            {
                SetMetaTags(Config.SEO.SendProfileTitleTemplate, Config.SEO.SendProfileMetaDescriptionTemplate,
                            Config.SEO.SendProfileMetaKeywordsTemplate);
            }
            else if (Page is ShowGroupEvents)
            {
                SetMetaTags(Config.SEO.ShowGroupEventsTitleTemplate, Config.SEO.ShowGroupEventsMetaDescriptionTemplate,
                            Config.SEO.ShowGroupEventsMetaKeywordsTemplate);
            }
            else if (Page is ShowGroupPhotos)
            {
                SetMetaTags(Config.SEO.ShowGroupPhotosTitleTemplate, Config.SEO.ShowGroupPhotosMetaDescriptionTemplate,
                            Config.SEO.ShowGroupPhotosMetaKeywordsTemplate);
            }
            else if (Page is SmsConfirm)
            {
                SetMetaTags(Config.SEO.SmsConfirmTitleTemplate, Config.SEO.SmsConfirmMetaDescriptionTemplate,
                            Config.SEO.SmsConfirmMetaKeywordsTemplate);
            }
            else if (Page is TopCharts)
            {
                SetMetaTags(Config.SEO.TopChartsTitleTemplate, Config.SEO.TopChartsMetaDescriptionTemplate,
                            Config.SEO.TopChartsMetaKeywordsTemplate);
            }

            // Add default page title
            if (Page.Header.Title.Length == 0)
            {
                Page.Header.Title = Config.SEO.DefaultTitleTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle);
            }

            Control[] controls = new Control[Page.Header.Controls.Count];
            Page.Header.Controls.CopyTo(controls, 0);

            var descriptionMetaTag = Array.Find(controls, c => c is HtmlMeta && ((HtmlMeta)c).Name.ToLower() == "description");
            var keywordsMetaTag = Array.Find(controls, c => c is HtmlMeta && ((HtmlMeta)c).Name.ToLower() == "keywords");

            if (descriptionMetaTag == null)
            {
                descriptionMetaTag = new HtmlMeta
                {
                    ID = "Description",
                    Name = "description",
                    Content = Config.SEO.DefaultMetaDescriptionTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle)
                };
                Page.Header.Controls.Add(descriptionMetaTag);
            }
            if (keywordsMetaTag == null)
            {
                keywordsMetaTag = new HtmlMeta
                {
                    ID = "Keywords",
                    Name = "keywords",
                    Content = Config.SEO.DefaultMetaKeywordsTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle)
                };
                Page.Header.Controls.Add(keywordsMetaTag);
            }

            if (!IsPostBack)
            {
                // Add dynamic class on the body element depending on the content page
                var index = Request.Url.Segments.Length - 1;
                var filename = Request.Url.Segments[index];
                var pageClass = filename.Split('.')[0].ToLower();

                // Add class based on the browser type and version
                var browserType = Request.Browser.Browser.Replace(" ", "") + Request.Browser.MajorVersion;

                body.Attributes.Add("class", pageClass + " " + browserType + (isAuthorized ? " loggedin" : " loggedout"));

                // Add Google Analytics
                ltrGoogleAnalytics.Text = Config.Misc.GoogleAnalyticsTrackingCode;
            }

            if (IsPostBack)
            {
                var postBackWebControl = Global.GetPostBackControl(this.Page) as WebControl;
                var postBackHtmlControl = Global.GetPostBackControl(this.Page) as HtmlGenericControl;


                if (postBackWebControl != null && postBackWebControl is LinkButton)
                {
                    SelectedLinkClientID = postBackWebControl.ClientID;
                }
                else if (postBackHtmlControl != null)
                {
                    SelectedLinkClientID = postBackHtmlControl.ClientID;
                }
            }
        }

        public string SelectedLinkClientID 
        {
            get
            {
                return ViewState["SelectedLinkClientID"] as string ?? String.Empty;
            }
            set
            {
                ViewState["SelectedLinkClientID"] = value;
            }
        }

        public string SuppressLinkSelection
        {
            get
            {
                return ViewState["SuppressLinkSelection"] as string ?? false.ToString().ToLower();
            }
            set
            {
                ViewState["SuppressLinkSelection"] = value;
            }
        }

        public void SetSelectedLink(string linkClientID)
        {
            SelectedLinkClientID = linkClientID;
        }

        public void SetSuppressLinkSelection()
        {
            SuppressLinkSelection = true.ToString().ToLower();
        }

        private void SetMetaTags(string title, string metaDescription, string metaKeywords)
        {
            Control descriptionMetaTag;
            Control keywordsMetaTag;

            Page.Header.Title = title;

            if (metaDescription.Length > 0)
            {
                descriptionMetaTag = new HtmlMeta
                {
                    ID = "Description",
                    Name = "description",
                    Content = metaDescription
                };
                Page.Header.Controls.Add(descriptionMetaTag);
            }
            
            if (metaKeywords.Length > 0)
            {
                keywordsMetaTag = new HtmlMeta
                {
                    ID = "Keywords",
                    Name = "keywords",
                    Content = metaKeywords
                };
                Page.Header.Controls.Add(keywordsMetaTag);
            }
        }
    }
}