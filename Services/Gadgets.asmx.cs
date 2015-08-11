using System;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using AspNetDating.Classes;

namespace AspNetDating.Services
{
    /// <summary>
    /// This class handles the vista gadgets
    /// </summary>
    [WebService(Namespace = "AspNetDating.Service")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Gadgets : WebService
    {
        /// <summary>
        /// Gets the online users count.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int GetOnlineUsersCount()
        {
            OnlineSearch oSearch = new OnlineSearch();
            UserSearchResults oResults = oSearch.GetResults();
            if (oResults != null && oResults.Usernames != null)
                return oResults.Usernames.Length;
            else
                return 0;
        }

        /// <summary>
        /// Gets the new messages count.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [WebMethod]
        public int GetNewMessagesCount(string username, string hash)
        {
            string encodedData = Convert.ToBase64String(Encoding.ASCII.GetBytes(username));
            string hash2 = Misc.HMACSHA1ToHex(encodedData, Properties.Settings.Default.SecretGadgetKey);

            if (hash != hash2) return 0;
            return Message.SearchUnread(username).Length;
        }

        /// <summary>
        /// Gets the new users count.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [WebMethod]
        public int GetNewUsersCount(string username, string hash)
        {
            string encodedData = Convert.ToBase64String(Encoding.ASCII.GetBytes(username));
            string hash2 = Misc.HMACSHA1ToHex(encodedData, Properties.Settings.Default.SecretGadgetKey);

            if (hash != hash2) return 0;
            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.UsersSince = Classes.User.Load(username).PrevLogin;
            UserSearchResults nuResults = nuSearch.GetResults(true);
            if (nuResults != null && nuResults.Usernames != null)
                return nuResults.Usernames.Length;
            else
                return 0;
        }

        /// <summary>
        /// Gets the new e-cards count.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [WebMethod]
        public int GetNewEcardsCount(string username, string hash)
        {
            string encodedData = Convert.ToBase64String(Encoding.ASCII.GetBytes(username));
            string hash2 = Misc.HMACSHA1ToHex(encodedData, Properties.Settings.Default.SecretGadgetKey);

            if (hash != hash2) return 0;
            return Ecard.FetchUnread(username).Length;
        }

        /// <summary>
        /// Gets the new profile views count.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [WebMethod]
        public int GetNewProfileViewsCount(string username, string hash)
        {
            string encodedData = Convert.ToBase64String(Encoding.ASCII.GetBytes(username));
            string hash2 = Misc.HMACSHA1ToHex(encodedData, Properties.Settings.Default.SecretGadgetKey);

            if (hash != hash2) return 0;
            return Classes.User.Load(username).ProfileViews;
        }

        /// <summary>
        /// The struct contains information about a new user
        /// </summary>
        public struct NewUserInfo
        {
            /// <summary>
            /// The username
            /// </summary>
            public string username;
            /// <summary>
            /// The profile URL
            /// </summary>
            public string profileUrl;
            /// <summary>
            /// The primary image URL
            /// </summary>
            public string imageUrl;
        }

        /// <summary>
        /// Gets the random new user.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns></returns>
        [WebMethod]
        public NewUserInfo GetRandomNewUser(string gender)
        {
            NewUserInfo info = new NewUserInfo();

            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.PhotoReq = true;
            nuSearch.UsersCount = 10;
            if (!string.IsNullOrEmpty(gender))
            {
                try
                {
                    nuSearch.Gender = (User.eGender)
                        Enum.Parse(typeof(User.eGender), gender);
                }
                catch (ArgumentException)
                {
                }
            }
            UserSearchResults nuResults = nuSearch.GetResults();

            if (nuResults != null && nuResults.Usernames != null)
            {
                Random rand = new Random();
                info.username = nuResults.Usernames[rand.Next(nuResults.Usernames.Length)];
                info.profileUrl = UrlRewrite.CreateShowUserUrl(info.username);
                info.imageUrl = ImageHandler.CreateImageUrl(Photo.GetPrimary(info.username).Id, 120, 120,
                                                            true, false, false);
            }

            return info;
        }

        /// <summary>
        /// Gets the pending photos count.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int GetPendingPhotosCount()
        {
            Photo[] photos = null;
            if (Config.CommunityFaceControlSystem.EnableCommunityFaceControl)
                photos = Photo.FetchNonApproved(true);
            else photos = Photo.FetchNonApproved();
            if (photos == null)
                return 0;
            else return photos.Length;
        }

        /// <summary>
        /// Gets the pending answers count.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int GetPendingAnswersCount()
        {
            return ProfileAnswer.FetchNonApproved().Length;
        }

        /// <summary>
        /// Gets the new users for the last 24 hours.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int GetNewUsersForTheLast24hoursCount()
        {
            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.ProfileReq = false;
            nuSearch.UsersSince = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
            UserSearchResults nuResults = nuSearch.GetResults();

            if (nuResults == null)
                return 0;
            else return nuResults.Usernames.Length;
        }
    }
}