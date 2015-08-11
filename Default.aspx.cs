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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class _default : PageBase
    {
        public _default()
        {
            RequiresAuthorization = false;
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (Config.Misc.EnableMobileVersion && Misc.IsMobileBrowser())
            {
                Response.Redirect("~/mobile/default.aspx");
                return;
            }

            if (Properties.Settings.Default.AutoAddWWW
                && !Request.Url.Host.ToLowerInvariant().Contains("www.")
                && Config.Urls.Home.ToLowerInvariant().Contains("www."))
            {
                Response.Redirect(Config.Urls.Home);
            }

            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserSession != null
                && CurrentUserSession.IsAuthorized)
            {
                Response.Redirect("Home.aspx");
            }

            if (!Page.IsPostBack)
            {
                preparePage();
                prepareRssLinks();
            }

            loadComponents();
        }

        private void preparePage()
        {
            #region Check is the user is invited to register

            string invitedByUsername = Request.QueryString["invitedBy"];
            if (!string.IsNullOrEmpty(invitedByUsername))
            {
                // ReSharper disable PossibleNullReferenceException
                Response.Cookies["invitedBy"].Value = invitedByUsername;
                Response.Cookies["invitedBy"].Expires = DateTime.Now.AddDays(7);
                // ReSharper restore PossibleNullReferenceException
            }

            #endregion

            #region Check is the user coming to register through affiliate

            string affiliateID = Request.QueryString["affid"];
            if (!string.IsNullOrEmpty(affiliateID))
            {
                // ReSharper disable PossibleNullReferenceException
                Response.Cookies["affiliateID"].Value = affiliateID;
                Response.Cookies["affiliateID"].Expires = DateTime.Now.AddDays(7);
                // ReSharper restore PossibleNullReferenceException
            }

            #endregion

            #region Check Remember Me option

            if (Request.Cookies["rememberMe"] != null)
            {
                Response.Redirect("~/Login.aspx");
            }

            #endregion
        }

        private void loadComponents()
        {
            string cacheKey = String.Format("_default_loadComponents_{0}", Theme);
            XDocument xdoc;
            if (Cache[cacheKey] != null)
            {
                xdoc = XDocument.Parse((string) Cache[cacheKey]);
            }
            else
            {

                var schemaPath = Server.MapPath("~/App_Themes/" + Theme + "/default.xml");
                if (!File.Exists(schemaPath)) return;
                xdoc = XDocument.Load(schemaPath);
                Cache.Insert(cacheKey, xdoc.ToString(), null, Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(10), CacheItemPriority.High, null);
            }
            var components = (from c in xdoc.Descendants("Body")
                              select c).First().Elements();
            foreach (var component in components)
            {
                switch (component.Name.LocalName)
                {
                    case "Component":
                        XAttribute srcAttribute = component.Attribute("Src");
                        if (srcAttribute == null) continue;
                        var control = LoadControl(srcAttribute.Value);
                        XAttribute idAttribute = component.Attribute("ID");
                        if (idAttribute != null)
                            control.ID = idAttribute.Value;
                        XAttribute skinAttribute = component.Attribute("SkinID");
                        if (skinAttribute != null)
                            control.SkinID = skinAttribute.Value;
                        XAttribute optionsAttribute = component.Attribute("Options");
                        if (control is IOptions && optionsAttribute != null)
                            ((IOptions) control).Options = optionsAttribute.Value;
                        plhComponents.Controls.Add(control);
                        break;
                    case "HtmlContent":
                        if (!string.IsNullOrEmpty(component.Value))
                        {
                            plhComponents.Controls.Add(new LiteralControl(component.Value));
                        }
                        break;
                    case "ContentView":
                        if (!String.IsNullOrEmpty(component.Value))
                        {
                            Components.ContentView cv = new Components.ContentView();
                            cv.Text = component.Value;
                            XAttribute key = component.Attribute("Key");
                            if (key != null) cv.Key = key.Value;
                            plhComponents.Controls.Add(cv);
                        }
                        break;
                }
            }
        }

        private void prepareRssLinks()
        {
            var rssNews = new HtmlLink();
            rssNews.Attributes.Add("rel", "alternate");
            rssNews.Attributes.Add("type", "application/rss+xml");
            rssNews.Attributes.Add("title", Config.Misc.SiteTitle + " - " + Lang.Trans("News"));
            rssNews.Attributes.Add("href", "Rss.ashx?action=news");
            Header.Controls.Add(rssNews);
        }
    }
}