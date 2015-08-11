#if !AJAXCHAT_INTEGRATION
using System;
using System.Data.SqlClient;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace AjaxChat.Classes
{
    public class ChatMessage
    {
        #region Types

        public class Formatting
        {
            public bool Bold;
            public bool Italic;
            public bool Underline;
            public string FontColor;
            public string FontName;
            public string FontSize;
        }

        public enum MessageTypeEnum
        {
            RoomMessage = 0,
            PrivateMessage = 1,
            JoinRoom = 2,
            LeaveRoom = 3,
            Kicked = 4,
            Banned = 5
        }

        #endregion

        #region Fields

        private int? id;
        private MessageTypeEnum messageType;
        private DateTime messageTime;
        private int? chatRoomId;
        private int? senderUserId;
        private string senderDisplayName;
        private int? targetUserId;
        private string text;
        private string textHtml;

        #endregion

        #region Properties

        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        public MessageTypeEnum MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        public DateTime MessageTime
        {
            get { return messageTime; }
            set { messageTime = value; }
        }

        public int? ChatRoomId
        {
            get { return chatRoomId; }
            set { chatRoomId = value; }
        }

        public int? SenderUserId
        {
            get { return senderUserId; }
            set { senderUserId = value; }
        }

        public string SenderDisplayName
        {
            get { return senderDisplayName; }
            set { senderDisplayName = value; }
        }

        public int? TargetUserId
        {
            get { return targetUserId; }
            set { targetUserId = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string TextHtml
        {
            get { return textHtml; }
            set { textHtml = value; }
        }

        #endregion

        private ChatMessage()
        {
        }

        public static ChatMessage Create(string senderDisplayName, string text)
        {
            ChatMessage message = new ChatMessage();
            message.messageTime = DateTime.Now;
            message.senderDisplayName = senderDisplayName;
            message.text = text;
            message.textHtml = ParseMessageText(text);
            return message;
        }

        public static string ParseMessageText(string text)
        {
            string parsed = text;

            parsed = HttpUtility.HtmlEncode(parsed);
            Smilies.Process(ref parsed);

            return parsed;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveChatMessage", id, messageTime, chatRoomId,
                    senderUserId, senderDisplayName, targetUserId, text, textHtml);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }
    }
}
#endif