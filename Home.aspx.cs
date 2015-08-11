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
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AspNetDating.Classes;
using AspNetDating.Components.WebParts;
using WebPartZone = AspNetDating.Classes.WebPartZone;
using System.Linq;

namespace AspNetDating
{
    public partial class Home : PageBase
    {
        private WebPartZone ZoneToAddPartsTo
        {
            get { return (WebPartZone)(ViewState["ZoneToAddPartsTo"] ?? WebPartZone.HomePageRightZone); }

            set { ViewState["ZoneToAddPartsTo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebPartManager1.SelectedWebPartChanged += WebPartManager1_SelectedWebPartChanged;
            WebPartManager1.DisplayModeChanged += WebPartManager1_DisplayModeChanged;

            if (!Page.IsPostBack)
            {
                loadStrings();
                preparePage();
                WebPartManager1.ReOpenClosedWebParts();
            }
        }

        private void WebPartManager1_DisplayModeChanged(object sender, WebPartDisplayModeEventArgs e)
        {
            if (WebPartManager1.DisplayMode == WebPartManager.EditDisplayMode)
            {
                lnkCatalogForLeftParts.Visible = false;
                lnkCatalogForRightParts.Visible = false;
                pnlCatalog.Visible = false;
            }
            else
            {
                lnkCatalogForLeftParts.Visible = true;
                lnkCatalogForRightParts.Visible = true;
            }
        }

        private void WebPartManager1_SelectedWebPartChanged(object sender, WebPartEventArgs e)
        {
            if (e.WebPart != null && WebPartManager1.DisplayMode == WebPartManager.EditDisplayMode)
            {
                if (e.WebPart.Zone == wpzHomePageLeftZone)
                {
                    pnlEditorZoneLeft.Visible = true;
                    pnlEditorZoneRight.Visible = false;
                }
                else if (e.WebPart.Zone == wpzHomePageRightZone)
                {
                    pnlEditorZoneLeft.Visible = false;
                    pnlEditorZoneRight.Visible = true;
                }
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("My Profile");
            lnkViewProfileViewers.Attributes.Add("title", Lang.Trans("Viewers"));
            lnkUsersOnline.Attributes.Add("title", Lang.Trans("Users Online"));
            lnkUsersBroadcasting.Attributes.Add("title", Lang.Trans("Users Broadcasting"));
            lnkPendingInvitations.Attributes.Add("title", Lang.Trans("Pending Invitations"));
            lnkViewMutualVotes.Attributes.Add("title", Lang.Trans("View Mutual"));
            btnCancelCatalog.Text = Lang.Trans("Cancel");
            ezEditorZoneLeft.HeaderText = Lang.Trans("Edit");
            ezEditorZoneRight.HeaderText = Lang.Trans("Edit");
            lnkEditStatusText.Attributes.Add("title", Lang.Trans("Edit Status Text"));
            lnkUpdateStatusText.Attributes.Add("title", Lang.Trans("Update Status Text"));

            if (Config.Misc.LockHomePageLayout)
            {
                lnkCatalogForLeftParts.Visible = false;
                lnkCatalogForRightParts.Visible = false;
            }
            else
            {
                lnkCatalogForLeftParts.Visible = true;
                lnkCatalogForRightParts.Visible = true;            
            }

            ezEditorZoneRight.OKVerb.Text = ezEditorZoneLeft.OKVerb.Text = Lang.Trans(ezEditorZoneLeft.OKVerb.Text);
            ezEditorZoneRight.CancelVerb.Text = ezEditorZoneLeft.CancelVerb.Text = Lang.Trans(ezEditorZoneLeft.CancelVerb.Text);
            ezEditorZoneRight.ApplyVerb.Text = ezEditorZoneLeft.ApplyVerb.Text = Lang.Trans(ezEditorZoneLeft.ApplyVerb.Text);
            ezEditorZoneRight.InstructionText = ezEditorZoneLeft.InstructionText = Lang.Trans(ezEditorZoneLeft.InstructionText);
        }

        private void preparePage()
        {
            #region Load Photo

            Photo primaryPhoto = null;
            try
            {
                primaryPhoto = CurrentUserSession.GetPrimaryPhoto();
            }
            catch (NotFoundException)
            {
            }
            catch (Exception err)
            {
                Log(err);
            }

            #region Check CurrentUserSession.Gender and set photoId

            int photoId;

            if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                photoId = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
            }
            else
            {
                photoId = primaryPhoto.Id;
            }

            #endregion

            if (Config.Photos.EnablePhotoStack)
            {
                imgPhoto.ImageUrl = ImageHandler.CreateImageStackUrl(CurrentUserSession.Username, 200, 150);
            }
            else
            {
                imgPhoto.ImageUrl = String.Format("~/Image.ashx?id={0}&width=120&height=120&diskCache=1&findFace=1", photoId);
                imgPhoto.CssClass = "img-thumbnail";
            }

            #endregion

            #region Show Profile Views

            if (Config.Users.EnableWhoViewedMyProfile)
            {
                lblProfileViews.Text = CurrentUserSession.ProfileViews.ToString();
                if (CurrentUserSession.ProfileViews == 0)
                {
                    lnkViewProfileViewers.Enabled = false;
                }
                liWhoViewedMyProfile.Visible = true;
            }
            else
            {
                liWhoViewedMyProfile.Visible = false;
            }

            #endregion

            #region Show Rating

            if (Config.Ratings.EnableProfileRatings)
            {
                pnlRating.Visible = true;
                try
                {
                    UserRating userRating = UserRating.FetchRating(CurrentUserSession.Username);

                    lblRating.Text = String.Format(
                        Lang.Trans("{0} ({1} votes)"),
                        userRating.AverageVote.ToString("0.00"), userRating.Votes);
                }
                catch (NotFoundException)
                {
                    lblRating.Text = Lang.Trans("no rating");
                }
            }
            else pnlRating.Visible = false;

            #endregion

            #region Show Votes

            if (Config.Ratings.EnableProfileVoting)
            {
                int score = UserVotes.FetchVotesScore(CurrentUserSession.Username);
                if (score > 0)
                {
                    pnlVotes.Visible = true;
                    lblVotes.Text = score.ToString();
                }
                else
                {
                    pnlVotes.Visible = false;
                }
            }
            else
            {
                pnlVotes.Visible = false;
            }

            #endregion

            #region Load New Users

            var nuSearch = new NewUsersSearch
                               {
                                   PhotoReq = Config.Users.RequirePhotoToShowInNewUsers,
                                   ProfileReq = Config.Users.RequireProfileToShowInNewUsers,
                                   UsersSince = CurrentUserSession.PrevLogin
                               };
            UserSearchResults nuResults = nuSearch.GetResults(true);
            if (nuResults == null)
            {
                pnlNewUsers.Visible = false;
            }
            else
            {
                if (nuResults.Usernames.Length == 1)
                {
                    lnkNewUsers.Text = Lang.Trans("1");
                }
                else
                {
                    lnkNewUsers.Text = String.Format(Lang.Trans("{0}"), nuResults.Usernames.Length);
                }


                pnlNewUsers.Visible = true;
            }

            #endregion

            #region Load Online Users

            var oSearch = new OnlineSearch();
            UserSearchResults oResults = oSearch.GetResults();

            if (oResults == null)
            {
                pnlUsersOnline.Visible = false;
            }
            else
            {
               if (oResults.Usernames.Length == 1) {
                   lblUsersOnline.Text = Lang.Trans("user online");
                   lnkUsersOnline.Text = Lang.Trans("1");
               }
                   else {
                       lblUsersOnline.Text = Lang.Trans("users online"); 
                       lnkUsersOnline.Text = String.Format(Lang.Trans("{0}"), oResults.Usernames.Length);
                   }


                pnlUsersOnline.Visible = true;
            }

            #endregion

            #region Show users broadcasting video

            if (Config.Misc.EnableProfileVideoBroadcast)
            {
                var vbSearch = new VideoBroadcastingSearch();
                UserSearchResults vbResults = vbSearch.GetResults();

                if (vbResults == null)
                {
                    pnlUsersBroadcasting.Visible = false;
                }
                else
                {
                    if (vbResults.Usernames.Length == 1)
                    {
                        lblUsersBroadcasting.Text = Lang.Trans("user broadcasting video");
                        lnkUsersBroadcasting.Text = Lang.Trans("1");
                    }
                    else
                    {
                        lblUsersBroadcasting.Text = Lang.Trans("users broadcasting video");
                        lnkUsersBroadcasting.Text = String.Format(Lang.Trans("{0}"), vbResults.Usernames.Length);
                    }

                    pnlUsersBroadcasting.Visible = true;
                }
            }
            else
            {
                pnlUsersBroadcasting.Visible = false;
            }

            #endregion

            #region Load Blocked Users

            int blockedUsers = Classes.User.FetchBlockedUsers(CurrentUserSession.Username).Count;

            if (blockedUsers == 0)
            {
                pnlBlockedUsers.Visible = false;
            }
            else
            {
                lblBlockedUsers.Text = blockedUsers == 1
                                           ? Lang.Trans("1")
                                           : String.Format(Lang.Trans("{0}"), blockedUsers);

                pnlBlockedUsers.Visible = true;
            }

            #endregion

            #region Show Unread Messages

            int unreadMsgCount = Message.SearchUnread(CurrentUserSession.Username).Length;
            if (unreadMsgCount > 0)
            {
                pnlNewMessages.Visible = true;

                if (unreadMsgCount == 1)
                {
                    if (lblNewMessages != null)
                    {
                        lblNewMessages.Text = Lang.Trans("unread message");
                        lnkNewMessages.Text = Lang.Trans("1");
                    }
                    else
                    {
                        lnkNewMessages.Text = Lang.Trans("unread message");
                    }
                }
                else
                {
                    if (lblNewMessages != null)
                    {
                        lblNewMessages.Text = Lang.Trans("unread messages");
                        lnkNewMessages.Text = String.Format(Lang.Trans("{0}"), unreadMsgCount);
                    }
                    else
                    {
                        lnkNewMessages.Text = Lang.Trans("unread messages");
                    }
                }
            }
            else
            {
                pnlNewMessages.Visible = false;
            }

            #endregion

            #region Show Relationship Requests

            if (Config.Users.EnableRelationshipStatus)
            {
                int relationshipRequests = Relationship.FetchRequests(CurrentUserSession.Username).Length;
                if (relationshipRequests > 0)
                {
                    pnlRelationshipRequests.Visible = true;

                    if (relationshipRequests == 1)
                    {
                        if (lblRelationshipRequests != null)
                        {
                            lblRelationshipRequests.Text = Lang.Trans("relationship request");
                            lnkRelationshipRequests.Text = Lang.Trans("1");
                        }
                        else
                        {
                            lnkRelationshipRequests.Text = Lang.Trans("1");
                        }
                    }
                    else
                    {
                        if (lblRelationshipRequests != null)
                        {
                            lblRelationshipRequests.Text = Lang.Trans("relationship requests");
                            lnkRelationshipRequests.Text = String.Format(Lang.Trans("{0}"), relationshipRequests);
                        }
                        else
                        {
                            lnkRelationshipRequests.Text = String.Format(Lang.Trans("{0}"), relationshipRequests);
                        }
                    }
                }
                else
                {
                    pnlRelationshipRequests.Visible = false;
                }
            }
            else pnlRelationshipRequests.Visible = false;

            #endregion

            #region Show Friend Requests

            int friendRequests = Classes.User.FetchFriendsRequests(CurrentUserSession.Username).Length;
            if (friendRequests > 0)
            {
                pnlFriendsRequests.Visible = true;

                if (friendRequests == 1)
                {
                    if (lblFriendsRequests != null)
                    {
                        lblFriendsRequests.Text = Lang.Trans("friend request");
                        lnkFriendsRequests.Text = Lang.Trans("1");
       
                    }
                    else
                    {
                        lnkFriendsRequests.Text = Lang.Trans("friend request");
                    }
                }
                else
                {
                    if (lblFriendsRequests != null)
                    {
                        lblFriendsRequests.Text = Lang.Trans("friend requests");
                        lnkFriendsRequests.Text = String.Format(Lang.Trans("{0}"), friendRequests);
                    }
                    else
                    {
                        lnkFriendsRequests.Text = Lang.Trans("friend request");
                    }
                }
            }
            else
            {
                pnlFriendsRequests.Visible = false;
            }

            #endregion

            #region Show New Ecards

            if (CurrentUserSession.CanSendEcards() != PermissionCheckResult.No)
            {
                int unreadEcardsCount = Ecard.FetchUnread(CurrentUserSession.Username).Length;
                if (unreadEcardsCount > 0)
                {
                    pnlNewEcards.Visible = true;

                    if (unreadEcardsCount == 1)
                    {
                        if (lblNewEcards != null)
                        {
                            lblNewEcards.Text = Lang.Trans("unread e-card");
                            lnkNewEcards.Text = Lang.Trans("1");
                        }
                        else
                        {
                            lblNewEcards.Text = Lang.Trans("unread e-card");
                        }
                    }
                    else
                    {
                        if (lblNewEcards != null)
                        {
                            lblNewEcards.Text = Lang.Trans("unread e-cards");
                            lnkNewEcards.Text = String.Format(Lang.Trans("{0}"), unreadEcardsCount);
                        }
                        else
                        {
                            lblNewEcards.Text = Lang.Trans("unread e-cards");
                        }
                    }
                }
                else
                {
                    pnlNewEcards.Visible = false;
                }
            }
            else pnlNewEcards.Visible = false;
            

            #endregion

            #region Add Initial Web Parts

            bool firstTime = CurrentUserSession.PersonalizationInfo == null;
            bool isDirty = firstTime;

            if (firstTime)
            {
                WebPartInfo[] infos = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageRightZone, true);

                for (int i = 0; i < infos.Length; ++i)
                    WebPartManager1.AddWebPartUserControl(infos[i], wpzHomePageRightZone, i);

                infos = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageLeftZone, true);

                for (int i = 0; i < infos.Length; ++i)
                    WebPartManager1.AddWebPartUserControl(infos[i], wpzHomePageLeftZone, i);
            }

            if (!Config.Groups.EnableGroups)
            {
                foreach (WebPart part in WebPartManager1.WebParts)
                {
                    if (part.Controls.Count > 0 && part.Controls[0] is NewGroupsWebPart)
                    {
                        WebPartManager1.DeleteWebPart(part);
                        isDirty = true;
                    }
                }
            }

            if (isDirty)
            {
                WebPartManager1.SetDirty();
                // ReSharper disable RedundantAssignment
                isDirty = false;
                // ReSharper restore RedundantAssignment
            }

            #endregion

            #region Show contest rankings

            if (Config.Ratings.EnablePhotoContests)
            {
                PhotoContestEntry[] entries = PhotoContestEntry.Load(null, null, CurrentUserSession.Username, null);
                if (entries != null && entries.Length > 0)
                {
                    var dtRanks = new DataTable();
                    dtRanks.Columns.Add("Rank", typeof(int));
                    dtRanks.Columns.Add("ContestName", typeof(string));

                    foreach (PhotoContestEntry entry in entries)
                    {
                        int rank = PhotoContestEntry.FindRank(entry.ContestId, entry.Id);
                        if (rank > 0)
                        {
                            PhotoContest contest = PhotoContest.Load(entry.ContestId);
                            if (contest.DateEnds.HasValue && contest.DateEnds < DateTime.Now) continue;

                            dtRanks.Rows.Add(new object[] { rank, contest.Name });
                        }
                    }

                    if (dtRanks.Rows.Count > 0)
                    {
                        rptContestsRanks.DataSource = dtRanks;
                        rptContestsRanks.DataBind();
                        rptContestsRanks.Visible = true;
                    }
                }
            }

            #endregion

            #region Show Pending Invitations

            if (Config.Groups.EnableGroups)
            {
                int pendingInvitations = Group.FetchPendingInvitations(CurrentUserSession.Username);

                if (pendingInvitations > 0)
                {
                    pnlPendingInvitations.Visible = true;

                    if (pendingInvitations == 1)
                    {
                        if (lblPendingInvitatinos != null)
                        {
                            lblPendingInvitatinos.Text = Lang.Trans("pending invitation");
                            lnkPendingInvitations.Text = Lang.Trans("1");
                        }
                        else
                        {
                            lblPendingInvitatinos.Text = Lang.Trans("pending invitation");
                        }
                    }
                    else
                    {
                        if (lblPendingInvitatinos != null)
                        {
                            lblPendingInvitatinos.Text = Lang.Trans("pending invitations");
                            lnkPendingInvitations.Text = String.Format(Lang.Trans("{0}"), pendingInvitations); 
                        }
                        else
                        {
                            lblPendingInvitatinos.Text = Lang.Trans("pending invitations");
                        }
                    }
                }
            }

            #endregion

            #region Show group topic subscriptions

            if (Config.Groups.EnableGroups)
            {
                DataTable dtGroupTopicSubscriptions = new DataTable("GroupTopicSubscriptions");

                dtGroupTopicSubscriptions.Columns.Add("GroupTopicID");
                dtGroupTopicSubscriptions.Columns.Add("GroupTopicName");
                dtGroupTopicSubscriptions.Columns.Add("GroupID");
                dtGroupTopicSubscriptions.Columns.Add("GroupName");

                GroupTopic[] groupTopics =
                    GroupTopic.FetchUpdatedGroupTopicsUserHasSubscribedTo(CurrentUserSession.Username);

                foreach (GroupTopic groupTopic in groupTopics)
                {
                    Group group = Group.Fetch(groupTopic.GroupID);

                    if (group != null)
                    {
                        dtGroupTopicSubscriptions.Rows.Add(new object[]
                                                           {
                                                               groupTopic.ID, groupTopic.Name, group.ID, group.Name
                                                           });
                    }
                }

                rptGroupTopicSubscriptions.DataSource = dtGroupTopicSubscriptions;
                rptGroupTopicSubscriptions.DataBind();
                rptGroupTopicSubscriptions.Visible = dtGroupTopicSubscriptions.Rows.Count > 0;
            }

            #endregion

            #region Show Status text

            if (Config.Users.EnableUserStatusText)
            {
                pnlStatusText.Visible = !Config.Misc.SiteIsPaid || Classes.User.IsPaidMember(CurrentUserSession.Username);
                lblStatusText.Text = Server.HtmlEncode(CurrentUserSession.StatusText) ?? "Not set".Translate();    
            }

            #endregion

            #region Notify Facebook for the changed status

            long? facebookID = CurrentUserSession.FacebookID;
            if (Config.Misc.EnableFacebookIntegration && facebookID.HasValue && facebookID > 0)
            {
                pnlInviteFriendsFromFacebook.Visible = true;
            }

            #endregion
        }

        protected void lnkNewUsers_Click(object sender, EventArgs e)
        {
            Session["NewUsersSearch"] = true;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkUsersOnline_Click(object sender, EventArgs e)
        {
            Session["OnlineUsersSearch"] = true;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkViewProfileViewers_Click(object sender, EventArgs e)
        {
            Session["ShowProfileViewers"] = true;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkViewMutualVotes_Click(object sender, EventArgs e)
        {
            Session["MutualVoteSearch"] = true;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkNewMessages_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx");
        }

        protected void lnkEcards_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx?sel=recec");
        }

        protected void lnkSentEcards_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx?sel=sentec");
        }

        protected void lnkPendingInvitations_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=pi");
        }

        protected void lnkncommingMessagesRestrictions_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Profile.aspx?sel=set");
        }

        protected void lnkCatalogForLeftParts_Click(object sender, EventArgs e)
        {
            pnlCatalog.Visible = true;
            BindLeftCatalog();
            lnkCatalogForLeftParts.Visible = false;
            lnkCatalogForRightParts.Visible = false;
        }

        protected void lnkCatalogForRightParts_Click(object sender, EventArgs e)
        {
            pnlCatalog.Visible = true;
            BindRightCatalog();
            lnkCatalogForLeftParts.Visible = false;
            lnkCatalogForRightParts.Visible = false;
        }

        private void BindRightCatalog()
        {
            WebPartInfo[] infos = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageRightZone);

            rptCatalogWebParts.DataSource = infos;
            rptCatalogWebParts.DataBind();

            ZoneToAddPartsTo = WebPartZone.HomePageRightZone;
        }

        private void BindLeftCatalog()
        {
            WebPartInfo[] infos = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageLeftZone);

            rptCatalogWebParts.DataSource = infos;
            rptCatalogWebParts.DataBind();

            ZoneToAddPartsTo = WebPartZone.HomePageLeftZone;
        }

        protected void rptCatalogWebParts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                WebPartInfo info = Config.WebParts.AllParts.FirstOrDefault(i => i.ControlPath == (string)e.CommandArgument);
                if (info != null)
                {
                    if (ZoneToAddPartsTo == WebPartZone.HomePageRightZone)
                        WebPartManager1.AddWebPartUserControl(info, wpzHomePageRightZone,
                                                              wpzHomePageRightZone.WebParts.Count);
                    else if (ZoneToAddPartsTo == WebPartZone.HomePageLeftZone)
                        WebPartManager1.AddWebPartUserControl(info, wpzHomePageLeftZone,
                                                              wpzHomePageLeftZone.WebParts.Count);

                    pnlCatalog.Visible = false;
                    lnkCatalogForLeftParts.Visible = true;
                    lnkCatalogForRightParts.Visible = true;
                }
            }
        }

        protected void btnCancelCatalog_Click(object sender, EventArgs e)
        {
            pnlCatalog.Visible = false;
            lnkCatalogForLeftParts.Visible = true;
            lnkCatalogForRightParts.Visible = true;
        }

        protected void imgbCloseCatalog_Click(object sender, EventArgs e)
        {
            pnlCatalog.Visible = false;
            lnkCatalogForLeftParts.Visible = true;
            lnkCatalogForRightParts.Visible = true;
        }

        protected void lnkUsersBroadcasting_Click(object sender, EventArgs e)
        {
            Session["BroadcastingUsersSearch"] = true;

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("~/Search.aspx");
            else
                Response.Redirect("~/Search2.aspx");
        }

        protected void lnkEditStatusText_Click(object sender, EventArgs e)
        {
            txtStatusText.Text = CurrentUserSession.StatusText ?? String.Empty;
            pnlEditStatusText.Visible = true;
            pnlViewStatusText.Visible = false;
        }

        private string _consumerKey = String.Empty;
        protected string FacebookAPIKey
        {
            get
            {
                if (_consumerKey.Length == 0)
                {
                    _consumerKey = Properties.Settings.Default.Facebook_API_Key;
                }
                return _consumerKey;
            }
            set { _consumerKey = value; }
        }

        protected void lnkUpdateStatusText_Click(object sender, EventArgs e)
        {
            string status = String.Empty;

            status = txtStatusText.Text.Trim();

            if (status.Length > 0)
            {
                lblStatusText.Text = Server.HtmlEncode(status);
                CurrentUserSession.StatusText = status;
                CurrentUserSession.Update();

                #region Add FriendUpdatedStatus Event & realtime notifications

                Event newEvent = new Event(CurrentUserSession.Username) { Type = Event.eType.FriendUpdatedStatus };

                var friendUpdatedStatus = new FriendUpdatedStatus { Status = status };
                newEvent.DetailsXML = Misc.ToXml(friendUpdatedStatus);

                newEvent.Save();

                string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification &&
                        (Classes.User.IsOnline(friendUsername) || Classes.User.IsUsingNotifier(friendUsername)))
                    {
                        var text = String.Format("Your friend {0} has changed their status to \"{1}\"".Translate(),
                                                 "<b>" + CurrentUserSession.Username + "</b>", status);
                        var imageID = 0;
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
                                                   Recipient = friendUsername,
                                                   Sender = CurrentUserSession.Username,
                                                   Text = text,
                                                   ThumbnailUrl = thumbnailUrl,
                                                   RedirectUrl = UrlRewrite.CreateShowUserUrl(CurrentUserSession.Username)
                                               };
                        RealtimeNotification.SendNotification(notification);
                    }
                }

                #endregion

                // Update Twitter status
                if (Config.Misc.EnableTwitterIntegration && Twitter.HasCredentials(CurrentUserSession.Username))
                {
                    try
                    {
                        Twitter.PublishTweet(CurrentUserSession.Username, status);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError("Twitter", err);
                    }
                }

                // Update Facebook status
                if (Config.Misc.EnableFacebookIntegration && Facebook.HasCredentials(CurrentUserSession.Username))
                {
                    try
                    {
                        Facebook.PublishStatus(CurrentUserSession.Username, CurrentUserSession.FacebookID.Value, status);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError("Facebook", err);
                    }
                }
            }
            else
            {
                lblStatusText.Text = "Not set".Translate();
                CurrentUserSession.StatusText = null;
                CurrentUserSession.Update();
            }

            pnlEditStatusText.Visible = false;
            pnlViewStatusText.Visible = true;
        }

        protected void lnkNewEcards_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx?sel=recec");
        }

        protected void lnkFriendsRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Friends.aspx");
        }

        protected void lnkRelationshipRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Friends.aspx");
        }
    }
}