using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using Config=AspNetDating.Classes.Config;
using Event=AspNetDating.Classes.Event;
using Lang=AspNetDating.Classes.Lang;
using Misc=AspNetDating.Classes.Misc;
using NewFriendFriend=AspNetDating.Classes.NewFriendFriend;

namespace AspNetDating
{
    public partial class AddRemoveFriend : PageBase
    {
        private string userID;
        private string source;
        private string command;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Users.EnableFriends)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (!ValidateParameters())
                    return;

                source = Request.Params["src"];
                command = Request.Params["cmd"];
                userID = Request.Params["uid"];

                LoadStrings();
                processCommand();
            }
            source = Request.Params["src"];
            command = Request.Params["cmd"];
            userID = Request.Params["uid"];
        }

        private bool ValidateParameters()
        {
            if (Request.Params["src"] == null ||
                Request.Params["uid"] == null ||
                Request.Params["cmd"] == null)
                return false;

            string uid = Request.Params["uid"];
            string cmd = Request.Params["cmd"];
            string src = Request.Params["src"];

            if (src != "profile" && src != "friends")
                return false;
            if (cmd != "add" && cmd != "remove")
                return false;
            if (src == "friends" && cmd != "remove")
                return false;

            return true;
        }

        private void LoadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Message");

            lnkFriends.Text = Lang.Trans("Go to Friends");
            if (source == "profile")
            {
                lnkBack.Text = Lang.Trans("Back to Profile");
                lnkBack.CommandArgument = "profile";
                pnlGoToFriends.Visible = true;
            }
            else if (source == "friends")
            {
                lnkBack.Text = Lang.Trans("Back to Friends");
                lnkBack.CommandArgument = "friends";
                pnlGoToFriends.Visible = false;
            }
        }

        private void processCommand()
        {
            if (command == "add")
            {
                User.eAddFriendResult result =
                    CurrentUserSession.AddToFriends(userID);
                switch (result)
                {
                    case Classes.User.eAddFriendResult.eAlreadyAdded:
                        lblMessage.CssClass = "alert text-danger";
                        lblMessage.Text = Lang.Trans("The user you selected already exists in your friend list");
                        break;
                    case Classes.User.eAddFriendResult.eInvalidUsername:
                        lblMessage.CssClass = "alert text-danger";
                        lblMessage.Text = Lang.Trans("No such user!");
                        break;
                    case Classes.User.eAddFriendResult.eMaximumFriendsReached:
                        lblMessage.CssClass = "alert text-danger";
                        lblMessage.Text =
                            String.Format(
                                Lang.Trans(
                                    "The maximum friends of {0} is reached! Please remove someone and try again!"),
                                Config.Users.MaxFriendUsers);
                        break;
                    case Classes.User.eAddFriendResult.eSuccess:
                        lblMessage.CssClass = "alert text-info";
                        lblMessage.Text = String.Format(Lang.Trans("{0} has been added to your friend list"), userID);

                        if (Classes.User.IsUserInFriendList(userID, CurrentUserSession.Username))
                        {
                            AddNewFriendFriendEvent(userID, CurrentUserSession.Username);
                            AddNewFriendFriendEvent(CurrentUserSession.Username, userID);
                        }

                        if (Config.Users.NewEventNotification)
                        {
                            int imageID = 0;
                            User user = null;
                            try
                            {
                                user = Classes.User.Load(userID);
                            }
                            catch (NotFoundException) { break; }

                            try
                            {
                                imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                            }
                            string text = String.Format("User {0} added you to friends".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>");

                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, user.Username, text,
                                                                     thumbnailUrl,
                                                                     UrlRewrite.CreateShowUserUrl(
                                                                         CurrentUserSession.Username));
                        }

                        break;
                }
            }
            else if (command == "remove")
            {
                if (CurrentUserSession.IsUserInFriendList(userID))
                {
                    CurrentUserSession.RemoveFromFriends(userID);

                    if (Classes.User.IsUserInFriendList(userID, CurrentUserSession.Username))
                    {
                        User user = null;

                        try
                        {
                            user = Classes.User.Load(userID);
                            user.RemoveFromFriends(CurrentUserSession.Username);
                        }
                        catch (NotFoundException)
                        {
                        }

                        AddRemovedFriendFriendEvent(userID, CurrentUserSession.Username);
                        AddRemovedFriendFriendEvent(CurrentUserSession.Username, userID);
                    }

                    lblMessage.CssClass = "alert text-info";
                    lblMessage.Text = String.Format(Lang.Trans("{0} has been removed from your friend list"), userID);
                }
                else
                {
                    lblMessage.CssClass = "alert text-danger";
                    lblMessage.Text = Lang.Trans("No such user!");
                }
            }
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

        private void AddRemovedFriendFriendEvent(string username, string favoriteUsername)
        {
            Event newEvent = new Event(username);

            newEvent.Type = Event.eType.RemovedFriendFriend;
            RemovedFriendFriend newFriendFriend = new RemovedFriendFriend();
            newFriendFriend.Username = favoriteUsername;
            newEvent.DetailsXML = Misc.ToXml(newFriendFriend);

            newEvent.Save();

            if (Config.Users.NewEventNotification)
            {
                string[] usernames = Classes.User.FetchMutuallyFriends(username);

                string text = String.Format("{0} {1} are no longer friends".Translate(),
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

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (lnkBack.CommandArgument == "profile")
                Response.Redirect(UrlRewrite.CreateShowUserUrl(userID));
            else if (lnkBack.CommandArgument == "friends")
                Response.Redirect("~/Friends.aspx");
        }

        protected void lnkFriends_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Friends.aspx");
        }
    }
}
