using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    public partial class ViewEvent : UserControl
    {
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int) ViewState["CurrentGroupId"];
                }
                throw new Exception("The field groupID is not set!");
            }
            set { ViewState["CurrentGroupId"] = value; }
        }

        public int EventID
        {
            get
            {
                if (ViewState["CurrentEventID"] != null)
                {
                    return (int) ViewState["CurrentEventID"];
                }
                return -1;
            }
            set { ViewState["CurrentEventID"] = value; }
        }

        protected string CreatedBy
        {
            get { return (string) ViewState["CreatedBy"]; }
            set { ViewState["CreatedBy"] = value; }
        }

        public GroupEvent CurrentEvent
        {
            get
            {
                int eventID;
                if (Int32.TryParse((Request.Params["eid"]), out eventID) && ViewState["CurrentEvent"] == null)
                {
                    ViewState["CurrentEvent"] = GroupEvent.Fetch(eventID);
                }

                return ViewState["CurrentEvent"] as GroupEvent;
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadEvent();
                loadComments();

                if (CurrentUserSession != null) loadAttenders(true);
                loadAttenders(false);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (CurrentUserSession != null)
            {
                bool isAttender = GroupEvent.IsAttender(EventID, CurrentUserSession.Username);
                btnJoinEvent.Visible = !isAttender && CurrentEvent.Date >= DateTime.Now;
                btnLeaveEvent.Visible = isAttender && CurrentEvent.Date >= DateTime.Now ? true : false;
                if (CurrentEvent.Date >= DateTime.Now)
                    lblAttending.Text = isAttender
                                            ? Lang.Trans("You�re attending this event.")
                                            : Lang.Trans("Are you in?");
            }
            else
            {
                btnJoinEvent.Visible = false;
                btnLeaveEvent.Visible = false;
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Group Event");
            btnSubmitNewComment.Text = Lang.Trans("Submit Comment");
            hlGroupEventsComments.Title = Lang.Trans("User Comments");
            hlUserFriends.Title = Lang.Trans("Attending Friends");
            hlAttenders.Title = Lang.Trans("Attenders");
            btnJoinEvent.Text = Lang.Trans("I'm attending");
            btnLeaveEvent.Text = Lang.Trans("I'm not going");

            divUserFriends.Visible = CurrentUserSession != null;
        }

        private void loadEvent()
        {
            GroupEvent groupEvent = GroupEvent.Fetch(EventID);

            if (groupEvent != null)
            {
                lblTitle.Text = groupEvent.Title;
                lblDescription.Text = groupEvent.Description;
                lblDate.Text = groupEvent.Date.ToString();
                CreatedBy = groupEvent.Username;
                lblLocation.Text = groupEvent.Location;

                if (Config.ThirdPartyServices.ShowGoogleMapsForGroupEvents && 
                    !String.IsNullOrEmpty(Config.ThirdPartyServices.GoogleMapsAPIKey))
                {
                    var coords = GoogleMaps.GetCoordinates(groupEvent.Location);
                    if (coords != null)
                    {
                        imgGoogleMap.ImageUrl = String.Format(
                            "http://maps.google.com/staticmap?center={0},{1}&zoom=14&size=400x400&key={2}",
                            coords[0].ToString(System.Globalization.CultureInfo.InvariantCulture),
                            coords[1].ToString(System.Globalization.CultureInfo.InvariantCulture), 
                            Config.ThirdPartyServices.GoogleMapsAPIKey);
                        imgGoogleMap.Visible = true;
                    }
                }
            }
        }

        private void loadComments()
        {
            var dtComments = new DataTable();
            dtComments.Columns.Add("ID", typeof (int));
            dtComments.Columns.Add("Date", typeof (DateTime));
            dtComments.Columns.Add("Username", typeof (string));
            dtComments.Columns.Add("Comment", typeof (string));
            dtComments.Columns.Add("CanDelete", typeof (bool));

            GroupEventsComment[] comments = GroupEventsComment.FetchByGroupEventID(EventID,
                                                                                   GroupEventsComment.eSortColumn.Date);

            if (CurrentUserSession != null)
                showHideComments();
            else
                spanAddNewComment.Visible = false;

            GroupEvent groupEvent = GroupEvent.Fetch(EventID);

            if (groupEvent != null)
            {
                int commentsFromCurrentUser = 0;

                foreach (GroupEventsComment comment in comments)
                {
                    bool canDelete = false;
                    if (CurrentUserSession != null)
                    {
                        if (comment.Username == CurrentUserSession.Username)
                        {
                            if (groupEvent.Username != CurrentUserSession.Username)
                            {
                                spanAddNewComment.Visible = false;
                            }

                            canDelete = true;
                            commentsFromCurrentUser++;
                        }

                        if (groupEvent.Username == CurrentUserSession.Username)
                        {
                            if (DateTime.Now.Subtract(comment.Date) < TimeSpan.FromSeconds(10) ||
                                commentsFromCurrentUser > 10)
                            {
                                spanAddNewComment.Visible = false;
                            }

                            canDelete = true;
                        }
                    }

                    dtComments.Rows.Add(new object[]
                                            {
                                                comment.Id, comment.Date, comment.Username,
                                                Server.HtmlEncode(comment.Comment), canDelete
                                            });
                }
            }

            rptComments.DataSource = dtComments;
            rptComments.DataBind();
        }

        private void loadAttenders(bool loadFriends)
        {
            var dtFriends = new DataTable();
            dtFriends.Columns.Add("Username", typeof (string));
            dtFriends.Columns.Add("ImageId", typeof (int));

            string[] usernames;

            if (loadFriends)
            {
                usernames = GroupEvent.GetFriendsAttenders(EventID, CurrentUserSession.Username);
                hlUserFriends.Title = String.Format(Lang.Trans("{0} of your friends attending"), usernames.Length);
            }
            else
            {
                usernames = GroupEvent.GetAttenders(EventID);
                hlAttenders.Title = String.Format(Lang.Trans("{0} people attending"), usernames.Length);
            }

            if (usernames.Length == 0)
            {
                if (loadFriends) divUserFriends.Visible = false;
                else
                {
                    divUserFriends.Visible = false;
                    pnlAttenders.Visible = false;
                }
                return;
            }
            foreach (string username in usernames)
            {
                int imageId;
                try
                {
                    imageId = Photo.GetPrimary(username).Id;
                }
                catch (NotFoundException)
                {
                    try
                    {
                        User user = User.Load(username);
                        imageId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }
                }

                dtFriends.Rows.Add(new object[] {username, imageId});
            }

            if (loadFriends)
            {
                dlUserFriends.DataSource = dtFriends;
                dlUserFriends.DataBind();
            }
            else
            {
                dlAttenders.DataSource = dtFriends;
                dlAttenders.DataBind();
            }
        }

        private void showHideComments()
        {
            spanAddNewComment.Visible = CanAddComments();
        }

        private bool CanAddComments()
        {
            if (ViewState["CanAddComments"] == null)
            {
                ViewState["CanAddComments"] = 
                    (CurrentUserSession.CanAddComments() == PermissionCheckResult.Yes ||
                        (CurrentUserSession.Level != null && 
                        CurrentUserSession.Level.Restrictions.UserCanAddComments)
                     ) && CurrentEvent != null && CurrentEvent.Date >= DateTime.Now;
            }

            return (bool) ViewState["CanAddComments"];
        }

        protected void btnSubmitNewComment_Click(object sender, EventArgs e)
        {
            if (txtNewComment.Text.Trim() == "")
            {
                return;
            }

            if (CurrentUserSession != null)
            {
                var comment = new GroupEventsComment(EventID, CurrentUserSession.Username)
                                  {
                                      Comment =
                                          (Config.Misc.EnableBadWordsFilterGroups
                                               ? Parsers.ProcessBadWords(txtNewComment.Text.Trim())
                                               : txtNewComment.Text.Trim())
                                  };

                comment.Save();

                loadComments();
            }
        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                GroupEventsComment comment = GroupEventsComment.Fetch(commentId);

                if (comment != null)
                {
                    GroupEvent groupEvent = GroupEvent.Fetch(EventID);
                    if (groupEvent != null && CurrentUserSession != null
                        && (comment.Username == CurrentUserSession.Username
                            || groupEvent.Username == CurrentUserSession.Username))
                    {
                        GroupEventsComment.Delete(commentId);

                        loadComments();
                    }
                }
            }
        }

        protected void btnJoinEvent_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                GroupEvent.SetAttender(EventID, CurrentUserSession.Username);

                var newEvent = new Event(CurrentUserSession.Username)
                {
                    Type = Event.eType.FriendAttendingEvent
                };

                var friendAttendingEvent = new FriendAttendingEvent();
                friendAttendingEvent.EventID = EventID;
                newEvent.DetailsXML = Misc.ToXml(friendAttendingEvent);

                newEvent.Save();

                string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        if (CurrentEvent != null)
                        {
                            Group group = Group.Fetch(CurrentEvent.GroupID);
                            if (group != null)
                            {
                                string text =
                                    String.Format(
                                        "Your friend {0} is attending the {1} event from the {2} group".Translate(),
                                        "<b>" + CurrentUserSession.Username + "</b>",
                                        Server.HtmlEncode(CurrentEvent.Title),
                                        Server.HtmlEncode(group.Name));
                                int imageID;
                                try
                                {
                                    imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                                }
                                catch (NotFoundException)
                                {
                                    imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                                }
                                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername, text,
                                                                 thumbnailUrl,
                                                                 UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString()));
                            }
                        }
                    }
                }

                loadAttenders(false);

                pnlAttenders.Visible = true;
            }
        }

        protected void btnLeaveEvent_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                GroupEvent.DeleteAttender(EventID, CurrentUserSession.Username);

                loadAttenders(false);
            }
        }

        protected void rptComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var lnkDeleteComment = (LinkButton) e.Item.FindControl("lnkDeleteComment");
            lnkDeleteComment.Attributes.Add("onclick",
                                            String.Format("javascript: return confirm('{0}')",
                                                          Lang.Trans("Do you really want to remove this comment?")));
        }
    }
}