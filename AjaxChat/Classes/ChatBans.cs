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
    /// <summary>
    /// Handles the banned users
    /// </summary>
    public class ChatBan
    {
        #region Fields

        private int? id;
        private int? chatRoomId;
        private int? userId;
        private string userIp;
        private DateTime banDate;
        private DateTime? banExpires;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        /// <summary>
        /// Gets or sets the chat room id.
        /// </summary>
        /// <value>The chat room id.</value>
        public int? ChatRoomId
        {
            get { return chatRoomId; }
            set { chatRoomId = value; }
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int? UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// Gets or sets the user ip.
        /// </summary>
        /// <value>The user ip.</value>
        public string UserIp
        {
            get { return userIp; }
            set { userIp = value; }
        }

        /// <summary>
        /// Gets or sets the ban date.
        /// </summary>
        /// <value>The ban date.</value>
        public DateTime BanDate
        {
            get { return banDate; }
            set { banDate = value; }
        }

        /// <summary>
        /// Gets or sets the ban expires.
        /// </summary>
        /// <value>The ban expires.</value>
        public DateTime? BanExpires
        {
            get { return banExpires; }
            set { banExpires = value; }
        }

        #endregion

        private ChatBan()
        {
        }

        /// <summary>
        /// Creates a user ban for the specified chat room id.
        /// </summary>
        /// <param name="chatRoomId">The chat room id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public static ChatBan Create(int? chatRoomId, int userId)
        {
            ChatBan ban = new ChatBan();
            ban.chatRoomId = chatRoomId;
            ban.userId = userId;
            ban.banDate = DateTime.Now;
            return ban;
        }

        /// <summary>
        /// Fetches the bans.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="chatRoomId">The chat room id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="userIp">The user ip.</param>
        /// <param name="expiresAfter">The expires after.</param>
        /// <returns></returns>
        public static List<ChatBan> FetchBans(int? id, int? chatRoomId, int? userId, string userIp,
            DateTime expiresAfter)
        {
            List<ChatBan> bans = new List<ChatBan>();

            using (SqlConnection conn = DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchChatBans", id, chatRoomId,
                    userId, userIp, expiresAfter);

                while (reader.Read())
                {
                    ChatBan ban = new ChatBan();

                    ban.id = (int)reader["Id"];
                    ban.chatRoomId = DB.ParseDBNull<int?>(reader["ChatRoomId"]);
                    ban.userId = DB.ParseDBNull<int?>(reader["UserId"]);
                    ban.userIp = DB.ParseDBNull<string>(reader["UserIp"]);
                    ban.banDate = (DateTime) reader["BanDate"];
                    ban.banExpires = DB.ParseDBNull<DateTime?>(reader["BanExpires"]);

                    bans.Add(ban);
                }
            }

            return bans;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveChatBan", id, chatRoomId, userId, userIp,
                    banDate, banExpires);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }
    }
}
#endif