using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetAjaxChat.Interfaces;
using AspNetDating.Classes;
using System.IO;
using User = AspNetAjaxChat.Interfaces.User;
using System.Collections.Specialized;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Diagnostics;

namespace AspNetDating.AjaxChatIntegration
{
    public class AspNetDatingChatUserProvider : IChatUserProvider
    {
        private static readonly Random Rand = new Random();

        ICacheProvider cacheProvider;

        public AspNetDatingChatUserProvider(ICacheProvider cacheProvider)
        {
            AspNetDating.Global.InitLogger();
            this.cacheProvider = cacheProvider;
        }

        private User GetChatUserFromDatingUser(string username)
        {
            Classes.User datingUser;
            try
            {
                datingUser = Classes.User.Load(username);
            }
            catch (NotFoundException)
            {
                throw new SecurityException(String.Format("User '{0}' no longer exist", username));
            }

            var chatUser = new User
            {
                DisplayName = datingUser.Username,
                Id = datingUser.Username,
            };

            #region Load Photo

            Photo primaryPhoto = null;
            try
            {
                primaryPhoto = datingUser.GetPrimaryPhoto();
            }
            catch (NotFoundException)
            {
            }
            catch (Exception)
            {
            }

            #region Check CurrentUserSession.Gender and set photoId

            int photoId;

            if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                photoId = ImageHandler.GetPhotoIdByGender(datingUser.Gender);
            }
            else
            {
                photoId = primaryPhoto.Id;
            }

            #endregion

            var baseUrl = ConfigurationManager.AppSettings["AspNetDatingHomeURL"];
            if (!baseUrl.EndsWith("/")) baseUrl += "/";
            chatUser.ThumbnailUrl = baseUrl + String.Format("Image.ashx?id={0}&width=50&height=50&diskCache=1&findFace=1", photoId);
            chatUser.PhotoUrl = baseUrl + String.Format("Image.ashx?id={0}&diskCache=1&findFace=1", photoId);
            chatUser.ProfileUrl = baseUrl + String.Format("ShowUser.aspx?uid={0}", username);

            #endregion

            return chatUser;
        }

        #region IChatUserProvider Members

        //public User GetCurrentlyLoggedUser()
        //{
        //    //var userSession = PageBase.GetCurrentUserSession();
        //    //return GetChatUserFromDatingUser(userSession);



        //    Global.CacheProvider.Set("RemoteAuthUserProvider_" + user.Id, user);


        //    var href = HttpContext.Current.Items["href"] as string;
        //    if (string.IsNullOrWhiteSpace(href)) return null;
        //    var hrefUri = new Uri(href);
        //    NameValueCollection hrefParams = HttpUtility.ParseQueryString(hrefUri.Query);
        //    if (hrefParams["timestamp"] != null)
        //    {
        //        #region Validate timestamp
        //        try
        //        {
        //            DateTime authDate = DateTime.FromFileTimeUtc(Convert.ToInt64(hrefParams["timestamp"]));
        //            if (DateTime.Now.Subtract(authDate) > TimeSpan.FromHours(24))
        //            {
        //                throw new SecurityException("Timestamp has expired!");
        //            }
        //        }
        //        catch (ArgumentOutOfRangeException)
        //        {
        //            throw new SecurityException("Invalid timestamp!");
        //        }

        //        var sha1 = new SHA1Managed();
        //        var paramBytes = Encoding.UTF8.GetBytes((hrefParams["id"] ?? String.Empty) +
        //            (hrefParams["name"] ?? String.Empty) + (hrefParams["thumbUrl"] ?? String.Empty) +
        //            hrefParams["timestamp"] + Settings.Default.SharedSecret);
        //        var hashBytes = sha1.ComputeHash(paramBytes);
        //        var calculatedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        //        if (hrefParams["hash"] != calculatedHash)
        //        {
        //            throw new SecurityException("Hash is invalid!");
        //        }
        //        #endregion

        //        // Create user object
        //        var user = new User { DisplayName = hrefParams["name"] ?? hrefParams["id"] ?? "guest_" + Rand.Next(1000) };
        //        user.Id = hrefParams["id"] ?? Regex.Replace(user.DisplayName, "[\\{\\}\\'\\\"]", String.Empty);
        //        user.ThumbnailUrl =
        //            hrefParams["thumbUrl"] ??
        //            (string.Format("http://www.gravatar.com/avatar/{0}.jpg?s=30&d=monsterid",
        //                           FormsAuthentication.HashPasswordForStoringInConfigFile(user.Id, "md5")).ToLower());
        //        Global.CacheProvider.Set("RemoteAuthUserProvider_" + user.Id, user);
        //        return user;
        //    }

        //    return null;
        //}

        //public User GetUser(string userId)
        //{
        //    Classes.User user;

        //    try
        //    {
        //        user = Classes.User.Load(userId);
        //    }
        //    catch (NotFoundException) { return null; }

        //    return GetChatUserFromDatingUser(user);

        //    //return Global.CacheProvider.Get("RemoteAuthUserProvider_" + userId) as User;
        //}
        public User GetCurrentlyLoggedUser()
        {
            var href = HttpContext.Current.Items["href"] as string;
            if (string.IsNullOrEmpty(href) || href.Trim() == String.Empty) return null;
            var hrefUri = new Uri(href);
            NameValueCollection hrefParams = HttpUtility.ParseQueryString(hrefUri.Query);
            if (hrefParams["timestamp"] != null)
            {
                #region Validate timestamp
                try
                {
                    DateTime authDate = DateTime.FromFileTimeUtc(Convert.ToInt64(hrefParams["timestamp"]));
                    if (DateTime.Now.Subtract(authDate) > TimeSpan.FromHours(24))
                    {
                        throw new SecurityException("Timestamp has expired!");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new SecurityException("Invalid timestamp!");
                }

                var calculatedHash =
                    Misc.CalculateChatAuthHash(hrefParams["id"] ?? String.Empty, hrefParams["target"] ?? String.Empty, hrefParams["timestamp"]);

                if (hrefParams["hash"] != calculatedHash)
                {
                    throw new SecurityException("Hash is invalid!");
                }
                #endregion

                var user = GetUser(hrefParams["id"]);

                //// Create user object
                //var user = new User { Id = datingUser.Username, DisplayName = datingUser.Username };
                ////user.Id = hrefParams["id"] ?? Regex.Replace(user.DisplayName, "[\\{\\}\\'\\\"]", String.Empty);

                //string thumbUrl = null;
                //try
                //{
                //thumbUrl = ImageHandler.CreateImageUrl(Photo.GetPrimary(user.Id).Id, 30, 30, false, true, true);
                //}
                //catch (NotFoundException) { }

                //user.ThumbnailUrl =
                //    thumbUrl ??
                //    (string.Format("http://www.gravatar.com/avatar/{0}.jpg?s=30&d=monsterid",
                //                   FormsAuthentication.HashPasswordForStoringInConfigFile(user.Id, "md5")).ToLower());

                return user;
            }

            return null;
        }

        public User GetUser(string userId)
        {
            if (cacheProvider.Get("RemoteAuthUserProvider_" + userId) == null)
            {
                var user = GetChatUserFromDatingUser(userId);
                cacheProvider.Set("RemoteAuthUserProvider_" + userId, user);
                return user;
            }

            return cacheProvider.Get("RemoteAuthUserProvider_" + userId) as User;
        }

        public void IgnoreUser(string userId, string ignoredUserId)
        {
            var chatUser = GetUser(userId);
            if (chatUser == null)
                return;

            var ignoredUser = GetUser(ignoredUserId);
            if (ignoredUser == null)
                return;
            //var currentUserSession = PageBase.GetCurrentUserSession();
            //if (currentUserSession == null || currentUserSession.Username != userId)
            //    return;

            try
            {
                Classes.User user = AspNetDating.Classes.User.Load(chatUser.DisplayName);
                user.BlockUser(ignoredUser.DisplayName);

                cacheProvider.Set(String.Format("IgnoredUsers_{0}_{1}", userId, ignoredUserId), true);
            }
            catch (NotFoundException) { return; }

            //string cacheKey = "IgnoredUsers_" + userId;
            //var ignoredUsers = Global.CacheProvider.Get(cacheKey) as IList<string>;

            //if (ignoredUsers == null)
            //    ignoredUsers = new List<string>();

            //if (!ignoredUsers.Contains(ignoredUser))
            //    ignoredUsers.Add(ignoredUser);

            //Global.CacheProvider.Set(cacheKey, ignoredUsers);
        }

        public bool IsUserIgnored(string userId, string ignoredUserId)
        {
            var chatUser = GetUser(userId);
            if (chatUser == null)
                return false;

            var ignoredUser = GetUser(ignoredUserId);
            if (ignoredUser == null)
                return false;

            bool? cached = cacheProvider.Get(String.Format("IgnoredUsers_{0}_{1}",userId, ignoredUserId)) as bool?;

            if (cached.HasValue)
                return cached.Value;

            bool isIgnored = Classes.User.IsUserBlocked(chatUser.DisplayName, ignoredUser.DisplayName);

            cacheProvider.Set(String.Format("IgnoredUsers_{0}_{1}", userId, ignoredUserId), isIgnored);

            return isIgnored;
        }

        public bool IsChatAdmin(string userId, string chatRoomId)
        {
            var chatUser = GetUser(userId);
            if (chatUser == null)
                return false;

            int groupId = Int32.Parse(chatRoomId);
            if (groupId > 0)
            {
                var group = Classes.Group.Fetch(groupId);
                return chatUser.DisplayName == group.Owner;
            }

            return chatUser.DisplayName == "admin";
        }

        public string GetLoginUrl(string backURL)
        {
            var baseUrl = ConfigurationManager.AppSettings["AspNetDatingHomeURL"];
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            var loginUrl = baseUrl + "Login.aspx";
                //(new Uri(new Uri(ConfigurationManager.AppSettings["AspNetDatingHomeURL"]), "Login.aspx")).ToString();
            return HttpUtility.UrlPathEncode(
                String.Format("{0}{2}timestamp={1}&back_url={3}",
                              loginUrl,
                              DateTime.Now.ToFileTimeUtc(),
                              loginUrl.Contains("?") ? "&" : "?",
                              HttpUtility.UrlEncode(backURL)));
            //var loginUrl = (new Uri(new Uri(Config.Urls.Home), "Login.aspx")).ToString();

            //return HttpUtility.UrlPathEncode(
            //    String.Format("{0}{2}timestamp={1}&back_url={3}",
            //                  loginUrl,
            //                  DateTime.Now.ToFileTimeUtc(),
            //                  loginUrl.Contains("?") ? "&" : "?",
            //                  HttpUtility.UrlEncode(backURL)));
        }

        #endregion
    }
}