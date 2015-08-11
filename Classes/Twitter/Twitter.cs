using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using AspNetDating.Model;
using oAuth;

namespace AspNetDating.Classes
{
    public class Twitter
    {
        public static void SaveCredentials(string username, string twitterUsername, string twitterPassword)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var credentials = db.TwitterCredentials.FirstOrDefault(tc => tc.u_username == username);
                if (credentials == null)
                {
                    credentials = new TwitterCredential {u_username = username};
                    db.TwitterCredentials.InsertOnSubmit(credentials);
                }

                credentials.tc_username = twitterUsername;
                credentials.tc_password = twitterPassword;

                db.SubmitChanges();
            }
        }

        public static bool HasCredentials(string username)
        {
            using (var db = new AspNetDatingDataContext())
            {
                return db.TwitterCredentials.Any(tc => tc.u_username == username);
            }
        }

        public static void PublishTweet(string username, string tweet)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var credentials = db.TwitterCredentials.FirstOrDefault(tc => tc.u_username == username);
                if (credentials == null) return;

                string url = "";
                string xml = "";
                oAuthTwitter oAuth = new oAuthTwitter();
                oAuth.Token = credentials.tc_username;
                oAuth.TokenSecret = credentials.tc_password;

                //POST 
                url = "http://twitter.com/statuses/update.xml";
                xml = oAuth.oAuthWebRequest(oAuthTwitter.Method.POST, url, "status=" + oAuth.UrlEncode(tweet));
            }
        }

        public static void RemoveCredentials(string username)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var credentials = db.TwitterCredentials.FirstOrDefault(tc => tc.u_username == username);
                if (credentials != null)
                {
                    db.TwitterCredentials.DeleteOnSubmit(credentials);
                    db.SubmitChanges();
                }
            }
        }
    }
}
