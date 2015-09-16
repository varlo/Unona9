using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class CreditsHistory
    {
        #region fields

        private int? id;
        private string username;
        private string ladyname;
        private DateTime date = DateTime.Now;
        private eService service;
        private int amount;
        private int refundsid;
        private int credits;
        private string source;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eService
        {
            None = 0,
            SendMessage = 1,
            ViewPhotos = 2,
            ViewVideo = 3,
            UseIM = 5,
            Translation = 5,
            EcardSend = 6,
            EcardReceive = 7,
            IMPerMinute = 8,
            Credit = 10,
            EssayTranslation = 11,
            CreateBlogPost = 12,
            ViewVideoStream = 13
        }

        public enum eSortColumn
        {
            None,
            Date,
            Username,
        }

        #endregion

        #region Constructors

        private CreditsHistory()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupPost"/> class.
        /// </summary>
        /// <param name="username">Represents the username that creates this post.</param>
        public CreditsHistory(string username)
        {
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
        /// Throws "Exceptioin" exception.
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
                throw new Exception("The field ID is not set!");
            }
        }

        /// <summary>
        /// Gets the username.
        /// The property is read-only.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
        }

        /// <summary>
        /// Gets the ladyname.
        /// </summary>
        /// <value>The ladyname.</value>
        public string Ladyname
        {
            get { return ladyname; }
            set { ladyname = value; }
        }

        /// <summary>
        /// Gets or sets the date posted.
        /// </summary>
        /// <value>The date posted.</value>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public eService Service
        {
            get { return service; }
            set { service = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int RefundsID
        {
            get { return refundsid; }
            set { refundsid = value; }
        }

        public int Credits
        {
            get { return credits; }
            set { credits = value; }
        }

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        #endregion

        #region Methods

        public static CreditsHistory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, sortColumn);
        }

        public static CreditsHistory[] Fetch(string username)
        {
            return Fetch(null, username, null, eSortColumn.None);
        }

        public static CreditsHistory Fetch(int id)
        {
            CreditsHistory[] creditsHistory = Fetch(id, null, null, eSortColumn.None);

            if (creditsHistory.Length > 0)
            {
                return creditsHistory[0];
            }
            return null;
        }

        private static CreditsHistory[] Fetch(int? id, string username, DateTime? date, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchCreditsHistory",
                                                               id, username, date, sortColumn);

                var lCreditsHistory = new List<CreditsHistory>();

                while (reader.Read())
                {
                    var creditsHistory = new CreditsHistory
                    {
                        id = (int)reader["ID"],
                        username = (string)reader["Username"],
                        ladyname = (string)reader["Ladyname"],
                        date = (DateTime)reader["Date"],
                        service = (eService)reader["Service"],
                        amount = (int)reader["Amount"],
                        refundsid = reader.IsDBNull(reader.GetOrdinal("RefundsID")) ? 0 : (int)reader["RefundsID"],
                        credits = (int)reader["Credits"],
                    };

                    lCreditsHistory.Add(creditsHistory);
                }

                return lCreditsHistory.ToArray();
            }
        }

        public static DataTable FetchByUsername(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchCreditsHistoryByUsername", username);
                var dtResults = new DataTable();
                dtResults.Columns.Add("Date");
                dtResults.Columns.Add("Ladyname");
                dtResults.Columns.Add("Credits");
                dtResults.Columns.Add("Service");
                dtResults.Columns.Add("Source");
                dtResults.Columns.Add("Id");
                while (reader.Read())
                {
                    string service = String.Empty;
                    var srv = (int)reader["Service"];
                    if (srv == (int)eService.SendMessage)
                        service = Lang.Trans("Sent Message");
                    else if (srv == (int)eService.Translation)
                        service = Lang.Trans("Translation");
                    else if (srv == (int)eService.UseIM || srv == (int)eService.IMPerMinute)
                        service = Lang.Trans("Instant Messenger");
                    else if (srv == (int)eService.ViewPhotos)
                        service = Lang.Trans("View Photos");
                    else if (srv == (int)eService.ViewVideo)
                        service = Lang.Trans("View Video");
                    else if (srv == (int)eService.Credit)
                        service = Lang.Trans("Credit");
                    else if (srv == (int)eService.EcardSend)
                        service = Lang.Trans("E-card send");
                    else if (srv == (int)eService.EcardReceive)
                        service = Lang.Trans("E-card receive");
                    else if (srv == (int)eService.EssayTranslation)
                        service = Lang.Trans("Essay Translation");
                    dtResults.Rows.Add((DateTime)reader["Date"], (string)reader["Ladyname"],
                        (int)reader["Credits"], service,
                        reader.IsDBNull(reader.GetOrdinal("Source")) ? String.Empty : (string)reader["Source"],
                        (int)reader["Id"]);
                }
                return dtResults;
            }
        }

        //        public static int[] Search(int? groupTopicID, string username, DateTime? datePosted, DateTime? dateEdited, string keyword, eSortColumn sortColumn)
        //        {
        //            using (SqlConnection conn = Config.DB.Open())
        //            {
        //                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "SearchGroupPosts",
        //                                                               groupTopicID, username, datePosted,
        //                                                               dateEdited, keyword, sortColumn);
        //
        //                List<int> lGroupPostsIDs = new List<int>();
        //
        //                while (reader.Read())
        //                {
        //                    lGroupPostsIDs.Add((int) reader["ID"]);    
        //                }
        //
        //                return lGroupPostsIDs.ToArray();
        //            }
        //        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveCreditsHistory",
                                                        id, username, date, service, amount);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes group post from DB by specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void Delete(int id)
        {
            Delete(id, null);
        }

        public static void Delete(string username)
        {
            Delete(null, username);
        }

        private static void Delete(int? id, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteCreditsHistory", id, username);
            }
        }

        public static int[] Search(string username, string service = null)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "SearchCreditsHistoryByUsername", username, service);
                var lCreditsHistoryIds = new List<int>();
                while (reader.Read())
                    lCreditsHistoryIds.Add((int)reader["Id"]);
                return lCreditsHistoryIds.ToArray();
            }
        }

        #endregion
    }

    /// <summary>
    /// The credits search results class
    /// </summary>
    [Serializable]
    public class CreditsHistorySearchResults : SearchResults<int, CreditsHistory>
    {
        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        /// <value>The credits.</value>
        public int[] CreditsHistory
        {
            get
            {
                return Results ?? new int[0];
            }
            set { Results = value; }
        }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <param name="creditsPerPage">The credits per page.</param>
        /// <returns></returns>
        public new int GetTotalPages(int creditsPerPage)
        {
            return base.GetTotalPages(creditsPerPage);
        }

        /// <summary>
        /// Loads the result.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected override CreditsHistory LoadResult(int id)
        {
            return Classes.CreditsHistory.Fetch(id);
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="creditsPerPage">creditsPerPage</param>
        /// <returns>Array of credits</returns>
        public new CreditsHistory[] GetPage(int Page, int creditsPerPage)
        {
            return base.GetPage(Page, creditsPerPage);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public CreditsHistory[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }
}
