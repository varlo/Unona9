#if !AJAXCHAT_INTEGRATION
using System;
using System.Collections.Generic;
using System.Web;

namespace AjaxChat.Classes
{
    public class ChatRoom
    {
        #region Fields

        private int id;
        private string name;
        private string topic;
        private DateTime beginTime;
        private DateTime activeTime;
        private bool locked;

        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Topic
        {
            get { return topic; }
            set { topic = value; }
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

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        #endregion
    }

    public class ChatRoomEvents
    {
        public List<ChatMessage> Messages = new List<ChatMessage>();

        private static Dictionary<int, List<ChatMessage>> dChatRooms =
            new Dictionary<int, List<ChatMessage>>();

        public static void AddEvent(ChatMessage chatRoomMessage)
        {
            lock (dChatRooms)
            {
                if (!dChatRooms.ContainsKey(chatRoomMessage.ChatRoomId.Value))
                {
                    dChatRooms.Add(chatRoomMessage.ChatRoomId.Value, new List<ChatMessage>());
                }
            }

            List<ChatMessage> lMessages = dChatRooms[chatRoomMessage.ChatRoomId.Value];
            lock (lMessages)
            {
                lMessages.Add(chatRoomMessage);
            }
        }

        public static ChatRoomEvents FetchEvents(int chatRoomId, int? fromId, ChatSession chatSession)
        {
            if (!dChatRooms.ContainsKey(chatRoomId)) return null;
            List<ChatMessage> lMessages = dChatRooms[chatRoomId];

            ChatRoomEvents chatRoomEvents = new ChatRoomEvents();

            lock (lMessages)
            {
                chatRoomEvents.Messages.AddRange(
                    lMessages.FindAll(delegate(ChatMessage msg)
                                          {
                                              return (!fromId.HasValue
                                                       || (fromId.HasValue && msg.Id > fromId.Value))
                                                  && (!msg.TargetUserId.HasValue 
                                                    || chatSession.ChatUserInstance.Id == msg.TargetUserId.Value);
                                          }));
                if (!fromId.HasValue && chatRoomEvents.Messages.Count > 10)
                    chatRoomEvents.Messages.RemoveRange(0, chatRoomEvents.Messages.Count - 10);
            }

            // Update last activity time
            ChatSession foundSession = dChatRoomUsers[chatRoomId].Find(
                delegate(ChatSession sess)
                    {
                        try
                        {
                            return sess.ChatUserInstance.Username == chatSession.ChatUserInstance.Username;
                        }
                        catch
                        {
                            return false;
                        }
                    });
            if (foundSession != null) foundSession.LastActivity = DateTime.Now;

            return chatRoomEvents;
        }

        public static Dictionary<int, List<ChatSession>> dChatRoomUsers =
            new Dictionary<int, List<ChatSession>>();

        public static void JoinUser(int chatRoomId, ChatSession chatSession)
        {
            if (!dChatRoomUsers.ContainsKey(chatRoomId))
                dChatRoomUsers.Add(chatRoomId, new List<ChatSession>());
            List<ChatSession> lSessions = dChatRoomUsers[chatRoomId];
            if (lSessions.Find(
                delegate(ChatSession sess)
                {
                    return sess.ChatUserInstance.Id == chatSession.ChatUserInstance.Id;
                }) == null)
            {
                lSessions.Add(chatSession);
            }

            chatSession.LastActivity = DateTime.Now;
        }

        public static void LeaveUser(int chatRoomId, ChatSession chatSession)
        {
            List<ChatSession> lSessions = dChatRoomUsers[chatRoomId];
            ChatSession sessInstance = lSessions.Find(
                delegate(ChatSession sess) { return sess.ChatUserInstance.Id == chatSession.ChatUserInstance.Id; });
            if (sessInstance != null)
            {
                lSessions.Remove(sessInstance);
                if (sessInstance.JoinedChatRooms.IndexOf(chatRoomId) >= 0)
                {
                    sessInstance.JoinedChatRooms.Remove(chatRoomId);
                }
            }
        }

        public static bool LeaveUser(int chatRoomId, string username)
        {
            List<ChatSession> lSessions = dChatRoomUsers[chatRoomId];
            ChatSession sessInstance = lSessions.Find(
                delegate(ChatSession sess) { return sess.ChatUserInstance.Username == username; });
            if (sessInstance != null)
            {
                lSessions.Remove(sessInstance);
                if (sessInstance.JoinedChatRooms.IndexOf(chatRoomId) >= 0)
                {
                    sessInstance.JoinedChatRooms.Remove(chatRoomId);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static OnlineUsersList FetchOnlineUsers(int chatRoomId)
        {
            OnlineUsersList onlineUsers = new OnlineUsersList();

            if (dChatRoomUsers.ContainsKey(chatRoomId))
            {
                List<ChatSession> lSessions = dChatRoomUsers[chatRoomId];
                foreach (ChatSession sess in lSessions)
                {
                    OnlineUser user = new OnlineUser();
                    user.Id = sess.ChatUserInstance.Id;
                    user.Username = sess.ChatUserInstance.Username;
                    user.DisplayName = sess.ChatUserInstance.DisplayName;
                    if (HttpContext.Current.ApplicationInstance is IHttpApplicationSupportProfiles)
                    {
                        user.ProfileUrl = ((IHttpApplicationSupportProfiles)
                                           HttpContext.Current.ApplicationInstance).GetUserProfileUrl(
                                           sess.ChatUserInstance.Username);
                    }
                    if (HttpContext.Current.ApplicationInstance is IHttpApplicationSupportAvatars)
                    {
                        user.ThumbImage = ((IHttpApplicationSupportAvatars)
                                           HttpContext.Current.ApplicationInstance).GetUserAvatar(
                                           sess.ChatUserInstance.Username);
                    }
                    onlineUsers.OnlineUsers.Add(user);
                }
            }

            return onlineUsers;
        }
    }

    public class OnlineUsersList
    {
        public List<OnlineUser> OnlineUsers = new List<OnlineUser>();
    }

    public class OnlineUser
    {
        public int? Id;
        public string Username;
        public string DisplayName;
        public string ThumbImage;
        public string ProfileUrl;
    }
}
#endif