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
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using System.Linq;

namespace AspNetDating.Components
{
    public partial class Header : UserControl
    {
        #region Server Controls

        protected HtmlAnchor lnkAboutUs;
        protected HtmlAnchor lnkContactUs;
        protected HtmlAnchor lnkFavourites;
        protected HtmlAnchor lnkHome;
        protected HtmlAnchor lnkMail;
        protected HtmlAnchor lnkProfile;
        protected HtmlAnchor lnkSearch;
        protected HtmlAnchor lnkSubscription;
        protected HtmlAnchor lnkTopPhotos;
        protected HtmlAnchor lnkTopUsers;
        protected HtmlGenericControl tdAboutUs;
        protected HtmlGenericControl tdContactUs;

        #endregion

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
            set { ((PageBase)Page).CurrentUserSession = value; }
        }

        void AddSelectionCssClass(HtmlGenericControl control)
        {
            control.Attributes.Add("class", "active");
        }
        void SetSelectedLinkClass()
        {
            if (Page is Home || Page is _default)
                AddSelectionCssClass(liHome);
            else if (Page is AspNetDating.Groups || Page is ShowGroup || 
                        Page is ShowGroupEvents || Page is ShowGroupPhotos || Page is ShowGroupTopics)
                AddSelectionCssClass(liGroups);
            else if (Page is BrowseVideos)
                AddSelectionCssClass(liVideos);
            else if (Page is RatePhotos)
                AddSelectionCssClass(liRatePhotos);
            else if (Page is Ads)
                AddSelectionCssClass(liAds);
            else if (Page is AspNetDating.PhotoContestsPage)
                AddSelectionCssClass(liContests);
            else if (Page is BroadcastVideo)
                AddSelectionCssClass(liBroadcast);
            else if (Page is TopCharts)
                AddSelectionCssClass(liTopCharts);
            else if (Page is AspNetDating.Search || Page is Search2)
                AddSelectionCssClass(liSearch);
            else if (Page is Favourites)
                AddSelectionCssClass(liFavorites);
            else if (Page is Friends)
                AddSelectionCssClass(liFriends);
            else if (Page is ReviewNewPhotos)
                AddSelectionCssClass(liReviewNewPhotos);
            else if (Page is ReviewNewUsers)
                AddSelectionCssClass(liReviewNewProfiles);
            else if (Page is MemberProfile)
                AddSelectionCssClass(liProfile);
            else if (Page is MemberBlog)
                AddSelectionCssClass(liBlog);
            else if (Page is MailBox)
                AddSelectionCssClass(liMailbox);
            else if (Page is ContentPage)
            {
                var li = rptPages.Items.Cast<RepeaterItem>().Select(i => i.FindControl("liContentPage")).Cast<HtmlGenericControl>().
                    Where(ctrl => ctrl.Attributes["data-id"] == Request.Params["id"]).FirstOrDefault();
                if (li != null)
                    AddSelectionCssClass(li);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PrepareStrings();
            }

            LoadPages();

            if (!IsPostBack)
                SetSelectedLinkClass();
        }

        private void PrepareStrings()
        {
            if (Page is PageBase && CurrentUserSession != null)
            {
                string username = CurrentUserSession.Username;
                lblWelcome.Text = String.Format(
                    Lang.Trans("<i class=\"fa fa-user\"></i> {0}"), username);
                string credits = CurrentUserSession.Credits == 1 ? "credit" : "credits";
                lblCredits.Text = "(" + CurrentUserSession.Credits + ") " + credits;
                lblCredits.Visible = CurrentUserSession.BillingPlanOptions.ContainsOptionWithEnabledCredits;
                pnlLogout.Visible = true;
                pnlLogin.Visible = false;
                pnlLanguage.Visible = false;

                var permissionCheckResult = CurrentUserSession.CanUseChat();

                int unreadMsgCount = Message.SearchUnread(CurrentUserSession.Username).Length;
                if (unreadMsgCount > 0)
                {
                    ltrNewMessages.Visible = true;
                    ltrNewMessages.Text = String.Format(" ({0})", unreadMsgCount);
                }
                else
                {
                    ltrNewMessages.Visible = false;
                }
                
                liAjaxChat.Visible = Config.Misc.EnableAjaxChat
                                     && !(Config.CommunityFaceControlSystem.EnableCommunityFaceControl && !CurrentUserSession.FaceControlApproved)
                                     && (permissionCheckResult != PermissionCheckResult.No
                                         ||
                                         (CurrentUserSession.Level != null &&
                                          CurrentUserSession.Level.Restrictions.UserCanUseChat));

#if AJAXCHAT_INTEGRATION
                var timestamp = DateTime.Now.ToFileTimeUtc().ToString();
                var hash = Misc.CalculateChatAuthHash(CurrentUserSession.Username, String.Empty, timestamp);
                lnkAjaxChat.HRef = String.Format("{0}/ChatRoom.aspx?id={1}&timestamp={2}&hash={3}",
                       Config.Urls.ChatHome,
                       CurrentUserSession.Username,
                       timestamp,
                       hash
                    );
#endif

                if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                    permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    lnkAjaxChatPay.Visible = true;
                    lnkAjaxChat.Visible = false;
                    //lnkAjaxChat.HRef = "~/Profile.aspx?sel=payment";
                    //lnkAjaxChat.Target = null;
                }
                else
                {
                    lnkAjaxChatPay.Visible = false;
                    lnkAjaxChat.Visible = true;
                    //lnkAjaxChat.HRef = "~/AjaxChat/ChatWindow.aspx";
                    //lnkAjaxChat.Target = "_ajaxchat";
                }

                liContests.Visible = Config.Ratings.EnablePhotoContests;
                liBroadcast.Visible = Config.Misc.EnableProfileVideoBroadcast;
                liReviewNewPhotos.Visible = Config.CommunityModeratedSystem.EnableCommunityPhotoApproval &&
                                            (CurrentUserSession.Level == null ||
                                             CurrentUserSession.Level.Restrictions.AllowToModeratePhotos) &&
                                            CurrentUserSession.ModerationScores >=
                                            Config.CommunityModeratedSystem.MinimumScoresToAllowModeration;
                liReviewNewProfiles.Visible = Config.CommunityFaceControlSystem.EnableCommunityFaceControl &&
                                              (CurrentUserSession.Level == null ||
                                               CurrentUserSession.Level.Restrictions.AllowToParticipateInFaceControl) &&
                                              CurrentUserSession.ModerationScores >=
                                              Config.CommunityFaceControlSystem.MinimumScoresToAllowModeration;
                liFavorites.Visible = Config.Users.EnableFavorites;
                liFriends.Visible = Config.Users.EnableFriends;

                if (Config.Misc.SiteIsPaid &&
                    !(Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female))
                {
                    if (liSubscription != null)
                        liSubscription.Visible = true;
                }
                else
                {
                    if (liSubscription != null)
                        liSubscription.Visible = false;
                }
            }
            else
            {
                liFavorites.Visible = false;
                liFriends.Visible = false;
                liMailbox.Visible = false;
                liProfile.Visible = false;
                liBlog.Visible = false;
                liRatePhotos.Visible = false;

                if (liSubscription != null)
                    liSubscription.Visible = false;

                pnlLogout.Visible = false;
                pnlLogin.Visible = true;
                pnlLanguage.Visible = true;
            }

            if (Config.Users.DisableGenderInformation
                || !Config.Ratings.EnablePhotoRatings
                && !Config.Ratings.EnableProfileRatings
                && (!Config.CommunityModeratedSystem.EnableTopModerators ||
                    !Config.CommunityModeratedSystem.EnableCommunityPhotoApproval))
            {
                liTopCharts.Visible = false;
            }

            if (!Config.Misc.EnableBlogs)
            {
                liBlog.Visible = false;
            }
            else if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Level != null)
                {
                    if (!CurrentUserSession.Level.Restrictions.CanCreateBlogs &&
                        CurrentUserSession.CanCreateBlogs() == PermissionCheckResult.No)
                    {
                        liBlog.Visible = false;
                    }
                }
                else if (CurrentUserSession.CanCreateBlogs() == PermissionCheckResult.No)
                {
                    liBlog.Visible = false;
                }
            }

            if (!Config.Ratings.EnableRatePhotos)
            {
                liRatePhotos.Visible = false;
            }
            else if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Level != null)
                {
                    if (!CurrentUserSession.Level.Restrictions.CanRatePhotos &&
                        CurrentUserSession.CanRatePhotos() == PermissionCheckResult.No)
                    {
                        liRatePhotos.Visible = false;
                    }
                }
                else if (CurrentUserSession.CanRatePhotos() == PermissionCheckResult.No)
                {
                    liRatePhotos.Visible = false;
                }
            }

            if (!Config.Groups.EnableGroups)
            {
                liGroups.Visible = false;
            }

            if (!Config.Ads.Enable)
            {
                liAds.Visible = false;
            }

            liVideos.Visible = Config.Misc.EnableVideoUpload || Config.Misc.EnableYouTubeVideos;

            if (!Page.IsPostBack)
            {
                string[] lang = Request.UserLanguages;
                foreach (Language language in Language.FetchAll())
                {
                    if (!language.Active) continue;
                    var listItem = new ListItem(language.Name, language.Id.ToString());
                    if (language.Id == ((PageBase)Page).LanguageId)
                        listItem.Selected = true;
                    ddLanguages.Items.Add(listItem);
                }

                if (ddLanguages.Items.Count <= 1)
                {
                    if (ddLanguages.Items.Count == 1)
                        ddLanguages.SelectedIndex = 0;
                    ((PageBase)Page).LanguageId = Convert.ToInt32(ddLanguages.SelectedValue);
                    pnlLanguage.Visible = false;
                }
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["rememberMe"] != null)
            {
                var cookie = new HttpCookie("rememberMe") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }

            bool logoutFromFacebook = false;
            if (CurrentUserSession != null)
            {
                logoutFromFacebook = CurrentUserSession.LoggedInThroughFacebook;
                CurrentUserSession = null;
            }

            Classes.MySpace.DataAvailability.RevokeAccess(Context);
            if (Config.Misc.EnableFacebookIntegration && logoutFromFacebook)
            {
                //var facebook = new Facebook();
                //facebook.Logout(Config.Urls.Home);
                //return;
            }

            if (Config.Users.RedirectAfterLogout.Length > 0)
            {
                Response.Redirect(Config.Users.RedirectAfterLogout);
                return;
            }

            Response.Redirect("~/default.aspx");
        }

        protected void ddLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((PageBase)Page).LanguageId = Convert.ToInt32(ddLanguages.SelectedValue);
            Response.Redirect(Request.Url.ToString());
        }

        private void LoadPages()
        {
            var lContentPages = new ArrayList();

            Classes.ContentPage[] contentPages =
                Classes.ContentPage.FetchContentPages(ddLanguages.SelectedItem == null ? PageBase.GetLanguageId() :
                                                        Convert.ToInt32(ddLanguages.SelectedValue),
                                                      Classes.ContentPage.eSortColumn.HeaderPosition);

            bool loggedOn = CurrentUserSession != null;
            bool isPaid = CurrentUserSession != null && Classes.User.IsPaidMember(CurrentUserSession.Username);

            foreach (Classes.ContentPage contentPage in contentPages)
            {
                if (contentPage.HeaderPosition != null)
                {
                    if (
                        ((loggedOn && ((contentPage.VisibleFor & Classes.ContentPage.eVisibility.LoggedOnUsers) != 0 ||
                            contentPage.VisibleFor == Classes.ContentPage.eVisibility.Paid && isPaid ||
                            contentPage.VisibleFor == Classes.ContentPage.eVisibility.Unpaid && !isPaid)))
                        ||
                        ((!loggedOn && (contentPage.VisibleFor & Classes.ContentPage.eVisibility.NotLoggedOnUsers) != 0))
                       )
                        lContentPages.Add(contentPage);
                }
            }

            rptPages.DataSource = lContentPages.ToArray();
            rptPages.DataBind();
        }

        protected void lnkAjaxChatPay_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.UserCanUseChat;
                Response.Redirect("~/Profile.aspx?sel=payment");
            }
        }
    }
}