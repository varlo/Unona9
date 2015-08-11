using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetAjaxChat.Interfaces;
using System.Timers;

namespace AspNetDating.AjaxChatIntegration
{
    public class MemoryMessengerPresenceProvider : IMessengerPresenceProvider
    {
        private readonly List<ChatRequest> lChatRequests = new List<ChatRequest>();
        private readonly Dictionary<string, DateTime> dLastOnline = new Dictionary<string, DateTime>();
        private readonly int presenceTimeout = 10/*Config.Misc.MessengerPresenceUpdateInterval*/ * 2; // in seconds
        private readonly Timer cleanupTimer = new Timer();

        public MemoryMessengerPresenceProvider()
        {
            cleanupTimer.Interval = presenceTimeout * 5 * 1000;
            cleanupTimer.Elapsed += cleanupTimer_Elapsed;
            cleanupTimer.Start();
        }

        void cleanupTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (dLastOnline)
            {
                var listKeys = new List<string>();
                foreach (string key in dLastOnline.Keys)
                {
                    if (DateTime.Now.Subtract(dLastOnline[key]).TotalSeconds > presenceTimeout * 5)
                        listKeys.Add(key);
                }
                foreach (string key in listKeys)
                {
                    dLastOnline.Remove(key);
                }
            }
        }

        #region IMessengerPresenceProvider Members

        public void AddChatRequest(ChatRequest request)
        {
            lock (lChatRequests)
            {
                lChatRequests.Add(request);
            }
        }

        public ChatRequest GetChatRequest(string toUserId)
        {
            lock (lChatRequests)
            {
                return lChatRequests.FirstOrDefault(r=>r.ToUserId == toUserId);
            }
        }

        public void RemoveChatRequest(string fromUserId, string toUserId)
        {
            lock (lChatRequests)
            {
                lChatRequests.RemoveAll(r=>r.ToUserId == toUserId && r.FromUserId == fromUserId);
            }
        }

        public void UpdateLastOnline(string userId)
        {
            lock (dLastOnline)
            {
                dLastOnline[userId] = DateTime.Now;
            }
        }

        #endregion
    }
}
