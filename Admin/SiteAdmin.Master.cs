using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetDating.Admin
{
    public partial class SiteAdmin : System.Web.UI.MasterPage
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string IconSource { get { return ResolveClientUrl(String.Format("{0}", Section)); } }
        public string PageName { get { return GetPageFileName(); } }

        public string Section
        {
            get
            {
                switch (PageName)
                {
                    case "home":
                        return "home";
                    case "browseusers":
                    case "browsephotos":
                    case "browsemessages":
                    case "browsespamsuspects":
                    case "browsevideouploads":
                    case "approvephotos2":
                    case "approvesalutephotos":
                    case "approvevideouploads":
                    case "approveaudiouploads":
                    case "approveanswers":
                    case "approveanswer":
                    case "approveblogposts":
                    case "editscheduledannouncements":
                    case "spamcheck":
                    case "abusereports":
                    case "manageuserlevels":
                    case "manageuser":
                    case "editprofile":
                    case "creditshistory":
                        return "user-management";
                    case "managecontests":
                    case "contestentries":
                        return "contests";
                    case "editadscategories":
                    case "approveads":
                        return "classifieds";
                    case "managegroupcategories":
                    case "browsegroups":
                    case "approvegroups":
                        return "group-management";
                    case "editlanguages":
                    case "edittopics":
                    case "editnews":
                    case "managewebparts":
                    case "managepolls":
                    case "managebadwords":
                    case "edittemplates":
                    case "editgoogleanalytics":
                    case "editbanners":
                    case "uploadlogo":
                    case "editcontentpages":
                    case "editcontentviews":
                    case "editecardtypes":
                    case "editstrings":
                    case "thememanager":
                    case "settings":
                        return "site-management";
                    case "browseadmins":
                    case "editadmin":
                        return "admin-management";
                    case "billingsettings":
                    case "creditspackages":
                        return "payment-management";
                    case "editmetatags":
                        return "seo-management";
                    case "browseaffiliates":
                    case "affiliatepayments":
                    case "paymenthistory":
                    case "commissionshistory":
                    case "affiliatebanners":
                    case "editaffiliate":
                        return "affiliate-management";
                    case "newusersstats":
                    case "onlineusersstats":
                        return "statistics";
                    default:
                        return string.Empty;
                }
            }
        }

        public MessageBox MessageBox { get { return MessageBox1; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private string GetPageFileName()
        {
            var index = Request.Url.Segments.Length - 1;
            var filenameWithExtension = Request.Url.Segments[index];
            return filenameWithExtension.Split('.')[0].ToLower();            
        }

    }
}