using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetAjaxChat.Interfaces;
using AspNetDating.Classes;

namespace AspNetDating.AjaxChatIntegration
{
    public class AspNetDatingChatRoomProvider : IChatRoomProvider
    {
        private readonly int MaxUsers = 100;
        private readonly Room mainChatRoom;
        private readonly Room messengerChatRoom;
        private IChatUserProvider chatUserProvider;
        private ICacheProvider cacheProvider;

        public AspNetDatingChatRoomProvider(IChatUserProvider chatUserProvider, ICacheProvider cacheProvider)
        {
            this.chatUserProvider = chatUserProvider;
            this.cacheProvider = cacheProvider;

            mainChatRoom = new Room
            {
                Id = "-1",
                MaxUsers = MaxUsers,
                Name = "Main Chat",
                Password = null,
                Topic = "Welcome to chat!",
                Visible = true
            };

            messengerChatRoom = new Room
            {
                Id = "-2",
                MaxUsers = Int32.MaxValue,
                Name = "Messenger",
                Password = null,
                Topic = "Welcome to chat!",
                Visible = false
            };
        }

        private Room GetChatRoomFromGroup(Classes.Group group)
        {
            return new Room
            {
                Id = group.ID.ToString(),
                Name = group.Name,
                Topic = String.Empty,
                MaxUsers = MaxUsers,
                Password = null,
                Visible = true
            };
        }

        #region IChatRoomProvider Members

        public Room GetChatRoom(string chatRoomId)
        {
            if (chatRoomId == "-1")
                return mainChatRoom;
            if (chatRoomId == "-2")
                return messengerChatRoom;

            if (Classes.Config.Groups.EnableGroups && Classes.Config.Groups.EnableAjaxChat)
            {
                int groupId;
                if (!Int32.TryParse(chatRoomId, out groupId))
                    return null;

                var group = Classes.Group.Fetch(groupId);
                
                if (group == null)
                    return null;

                return GetChatRoomFromGroup(group);
            }

            return null;
        }

        public IEnumerable<Room> GetChatRooms()
        {
            List<Room> rooms = new List<Room>();

            rooms.Add(mainChatRoom);

            if (Classes.Config.Groups.EnableGroups && Classes.Config.Groups.EnableAjaxChat)
            {
                foreach (var group in Classes.Group.Fetch(Classes.Group.eSortColumn.Name))
                {
                    rooms.Add(GetChatRoomFromGroup(group));
                }
            }
            
            return rooms;
        }

        public void BanUser(string chatRoomId, string userId, string bannedUserId)
        {
            var groupId = Int32.Parse(chatRoomId);
            var bannedUser = chatUserProvider.GetUser(bannedUserId);

            //banned users in the main chat are stored to the cache
            if (chatRoomId == "-1")
                cacheProvider.Set(String.Format("Ban_{0}", bannedUserId), DateTime.Now);

            if (groupId > 0)
            {
                GroupBan ban = new GroupBan(groupId, bannedUser.DisplayName);
                ban.Save();
            }
        }

        public bool HasChatAccess(string userId, string chatRoomId, out string reason)
        {
            var chatUser = chatUserProvider.GetUser(userId);

            if (chatUser == null)
            {
                reason = "Invalid userId";
                return false;
            }

            int roomId;

            if (!Int32.TryParse(chatRoomId, out roomId))
            {
                reason = "Invalid chatRoomId";
                return false;
            }

            if (roomId > 0)
            {
                if (Classes.Config.Groups.EnableGroups && Classes.Config.Groups.EnableAjaxChat)
                {
                    var member = Classes.GroupMember.Fetch(roomId, chatUser.DisplayName);

                    if (member == null)
                    {
                        reason = "Not a group member";
                        return false;
                    }
                }
                else
                {
                    reason = "Invalid roomId";
                    return false;
                }

                if (Classes.GroupMember.IsBanned(chatUser.DisplayName, roomId))
                {
                    reason = "You are banned from the group chat room";
                    return false;
                }
            }

            Classes.User user;

            try
            {
                user = Classes.User.Load(chatUser.DisplayName);
            }
            catch (NotFoundException) { reason = "Invalid username";  return false; }

            var result = HasAccess(user, chatRoomId == "-2");
            switch (result)
            {
                case PermissionCheckResult.No:
                    if (!HasAccessThroughLevels(user))
                    {
                        reason = "Access denied";
                        return false;
                    }
                    break;
                case PermissionCheckResult.YesButMoreCreditsNeeded:
                    reason = "More credits needed";
                    return false;
                case PermissionCheckResult.YesButPlanUpgradeNeeded:
                    reason = "Upgrade billing plan";
                    return false;
            }

            if (chatRoomId == "-1")
            {
                var cacheKey = String.Format("Ban_{0}", userId);
                var dateTimeBanned = cacheProvider.Get(cacheKey) as DateTime?;
                if (dateTimeBanned.HasValue)
                {
                    //remove the ban if 24 hours elapsed
                    if (DateTime.Now - dateTimeBanned.Value > TimeSpan.FromHours(24))
                        cacheProvider.Remove(cacheKey);
                    else
                    {
                        reason = String.Format("You are banned from the chat on {0}", dateTimeBanned.Value);
                        return false;
                    }
                }
            }

            reason = null;
            return true;
        }

        private bool HasAccessThroughLevels(Classes.User user)
        {
            if (!Config.UserScores.EnableUserLevels)
                return false;

            var level = UserLevel.GetLevelByScore(user.Score);

            if (level == null)
                return false;

            return level.Restrictions.UserCanUseChat;
        }

        private PermissionCheckResult HasAccess(Classes.User user, bool isIM)
        {
            Subscription subscription = Subscription.FetchActiveSubscription(user.Username);
            BillingPlanOptions billingPlanOptions;

            if (subscription == null)
                billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
            else
            {
                BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                billingPlanOptions = plan.Options;
            }

            if (isIM)
                return CanDoAction(user, billingPlanOptions.CanIM);
            else
                return CanDoAction(user, billingPlanOptions.UserCanUseChat);

        }

        private PermissionCheckResult CanDoAction(Classes.User user, BillingPlanOption<bool> option)
        {
            if (user.IsAdmin() || option.Value)
                return PermissionCheckResult.Yes;

            if (Config.Users.FreeForFemales && user.Gender == AspNetDating.Classes.User.eGender.Female &&
                (option.EnableCreditsPayment || option.UpgradableToNextPlan))
                return PermissionCheckResult.Yes;

            if (option.EnableCreditsPayment)
            {
                if (user.Credits < option.Credits)
                    return PermissionCheckResult.YesButMoreCreditsNeeded;
                else
                    return PermissionCheckResult.YesWithCredits;
            }

            if (option.UpgradableToNextPlan)
                return PermissionCheckResult.YesButPlanUpgradeNeeded;

            return PermissionCheckResult.No;
        }

        #endregion
    }
}
