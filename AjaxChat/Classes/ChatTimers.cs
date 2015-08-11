#if !AJAXCHAT_INTEGRATION
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Timer = System.Timers.Timer;

namespace AjaxChat.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public static class CleanTimedOutUsers
    {
        private static Timer timer;
        private static bool timerLock = false;

        /// <summary>
        /// Pings this instance.
        /// </summary>
        public static void Ping()
        {
            // Dummy method
            // Should be called at some point in order to run the static constructor
        }

        static CleanTimedOutUsers()
        {
            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncCleanTimedOutUsers), null);
        }

        private static void AsyncCleanTimedOutUsers(object data)
        {
            if (timerLock)
            {
                return;
            }

            try
            {
                timerLock = true;

                foreach (int chatRoomId in ChatRoomEvents.dChatRoomUsers.Keys)
                {
                    List<ChatSession> sessions = new List<ChatSession>();
                    foreach (ChatSession sess in ChatRoomEvents.dChatRoomUsers[chatRoomId])
                    {
                        if (sess.LastActivity < DateTime.Now.Subtract(TimeSpan.FromMinutes(5)))
                        {
                            sessions.Add(sess);
                        }
                    }
                    foreach (ChatSession sess in sessions)
                    {
                        if (sess.JoinedChatRooms.IndexOf(chatRoomId) >= 0)
                        {
                            sess.JoinedChatRooms.Remove(chatRoomId);
                        }

                        string leftRoomMessage = "User {0} left the room (timeout)";
                        if (ChatEngine.ApplicationInstance is IHttpApplicationSupportTranslations)
                            leftRoomMessage = ((IHttpApplicationSupportTranslations)ChatEngine.ApplicationInstance)
                                .Translate(leftRoomMessage);
                        ChatMessage msg = ChatMessage.Create("system", String.Format(
                            leftRoomMessage, sess.ChatUserInstance.DisplayName));
                        msg.MessageType = ChatMessage.MessageTypeEnum.LeaveRoom;
                        msg.ChatRoomId = chatRoomId;
                        msg.SenderUserId = sess.ChatUserInstance.Id;
                        msg.Save();
                        ChatRoomEvents.AddEvent(msg);
                        

                        ChatRoomEvents.LeaveUser(chatRoomId, sess.ChatUserInstance.Username);
                    }
                }
            }
            finally
            {
                timerLock = false;
            }
        }
    }
}
#endif