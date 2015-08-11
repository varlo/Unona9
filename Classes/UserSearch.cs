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
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using AspNetDating.Model;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    [Serializable]
    public class UserSearchResults : SearchResults<string, User>
    {
        public string[] Usernames
        {
            get { return Results; }
            set { Results = value; }
        }

        public int GetTotalPages()
        {
            return GetTotalPages(Config.Search.UsersPerPage);
        }

        public new int GetTotalPages(int usersPerPage)
        {
            return base.GetTotalPages(usersPerPage);
        }

        protected override User LoadResult(string id)
        {
            try
            {
                return User.Load(id);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Use this method to get the search results
        /// Number of users per page is defined in Config.Search
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <returns>Array of usernames</returns>
        public User[] GetPage(int Page)
        {
            return GetPage(Page, Config.Search.UsersPerPage);
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="usersPerPage">usersPerPage</param>
        /// <returns>Array of usernames</returns>
        public new User[] GetPage(int Page, int usersPerPage)
        {
            return base.GetPage(Page, usersPerPage);
        }

        public User[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }


    [Serializable]
    public class BasicSearch
    {
        #region Properties

        private bool active;
        public bool active_isSet;
        private string city = "";
        private string country = "";
        private bool deleted;
        public bool deleted_isSet;
        private string email;

        private bool? faceControlApproved = Config.CommunityFaceControlSystem.EnableCommunityFaceControl
                                                ? (bool?) true
                                                : null;

        private User.eGender gender;
        public bool gender_isSet;
        private bool hasAnswer;
        public bool hasAnswer_isSet;
        private bool hasPhoto;
        public bool hasPhoto_isSet;

        private User.eGender interestedIn;
        public bool interestedIn_isSet;
        private string ip;
        private int? languageID;
        private DateTime? lastLogin;
        private DateTime? userSince;
        private int maxAge = Config.Users.MaxAge;

        private int minAge = Config.Users.MinAge;
        private string name;
        private bool paid;
        public bool paid_isSet;
        private bool sortAsc;
        private string sortColumn;
        private bool? spamSuspected;
        private string state = "";
        private string username;
        private bool visible;
        public bool visible_isSet;
        private string zip = "";

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                gender_isSet = true;
            }
        }

        public User.eGender InterestedIn
        {
            get { return interestedIn; }
            set
            {
                interestedIn = value;
                interestedIn_isSet = true;
            }
        }

        public int MinAge
        {
            get { return minAge; }
            set
            {
                if (minAge < Config.Users.MinAge)
                    minAge = Config.Users.MinAge;
                else
                    minAge = value;
            }
        }

        public int MaxAge
        {
            get { return maxAge; }
            set
            {
                if (maxAge > Config.Users.MaxAge)
                    maxAge = Config.Users.MaxAge;
                else
                    maxAge = value;
            }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Country
        {
            get { return country; }
            set
            {
                //if (Config.Users.CountriesHash[value] != null)
                //    country = (string) Config.Users.CountriesHash[value];
                //else
                country = value;
            }
        }

        public string State
        {
            get { return state; }
            set
            {
                //if (Config.Users.StateHash[value] != null)
                //{
                //    if ((string) Config.Users.StateHash[value] == "NA")
                //        state = "";
                //    else
                //        state = (string) Config.Users.StateHash[value];
                //}
                //else
                state = value;
            }
        }

        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }

        public bool HasPhoto
        {
            get { return hasPhoto; }
            set
            {
                hasPhoto = value;
                hasPhoto_isSet = true;
            }
        }

        public bool HasAnswer
        {
            get { return hasAnswer; }
            set
            {
                hasAnswer = value;
                hasAnswer_isSet = true;
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                visible_isSet = true;
            }
        }

        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                active_isSet = true;
            }
        }

        public bool Deleted
        {
            get { return deleted; }
            set
            {
                deleted = value;
                deleted_isSet = true;
            }
        }

        public bool Paid
        {
            get { return paid; }
            set
            {
                paid = value;
                paid_isSet = true;
            }
        }

        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public bool? SpamSuspected
        {
            get { return spamSuspected; }
            set { spamSuspected = value; }
        }

        public bool? FaceControlApproved
        {
            get { return faceControlApproved; }
            set { faceControlApproved = value; }
        }

        public int? LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
        }

        public DateTime? LastLogin
        {
            get { return lastLogin; }
            set { lastLogin = value; }
        }

        public DateTime? UserSince
        {
            get { return userSince; }
            set { userSince = value; }
        }

        public bool OnlineOnly { get; set; }

        public string SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }


        public Location FromLocation
        {
            get;
            set;
        }

        public double? Distance
        {
            get;
            set;
        }

        #endregion

        public BasicSearch()
        {
            // Set defaults
            HasAnswer = true;
            Visible = true;
            Active = true;
            Deleted = false;
            SortColumn = "LastOnline";
            SortAsc = false;
        }

        public UserSearchResults GetResults()
        {
            DateTime fromBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1)*365.25));
            DateTime toBirthdate = DateTime.Now.Subtract(TimeSpan.FromDays(minAge*365.25));

            using (var db = new AspNetDatingDataContext())
            {
                var q = (from u in db.Users
                           where (username == null || u.u_username.Contains(username))
                                 && (name == null || u.u_name.Contains(name))
                                 && (!gender_isSet || u.u_gender == (int) gender)
                                 && (!(Config.Users.InterestedInFieldEnabled && interestedIn_isSet) ||
                                     u.u_interested_in == (int) interestedIn)
                                 &&
                                 (u.u_birthdate >= fromBirthdate && (u.u_gender != 3 || u.u_birthdate2 >= fromBirthdate))
                                 && (u.u_birthdate <= toBirthdate && (u.u_gender != 3 || u.u_birthdate2 <= toBirthdate))
                                 && (!hasPhoto_isSet ||
                                     (hasPhoto &&
                                      (from p in db.Photos where p.p_approved select p.u_username).Contains(u.u_username)) ||
                                     (!hasPhoto &&
                                      !(from p in db.Photos where p.p_approved select p.u_username).Contains(
                                           u.u_username))
                                    )
                               && ((FromLocation != null && Distance.HasValue) || string.IsNullOrEmpty(city) || city == u.u_city)
                               && ((FromLocation != null && Distance.HasValue) || string.IsNullOrEmpty(country) || country == u.u_country)
                               && ((FromLocation != null && Distance.HasValue) || string.IsNullOrEmpty(state) || state == u.u_state)
                             //&& (string.IsNullOrEmpty(zip) || zip == u.u_zip_code)
                                 && (!hasAnswer_isSet ||
                                     (hasAnswer &&
                                      (from a in db.ProfileAnswers select a.u_username).Contains(u.u_username)) ||
                                     (!hasAnswer &&
                                      !(from a in db.ProfileAnswers select a.u_username).Contains(u.u_username))
                                    )
                                 && (ip == null || ip == u.u_signup_ip)
                                 && (email == null || email == u.u_email)
                                 && (!spamSuspected.HasValue || spamSuspected == u.u_spamsuspected)
                                 && (!faceControlApproved.HasValue || faceControlApproved == u.u_face_control_approved)
                                 && (!visible_isSet || visible == u.u_profilevisible)
                                 && (!active_isSet || active == u.u_active)
                                 && (!deleted_isSet || deleted == u.u_deleted)
                                 && (!paid_isSet || paid == u.u_paid_member)
                                 && (!languageID.HasValue || languageID == u.l_id)
                                 && (!lastLogin.HasValue || u.u_lastlogin.Date == lastLogin)
                                 && (!userSince.HasValue || u.u_usersince.Date == userSince)
                           select u
                          );

                if (FromLocation != null && Distance.HasValue)
                {
                    RadiusBox radBox = RadiusBox.Create(FromLocation, Distance.Value);

                    var predicate = PredicateBuilder.True<Model.User>();
                    predicate = predicate.And(u => !u.u_latitude.HasValue || u.u_latitude >= radBox.BottomLine);
                    predicate = predicate.And(u => !u.u_latitude.HasValue || u.u_latitude <= radBox.TopLine);
                    predicate = predicate.And(u => !u.u_longitude.HasValue || u.u_longitude >= radBox.LeftLine);
                    predicate = predicate.And(u => !u.u_longitude.HasValue || u.u_longitude <= radBox.RightLine);
                    predicate = predicate.And(u => Math.Sqrt(Math.Pow(69.1 * (double)(u.u_latitude - FromLocation.Latitude), 2.0) +
                        Math.Pow(53 * ((double)u.u_longitude - FromLocation.Longitude), 2.0)
                        ) <= radBox.Radius);


                    q = q.Where(predicate);
                }

                IEnumerable<Model.User> query = q.ToArray();


                var res = from u in query
                          select new
                                  {
                                      u.u_username,
                                      u.u_name,
                                      u.u_email,
                                      u.u_signup_ip,
                                      u.u_gender,
                                      u.u_lastonline,
                                      u.u_usersince
                                  };

                switch (sortColumn)
                {
                    case "Username":
                        res = res.OrderBy(u => u.u_username);
                        break;
                    case "Name":
                        res = res.OrderBy(u => u.u_name);
                        break;
                    case "Email":
                        res = res.OrderBy(u => u.u_email);
                        break;
                    case "SignupIP":
                        res = res.OrderBy(u => u.u_signup_ip);
                        break;
                    case "Gender":
                        res = res.OrderBy(u => u.u_gender);
                        break;
                    case "LastOnline":
                        res = res.OrderBy(u => u.u_lastonline);
                        break;
                    case "SignupDate":
                        res = res.OrderBy(u => u.u_usersince);
                        break;
                }


                List<string> lUsernames = (from r in res select r.u_username).ToList();

                if (sortColumn == "LastOnline")
                    UserSession.SortUsersByOnlineStatus(lUsernames, OnlineOnly);

                if (!sortAsc) lUsernames.Reverse();

                if (lUsernames.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lUsernames.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    [Serializable]
    public class UsernameSearch
    {
        #region Properties

        private bool? hasAnswer;
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool? HasAnswer
        {
            get { return hasAnswer; }
            set { hasAnswer = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "UsernameSearchUsers",
                                            "%" + username + "%", hasAnswer);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var _username = (string) reader["Username"];

                    lResults.Add(_username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class DistanceSearch
    {
        #region Properties From BasicSearch

        private User.eGender gender;
        private bool? hasAnswer;
        private int maxAge = Config.Users.MaxAge;

        private int minAge = Config.Users.MinAge;
        private bool photoReq;

        public User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int MinAge
        {
            get { return minAge; }
            set
            {
                if (minAge < Config.Users.MinAge)
                    minAge = Config.Users.MinAge;
                else
                    minAge = value;
            }
        }

        public int MaxAge
        {
            get { return maxAge; }
            set
            {
                if (maxAge > Config.Users.MaxAge)
                    maxAge = Config.Users.MaxAge;
                else
                    maxAge = value;
            }
        }

        public bool PhotoReq
        {
            get { return photoReq; }
            set { photoReq = value; }
        }

        public bool? HasAnswer
        {
            get { return hasAnswer; }
            set { hasAnswer = value; }
        }

        #endregion

        #region DistanceSearchRelated Properties

        //private string fromZipCode;

        //public string FromZipCode
        //{
        //    get { return fromZipCode; }
        //    set { fromZipCode = value; }
        //}

        private double distance;
        private Location fromLocation;
        private int maxResults;

        public Location FromLocation
        {
            get { return fromLocation; }
            set { fromLocation = value; }
        }

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public int MaxResults
        {
            get { return maxResults; }
            set { maxResults = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            if ( /*fromZipCode == null || fromZipCode == "" || */distance <= 0.0 || maxResults < 0)
                throw new ArgumentException("Invalid values for Distance Search");

            //Location location = ZipCode.DoLookupByZipCode(fromZipCode);

            if (fromLocation == null)
                throw new ArgumentException("There's no data for your location in our database");

            string[] usernames = Config.Users.GetUsersWithinRadius(fromLocation, hasAnswer, gender, maxAge, minAge,
                                                                   photoReq,
                                                                   distance, maxResults);

            //Location[] locations = Radius.UserLocationsWithinRadius(location, gender, maxAge, minAge, photoReq,
            //                                                        distance, maxResults);

            //string zipcodes = ZipCode.CreateCommaDelimitedZipCodeString(locations);

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader =
            //        SqlHelper.ExecuteReader(
            //            conn,
            //            "SearchUsersByZipCodes",
            //            (int) gender,
            //            DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1)*365.25)),
            //            DateTime.Now.Subtract(TimeSpan.FromDays(minAge*365.25)),
            //            photoReq,
            //            zipcodes,
            //            maxResults);

            //    List<string> lResults = new List<string>();

            //    while (reader.Read())
            //    {
            //        string username = (string) reader["Username"];

            //        lResults.Add(username);
            //    }

            //if (lResults.Count > 0)
            if (usernames.Length > 0)
            {
                var results = new UserSearchResults();
                results.Usernames = usernames;
                return results;
            }
            else
                return null;
            //}
        }
    }

    public class OnlineSearch
    {
        #region Properties

        private User.eGender gender;
        public bool gender_isSet;
        private int? maxAge;

        private int? minAge;

        public User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                gender_isSet = true;
            }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader =
            //        SqlHelper.ExecuteReader(conn, "OnlineSearchUsers",
            //                                DateTime.Now.Subtract(TimeSpan.FromMinutes(Config.Users.OnlineCheckTime)),
            //                                gender_isSet ? (object)(int)gender : null,
            //                                maxAge == null ? null : (object) DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1) * 365.25)),
            //                                minAge == null ? null : (object) DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value * 365.25))
            //                                );

            //    List<string> lResults = new List<string>();

            //    while (reader.Read())
            //    {
            //        string username = (string) reader["Username"];

            //        lResults.Add(username);
            //    }

            //    if (lResults.Count > 0)
            //    {
            //        UserSearchResults results = new UserSearchResults();
            //        results.Usernames = lResults.ToArray();
            //        return results;
            //    }
            //    else
            //        return null;
            //}
            string[] onlineUsers = UserSession.getOnline(gender_isSet ? gender : (User.eGender?) null,
                                                         minAge, maxAge);

            if (onlineUsers.Length > 0)
            {
                var results = new UserSearchResults {Usernames = onlineUsers};
                return results;
            }
            return null;
        }
    }

    public class VideoBroadcastingSearch
    {
        #region Properties

        #endregion

        public UserSearchResults GetResults()
        {
            var lResults = new List<string>();

            foreach (var broadcast in VideoBroadcast.GetBroadcasts())
            {
                lResults.Add(broadcast.Key);
            }

            if (lResults.Count > 0)
            {
                var results = new UserSearchResults();
                results.Usernames = lResults.ToArray();
                return results;
            }
            else
                return null;
        }
    }

    public class BirthdaySearch
    {
        #region Fields

        private DateTime birthdate = DateTime.Now.Add(Config.Misc.TimeOffset);
        private User.eGender? gender;
        private int? maxAge;
        private int? minAge;

        #endregion

        #region Properties

        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; }
        }

        public User.eGender? Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "BirthdaySearchUsers",
                                            birthdate, gender,
                                            maxAge == null
                                                ? null
                                                : (object)
                                                  DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1)*365.25)),
                                            minAge == null
                                                ? null
                                                : (object) DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value*365.25)));

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string) reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class GroupUsersSearch
    {
        #region Fields

        public enum eSortColumn
        {
            JoinDate,
            LastOnline
        }

        private bool? active;
        private User.eGender? gender;
        private int? groupID;
        private DateTime? joinDate;
        private int maxAge = Config.Users.MaxAge;
        private int minAge = Config.Users.MinAge;
        private eSortColumn? sortColumn;
        private GroupMember.eType? type;

        #endregion

        #region Properties

        public int GroupID
        {
            get { return groupID.Value; }
            set { groupID = value; }
        }

        public User.eGender Gender
        {
            get { return gender.Value; }
            set { gender = value; }
        }

        public int MinAge
        {
            get { return minAge; }
            set
            {
                if (minAge < Config.Users.MinAge)
                    minAge = Config.Users.MinAge;
                else
                    minAge = value;
            }
        }

        public int MaxAge
        {
            get { return maxAge; }
            set
            {
                if (maxAge > Config.Users.MaxAge)
                    maxAge = Config.Users.MaxAge;
                else
                    maxAge = value;
            }
        }

        public GroupMember.eType Type
        {
            get { return type.Value; }
            set { type = value; }
        }

        public bool Active
        {
            get { return active.Value; }
            set { active = value; }
        }

        public DateTime JoinDate
        {
            get { return joinDate.Value; }
            set { joinDate = value; }
        }

        public eSortColumn? SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "GroupUsersSearch",
                                            groupID, (int?) gender,
                                            DateTime.Now.Subtract(TimeSpan.FromDays((maxAge + 1)*365.25)),
                                            DateTime.Now.Subtract(TimeSpan.FromDays(minAge*365.25)),
                                            (int?) type, active, joinDate, (int?) sortColumn);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string) reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class KeywordSearch
    {
        #region Properties

        private User.eGender interestedIn;
        public bool interestedIn_isSet;
        private string keyword;

        public string Keyword
        {
            get { return keyword; }
            set
            {
                keyword = value;
                keyword = keyword.Replace("%", "\\%");
                keyword = keyword.Replace("_", "\\_");
            }
        }

        public User.eGender InterestedIn
        {
            get { return interestedIn; }
            set
            {
                interestedIn = value;
                interestedIn_isSet = true;
            }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "KeywordSearchUsers",
                                            Keyword,
                                            Config.Users.InterestedInFieldEnabled && interestedIn_isSet
                                                ? (object) (int) interestedIn
                                                : null);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string) reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    [Serializable]
    public class CustomSearch : BasicSearch
    {
        #region Properties

        public ProfileAnswer[][] Answers { get; set; }

        #endregion

        public new UserSearchResults GetResults()
        {
            UserSearchResults results = base.GetResults();

            var lMatches = new List<string>();

            if (results != null)
            {
                foreach (string username in results.Usernames)
                {
                    bool isMatch = false;

                    for (int qId = 0; qId < Answers.Length; qId++)
                    {
                        isMatch = false;

                        for (int aId = 0; aId < Answers[qId].Length; aId++)
                        {
                            ProfileAnswer uAnswer;

                            try
                            {
                                uAnswer =
                                    ProfileAnswer.Fetch(username, Answers[qId][aId].Question.Id);
                            }
                            catch
                            {
                                continue;
                            }

                            foreach (string sourceVal in Answers[qId][aId].Value.Split(':'))
                                foreach (string destVal in uAnswer.Value.Split(':'))
                                    if (destVal == sourceVal)
                                    {
                                        isMatch = true;
                                        break;
                                    }
                        }

                        if (!isMatch)
                            break;
                    }

                    if (isMatch || Answers.Length == 0)
                        lMatches.Add(username);
                }

                results.Usernames = lMatches.ToArray();
            }

            return results;
        }
    }

    public class TopUsersSearch
    {
        #region Properties

        private User.eGender gender;

        private int minVotes = Config.Ratings.TopUsersMinVotes;

        private int usersCount = Config.Ratings.TopUsersCount;

        public User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int MinVotes
        {
            get { return minVotes; }
            set { minVotes = value; }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            string cacheKey = String.Format("TopUsersSearch_GetResults_{0}_{1}_{2}_{3}",
                                            gender, minVotes, Config.Users.TopUserMaxTimeAway, usersCount);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserSearchResults;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "TopUsersSearch", (int) gender, minVotes, Config.Users.TopUserMaxTimeAway,
                                            usersCount);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string) reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Insert(cacheKey, results, null, DateTime.Now.AddMinutes(1),
                                                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }

                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class TopPhotosSearch
    {
        #region Properties

        private User.eGender gender;

        private int minVotes = Config.Ratings.TopPhotosMinVotes;

        private int usersCount = Config.Ratings.TopPhotosCount;

        public User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int MinVotes
        {
            get { return minVotes; }
            set { minVotes = value; }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (var db = new AspNetDatingDataContext())
            {
                IQueryable<string> usernames = (from p in db.Photos
                                                join u in db.Users on p.u_username equals u.u_username
                                                where u.u_gender == (int) gender
                                                      &&
                                                      u.u_lastlogin.AddDays(Config.Users.TopUserMaxTimeAway) >=
                                                      DateTime.Now
                                                      && u.u_profilevisible && !u.u_deleted
                                                      && p.p_approved && !p.p_private
                                                      && (from r1 in db.PhotoRatings
                                                          where r1.p_id == p.p_id
                                                          select 1).Count() >= Config.Ratings.TopPhotosMinVotes
                                                orderby (from r in db.PhotoRatings
                                                         where r.p_id == p.p_id
                                                         select r).Average(r => (decimal) r.pr_rating) descending
                                                select p.u_username).Take(usersCount*usersCount);

                var lUsernames = new List<string>();
                foreach (string username in usernames)
                {
                    if (!lUsernames.Contains(username))
                        lUsernames.Add(username);
                    if (lUsernames.Count >= usersCount)
                        break;
                }
                if (lUsernames.Count > 0)
                {
                    var results = new UserSearchResults {Usernames = lUsernames.ToArray()};
                    return results;
                }
                return null;
            }
        }
    }

    public class TopModeratorsSearch
    {
        #region Properties

        private User.eGender gender;

        private int usersCount = Config.CommunityModeratedSystem.TopModeratorsCount;

        public User.eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            string cacheKey = String.Format("TopModeratorsSearch_GetResults_{0}_{1}_{2}",
                                            gender, Config.CommunityModeratedSystem.TopModeratorsMaxTimeAway, usersCount);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserSearchResults;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "TopModeratorsSearch", (int) gender,
                                            Config.CommunityModeratedSystem.TopModeratorsMaxTimeAway, usersCount);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string) reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Insert(cacheKey, results, null, DateTime.Now.AddMinutes(1),
                                                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }

                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class NewUsersSearch
    {
        #region Properties

        private User.eGender gender;
        private bool genderSearch;
        private bool? isFeaturedMember;
        private int? maxAge;
        private int? minAge;
        private bool photoReq;
        private bool profileReq = true;
        private bool showInvisible;
        private int usersCount;

        private DateTime usersSince = DateTime.MinValue;
        private bool usersSinceSearch;

        public User.eGender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                genderSearch = true;
            }
        }

        public DateTime UsersSince
        {
            get { return usersSince; }
            set
            {
                usersSince = value;
                usersSinceSearch = true;
            }
        }

        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        public bool PhotoReq
        {
            get { return photoReq; }
            set { photoReq = value; }
        }

        public bool ProfileReq
        {
            get { return profileReq; }
            set { profileReq = value; }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        public bool? IsFeaturedMember
        {
            get { return isFeaturedMember; }
            set { isFeaturedMember = value; }
        }

        public bool ShowInvisible
        {
            get { return showInvisible; }
            set { showInvisible = value; }
        }
        #endregion

        public UserSearchResults GetResults(bool useCache)
        {
            if (!useCache || HttpContext.Current == null)
                return GetResults();

            string cacheKey = String.Format("NewUsersSearch_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}",
                                            gender, usersSince, usersSinceSearch, usersCount, photoReq,
                                            profileReq, minAge, maxAge, showInvisible);

            Cache cache = HttpContext.Current.Cache;
            if (cache[cacheKey] != null && cache[cacheKey] is UserSearchResults)
            {
                return (UserSearchResults) cache[cacheKey];
            }
            else
            {
                UserSearchResults results = GetResults();
                if (results != null)
                    cache.Add(cacheKey, results, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                return results;
            }
        }

        public UserSearchResults GetResults()
        {
            DateTime? fromBirthdate = !maxAge.HasValue ? (DateTime?)null : DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1) * 365.25));
            DateTime? toBirthdate = !minAge.HasValue ? (DateTime?)null : DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value * 365.25));

            using (var db = new AspNetDatingDataContext())
            {
                IQueryable<string> usernames = (from u in db.Users
                                                where (!usersSinceSearch || u.u_usersince >= usersSince) &&
                                                      (!genderSearch || u.u_gender == (int)gender) &&
                                                      (!photoReq || (from p in db.Photos where p.p_approved && !p.p_private select p.u_username).Contains(u.u_username)) &&
                                                      (!profileReq || (u.u_profilevisible && (from pa in db.ProfileAnswers select pa.u_username).Contains(u.u_username))) &&
                                                      u.u_active && u.u_face_control_approved && !u.u_deleted &&
                                                      (showInvisible || u.u_profilevisible) &&
                                                      (!fromBirthdate.HasValue || (u.u_birthdate >= fromBirthdate && (u.u_gender != 3 || u.u_birthdate2 >= fromBirthdate))) &&
                                                      (!toBirthdate.HasValue || (u.u_birthdate <= toBirthdate && (u.u_gender != 3 || u.u_birthdate2 <= toBirthdate))) &&
                                                      (!isFeaturedMember.HasValue || isFeaturedMember == u.u_featuredmember)
                                                orderby u.u_usersince descending
                                                select u.u_username);

                if (usersCount > 0)
                    usernames = usernames.Take(usersCount);

                var usernamesArray = usernames.ToArray();

                if (usernamesArray.Length > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = usernamesArray;
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class MutualVoteSearch
    {
        #region Properties

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "MutualVoteSearch", username);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    lResults.Add((string) reader["Username"]);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults();
                    results.Usernames = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    public class IrregularSearchUsers
    {
        #region Properties

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "IrregularSearchUsers",
                                            DateTime.Now.Subtract(TimeSpan.FromDays(Config.Misc.NotVisitedSiteDays)).
                                                Date);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    var username = (string) reader["Username"];

                    lResults.Add(username);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults {Usernames = lResults.ToArray()};
                    return results;
                }
                return null;
            }
        }
    }

    [Serializable]
    public class MutualFriendsSearch
    {
        #region Properties

        private string viewed;
        private string viewer;

        public string Viewer
        {
            get { return viewer; }
            set { viewer = value; }
        }

        public string Viewed
        {
            get { return viewed; }
            set { viewed = value; }
        }

        #endregion

        public UserSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "MutualFriendsSearch", viewer, viewed);

                var lResults = new List<string>();

                while (reader.Read())
                {
                    lResults.Add((string) reader["Friend"]);
                }

                if (lResults.Count > 0)
                {
                    var results = new UserSearchResults {Usernames = lResults.ToArray()};
                    return results;
                }

                return null;
            }
        }
    }

    [Serializable]
    public class FriendsConnectionSearch
    {
        #region Properties

        public string Viewer { get; set; }

        public string Viewed { get; set; }

        #endregion

        public UserSearchResults GetResults()
        {
            List<string> usersChain;
            bool success = User.FindFriendsConnection(Viewer, Viewed, Config.Users.MaxFriendsHops,
                                                      out usersChain);
            if (!success) return null;
            var results = new UserSearchResults {Usernames = usersChain.ToArray()};
            return results;
        }
    }
}