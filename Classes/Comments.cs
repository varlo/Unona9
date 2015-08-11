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
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// This class handles the profile comments
    /// </summary>
    public class Comment
    {
        #region Properties

        private int id;

        private string fromUsername;

        private string toUsername;

        private string commentText;

        private DateTime datePosted;

        private bool approved;

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets or sets the "from" username.
        /// </summary>
        /// <value>From username.</value>
        public string FromUsername
        {
            get { return fromUsername; }
            set { fromUsername = value; }
        }

        /// <summary>
        /// Gets or sets the "to" username.
        /// </summary>
        /// <value>To username.</value>
        public string ToUsername
        {
            get { return toUsername; }
            set { toUsername = value; }
        }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        public string CommentText
        {
            get { return commentText; }
            set { commentText = value; }
        }

        /// <summary>
        /// Gets or sets the date posted.
        /// </summary>
        /// <value>The date posted.</value>
        public DateTime DatePosted
        {
            get { return datePosted; }
            set { datePosted = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Comment"/> is approved.
        /// </summary>
        /// <value><c>true</c> if approved; otherwise, <c>false</c>.</value>
        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        private Comment()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="commentText">The comment text.</param>
        /// <returns></returns>
        public static Comment Create(string fromUsername, string toUsername, string commentText)
        {
            var comment = new Comment
                              {
                                  id = (-1),
                                  fromUsername = fromUsername,
                                  toUsername = toUsername,
                                  commentText = commentText,
                                  datePosted = DateTime.Now
                              };
            if (Config.AdminSettings.ApproveComments.AutoApprove)
            {
                comment.approved = true;
            }
            else
            {
                comment.approved = false;
            }

            return comment;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn,
                                                        "SaveComment", id, fromUsername, toUsername, commentText,
                                                        datePosted, approved);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }

            try
            {
                string cacheKey = String.Format("Comment_Load5_{0}", toUsername);
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError(err);
            }
        }

        /// <summary>
        /// Loads the comment with specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Comment Load(int id)
        {
            Comment[] comments = Load(id, null, null, false, false, int.MaxValue);

            if (comments.Length > 0)
                return comments[0];
            else
                throw new NotFoundException();
        }

        /// <summary>
        /// Loads comments
        /// </summary>
        /// <param name="toUsername">To username.</param>
        /// <param name="countLimit">The count limit.</param>
        /// <returns></returns>
        public static Comment[] Load(string toUsername, int countLimit)
        {
            // NOTE: results are cached only if countLimit == 5
            string cacheKey = String.Format("Comment_Load5_{0}", toUsername);
            if (countLimit == 5 && HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Comment[];
            }

            Comment[] comments = Load(-1, null, toUsername, true, true, countLimit);

            if (countLimit == 5 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, comments, null, DateTime.Now.AddMinutes(30),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return comments;
        }

        public static Comment[] Load(string fromUsername, string toUsername)
        {
            return Load(-1, fromUsername, toUsername, true, true, -1);
        }

        /// <summary>
        /// Loads comments
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="approveFilter">if set to <c>true</c> [approve filter].</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <param name="countLimit">The count limit.</param>
        /// <returns></returns>
        public static Comment[] Load(int id, string fromUsername, string toUsername, bool approveFilter,
                                     bool approved, int countLimit)
        {
            using (var db = new Model.AspNetDatingDataContext())
            {
                var comments = from c in db.Comments
                               where (id == -1 || id == c.c_id)
                                     && (fromUsername == null || fromUsername == c.c_from_username)
                                     && (toUsername == null || toUsername == c.c_to_username)
                                     && (!approveFilter || approved == c.c_approved)
                               orderby c.c_date_posted descending
                               select new Comment
                                          {
                                              id = c.c_id,
                                              fromUsername = c.c_from_username,
                                              toUsername = c.c_to_username,
                                              commentText = c.c_comment_text,
                                              datePosted = c.c_date_posted,
                                              approved = c.c_approved
                                          };

                if (countLimit > -1)
                    comments = comments.Take(countLimit);

                return comments.ToArray();
                
            }

            //List<Comment> lComments = new List<Comment>();

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchComments", id, fromUsername,
            //                                                   toUsername, approveFilter ? (object) approved : null);

            //    int count = 0;
            //    while (reader.Read())
            //    {
            //        Comment comment = new Comment();

            //        comment.id = (int) reader["Id"];
            //        comment.fromUsername = (string) reader["FromUsername"];
            //        comment.toUsername = (string) reader["ToUsername"];
            //        comment.commentText = (string) reader["CommentText"];
            //        comment.datePosted = (DateTime) reader["DatePosted"];
            //        comment.approved = (bool) reader["Approved"];

            //        lComments.Add(comment);
            //        if (++count >= countLimit && countLimit > -1) break;
            //    }
            //}

            //return lComments.ToArray();
        }

        /// <summary>
        /// Fetches the new comments.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static int FetchNewComments(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, "FetchNewCommentsCount", username));
            }
        }

        /// <summary>
        /// Deletes the comment with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            try
            {
                Comment comment = Load(id);
                string cacheKey = String.Format("Comment_Load5_{0}", comment.toUsername);
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
            catch (NotFoundException){}
            catch (Exception err)
            {
                Global.Logger.LogError(err);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteComment", id);
            }
        }
    }
}