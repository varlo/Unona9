﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using Lang=AspNetDating.Classes.Lang;

namespace AspNetDating
{
    public partial class Friends : PageBase
    {
        #region properties

        protected bool showSlogan = false;

        public bool ShowSlogan
        {
            set { showSlogan = value; }
        }

        protected bool showLastOnline = true;

        public bool ShowLastOnline
        {
            set { showLastOnline = value; }
        }

        protected bool showRating = false;

        public bool ShowRating
        {
            set { showRating = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Users.EnableFriends)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (!Config.Users.EnableRelationshipStatus) pnlRelationshipRequests.Visible = false;
                else LoadRelationshipRequests();

                LoadStrings();
                LoadFriends();
                LoadFriendsRequests();
            }
        }

        private void LoadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Friends");
            LargeBoxStart2.Title = Lang.Trans("Friends Requests");
            LargeBoxStart3.Title = Lang.Trans("Relationship Requests");
        }

        private void LoadFriends()
        {
            DataTable dtFriends = new DataTable();

            dtFriends.Columns.Add("Username");
            dtFriends.Columns.Add("PhotoId", typeof(int));
            dtFriends.Columns.Add("Slogan");
            dtFriends.Columns.Add("Age");
            dtFriends.Columns.Add("LastOnlineString");
            dtFriends.Columns.Add("Rating");
            dtFriends.Columns.Add("AddedToFriendsOn");

            Classes.User[] users = CurrentUserSession.FetchFriends();

            if (users.Length > 0)
            {
                foreach (User user in users)
                {
                    Photo primaryPhoto = null;
                    try
                    {
                        primaryPhoto = user.GetPrimaryPhoto();
                    }
                    catch (NotFoundException)
                    {
                    }
                    string slogan = "";
                    try
                    {
                        ProfileAnswer sloganAnswer = user.FetchSlogan();
                        if (sloganAnswer.Approved)
                            slogan = sloganAnswer.Value;
                        else
                            slogan = Lang.Trans("-- pending approval --");
                    }
                    catch (NotFoundException)
                    {
                    }

                    string ratingString = "";
                    if (showRating)
                    {
                        try
                        {
                            UserRating userRating = UserRating.FetchRating(user.Username);
                            ratingString = String.Format(
                                Lang.Trans("{0} ({1} votes)"),
                                userRating.AverageVote.ToString("0.00"), userRating.Votes);
                        }
                        catch (NotFoundException)
                        {
                            ratingString = Lang.Trans("no rating");
                        }
                    }

                    DateTime date =
                        CurrentUserSession.FetchFriendTimeStamp(user.Username);

                    #region Check user.Gender and set photoId

                    int photoId = 0;

                    if (primaryPhoto == null || !primaryPhoto.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = primaryPhoto.Id;
                    }

                    #endregion

                    dtFriends.Rows.Add(new object[]
                                              {
                                                  user.Username,
                                                  photoId,
                                                  slogan,
                                                  user.Age,
                                                  user.LastOnlineString,
                                                  ratingString,
                                                  date.ToShortDateString()
                                              });
                }
            }
            else
            {
                lblMessage.Text = Lang.Trans("No friends found!");
            }

            dlFriends.DataSource = dtFriends;
            dlFriends.DataBind();
        }

        protected void dlFriends_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            string username = (string)DataBinder.Eval(e.Item.DataItem, "Username");

            HyperLink lnkSendMessage = (HyperLink)e.Item.FindControl("lnkSendMessage");
            lnkSendMessage.NavigateUrl =
                String.Format("~/SendMessage.aspx?to_user={0}&src=friends", username);

            HyperLink lnkRemoveFromFavourites = (HyperLink)e.Item.FindControl("lnkRemoveFromFriends");
            lnkRemoveFromFavourites.NavigateUrl =
                String.Format("AddRemoveFriend.aspx?uid={0}&cmd=remove&src=friends", username);
        }

        protected void dlFriends_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var lnkSendMessage = (HyperLink)e.Item.FindControl("lnkSendMessage");
            var lnkRemoveFromFriends = (HyperLink)e.Item.FindControl("lnkRemoveFromFriends");
            var pnlAge = (HtmlGenericControl)e.Item.FindControl("pnlAge");
            var pnlAgeValue = (HtmlGenericControl)e.Item.FindControl("pnlAgeValue");

            lnkRemoveFromFriends.Attributes.Add("title", Lang.Trans("Remove from Friends"));
            lnkSendMessage.Attributes.Add("title", Lang.Trans("Send Message"));

            pnlAge.Visible = !Config.Users.DisableAgeInformation;
            pnlAgeValue.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void dlPendingFriendsRequests_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var lnkAccept = (LinkButton)e.Item.FindControl("lnkAccept");
            var lnkReject = (LinkButton)e.Item.FindControl("lnkReject");
            var pnlAge = (HtmlGenericControl)e.Item.FindControl("pnlAge");
            var pnlAgeValue = (HtmlGenericControl)e.Item.FindControl("pnlAgeValue");

            lnkAccept.Attributes.Add("title", Lang.Trans("Accept"));
            lnkReject.Attributes.Add("title", Lang.Trans("Reject"));
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
            pnlAgeValue.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void dlPendingFriendsRequests_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Accept")
            {
                User.eAddFriendResult friendResult =
                    CurrentUserSession.AddToFriends((string) e.CommandArgument);

                if (friendResult == Classes.User.eAddFriendResult.eSuccess)
                {
                    #region Add NewFriendFriend Event

                    AddNewFriendFriendEvent(CurrentUserSession.Username, (string) e.CommandArgument);
                    AddNewFriendFriendEvent((string) e.CommandArgument, CurrentUserSession.Username);

                    #endregion
                }
                else
                {
                    switch (friendResult)
                    {
                        case Classes.User.eAddFriendResult.eInvalidUsername:
                            ((PageBase)Page).StatusPageMessage = String.Format(Lang.Trans("User {0} does not exists!"), e.CommandArgument);
                            break;
                        case Classes.User.eAddFriendResult.eAlreadyAdded:
                            ((PageBase)Page).StatusPageMessage = String.Format(Lang.Trans("User {0} is already your friend!"), e.CommandArgument);
                            break;
                        case Classes.User.eAddFriendResult.eMaximumFriendsReached:
                            ((PageBase)Page).StatusPageMessage = Lang.Trans("You have reached the maximum number of friends!");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Response.Redirect("ShowStatus.aspx");
                }
            }
            else if (e.CommandName == "Reject")
            {
                User user = null;
                try
                {
                    user = Classes.User.Load((string) e.CommandArgument);
                    user.RemoveFromFriends(CurrentUserSession.Username);
                }
                catch (NotFoundException)
                {
                    return;
                }
            }

            LoadFriends();
            LoadFriendsRequests();
        }

        private void LoadFriendsRequests()
        {
            DataTable dtFriends = new DataTable();

            dtFriends.Columns.Add("Username");
            dtFriends.Columns.Add("PhotoId", typeof(int));
            dtFriends.Columns.Add("Slogan");
            dtFriends.Columns.Add("Age");
            dtFriends.Columns.Add("LastOnlineString");
            dtFriends.Columns.Add("Rating");
            dtFriends.Columns.Add("AddedToFriendsOn");

            string[] friends = Classes.User.FetchFriendsRequests(CurrentUserSession.Username);

            if (friends.Length > 0)
            {
                foreach (string friend in friends)
                {
                    User user = null;
                    try
                    {
                        user = Classes.User.Load(friend);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }
                    Photo primaryPhoto = null;
                    try
                    {
                        primaryPhoto = user.GetPrimaryPhoto();
                    }
                    catch (NotFoundException)
                    {
                    }
                    string slogan = "";
                    try
                    {
                        ProfileAnswer sloganAnswer = user.FetchSlogan();
                        if (sloganAnswer.Approved)
                            slogan = sloganAnswer.Value;
                        else
                            slogan = Lang.Trans("-- pending approval --");
                    }
                    catch (NotFoundException)
                    {
                    }

                    string ratingString = "";
                    if (showRating)
                    {
                        try
                        {
                            UserRating userRating = UserRating.FetchRating(user.Username);
                            ratingString = String.Format(
                                Lang.Trans("{0} ({1} votes)"),
                                userRating.AverageVote.ToString("0.00"), userRating.Votes);
                        }
                        catch (NotFoundException)
                        {
                            ratingString = Lang.Trans("no rating");
                        }
                    }

                    DateTime date =
                        CurrentUserSession.FetchFriendRequestTimeStamp(user.Username);

                    #region Check user.Gender and set photoId

                    int photoId = 0;

                    if (primaryPhoto == null || !primaryPhoto.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = primaryPhoto.Id;
                    }

                    #endregion

                    dtFriends.Rows.Add(new object[]
                                              {
                                                  user.Username,
                                                  photoId,
                                                  slogan,
                                                  user.Age,
                                                  user.LastOnlineString,
                                                  ratingString,
                                                  date.ToShortDateString()
                                              });
                }
            }

            dlPendingFriendsRequests.DataSource = dtFriends;
            dlPendingFriendsRequests.DataBind();
            pnlFriendsRequests.Visible = dtFriends.Rows.Count > 0;
        }

        private void LoadRelationshipRequests()
        {
            DataTable requests = new DataTable();

            requests.Columns.Add("Username");
            requests.Columns.Add("PhotoId", typeof(int));
            requests.Columns.Add("Slogan");
            requests.Columns.Add("Age");
            requests.Columns.Add("LastOnlineString");
            requests.Columns.Add("Rating");
            requests.Columns.Add("Type");

            string[] users = Relationship.FetchRequests(CurrentUserSession.Username);

            if (users.Length > 0)
            {
                foreach (string u in users)
                {
                    User user = null;
                    try
                    {
                        user = Classes.User.Load(u);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }
                    Photo primaryPhoto = null;
                    try
                    {
                        primaryPhoto = user.GetPrimaryPhoto();
                    }
                    catch (NotFoundException)
                    {
                    }
                    string slogan = "";
                    try
                    {
                        ProfileAnswer sloganAnswer = user.FetchSlogan();
                        if (sloganAnswer.Approved)
                            slogan = sloganAnswer.Value;
                        else
                            slogan = Lang.Trans("-- pending approval --");
                    }
                    catch (NotFoundException)
                    {
                    }

                    string ratingString = "";
                    if (showRating)
                    {
                        try
                        {
                            UserRating userRating = UserRating.FetchRating(user.Username);
                            ratingString = String.Format(
                                Lang.Trans("{0} ({1} votes)"),
                                userRating.AverageVote.ToString("0.00"), userRating.Votes);
                        }
                        catch (NotFoundException)
                        {
                            ratingString = Lang.Trans("no rating");
                        }
                    }

                    #region Check user.Gender and set photoId

                    int photoId = 0;

                    if (primaryPhoto == null || !primaryPhoto.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = primaryPhoto.Id;
                    }

                    #endregion

                    string relationshipType = String.Empty;

                    var relationship = Relationship.Fetch(user.Username, CurrentUserSession.Username);

                    if (relationship != null)
                    {
                        relationshipType = relationship.PendingType.HasValue
                                               ? Relationship.GetRelationshipStatusString(relationship.PendingType.Value)
                                               : Relationship.GetRelationshipStatusString(relationship.Type);
                    }

                    requests.Rows.Add(new object[]
                                              {
                                                  user.Username,
                                                  photoId,
                                                  slogan,
                                                  user.Age,
                                                  user.LastOnlineString,
                                                  ratingString,
                                                  relationshipType
                                              });
                }
            }

            dlRelationshipRequests.DataSource = requests;
            dlRelationshipRequests.DataBind();
            pnlRelationshipRequests.Visible = requests.Rows.Count > 0;
        }

        private void AddNewFriendFriendEvent(string username, string favoriteUsername)
        {
            Event newEvent = new Event(username);

            newEvent.Type = Event.eType.NewFriendFriend;
            NewFriendFriend newFriendFriend = new NewFriendFriend();
            newFriendFriend.Username = favoriteUsername;
            newEvent.DetailsXML = Misc.ToXml(newFriendFriend);

            newEvent.Save();

            if (Config.Users.NewEventNotification)
            {
                string[] usernames = Classes.User.FetchMutuallyFriends(username);

                string text = String.Format("{0} and {1} are now friends".Translate(),
                                              "<b>" + username + "</b>", favoriteUsername);
                int imageID = 0;
                try
                {
                    imageID = Photo.GetPrimary(username).Id;
                }
                catch (NotFoundException)
                {
                    User user = null;
                    try
                    {
                        user = Classes.User.Load(username);
                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    catch (NotFoundException) { return; }
                }
                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);

                foreach (string friendUsername in usernames)
                {
                    if (favoriteUsername == friendUsername) continue;

                    Classes.User.SendOnlineEventNotification(username, friendUsername, text, thumbnailUrl,
                                                         UrlRewrite.CreateShowUserUrl(favoriteUsername));
                }
            }
        }

        private void AddNewRelationshipEvent(string fromUsername, string toUsername, Relationship.eRelationshipStatus type)
        {
            Event newEvent = new Event(fromUsername);

            newEvent.Type = Event.eType.NewFriendRelationship;
            NewFriendRelationship newRelationship = new NewFriendRelationship();
            newRelationship.Username = toUsername;
            newRelationship.Type = type;
            newEvent.DetailsXML = Misc.ToXml(newRelationship);

            newEvent.Save();

            if (Config.Users.NewEventNotification)
            {
                string[] usernames = Classes.User.FetchMutuallyFriends(fromUsername);

                string text = String.Format("{0} and {1} are now in relationship ({2})".Translate(),
                                            "<b>" + fromUsername + "</b>", toUsername,
                                            Relationship.GetRelationshipStatusString(type));
                int imageID = 0;
                try
                {
                    imageID = Photo.GetPrimary(fromUsername).Id;
                }
                catch (NotFoundException)
                {
                    User user = null;
                    try
                    {
                        user = Classes.User.Load(fromUsername);
                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    catch (NotFoundException) { return; }
                }
                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);

                foreach (string friendUsername in usernames)
                {
                    if (toUsername == friendUsername) continue;

                    Classes.User.SendOnlineEventNotification(fromUsername, friendUsername, text, thumbnailUrl,
                                                         UrlRewrite.CreateShowUserUrl(toUsername));
                }
            }
        }

        protected void dlRelationshipRequests_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkAccept = (LinkButton)e.Item.FindControl("lnkAccept");
            LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");
            HtmlGenericControl pnlAge = (HtmlGenericControl) e.Item.FindControl("pnlAge");
            HtmlGenericControl pnlAgeValue = (HtmlGenericControl)e.Item.FindControl("pnlAgeValue");

            lnkAccept.Text = Lang.Trans("Accept");
            lnkReject.Text = Lang.Trans("Reject");
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
            pnlAgeValue.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void dlRelationshipRequests_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string username = (string) e.CommandArgument;
            var requested = Relationship.Fetch(username, CurrentUserSession.Username);

            if (requested != null)
            {
                if (e.CommandName == "Accept")
                {
                    if (!requested.PendingType.HasValue)
                    {
                        Relationship.Delete(CurrentUserSession.Username, null, true);
                        Relationship.Delete(null, CurrentUserSession.Username, true);
                    }

                    var existed = Relationship.Fetch(CurrentUserSession.Username, username) ??
                                  new Relationship(CurrentUserSession.Username, username);

                    if (requested.PendingType.HasValue) requested.Type = requested.PendingType.Value;

                    existed.Type = requested.Type;
                    requested.Accepted = true;
                    existed.Accepted = true;
                    requested.PendingType = null;
                    existed.PendingType = null;
                    requested.Save();
                    existed.Save();

                    Relationship.Delete(null, CurrentUserSession.Username, false);

                    #region Add NewRelationship Event

                    AddNewRelationshipEvent(CurrentUserSession.Username, username, requested.Type);
                    AddNewRelationshipEvent(username, CurrentUserSession.Username, requested.Type);

                    #endregion
                }
                else if (e.CommandName == "Reject")
                {
                    if (requested.PendingType.HasValue)
                    {
                        requested.PendingType = null;
                        requested.Save();
                    }
                    else
                        Relationship.Delete(username, CurrentUserSession.Username, null);
                }
            }

            LoadRelationshipRequests();
        }
    }
}
