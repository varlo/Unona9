/* ASPnetDating 
 * Copyright (C) 2003-2014 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.ApplicationBlocks.Data;
using System.Drawing;
using Timer=System.Timers.Timer;

namespace AspNetDating.Classes
{
    [Serializable]
    public class Group
    {
        #region fields

        private int? id = null;
        private string name;
        private string description;
        private DateTime dateCreated = DateTime.Now;
        private bool approved = false;
        private eAccessLevel accessLevel;
        private string owner;
        private int activeMembers;
        private eSortColumn sortColumn;
        private string joinTerms = String.Empty;
        private string joinQuestion = String.Empty;
        //Non Members can view - Group, MessageBoard, Gallery, Events
        private ulong permissions = 131821055;
        private int? minAge = null;
        private bool autojoin = false;
        private string autojoinCountry;
        private string autojoinRegion;
        private string autojoinCity;

        /// <summary>
        /// Specifies the action which will be perform.
        /// </summary>
        private enum eAction
        {
            Assign = 1,
            Remove = 2
        }

        /// <summary>
        /// Specifies the access level for the group.
        /// </summary>
        public enum eAccessLevel
        {
            /// <summary>
            /// Group content can by viewed by all site visitors.
            /// Any member can join this group without approval of the owner.
            /// </summary>
            Public = 1,

            /// <summary>
            /// Group content can by viewed by all site vistors.
            /// Requests to join the group will need to be approved by group owner or moderator.
            /// </summary>
            Moderated = 2,
            
            /// <summary>
            /// Group content can only be viewed by group members.
            /// Requests to join the group will need to be approved by group owner or moderator.
            /// </summary>
            Private = 3
        }

        /// <summary>
        /// Specify on which column groups will be sorted.
        /// </summary>
        public enum eSortColumn
        {
            /// <summary>
            /// No sort.
            /// </summary>
            None,
            /// <summary>
            /// On date created column.
            /// </summary>
            DateCreated,
            /// <summary>
            /// On name column.
            /// </summary>
            Name,
            /// <summary>
            /// On owner column.
            /// </summary>
            Owner,
            /// <summary>
            /// On members column
            /// </summary>
            ActiveMembers,
            /// <summary>
            /// On access level column.
            /// </summary>
            AccessLevel
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
        /// Throws "Exception" exception.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;    
                }
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Group"/> is approved.
        /// </summary>
        /// <value><c>true</c> if approved; otherwise, <c>false</c>.</value>
        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>The access level.</value>
        public eAccessLevel AccessLevel
        {
            get { return accessLevel; }
            set { accessLevel = value; }
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// Gets or sets the mebers.
        /// </summary>
        /// <value>The mebers.</value>
        public int ActiveMembers
        {
            get { return activeMembers; }
            set { activeMembers = value; }
        }

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        /// <summary>
        /// Gets or sets the terms the user must agree on joining the group.
        /// </summary>
        /// <value>The join terms.</value>
        public string JoinTerms
        {
            get { return joinTerms; }
            set { joinTerms = value; }
        }

        /// <summary>
        /// Gets or sets the question the user must answer on joining moderated group.
        /// </summary>
        /// <value>The question.</value>
        public string JoinQuestion
        {
            get { return joinQuestion; }
            set { joinQuestion = value; }
        }

        public ulong Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }

        public int? MinAge
        {
            get {return minAge; }
            set { minAge = value; }
        }

        public bool Autojoin
        {
            get { return autojoin; }
            set { autojoin = value; }
        }

        public string AutojoinCountry
        {
            get { return autojoinCountry; }
            set { autojoinCountry = value; }
        }

        public string AutojoinRegion
        {
            get { return autojoinRegion; }
            set { autojoinRegion = value; }
        }

        public string AutojoinCity
        {
            get { return autojoinCity; }
            set { autojoinCity = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all groups from DB and sorts them by specified sort column.
        /// If there are no groups in DB it returns an empty array.
        /// </summary>
        /// <returns>An array of groups or an empty array if there are no groups in DB.</returns>
        public static Group[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, null, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches all approved or non approved groups and sorts them by specified sort column.
        /// If there are no groups by specified arguments in DB it returns an empty array.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <returns></returns>
        public static Group[] Fetch(bool approved, eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, approved, null, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches the specified number of groups which are approved or non approved
        /// and sorts them by specified sort column.
        /// If there are no groups by specified arguments in DB it returns an empty array.
        /// </summary>
        /// <param name="numberOfGroups">The number of groups.</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static Group[] Fetch(int numberOfGroups, bool approved, eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, approved, null, null, null, null, numberOfGroups, sortColumn);
        }

        /// <summary>
        /// Fetches group by specified id from DB.
        /// If the group doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>If the specified group doesn't exist returns NULL.</returns>
        public static Group Fetch(int id)
        {
            string cacheKey = String.Format("Group_Fetch_Id_{0}", id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Group;
            }

            Group[] groups = Fetch(id, null, null, null, null, null, null, null, null, null, eSortColumn.None);

            if (groups.Length > 0 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, groups[0], null, DateTime.Now.AddMinutes(10),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            
            return groups.Length > 0 ? groups[0] : null;
        }

        /// <summary>
        /// Fetches group by specified name from DB.
        /// If the group doesn't exist returns NULL.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>If the specified group doesn't exist returns NULL.</returns>
        public static Group Fetch(string name)
        {
            Group[] groups = Fetch(null, name, null, null, null, null, null, null, null, null, eSortColumn.None);

            if (groups.Length > 0)
            {
                return groups[0];
            }
            else
            {
                return null;
            }
        }

        public static Group[] Fetch(bool autojoin)
        {
            return Fetch(null, null, null, null, null, null, null, null, autojoin, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches groups by specified arguments.
        /// It returns an empty array if there are no groups in DB by specified arguments.
        /// If these arguments are null it returns all groups from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="approved">The approved.</param>
        /// <param name="accessLevel">The access level.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static Group[] Fetch(int? id, string name, string description, DateTime? dateCreated,
                                    bool? approved, eAccessLevel? accessLevel, string owner,
                                    int? activeMembers, bool? autojoin, int? numberOfGroups, eSortColumn sortColumn)
        {
            using (var db = new Model.AspNetDatingDataContext())
            {
                var groups = from g in db.Groups
                             where (!id.HasValue || id == g.g_id)
                                   && (name == null || name == g.g_name)
                                   && (description == null || description == g.g_description)
                                   //&& (!dateCreated.HasValue || dateCreated == g.g_datecreated)
                                   && (!approved.HasValue || approved == g.g_approved)
                                   && (!accessLevel.HasValue || accessLevel == (eAccessLevel?) g.g_accesslevel)
                                   && (owner == null || owner == g.g_owner)
                                   && (!activeMembers.HasValue || activeMembers == g.g_activemembers)
                                   && (!autojoin.HasValue || autojoin == g.g_autojoin)
                             select new Group
                                        {
                                            id = g.g_id,
                                            name = g.g_name,
                                            description = g.g_description,
                                            dateCreated = g.g_datecreated,
                                            approved = g.g_approved,
                                            accessLevel = (eAccessLevel) g.g_accesslevel,
                                            owner = g.g_owner,
                                            activeMembers = g.g_activemembers,
                                            joinTerms = g.g_jointerms,
                                            joinQuestion = g.g_joinquestion,
                                            permissions = (ulong) g.g_permissions,
                                            minAge = g.g_minage,
                                            autojoin = g.g_autojoin,
                                            autojoinCountry = g.g_autojoincountry,
                                            autojoinRegion = g.g_autojoinregion,
                                            autojoinCity = g.g_autojoincity
                                        };

                switch (sortColumn)
                {
                    case eSortColumn.None:
                        break;
                    case eSortColumn.DateCreated:
                        groups = groups.OrderByDescending(g => g.dateCreated);
                        break;
                    case eSortColumn.Name:
                        groups = groups.OrderBy(g => g.name);
                        break;
                    case eSortColumn.Owner:
                        groups = groups.OrderBy(g => g.owner);
                        break;
                    case eSortColumn.ActiveMembers:
                        groups = groups.OrderByDescending(g => g.activeMembers);
                        break;
                    case eSortColumn.AccessLevel:
                        break;
                    default:
                        break;
                }

                if (numberOfGroups.HasValue)
                    groups = groups.Take(numberOfGroups.Value);

                return groups.ToArray();
            }

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchGroups", id, name, description, dateCreated,
            //                            approved, accessLevel, owner, activeMembers, numberOfGroups, sortColumn);

            //    var lGroups = new List<Group>();

            //    while (reader.Read())
            //    {
            //        var group = new Group
            //                        {
            //                            id = ((int) reader["ID"]),
            //                            name = ((string) reader["Name"]),
            //                            description = ((string) reader["Description"]),
            //                            dateCreated = ((DateTime) reader["DateCreated"]),
            //                            approved = ((bool) reader["Approved"]),
            //                            accessLevel = ((eAccessLevel) reader["AccessLevel"]),
            //                            owner = ((string) reader["Owner"]),
            //                            activeMembers = ((int) reader["ActiveMembers"]),
            //                            joinTerms = ((string) reader["JoinTerms"]),
            //                            joinQuestion = ((string) reader["JoinQuestion"]),
            //                            permissions = Convert.ToUInt64(reader["Permissions"]),
            //                            minAge = (reader["MinAge"] == DBNull.Value ? null : (int?) reader["MinAge"])
            //                        };

            //        lGroups.Add(group);
            //    }

            //    return lGroups.ToArray();
            //}
        }

        /// <summary>
        /// Searches groups by specified arguments.
        /// If there are no groups by specified arguments it returns an empty array.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="dateCreated">The date created.</param>
        /// <param name="approved">The approved.</param>
        /// <param name="accessLevel">The access level.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static int[] Search(int? categoryID, string name, string description, DateTime? dateCreated,
                                    bool? approved, eAccessLevel? accessLevel, string owner, int? activeMembers,
                                    string keyword, bool searchInDescription, int? numberOfGroups, eSortColumn sortColumn)
        {
            using (var db = new Model.AspNetDatingDataContext())
            {
                var groups = from g in db.Groups
                             where (name == null || name == g.g_name)
                                   && (description == null || description == g.g_description)
                                   && (!approved.HasValue || approved == g.g_approved)
                                   && (!accessLevel.HasValue || accessLevel == (eAccessLevel) g.g_accesslevel)
                                   && (owner == null || owner == g.g_owner)
                                   && (!activeMembers.HasValue || activeMembers == g.g_activemembers)
                                   &&
                                   (keyword == null || g.g_name.Contains(keyword) ||
                                    (searchInDescription && g.g_description.Contains(keyword)))
                                   &&
                                   (!categoryID.HasValue ||
                                    (from gc in db.GroupsCategories where categoryID == gc.c_id select gc.g_id).Contains
                                        (g.g_id))
                             select new
                                        {
                                            id = g.g_id,
                                            dateCreated = g.g_datecreated,
                                            name = g.g_name,
                                            owner = g.g_owner,
                                            activeMembers = g.g_activemembers
                                        };

                switch (sortColumn)
                {
                    case eSortColumn.None:
                        break;
                    case eSortColumn.DateCreated:
                        groups = groups.OrderByDescending(g => g.dateCreated);
                        break;
                    case eSortColumn.Name:
                        groups = groups.OrderBy(g => g.name);
                        break;
                    case eSortColumn.Owner:
                        groups = groups.OrderBy(g => g.owner);
                        break;
                    case eSortColumn.ActiveMembers:
                        groups = groups.OrderByDescending(g => g.activeMembers);
                        break;
                    case eSortColumn.AccessLevel:
                        break;
                    default:
                        break;
                }

                if (numberOfGroups.HasValue)
                    groups = groups.Take(numberOfGroups.Value);

                return (from g in groups select g.id).ToArray();
            }

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = SqlHelper.ExecuteReader(conn, "SearchGroups",
            //                                                   categoryID, name, description, dateCreated,
            //                                                   approved, accessLevel, owner, activeMembers,
            //                                                   keyword, searchInDescription, numberOfGroups, sortColumn);

            //    List<int> lGroupIDs = new List<int>();

            //    while (reader.Read())
            //    {
            //        lGroupIDs.Add((int)reader["ID"]);
            //    }

            //    return lGroupIDs.ToArray();
            //}
        }

        /// <summary>
        /// Fetches groups pending invitations for the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static int FetchPendingInvitations(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, "FetchGroupsPendingInvitations", username));
            }
        }

        /// <summary>
        /// Fetches groups by specified category ID from DB.
        /// If there are no groups in DB for specified category ID it returns an empty array.
        /// </summary>
        /// <param name="categoryID">The id.</param>
        /// <returns>Group array or an emptry array if there are no groups in DB for specified category ID.</returns>
        public static Group[] FetchGroupsByCategory(int categoryID)
        {
            return FetchGroupsBy(categoryID, null, null);
        }

        /// <summary>
        /// Fetches all approved or non approved groups by specified category ID from DB.
        /// If there are no groups in DB for specified arguments it returns an empty array.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <returns></returns>
        public static Group[] FetchGroupsByCategory(int categoryID, bool approved)
        {
            return FetchGroupsBy(categoryID, null, approved);
        }

        /// <summary>
        /// Fetches groups by specified username from DB.
        /// If there are no groups in DB for specified username it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Group array or an emptry array if there are no groups in DB for specified username.</returns>
        public static Group[] FetchGroupsByUsername(string username)
        {
            return FetchGroupsBy(null, username, null);
        }

        /// <summary>
        /// Fetches all approved or non approved groups by specified username from DB.
        /// If there are no groups in DB for specified arguments it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <returns></returns>
        public static Group[] FetchGroupsByUsername(string username, bool approved)
        {
            return FetchGroupsBy(null, username, approved);
        }

        /// <summary>
        /// Fetches groups by specified arguments from DB.
        /// If there are no groups in DB for specified arguments it returns an empty array.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="approved">The approved.</param>
        /// <returns></returns>
        private static Group[] FetchGroupsBy(int? categoryID, string username, bool? approved)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = null;

                if (categoryID != null)
                {
                    reader = SqlHelper.ExecuteReader(conn, "FetchGroupsByCategory", categoryID, approved);
                }
                else
                {
                    reader = SqlHelper.ExecuteReader(conn, "FetchGroupsByUsername", username, approved);
                }

                List<Group> groups = new List<Group>();

                while (reader.Read())
                {
                    Group group = new Group();

                    group.id = (int)reader["ID"];
                    group.name = (string)reader["Name"];
                    group.description = (string)reader["Description"];
                    group.dateCreated = (DateTime)reader["DateCreated"];
                    group.approved = (bool)reader["Approved"];
                    group.accessLevel = (eAccessLevel)reader["AccessLevel"];
                    group.owner = (string)reader["Owner"];
                    group.activeMembers = (int) reader["ActiveMembers"];
                    group.joinTerms = (string)reader["JoinTerms"];
                    group.joinQuestion = (string)reader["JoinQuestion"];
                    group.permissions = Convert.ToUInt64(reader["Permissions"]);
                    group.minAge = reader["MinAge"] == DBNull.Value ? null : (int?)reader["MinAge"];
                    group.autojoin = (bool) reader["Autojoin"];
                    group.autojoinCountry = reader["AutojoinCountry"] == DBNull.Value
                                                ? null
                                                : (string) reader["AutojoinCountry"];
                    group.autojoinRegion = reader["AutojoinRegion"] == DBNull.Value
                                               ? null
                                               : (string) reader["AutojoinRegion"];
                    group.autojoinCity = reader["AutojoinCity"] == DBNull.Value ? null : (string) reader["AutojoinCity"];

                    groups.Add(group);
                }

                return groups.ToArray();
            }
        }

        public static int[] SearchGroupsByUsername(string username, eSortColumn eSortColumn)
        {
            return SearchGroupsByUsername(username, null, eSortColumn);
        }

        private static int[] SearchGroupsByUsername(string username, bool? approved, eSortColumn eSortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "SearchGroupsByUsername", username, approved, eSortColumn);

                List<int> lGroupsIDs = new List<int>();

                while (reader.Read())
                {
                    lGroupsIDs.Add((int)reader["ID"]);
                }

                return lGroupsIDs.ToArray();
            }
        }

        /// <summary>
        /// Fetches all approved or non approved groups by specified category ID.
        /// It returns 0 if there are no groups in DB for the specified arguments.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <returns></returns>
        public static int FetchGroupsCount(int categoryID, bool approved)
        {
            return FetchGroupsByCategoryCount(categoryID, approved);
        }

        /// <summary>
        /// Counts how many groups the specified category ID has.
        /// It returns 0 if there are no groups in DB for the specified category ID.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <returns></returns>
        public static int FetchGroupsCount(int categoryID)
        {
            return FetchGroupsByCategoryCount(categoryID, null);
        }

        /// <summary>
        /// Calculates groups by specified arguments.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="approved">The approved.</param>
        /// <returns></returns>
        private static int FetchGroupsByCategoryCount(int categoryID, bool? approved)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, "FetchGroupsByCategoryCount", categoryID, approved));
            }
        }

        /// <summary>
        /// Saves this group in DB. It throws Exception exception if group name already exists in DB.
        /// When is used to update this group 'IsNameUsed' method should be called first.
        /// Throws "Exception" exception.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown if group name already exists in DB.
        /// </exception>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveGroup",
                                                        id, name, description, dateCreated,
                                                        approved, accessLevel, owner, activeMembers,
                                                        joinTerms, joinQuestion, permissions, minAge,
                                                        autojoin, autojoinCountry, autojoinRegion, autojoinCity);
                if (id == null)
                {
                    int errorCode = Convert.ToInt32(result);
                    if (errorCode == -1)
                    {
                        throw new Exception(Lang.Trans("Group name already exists."));
                    }

                    id = Convert.ToInt32(result);
                }
                else
                {
                    string cacheKey = String.Format("Group_Fetch_Id_{0}", id);
                    if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes group from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "DeleteGroup", (int?) 3600, new [] {new SqlParameter("@ID", id)});
            }

            string cacheKey = String.Format("Group_Fetch_Id_{0}", id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Deletes the inactive groups.
        /// </summary>
        /// <param name="daysOfInactivity">The days of inactivity.</param>
        public static void DeleteInactiveGroups(int daysOfInactivity)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteInactiveGroups", DateTime.Now - TimeSpan.FromDays(daysOfInactivity));
            }
        }

        /// <summary>
        /// Moves the specified group from one category to another.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="oldCategoryID">The old category ID.</param>
        /// <param name="newCategoryID">The new category ID.</param>
        public void MoveGroupToCategory(int groupID, int oldCategoryID, int newCategoryID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "MoveGroupToCategory",
                                                        groupID, oldCategoryID, newCategoryID);
            }
        }

        /// <summary>
        /// Assigns the specified group to the specified category.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="categoryID">The category ID.</param>
        public void AssignGroupToCategory(int groupID, int categoryID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "AssignRemoveGroupToFromCategory",
                                                        groupID, categoryID, eAction.Assign);
            }
        }

        /// <summary>
        /// Removes the specified group from the specified category.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="categoryID">The category ID.</param>
        public void RemoveGroupFromCategory(int groupID, int categoryID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "AssignRemoveGroupToFromCategory",
                                                        groupID, categoryID, eAction.Remove);
            }
        }

        /// <summary>
        /// Sets the categories for this instance.
        /// </summary>
        /// <param name="categoriesIDs">The categories IDs.</param>
        public void SetCategories(int[] categoriesIDs)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteGroupFromCategories", id);

                foreach (int categoryID in categoriesIDs)
                {
                    SqlHelper.ExecuteNonQuery(conn, "AssignRemoveGroupToFromCategory", id, categoryID, eAction.Assign);
                }
            }
        }

        /// <summary>
        /// Gets the categories string. Categories are separated with "," symbol.
        /// It returns an empty string if this group doesn't have category.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public string GetCategoriesString(int groupID)
        {
            Category[] categories = Category.FetchCategoriesByGroup(groupID);
            if (categories.Length > 0)
            {
                List<string> lCategories = new List<string>();

                foreach (Category category in categories)
                {
                    lCategories.Add(category.Name);
                }
                return String.Join(", ", lCategories.ToArray());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Determines whether the specified group name is used.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>
        /// 	<c>true</c> if [is name used] [the specified group name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNameUsed(string groupName)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return
                    (bool)SqlHelper.ExecuteScalar(conn, "IsGroupNameUsed", groupName);
            }
        }

        /// <summary>
        /// Loads the icon for specified group ID.
        /// Returns 'PHOTO NOT AVAILABLE' image if the group doesn't have an icon.
        /// Returns NULL if the specified group doesn't exist.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns>Image object.</returns>
        public static Image LoadIcon(int groupID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchGroupIcon", groupID);

                if (reader.Read())
                {
                    if (reader["Image"] is DBNull)
                    {
                        return Image.FromFile(HttpContext.Current.Server.MapPath("~/Images") + "/no_photo_large.png");
                    }
                    var buffer = (byte[])reader["Image"];
                    var imageStream = new MemoryStream(buffer);
                    return Image.FromStream(imageStream);
                }
                return null;
            }
        }

        /// <summary>
        /// Saves the icon for specified group ID.
        /// </summary>
        /// <param name="goupID">The goup ID.</param>
        /// <param name="image">The image.</param>
        public static void SaveIcon(int goupID, Image image)
        {
            if (image.Width > Config.Groups.IconMaxWidth || image.Height > Config.Groups.IconMaxHeight)
            {
                image = Photo.ResizeImage(image, Config.Groups.IconMaxWidth, Config.Groups.IconMaxHeight);
            }

            MemoryStream imageStream = new MemoryStream();
            image.Save(imageStream, ImageFormat.Jpeg);
            imageStream.Position = 0;
            BinaryReader reader = new BinaryReader(imageStream);
            byte[] bytesImage = reader.ReadBytes((int)imageStream.Length);

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "SaveGroupIcon", goupID, bytesImage);
            }
        }

        /// <summary>
        /// Deletes the icon for specified group ID.
        /// </summary>
        /// <param name="goupID">The goup ID.</param>
        public static void DeleteIcon(int goupID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteGroupIcon", goupID);
            }
        }

        public bool IsPermissionEnabled(eGroupPermissions permission)
        {
            return (Permissions & (ulong)permission) == (ulong)permission;
        }

        #endregion
    }

    [Serializable]
    public class GroupSearchResults : SearchResults<int, Group>
    {
        public int[] Groups
        {
            get
            {
                if (Results == null)
                    return new int[0];
                else
                    return Results;
            }
            set { Results = value; }
        }

        public new int GetTotalPages(int groupsPerPage)
        {
            return base.GetTotalPages(groupsPerPage);
        }

        protected override Group LoadResult(int id)
        {
            return Group.Fetch(id);
        }

        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="groupsPerPage">The groups per page.</param>
        /// <returns></returns>
        public new Group[] GetPage(int Page, int groupsPerPage)
        {
            return base.GetPage(Page, groupsPerPage);
        }

        public Group[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }

    [Serializable]
    public class BasicSearchGroup
    {
        #region Properties

        private int? categoryID = null;

        public int CategoryID
        {
            get { return categoryID.Value; }
            set { categoryID = value; }
        }

        private string owner = null;

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private string name = null;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private DateTime? dateCreated = null;

        public DateTime DateCreated
        {
            get { return dateCreated.Value; }
            set { dateCreated = value; }
        }

        private bool? approved = null;

        public bool Approved
        {
            get { return approved.Value ; }
            set { approved = value; }
        }

        private Group.eAccessLevel? accessLevel = null;

        public Group.eAccessLevel AccessLevel
        {
            get { return accessLevel.Value; }
            set { accessLevel = value; }
        }

        private Group.eSortColumn sortColumn;

        public Group.eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        private bool sortAsc;

        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }

        #endregion

        public BasicSearchGroup()
        {
            // Set defaults
            SortColumn = Group.eSortColumn.DateCreated;
            SortAsc = false;
        }

        public GroupSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "BasicSearchGroup",
                                            categoryID, owner, name,
                                            dateCreated, approved, accessLevel, sortColumn);

                List<int> lResults = new List<int>();

                while (reader.Read())
                {
                    lResults.Add((int)reader["ID"]);
                }

                if (!sortAsc) lResults.Reverse();

                if (lResults.Count > 0)
                {
                    GroupSearchResults results = new GroupSearchResults();
                    results.Groups = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    // ulong permissions;
    // permissions |= ViewGroupNonMembers    - add ViewGroupNonMembers
    // if (permissions & ViewGroupNonMembers != 0)   - check if ViewGroupNonMembers is set
    // permissions &= ~ViewGroupNonMembers   - remove ViewGroupNonMembers

    public enum eGroupPermissionType
    {
        ViewGroup,
        ViewMessageBoard,
        ViewGallery,
        ViewMembers,
        ViewEvents,
        UploadPhoto,
        UseChat,
        AddTopic,
        AddPost,
        AddEvent
    }

    public enum eGroupPermissions : ulong
    {
        ViewGroupNonMembers                 = 1L,
        ViewGroupMembers                    = 1L << 1,
        ViewGroupVip                        = 1L << 2,
        ViewMessageBoardNonMembers          = 1L << 3,
        ViewMessageBoardMembers             = 1L << 4,
        ViewMessageBoardVip                 = 1L << 5,
        ViewGalleryNonMembers               = 1L << 6,
        ViewGalleryMembers                  = 1L << 7,
        ViewGalleryVip                      = 1L << 8,
        ViewMembersNonMembers               = 1L << 9,
        ViewMembersMembers                  = 1L << 10,
        ViewMembersVip                      = 1L << 11,
        UploadPhotoNonMembers               = 1L << 12,
        UploadPhotoMembers                  = 1L << 13,
        UploadPhotoVip                      = 1L << 14,
        UseChatNonMembers                   = 1L << 15,
        UseChatMembers                      = 1L << 16,
        UseChatVip                          = 1L << 17,
        AddTopicNonMembers                  = 1L << 18,
        AddTopicMembers                     = 1L << 19,
        AddTopicVip                         = 1L << 20,
        AddPostNonMembers                   = 1L << 21,
        AddPostMembers                      = 1L << 22,
        AddPostVip                          = 1L << 23,
        ViewEventsNonMembers                = 1L << 24,
        ViewEventsMembers                   = 1L << 25,
        ViewEventsVip                       = 1L << 26,
        AddEventNonMembers                  = 1L << 27,
        AddEventMembers                     = 1L << 28,
        AddEventVip                         = 1L << 29
    }

    class InactiveGroups
    {
        private static Timer timer;
        private static bool mailerLock = false;

        public static void InitializeTimer()
        {
            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromDays(7).TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            // Run processing the 1st time
            timer_Elapsed(null, null);
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("InactiveGroups_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromDays(Config.Groups.AutoDeleteGroups))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DeleteInactiveGroups), null);
                DBSettings.Set("InactiveGroups_LastTimerTick", DateTime.Now);
            }
        }

        private static void DeleteInactiveGroups(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                Group.DeleteInactiveGroups(Config.Groups.AutoDeleteGroups);

            }
            catch (Exception err)
            {
                Global.Logger.LogError("InactiveGroups", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }
}
