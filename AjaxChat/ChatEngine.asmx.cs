#if !AJAXCHAT_INTEGRATION
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxChat.Classes;

namespace AjaxChat
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    [ComVisible(false)]
    public class ChatEngine : WebService
    {
        public static HttpApplication ApplicationInstance;

        [WebMethod(EnableSession = true)]
        public ChatSession JoinChatRoom(int chatRoomId)
        {
            if (ApplicationInstance == null) ApplicationInstance = Context.ApplicationInstance;
            CleanTimedOutUsers.Ping();

            if (ChatSession.Current == null) ChatSession.Current = new ChatSession();
            ChatSession sess = ChatSession.Current;

            HttpApplication app = Context.ApplicationInstance;
            if (app is IHttpApplicationUserAdapter)
            {
                string username = ((IHttpApplicationUserAdapter) app).GetCurrentlyLoggedUsername();
                if (username == null)
                {
                    sess.Authorized = false;
                    if (app is IHttpApplicationSupportLogin)
                    {
                        sess.AuthorizeUrl = ((IHttpApplicationSupportLogin) app).GetLoginUrl();
                    }
                }
                else
                {
                    sess.ChatUserInstance = ChatUser.FetchByUsername(username);

                    List<ChatBan> bans = null;
                    if (sess.ChatUserInstance != null)
                    {
                        bans = ChatBan.FetchBans(null, chatRoomId, sess.ChatUserInstance.Id,
                                                 null, DateTime.Now);
                    }
                    if (bans != null && bans.Count > 0)
                    {
                        sess.Authorized = false;
                        sess.Banned = true;
                    }
                    else
                    {
                        sess.Authorized = true;
                        sess.ChatUserInstance = ChatUser.FetchByUsername(username);
                        if (sess.ChatUserInstance == null)
                        {
                            ChatUser user = new ChatUser();
                            user.Username = username;
                            user.DisplayName = username;
                            user.BeginTime = DateTime.Now;
                            user.ActiveTime = DateTime.Now;
                            user.Ip = Context.Request.UserHostAddress;
                            user.Save();
                            sess.ChatUserInstance = user;
                        }
                        if (sess.JoinedChatRooms.IndexOf(chatRoomId) < 0)
                        {
                            sess.JoinedChatRooms.Add(chatRoomId);
                        }

                        ChatRoomEvents.JoinUser(chatRoomId, sess);

                        string joinedRoomMessage = "User {0} joined the room";
                        if (Context.ApplicationInstance is IHttpApplicationSupportTranslations)
                            joinedRoomMessage = ((IHttpApplicationSupportTranslations) Context.ApplicationInstance)
                                .Translate(joinedRoomMessage);
                        ChatMessage msg = ChatMessage.Create("system", String.Format(
                                                                           joinedRoomMessage,
                                                                           sess.ChatUserInstance.DisplayName));
                        msg.MessageType = ChatMessage.MessageTypeEnum.JoinRoom;
                        msg.SenderUserId = sess.ChatUserInstance.Id;
                        msg.ChatRoomId = chatRoomId;
                        msg.Save();
                        ChatRoomEvents.AddEvent(msg);
                    }
                }
            }
            else
            {
                sess.Authorized = true;
            }

            return sess;
        }

        [WebMethod(EnableSession = true)]
        public ChatRoom FetchChatRoom(int chatRoomId)
        {
            if (Context.ApplicationInstance is IHttpApplicationChatRoomProvider)
            {
                return ((IHttpApplicationChatRoomProvider) Context.ApplicationInstance).GetChatRoom(chatRoomId);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [WebMethod(EnableSession = true)]
        public void LeaveChatRoom(int chatRoomId)
        {
            ChatSession sess = ChatSession.Current;
            if (sess.JoinedChatRooms.IndexOf(chatRoomId) >= 0)
            {
                sess.JoinedChatRooms.Remove(chatRoomId);

                string leftRoomMessage = "User {0} left the room";
                if (Context.ApplicationInstance is IHttpApplicationSupportTranslations)
                    leftRoomMessage = ((IHttpApplicationSupportTranslations)Context.ApplicationInstance)
                        .Translate(leftRoomMessage);
                ChatMessage msg = ChatMessage.Create("system", String.Format(
                    leftRoomMessage, sess.ChatUserInstance.DisplayName));
                msg.MessageType = ChatMessage.MessageTypeEnum.LeaveRoom;
                msg.ChatRoomId = chatRoomId;
                msg.SenderUserId = sess.ChatUserInstance.Id;
                msg.Save();
                ChatRoomEvents.AddEvent(msg);
            }

            ChatRoomEvents.LeaveUser(chatRoomId, sess);
        }

        [WebMethod(EnableSession = true)]
        public ChatRoomEvents FetchNewEvents(int chatRoomId, int? fromId)
        {
            if (ChatSession.Current == null) return null;
            ChatRoomEvents events = ChatRoomEvents.FetchEvents(chatRoomId, fromId, ChatSession.Current);
            return events;
        }

        [WebMethod(EnableSession = true)]
        public int SendMessage(int? chatRoomId, int? targetUserId, string text)
        {
            if (text.Trim().Length == 0) return -1;
            if (!chatRoomId.HasValue && !targetUserId.HasValue) return -1;

            if (ChatSession.Current == null)
            {
                if (chatRoomId.HasValue)
                {
                    JoinChatRoom(chatRoomId.Value);
                }
                else
                {
                    return -1;
                }
            }
            if (chatRoomId.HasValue && ChatSession.Current.JoinedChatRooms.IndexOf(chatRoomId.Value) < 0)
            {
                JoinChatRoom(chatRoomId.Value);

                if (ChatSession.Current.JoinedChatRooms.IndexOf(chatRoomId.Value) < 0)
                    return -1;
            }

            if (ChatSession.Current == null) return -1;

            if (text.StartsWith("/"))
            {
                string command = text.Substring(1);
                if (command.IndexOf(' ') > 0)
                    command = command.Remove(command.IndexOf(' '));
                IHttpApplicationUserAdapter appAdapter =
                    ((IHttpApplicationUserAdapter)Context.ApplicationInstance);
                switch (command.ToLower())
                {
                    case "kick":
                        if (appAdapter.IsAdministrator(ChatSession.Current.ChatUserInstance.Username)
                            || appAdapter.IsRoomAdmin(ChatSession.Current.ChatUserInstance.Username, 
                                chatRoomId.Value))
                        {
                            string senderDisplayName = ChatSession.Current.ChatUserInstance.DisplayName;

                            if (text.IndexOf(' ') < 0) return -1;
                            string targetUsername = text.Substring(text.IndexOf(' ') + 1);

                            ChatUser targetChatUser = ChatUser.FetchByUsername(targetUsername);
                            if (targetChatUser == null) return -1;

                            bool kicked = ChatRoomEvents.LeaveUser(chatRoomId.Value, targetUsername);

                            if (kicked)
                            {
                                string kickedUserMessage = "User {0} was kicked by {1}";
                                if (Context.ApplicationInstance is IHttpApplicationSupportTranslations)
                                    kickedUserMessage = ((IHttpApplicationSupportTranslations)Context.ApplicationInstance)
                                        .Translate(kickedUserMessage);

                                string messageText = String.Format(kickedUserMessage,
                                    targetChatUser.DisplayName, senderDisplayName);

                                ChatMessage msg = ChatMessage.Create(senderDisplayName, messageText);
                                msg.MessageType = ChatMessage.MessageTypeEnum.Kicked;
                                msg.SenderUserId = ChatSession.Current.ChatUserInstance.Id;
                                msg.ChatRoomId = chatRoomId;
                                msg.Save();
                                ChatRoomEvents.AddEvent(msg);

                                string kickedUserMessage2 = "You were kicked by {0}";
                                if (Context.ApplicationInstance is IHttpApplicationSupportTranslations)
                                    kickedUserMessage2 = ((IHttpApplicationSupportTranslations)Context.ApplicationInstance)
                                        .Translate(kickedUserMessage2);
                                string messageText2 = String.Format(kickedUserMessage2,
                                    senderDisplayName);

                                ChatMessage msg2 = ChatMessage.Create(senderDisplayName, messageText2);
                                msg2.MessageType = ChatMessage.MessageTypeEnum.Kicked;
                                msg2.SenderUserId = ChatSession.Current.ChatUserInstance.Id;
                                msg2.TargetUserId = targetChatUser.Id;
                                msg2.ChatRoomId = chatRoomId;
                                msg2.Save();
                                ChatRoomEvents.AddEvent(msg2);

                                return msg.Id;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    case "ban" :
                        if (appAdapter.IsAdministrator(ChatSession.Current.ChatUserInstance.Username)
                            || appAdapter.IsRoomAdmin(ChatSession.Current.ChatUserInstance.Username,
                                chatRoomId.Value))
                        {
                            string senderDisplayName = ChatSession.Current.ChatUserInstance.DisplayName;

                            if (text.IndexOf(' ') < 0) return -1;
                            string targetUsername = text.Substring(text.IndexOf(' ') + 1);

                            ChatUser targetChatUser = ChatUser.FetchByUsername(targetUsername);
                            if (targetChatUser == null) return -1;

                            ChatBan ban = ChatBan.Create(chatRoomId, targetChatUser.Id.Value);
                            ban.Save();

                            bool kicked = ChatRoomEvents.LeaveUser(chatRoomId.Value, targetUsername);

                            if (kicked)
                            {
                                string bannedUserMessage = "User {0} was banned by {1}";
                                if (Context.ApplicationInstance is IHttpApplicationSupportTranslations)
                                    bannedUserMessage = ((IHttpApplicationSupportTranslations)Context.ApplicationInstance)
                                        .Translate(bannedUserMessage);

                                string messageText = String.Format(bannedUserMessage,
                                    targetChatUser.DisplayName, senderDisplayName);

                                ChatMessage msg = ChatMessage.Create(senderDisplayName, messageText);
                                msg.MessageType = ChatMessage.MessageTypeEnum.Banned;
                                msg.SenderUserId = ChatSession.Current.ChatUserInstance.Id;
                                msg.ChatRoomId = chatRoomId;
                                msg.Save();
                                ChatRoomEvents.AddEvent(msg);

                                string bannedUserMessage2 = "You were banned by {0}";
                                if (Context.ApplicationInstance is IHttpApplicationSupportTranslations)
                                    bannedUserMessage2 = ((IHttpApplicationSupportTranslations)Context.ApplicationInstance)
                                        .Translate(bannedUserMessage2);

                                string messageText2 = String.Format(bannedUserMessage2,
                                    senderDisplayName);

                                ChatMessage msg2 = ChatMessage.Create(senderDisplayName, messageText2);
                                msg2.MessageType = ChatMessage.MessageTypeEnum.Banned;
                                msg2.SenderUserId = ChatSession.Current.ChatUserInstance.Id;
                                msg2.TargetUserId = targetChatUser.Id;
                                msg2.ChatRoomId = chatRoomId;
                                msg2.Save();
                                ChatRoomEvents.AddEvent(msg2);

                                return msg.Id;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    default:
                        return -1;
                }
            }
            else
            {
                string senderDisplayName = ChatSession.Current.ChatUserInstance.DisplayName;
                ChatMessage msg = ChatMessage.Create(senderDisplayName, text);
                msg.SenderUserId = ChatSession.Current.ChatUserInstance.Id;
                msg.ChatRoomId = chatRoomId;
                msg.Save();
                ChatRoomEvents.AddEvent(msg);

                return msg.Id;
            }
        }

        [WebMethod(EnableSession = true)]
        public OnlineUsersList GetOnlineUsers(int chatRoomId)
        {
            return ChatRoomEvents.FetchOnlineUsers(chatRoomId);
        }
    }
}
#endif