﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowUser : MasterPage
    {
        private UserSession CurrentUserSession
        {
            get { return Page.CurrentUserSession; }
        }

        private User viewedUser;

        public User ViewedUser
        {
            set
            {
                viewedUser = value;
                ViewState["ShowUser_ViewedUsername"] = value.Username;
            }
            get
            {
                return viewedUser ?? User.Load((string)ViewState["ShowUser_ViewedUsername"]);
            }
        }

        //private bool IMLocked
        //{
        //    get
        //    {
        //        return Config.Credits.Required && Config.Credits.CreditsForIM > 0 &&
        //            (CurrentUserSession == null ||
        //            (Config.Credits.Required && CurrentUserSession.Username != ViewedUser.Username &&
        //             !(Config.Users.FreeForFemales && CurrentUserSession.Gender == User.eGender.Female) &&
        //             !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM, null)));
        //    }
        //}

        private string strTargetUserID
        {
            get { return ViewedUser.Username; }
        }

        private string strUserID
        {
            get { return CurrentUserSession.Username; }
        }

        private new PageBase Page
        {
            get { return base.Page as PageBase; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["uid"] == null)
                {
                    Response.Redirect("~/Home.aspx");
                    return;
                }

                if (Config.Misc.EnableMobileVersion && Misc.IsMobileBrowser())
                {
                    Response.Redirect(UrlRewrite.CreateMobileShowUserUrl(Request.Params["uid"]));
                    return;
                }

                if (CurrentUserSession == null && Config.Users.RegistrationRequiredToBrowse)
                {
                    Response.Redirect("Login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                    return;
                }

                if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasProfile && !CurrentUserSession.IsAdmin())
                {
                    Response.Redirect("Profile.aspx?err=profnotcompl");
                    return;
                }

                if (Config.Users.PhotoRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasPhotos && !CurrentUserSession.IsAdmin())
                {
                    Response.Redirect("Profile.aspx?err=nophoto");
                    return;
                }

                LoadStrings();

                try
                {
                    User user = User.Load(Request.Params["uid"]);

                    if (user.Deleted)
                    {
                        if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                            Page.StatusPageMessage = "This user has been deleted!".Translate();
                        else
                            Page.StatusPageMessage =
                                String.Format(
                                    "This user has been deleted for the following reason:<br><br>{0}".Translate(),
                                    user.DeleteReason);
                        Response.Redirect("ShowStatus.aspx");
                        return;
                    }

                    ViewedUser = user;

                    // Save profile view
                    if (Page is ShowUserPage && CurrentUserSession != null &&
                        !CurrentUserSession.IsOptionEnabled(eUserOptions.DisableProfileViews))
                    {
                        User.SaveProfileView(
                            CurrentUserSession.Username, ViewedUser.Username);

                        User.AddScore(CurrentUserSession.Username,
                                      Config.UserScores.ViewingProfile, "ViewingProfile");
                        User.AddScore(ViewedUser.Username,
                                      Config.UserScores.ViewedProfile, "ViewedProfile");

                        if (Config.Users.NewEventNotification && CurrentUserSession.Username != ViewedUser.Username
                            && (ViewedUser.IsOnline() || User.IsUsingNotifier(ViewedUser.Username)))
                        {
                            var text = String.Format("User {0} is viewing your profile!".Translate(),
                                                     "<b>" + CurrentUserSession.Username + "</b>");
                            int imageID;
                            try
                            {
                                imageID = CurrentUserSession.GetPrimaryPhoto().Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                            }
                            var thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            var notification = new GenericEventNotification
                            {
                                Recipient = ViewedUser.Username,
                                Sender = CurrentUserSession.Username,
                                Text = text,
                                ThumbnailUrl = thumbnailUrl,
                                RedirectUrl = UrlRewrite.CreateShowUserUrl(CurrentUserSession.Username)
                            };
                            RealtimeNotification.SendNotification(notification);
                        }
                    }

                    #region show/hide IM link

                    string reason;
                    if (Config.Misc.EnableIntegratedIM &&
                        CurrentUserSession != null &&
                        !CurrentUserSession.StealthMode &&
                        ViewedUser.IsOnline() &&
                        CurrentUserSession.Username != ViewedUser.Username &&
                        User.CanSendMessage(CurrentUserSession, ViewedUser, out reason) &&
                        !Classes.User.IsUserBlocked(ViewedUser.Username, CurrentUserSession.Username))
                    {
                        pnlInstantMessenger.Visible = true;

                        var permissionCheckResult = CurrentUserSession.CanIM();

                        if (permissionCheckResult == PermissionCheckResult.No)
                            pnlInstantMessenger.Visible = false;
                        else
                        {
                            string root = HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/');
                            var sectionUnlocked = UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM, null);
                            if (permissionCheckResult == PermissionCheckResult.Yes || sectionUnlocked)
                            {
                                var timestamp = DateTime.Now.ToFileTimeUtc().ToString();
                                var hash = Misc.CalculateChatAuthHash(CurrentUserSession.Username, ViewedUser.Username, timestamp);
                                lnkInstantMessenger.HRef = "#";
                                lnkInstantMessenger.Attributes.Add("onclick",
                                                                   String.Format(
                                                                       "window.open('{0}/MessengerWindow.aspx?init=1&id={1}&target={2}&timestamp={3}&hash={4}', 'ajaxim_{1}_{2}', 'width=650,height=500,resizable=1,menubar=0,status=0,toolbar=0'); return false;",
                                                                       Config.Urls.ChatHome,
                                                                       CurrentUserSession.Username,
                                                                       ViewedUser.Username,
                                                                       timestamp,
                                                                       hash));
                                lnkInstantMessenger.Target = "AjaxIM_" + ViewedUser.Username;
                            }
                            else if (permissionCheckResult == PermissionCheckResult.YesWithCredits)
                            {
                                string url = String.Format(
                                    "if (confirm('{4}')) window.open('{0}/LaunchIM.aspx?targetUsername={1}', 'ajaxim_{2}_{3}', 'width=650,height=500,resizable=1,menubar=0,status=0,toolbar=0'); return false;",
                                    root, strTargetUserID,
                                    Regex.Replace(strUserID, @"[^A-Za-z0-9]", "_"),
                                    Regex.Replace(strTargetUserID, @"[^A-Za-z0-9]", "_"),
                                    String.Format(Lang.Trans("Opening the chat session will subtract {0} credits from your balance."), CurrentUserSession.BillingPlanOptions.CanIM.Credits /*Config.Credits.CreditsForIM*/));

                                lnkInstantMessenger.Attributes.Add("onclick", url);
                                lnkInstantMessenger.Attributes.Add("href", "");
                            }
                            else if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                                    permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                            {
                                lnkInstantMessenger.Visible = false;
                                lnkInstantMessengerPay.Visible = true;
                            }
                        }
                    }
                    else pnlInstantMessenger.Visible = false;

                    #endregion

                    #region set "allow/disallow user to view your private photos" links

                    if (Config.Photos.EnablePrivatePhotos &&
                        CurrentUserSession != null &&
                        CurrentUserSession.HasPrivatePhotos())
                    {
                        if (CurrentUserSession.HasUserAccessToPrivatePhotos(user.Username))
                            pnlGrantAccessToPrivatePhotos.Visible = false;
                        else
                            pnlDenyAccessToPrivatePhotos.Visible = false;
                    }
                    else
                    {
                        pnlGrantAccessToPrivatePhotos.Visible = false;
                        pnlDenyAccessToPrivatePhotos.Visible = false;
                    }

                    #endregion

                    #region set "allow/disallow user to view your private video" links

                    if (
                        CurrentUserSession != null &&
                        (CurrentUserSession.HasPrivateVideo()
                         || CurrentUserSession.HasPrivateVideoUpload()))
                    {
                        if (CurrentUserSession.HasUserAccessToPrivateVideo(user.Username))
                            pnlGrantAccessToPrivateVideo.Visible = false;
                        else
                            pnlDenyAccessToPrivateVideo.Visible = false;
                    }
                    else
                    {
                        pnlGrantAccessToPrivateVideo.Visible = false;
                        pnlDenyAccessToPrivateVideo.Visible = false;
                    }

                    #endregion

                    #region set "allow/disallow user to view your private audio" links

                    if (Config.Misc.EnableAudioUpload && CurrentUserSession != null &&
                        CurrentUserSession.HasPrivateAudio())
                    {
                        if (CurrentUserSession.HasUserAccessToPrivateAudio(user.Username))
                            pnlGrantAccessToPrivateAudio.Visible = false;
                        else
                            pnlDenyAccessToPrivateAudio.Visible = false;
                    }
                    else
                    {
                        pnlGrantAccessToPrivateAudio.Visible = false;
                        pnlDenyAccessToPrivateAudio.Visible = false;
                    }

                    #endregion

                    #region activate/deactivate RealPerson Verification links

                    if (Config.Users.EnableRealPersonVerificationFunctionality &&
                        CurrentUserSession != null)
                    {

                        using (var db = new Model.AspNetDatingDataContext())
                        {
                            var verifiedByThisUser =
                                db.VerifiedUsers.Any(u => u.vu_verifiedby == CurrentUserSession.Username
                                                               && u.vu_verifieduser == ViewedUser.Username);

                            if (verifiedByThisUser || CurrentUserSession.IsUserVerified(user.Username))
                                pnlCertifyUserIsGenuine.Visible = false;
                            else
                                pnlRemoveVerifiedUserStatus.Visible = false;
                        }
                    }
                    else
                    {
                        pnlCertifyUserIsGenuine.Visible = false;
                        pnlRemoveVerifiedUserStatus.Visible = false;
                    }

                    #endregion

                    #region Set "block/unlblock user" links

                    if (CurrentUserSession != null)
                    {
                        if (CurrentUserSession.IsUserBlocked(ViewedUser.Username))
                            pnlBlockUser.Visible = false;
                        else
                            pnlUnblockUser.Visible = false;
                    }
                    else
                    {
                        pnlBlockUser.Visible = false;
                        pnlUnblockUser.Visible = false;
                    }

                    #endregion

                    #region Enable/Disable ViewBlog

                    pnlBlog.Visible = Config.Misc.EnableBlogs && Blog.HasPosts(ViewedUser.Username);

                    #endregion

                    #region Enable/Disable ViewEvents

                    pnlViewEvents.Visible = Config.Users.EnableUserEventsPage;

                    #endregion

                    #region Add report abuse option

                    if (Config.AbuseReports.UserCanReportProfileAbuse
                        && (CurrentUserSession != null && (CurrentUserSession.BillingPlanOptions.UserCanReportAbuse.Value
                        || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanReportAbuse)))
                    {
                        pnlReportAbuseLink.Visible = true;
                    }

                    #endregion

                    #region Show blog if param is supplied

                    // Left for compatibility with old links to blog posts
                    if (Request.Params["bpid"] != null && !(Page is ShowUserBlog))
                    {
                        try
                        {
                            int blogPostId = Convert.ToInt32(Request.Params["bpid"]);
                            Response.Redirect(UrlRewrite.CreateShowUserBlogUrl(ViewedUser.Username,
                                blogPostId));
                            return;
                        }
                        catch (ArgumentException)
                        {
                        }
                    }

                    #endregion

                    #region Set meta tags

                    Parser parse = delegate(string text)
                                       {
                                           string result = text
                                               .Replace("%%USERNAME%%", user.Username)
                                               .Replace("%%AGE%%", user.Age.ToString())
                                               .Replace("%%GENDER%%", Lang.Trans(user.Gender.ToString()))
                                               .Replace("%%COUNTRY%%", user.Country)
                                               .Replace("%%STATE%%", user.State)
                                               .Replace("%%ZIP%%", user.ZipCode)
                                               .Replace("%%CITY%%", user.City);

                                           var regex = new Regex(@"%%Q_(\d+)%%");
                                           Match match = regex.Match(result);
                                           while (match.Success)
                                           {
                                               foreach (Capture capture in match.Groups[1].Captures)
                                               {
                                                   int questionId;
                                                   if (!int.TryParse(capture.Value, out questionId)) continue;
                                                   try
                                                   {
                                                       ProfileAnswer answer =
                                                           ProfileAnswer.Fetch(user.Username, questionId);
                                                       result =
                                                           result.Replace(String.Format("%%Q_{0}%%", questionId),
                                                                          Server.HtmlEncode(answer.Value));
                                                   }
                                                   catch (NotFoundException)
                                                   {
                                                       continue;
                                                   }
                                               }
                                               match = match.NextMatch();
                                           }

                                           return result;
                                       };

                    Page.Header.Title = parse(Config.SEO.ShowUserTitleTemplate);

                    var metaDesc = new HtmlMeta
                                       {
                                           ID = "Description",
                                           Name = "description",
                                           Content = parse(Config.SEO.ShowUserMetaDescriptionTemplate)
                                       };
                    Page.Header.Controls.Add(metaDesc);

                    var metaKeywords = new HtmlMeta
                                           {
                                               ID = "Keywords",
                                               Name = "keywords",
                                               Content = parse(Config.SEO.ShowUserMetaKeywordsTemplate)
                                           };
                    Page.Header.Controls.Add(metaKeywords);

                    #endregion

                }
                catch (ThreadAbortException)
                {
                }
                catch (ArgumentException)
                {
                    Response.Redirect("~/Home.aspx");
                }
                catch (NotFoundException)
                {
                    Response.Redirect("~/Home.aspx");
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err);
                    Response.Redirect("~/Home.aspx");
                }

                #region Show/Hide links

                if (CurrentUserSession != null)
                {
                    if (Config.Users.EnableFavorites)
                    {
                        if (CurrentUserSession.IsUserInFavouriteList(ViewedUser.Username))
                        {
                            pnlRemoveFromFavourites.Visible = true;
                            pnlAddToFavourites.Visible = false;
                        }
                        else
                        {
                            pnlRemoveFromFavourites.Visible = false;
                            pnlAddToFavourites.Visible = true;
                        }
                    }
                    else
                    {
                        pnlRemoveFromFavourites.Visible = false;
                        pnlAddToFavourites.Visible = false;
                    }

                    if (Config.Users.EnableFriends)
                    {
                        if (CurrentUserSession.IsUserInFriendList(ViewedUser.Username))
                        {
                            pnlRemoveFromFriends.Visible = true;
                            pnlAddToFriends.Visible = false;
                            pnlViewMutualFriends.Visible = false;
                        }
                        else
                        {
                            pnlRemoveFromFriends.Visible = false;
                            pnlAddToFriends.Visible = true;
                        }
                    }
                    else
                    {
                        pnlAddToFriends.Visible = false;
                        pnlRemoveFromFriends.Visible = false;
                        pnlViewMutualFriends.Visible = false;
                    }
                }
                else
                {
                    pnlRemoveFromFavourites.Visible = false;
                    pnlAddToFavourites.Visible = false;
                    pnlAddToFriends.Visible = false;
                    pnlRemoveFromFriends.Visible = false;
                    pnlViewMutualFriends.Visible = false;
                }

                if (Page is ShowUserPage && Config.ThirdPartyServices.UseBingTranslate)
                {
                    divTranslate.Visible = true;
                }

                #endregion

                if (Page is ShowUserPage)
                    Master.SetSelectedLink(lnkViewProfile.ClientID);
                else if (Page is ShowUserPhotos)
                    Master.SetSelectedLink(lnkViewPhotos.ClientID);
                else if (Page is ShowUserBlog)
                    Master.SetSelectedLink(lnkViewBlog.ClientID);
                else if (Page is ReportUserAbuse)
                    Master.SetSelectedLink(lnkReportAbuse.ClientID);
                else if (Page is ShowUserEvents)
                    Master.SetSelectedLink(lnkViewEvents.ClientID);

                SetSimilarProfilesProperties();
            }

            #region Apply profile skin

            if (ViewedUser.ProfileSkin != null || Request.Params["skin"] != null)
            {
                var cssSkinCommon = new HtmlLink();
                cssSkinCommon.Attributes.Add("rel", "stylesheet");
                cssSkinCommon.Attributes.Add("type", "text/css");
                cssSkinCommon.Href = "Skins/common.css";
                Page.Header.Controls.Add(cssSkinCommon);

                var cssSkin = new HtmlLink();
                cssSkin.Attributes.Add("rel", "stylesheet");
                cssSkin.Attributes.Add("type", "text/css");
                cssSkin.Href = Request.Params["skin"] ?? ViewedUser.ProfileSkin;
                Page.Header.Controls.Add(cssSkin);
            }

            #endregion

            PrepareLinks();            
        }

        private void SetSimilarProfilesProperties()
        {
            SimilarProfiles1.Visible = Config.Profiles.EnableSimilarProfiles;

            if (SimilarProfiles1.Visible) {
                SimilarProfiles1.ViewedUser = ViewedUser;
            }            
        }

        private void LoadStrings()
        {
            SmallBoxStart2.Title = Lang.Trans("Actions");
        }

        private void PrepareLinks()
        {
            if (Page is ShowUserPage)
                lnkViewProfile.Disabled = true;
            else
                lnkViewProfile.HRef = UrlRewrite.CreateShowUserUrl(ViewedUser.Username);

            if (Page is ShowUserPhotos)
                lnkViewPhotos.Disabled = true;
            else
                lnkViewPhotos.HRef = UrlRewrite.CreateShowUserPhotosUrl(ViewedUser.Username);

            if (Page is ShowUserBlog)
                lnkViewBlog.Disabled = true;
            else
                lnkViewBlog.HRef = UrlRewrite.CreateShowUserBlogUrl(ViewedUser.Username);

            if (Page is ShowUserEvents)
                lnkViewEvents.Disabled = true;
            else
                lnkViewEvents.HRef = UrlRewrite.CreateShowUserEventsUrl(ViewedUser.Username);

            if (Page is ReportUserAbuse)
                lnkReportAbuse.Disabled = true;
            else
                lnkReportAbuse.HRef = UrlRewrite.CreateReportUserAbuseUrl(ViewedUser.Username);

            //pnlSendEcard.Visible = Config.Users.EnableEcards;
            if (CurrentUserSession == null || CurrentUserSession.Username == ViewedUser.Username)
            {
                lnkSendMessage.Disabled = true;
                pnlSendEcard.Visible = false;
                //lnkSendEcard.Disabled = true;
            }
            else
            {
                var canSendResult = CurrentUserSession.CanSendEcards();
                
                if (canSendResult == PermissionCheckResult.No)
                    pnlSendEcard.Visible = false;
                else
                    lnkSendEcard.HRef = "SendEcard.aspx?uid=" + ViewedUser.Username;

                lnkSendMessage.HRef = "SendMessage.aspx?to_user=" + ViewedUser.Username + "&src=profile";                
            }

            lnkSendToFriend.HRef = "SendProfile.aspx?uid=" + ViewedUser.Username;
            if (CurrentUserSession == null || CurrentUserSession.Username == ViewedUser.Username)
            {
                pnlAddToFavourites.Visible = false;
                pnlAddToFriends.Visible = false;
                pnlCertifyUserIsGenuine.Visible = false;
                pnlRemoveVerifiedUserStatus.Visible = false;
                pnlGrantAccessToPrivatePhotos.Visible = false;
                pnlDenyAccessToPrivatePhotos.Visible = false;
                pnlBlockUser.Visible = false;
                pnlUnblockUser.Visible = false;
                pnlViewMutualFriends.Visible = false;
            }
            if (!Config.Users.EnableFriendsConnectionSearch)
            {
                pnlViewMutualFriends.Visible = false;
            }
            lnkAddToFavourites.HRef = String.Format("AddRemoveFavourite.aspx?uid={0}&cmd={1}&src={2}",
                                                    ViewedUser.Username, "add", "profile");
            lnkRemoveFromFavourites.HRef = String.Format("AddRemoveFavourite.aspx?uid={0}&cmd={1}&src={2}",
                                                         ViewedUser.Username, "remove", "profile");
            lnkAddToFriends.HRef = String.Format("AddRemoveFriend.aspx?uid={0}&cmd={1}&src={2}",
                                                    ViewedUser.Username, "add", "profile");
            lnkRemoveFromFriends.HRef = String.Format("AddRemoveFriend.aspx?uid={0}&cmd={1}&src={2}",
                                                         ViewedUser.Username, "remove", "profile");
            lnkBlockUser.Text = "Block this User".Translate();
            lnkUnblockUser.Text = "Unblock this User".Translate();
            lnkGrantAccess.Text = "Grant Access to my Private Photos".Translate();
            lnkDenyAccess.Text = "Revoke Access to my Private Photos".Translate();
            lnkGrantVideoAccess.Text = "Grant Access to my Private Video".Translate();
            lnkDenyVideoAccess.Text = "Revoke Access to my Private Video".Translate();
            lnkGrantAudioAccess.Text = "Grant Access to my Private Audio".Translate();
            lnkDenyAudioAccess.Text = "Revoke Access to my Private Audio".Translate();
            lnkCertifyUserIsGenuine.Text = Lang.Trans("Certify that this user is Genuine");
            lnkRemoveVerifiedUserStatus.Text = Lang.Trans("Remove \"Certified\" status");
            lnkViewMutualFriends.Text = "View mutual friends".Translate();
        }

        #region Nested type: Parser

        private delegate string Parser(string s);

        #endregion

        /// <summary>
        /// Handles the Click event of the lnkShowInterest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkShowInterest_Click(object sender, EventArgs e)
        {
            //            if (CurrentUserSession == null)
            //                Response.Redirect("~/Login.aspx");
            //
            //            try
            //            {
            //                bool sent = Interest.Send(CurrentUserSession.Username, ViewedUser.Username);
            //
            //                if (sent)
            //                {
            //                    Page.StatusPageMessage = String.Format("{0} will now be informed that you have shown interest in their profile.".Translate(), 
            //                        ViewedUser.Username);
            //
            //                    #region Add NewInterest Event
            //
            //                    Event[] evt = Event.Fetch(ViewedUser.Username, null, Event.eType.NewInterest, DateTime.Now.Date);
            //
            //                    if (evt.Length > 0)
            //                    {
            //                        NewInterest newInterest =
            //                            Misc.FromXml<NewInterest>(evt[0].DetailsXML);
            //                        if (!newInterest.Usernames.Contains(CurrentUserSession.Username))
            //                        {
            //                            newInterest.Usernames.Add(CurrentUserSession.Username);
            //                            evt[0].DetailsXML = Misc.ToXml(newInterest);
            //                            evt[0].Save();
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Event newEvent = new Event(ViewedUser.Username);
            //
            //                        newEvent.Type = Event.eType.NewInterest;
            //                        NewInterest newInterest = new NewInterest();
            //                        newInterest.Usernames.Add(CurrentUserSession.Username);
            //                        newEvent.DetailsXML = Misc.ToXml(newInterest);
            //
            //                        newEvent.Save();
            //                    }
            //
            //                    #endregion
            //                }
            //                else
            //                {
            //                    Page.StatusPageMessage = String.Format("You have already shown interest in {0}".Translate(), 
            //                        ViewedUser.Username);
            //                }
            //            }
            //            catch (NotFoundException)
            //            {
            //                Page.StatusPageMessage = "The user no longer exists!".Translate();
            //            }
            //
            //            Page.StatusPageLinkText = "Back to profile".Translate();
            //            Page.StatusPageLinkURL = UrlRewrite.CreateShowUserUrl(ViewedUser.Username);
            //
            //            Response.Redirect("ShowStatus.aspx");
        }

        /// <summary>
        /// Handles the Click event of the lnkBlockUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkBlockUser_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.BlockUser(ViewedUser.Username);
            pnlBlockUser.Visible = false;
            pnlUnblockUser.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkUnblockUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkUnblockUser_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.UnblockUser(ViewedUser.Username);
            pnlBlockUser.Visible = true;
            pnlUnblockUser.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lnkGrantAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkGrantAccess_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAccessToPrivatePhotos(ViewedUser.Username, true);

            pnlDenyAccessToPrivatePhotos.Visible = true;
            pnlGrantAccessToPrivatePhotos.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lnkDenyAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkDenyAccess_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAccessToPrivatePhotos(ViewedUser.Username, false);

            pnlDenyAccessToPrivatePhotos.Visible = false;
            pnlGrantAccessToPrivatePhotos.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkGrantVideoAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkGrantVideoAccess_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAccessToPrivateVideo(ViewedUser.Username, true);

            pnlDenyAccessToPrivateVideo.Visible = true;
            pnlGrantAccessToPrivateVideo.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lnkDenyVideoAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkDenyVideoAccess_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAccessToPrivateVideo(ViewedUser.Username, false);

            pnlDenyAccessToPrivateVideo.Visible = false;
            pnlGrantAccessToPrivateVideo.Visible = true;
        }

        protected void lnkGrantAudioAccess_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAccessToPrivateAudio(ViewedUser.Username, true);

            pnlDenyAccessToPrivateAudio.Visible = true;
            pnlGrantAccessToPrivateAudio.Visible = false;
        }

        protected void lnkDenyAudioAccess_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAccessToPrivateAudio(ViewedUser.Username, false);

            pnlDenyAccessToPrivateAudio.Visible = false;
            pnlGrantAccessToPrivateAudio.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkCertifyUserIsGenuine control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkCertifyUserIsGenuine_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.SetAsVerified(ViewedUser.Username);

            pnlCertifyUserIsGenuine.Visible = false;
            pnlRemoveVerifiedUserStatus.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkRemoveVerifiedUserStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkRemoveVerifiedUserStatus_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            CurrentUserSession.RemoveVerifiedStatus(ViewedUser.Username);

            pnlCertifyUserIsGenuine.Visible = true;
            pnlRemoveVerifiedUserStatus.Visible = false;
        }

        protected void lnkSendEcard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SendEcard.aspx?uid=" + ViewedUser.Username);
        }

        protected void lnkViewMutualFriends_Click(object sender, EventArgs e)
        {
            var search = new MutualFriendsSearch
            {
                Viewer = CurrentUserSession.Username,
                Viewed = ViewedUser.Username
            };

            Session["MutualFriendsSearch"] = search;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkViewFriendsConnection_Click(object sender, EventArgs e)
        {
            var search = new FriendsConnectionSearch
            {
                Viewer = CurrentUserSession.Username,
                Viewed = ViewedUser.Username
            };

            Session["FriendsConnectionSearch"] = search;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkInstantMessengerPay_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanIM;
                Response.Redirect("~/Profile.aspx?sel=payment");
            }
        }
    }
}