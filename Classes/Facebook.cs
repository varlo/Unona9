using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AspNetDating.Model;

namespace AspNetDating.Classes
{
    public class Facebook
    {
        public static void SaveCredentials(string username, string token)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var credentials = db.FacebookCredentials.FirstOrDefault(tc => tc.u_username == username);
                if (credentials == null)
                {
                    credentials = new FacebookCredential { u_username = username };
                    db.FacebookCredentials.InsertOnSubmit(credentials);
                }

                credentials.fc_token = token;

                db.SubmitChanges();
            }
        }

        public static bool HasCredentials(string username)
        {
            using (var db = new AspNetDatingDataContext())
            {
                return db.FacebookCredentials.Any(tc => tc.u_username == username);
            }
        }

        public static void RemoveCredentials(string username)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var credentials = db.FacebookCredentials.FirstOrDefault(tc => tc.u_username == username);
                if (credentials != null)
                {
                    db.FacebookCredentials.DeleteOnSubmit(credentials);
                    db.SubmitChanges();
                }
            }
        }

        public static void PublishStatus(string username, long facebookId, string status)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var credentials = db.FacebookCredentials.FirstOrDefault(tc => tc.u_username == username);
                if (credentials == null) return;

                using (var client = new WebClient())
                {
                    var fields = new NameValueCollection {{"access_token", credentials.fc_token}, {"message", status}};
                    client.UploadValues(string.Format("https://graph.facebook.com/{0}/feed", facebookId), fields);
                }
            }
        }
    }
}
