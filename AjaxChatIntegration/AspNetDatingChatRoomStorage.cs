using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetAjaxChat.Interfaces;

namespace AspNetDating.AjaxChatIntegration
{
    //public class AspNetDatingChatRoomStorage : IChatRoomStorage
    //{
    //    private readonly Dictionary<string, List<Message>> messagesStorage;
    //    private readonly Dictionary<string, string> userTokens;
    //    private readonly Dictionary<string, Dictionary<string, DateTime>> usersStorage;

    //    public AspNetDatingChatRoomStorage()
    //    {
    //        usersStorage = new Dictionary<string, Dictionary<string,DateTime>>();
    //        messagesStorage = new Dictionary<string, List<Message>>();
    //        userTokens = new Dictionary<string, string>();
    //    }

    //    #region IChatRoomStorage Members

    //    public string GenerateUserToken(string userId)
    //    {
    //        string token = Guid.NewGuid().ToString();
    //        lock (userTokens)
    //        {
    //            userTokens[token] = userId;
    //        }
    //        return token;
    //    }

    //    public string GetUserIdByToken(string token)
    //    {
    //        lock (userTokens)
    //        {
    //            return userTokens.ContainsKey(token)
    //                       ? userTokens[token]
    //                       : null;
    //        }
    //    }

    //    public void UpdateOnline(string chatRoomId, string userId)
    //    {
    //        lock (usersStorage)
    //        {
    //            //if room does not exist then do nothing
    //            if (!usersStorage.ContainsKey(chatRoomId))
    //                return;

    //            Dictionary<string, DateTime> users = usersStorage[chatRoomId];

    //            lock (users)
    //            {
    //                //if user does not exist then do nothing
    //                if (!users.ContainsKey(userId))
    //                    return;
                    
    //                users[userId] = DateTime.Now;
    //            }
    //        }
    //    }

    //    public void AddUserToRoom(string chatRoomId, string userId)
    //    {
    //        lock (usersStorage)
    //        {
    //            if (!usersStorage.ContainsKey(chatRoomId))
    //                usersStorage.Add(chatRoomId, new Dictionary<string,DateTime>());

    //            Dictionary<string,DateTime> users = usersStorage[chatRoomId];

    //            lock (users)
    //            {
    //                if (!users.ContainsKey(userId))
    //                    users.Add(userId, DateTime.Now);
    //                else
    //                    users[userId] = DateTime.Now;
    //            }
    //        }
    //    }

    //    public void RemoveUserFromRoom(string chatRoomId, string userId)
    //    {
    //        lock (usersStorage)
    //        {
    //            if (!usersStorage.ContainsKey(chatRoomId))
    //                return;

    //            Dictionary<string, DateTime> users = usersStorage[chatRoomId];

    //            lock (users)
    //            {
    //                users.Remove(userId);
    //            }
    //        }
    //    }

    //    //returns removed users (roomId, userId)
    //    public Dictionary<string, string> RemoveInactiveUsers(TimeSpan timeOfInactivity)
    //    {
    //        var removedUsers = new Dictionary<string, string>();

    //        lock (usersStorage)
    //        {
    //            foreach (var room in usersStorage)
    //            {
    //                var users = usersStorage[room.Key];

    //                lock (users)
    //                {
    //                    var userIds = users.Where(pair => DateTime.Now - pair.Value > timeOfInactivity).Select(pair => pair.Key).ToArray();

    //                    foreach (var userId in userIds)
    //                    {
    //                        users.Remove(userId);
    //                        removedUsers.Add(room.Key, userId);
    //                    }
    //                }
    //            }
    //        }

    //        return removedUsers;
    //    }

    //    public string[] GetUsersInRoom(string chatRoomId)
    //    {
    //        lock (usersStorage)
    //        {
    //            if (!usersStorage.ContainsKey(chatRoomId))
    //                return new string[0];

    //            Dictionary<string, DateTime> users = usersStorage[chatRoomId];

    //            lock (users)
    //            {
    //                return users.Keys.ToArray();
    //            }
    //        }
    //    }

    //    public bool IsUserInRoom(string chatRoomId, string userId)
    //    {
    //        lock (usersStorage)
    //        {
    //            if (!usersStorage.ContainsKey(chatRoomId))
    //                return false;

    //            Dictionary<string, DateTime> users = usersStorage[chatRoomId];

    //            lock (users)
    //            {
    //                return users.Keys.Any(u => u == userId);
    //            }
    //        }
    //    }

    //    public void AddMessage(string chatRoomId, Message message)
    //    {
    //        lock (messagesStorage)
    //        {
    //            if (!messagesStorage.ContainsKey(chatRoomId))
    //                messagesStorage.Add(chatRoomId, new List<Message>());

    //            List<Message> messages = messagesStorage[chatRoomId];

    //            lock (messages)
    //            {
    //                messages.Add(message);
    //            }
    //        }
    //    }

    //    public Message[] GetMessages(string chatRoomId, string userId, long fromTimestamp)
    //    {
    //        lock (messagesStorage)
    //        {
    //            if (!messagesStorage.ContainsKey(chatRoomId))
    //                return new Message[0];

    //            List<Message> messages = messagesStorage[chatRoomId];

    //            lock (messages)
    //            {
    //                IEnumerable<Message> result = from m in messages
    //                                              where (fromTimestamp == 0 || m.Timestamp > fromTimestamp)
    //                                                    && (m.ToUserId == null || m.ToUserId == userId)
    //                                              select m;
    //                // If first request
    //                if (fromTimestamp == 0)
    //                {
    //                    //do not get my kick message
    //                    result = result.Where(m=>!(m.FromUserId == userId && m.MessageType == MessageTypeEnum.Kicked));
    //                    //
    //                    result = result.OrderByDescending(m => m.Timestamp).Take(20);
    //                }
    //                else
    //                {
    //                    //do not get my own messages except if it is a kick message
    //                    result = result.Where(m => m.FromUserId != userId || m.MessageType == MessageTypeEnum.Kicked);
    //                }
    //                return result.OrderBy(m => m.Timestamp).ToArray();
    //            }
    //        }
    //    }

    //    #endregion
    //}
}
