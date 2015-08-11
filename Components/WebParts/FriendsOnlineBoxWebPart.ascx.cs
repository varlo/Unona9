using System;
using System.Data;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    [Editable]
    public partial class FriendsOnlineBoxWebPart : WebPartUserControl
    {
        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                loadFriends();
            }
        }

        private void loadFriends()
        {
            DataTable dtFriends = new DataTable("Friends");

            dtFriends.Columns.Add("Username");
            dtFriends.Columns.Add("StatusText");

            string[] friendsUsernames = User.FetchMutuallyFriends(CurrentUserSession.Username);
            string statusText = String.Empty;

            foreach (string username in friendsUsernames)
            {
                try
                {
                    User user = User.Load(username);

                    if (user.IsOnline())
                    {
                        statusText = Server.HtmlEncode(user.StatusText);
                        dtFriends.Rows.Add(new object[] {username, statusText});
                    }
                }
                catch (NotFoundException)
                {
                    continue;
                }
            }

            if (dtFriends.Rows.Count > 0)
            {
                rptFriends.DataSource = dtFriends;
                rptFriends.DataBind();
                mvFriends.SetActiveView(vFriends);
            }
            else
            {
                mvFriends.SetActiveView(vNoFriends);
            }
        }
    }
}