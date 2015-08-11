#if !AJAXCHAT_INTEGRATION
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.Data;

namespace AjaxChat.Classes
{
    [Serializable]
    public class ChatUser
    {
        #region Fields

        private int? id;
        private string username;
        private string displayName;
        private DateTime beginTime;
        private DateTime activeTime;
        private string ip;

        #endregion

        #region Properties

        public int? Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string DisplayName
        {
            get
            {
                if (displayName == null) 
                    displayName = username;
                return displayName;
            }
            set { displayName = value; }
        }

        public DateTime BeginTime
        {
            get { return beginTime; }
            set { beginTime = value; }
        }

        public DateTime ActiveTime
        {
            get { return activeTime; }
            set { activeTime = value; }
        }

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        #endregion

        public static ChatUser FetchByUsername(string username)
        {
            List<ChatUser> users = Fetch(null, username, null, null);
            return users.Count > 0 ? users[0] : null;
        }

        private static List<ChatUser> Fetch(int? id, string username, string displayName, string ip)
        {
            List<ChatUser> users = new List<ChatUser>();

            using (SqlConnection conn = DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchChatUsers", id, username,
                    displayName, ip);

                while (reader.Read())
                {
                    ChatUser user = new ChatUser();
                    
                    user.id = (int)reader["Id"];
                    user.username = (string) reader["Username"];
                    user.displayName = (string)reader["DisplayName"];
                    user.beginTime = (DateTime)reader["BeginTime"];
                    user.activeTime = (DateTime)reader["ActiveTime"];
                    user.ip = (string)reader["Ip"];

                    users.Add(user);
                }
            }

            return users;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveChatUser", id, username, displayName,
                    beginTime, activeTime, ip);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }
    }

    [Serializable]
    public class ChatSession
    {
        public bool Authorized;
        public bool Banned;
        public string AuthorizeUrl;
        public ChatUser ChatUserInstance;
        public DateTime LastActivity;
        public List<int> JoinedChatRooms = new List<int>();

        public static ChatSession Current
        {
            get
            {
                if (HttpContext.Current.Session["AjaxChatSession"] != null)
                {
                    return (ChatSession) HttpContext.Current.Session["AjaxChatSession"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["AjaxChatSession"] = value;
            }
        }
    }
}
#endif