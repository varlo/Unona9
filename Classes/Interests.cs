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
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class Interest
    {
        #region Properties

        private int id;

        public int Id
        {
            get { return id; }
        }

        private string fromUsername;

        public string FromUsername
        {
            get { return fromUsername; }
        }

        private string toUsername;

        public string ToUsername
        {
            get { return toUsername; }
        }

        private DateTime datePosted;

        public DateTime DatePosted
        {
            get { return datePosted; }
        }

        private bool deletedByFromUser;

        public bool DeletedByFromUser
        {
            get { return deletedByFromUser; }
        }

        private bool deletedByToUser;

        public bool DeletedByToUser
        {
            get { return deletedByToUser; }
        }

        #endregion

        public enum eFolder
        {
            None = 0,
            Received = 1,
            Sent = 2
        }

        private enum eStatus
        {
            AlreadySent = 0,
            Sent = 1,
            Updated = 2
        }

        public Interest()
        {
        }

        /// <summary>
        /// Sends the specified from username.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <returns></returns>
        public static bool Send(string fromUsername, string toUsername)
        {
            eStatus status;
            using (SqlConnection conn = Config.DB.Open())
            {
                status = (eStatus)SqlHelper.ExecuteScalar(conn, "SendInterest", fromUsername, toUsername);
            }

            switch (status)
            {
                case eStatus.Sent:
                    User senderUser = User.Load(fromUsername);
                    User targetUser = User.Load(toUsername);

                    if (targetUser.ReceiveEmails)
                    {
                        //                        MiscTemplates.ShowInterestMessage showInterestMessageTemplate =
                        //                            new MiscTemplates.ShowInterestMessage(targetUser.LanguageId);
                        //                        string subject = showInterestMessageTemplate.Subject;
                        //                        string user_sender_image = ImageHandler.RenderImageTagUsername(senderUser.Username, 150, 150, null, false, true, true);
                        //                        string body = showInterestMessageTemplate.Message.Replace("%%IMAGE_SENDER%%", user_sender_image);
                        //                        body = body.Replace("%%USER%%", fromUsername);

                        var showInterest = new EmailTemplates.ShowInterest(targetUser.LanguageId);
                        string user_sender_image = ImageHandler.RenderImageTagUsername(senderUser.Username, 150, 150, null, false, true, true);

                        try
                        {
                            Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, targetUser.Username, targetUser.Email,
                                       showInterest.GetFormattedSubject(senderUser.Username),
                                       showInterest.GetFormattedBody(targetUser.Username, senderUser.Username, user_sender_image), false);
                            //Email.Send(Config.Misc.SiteEmail, targetUser.Email, subject, body, false);
                            //Message.Send(Config.Users.SystemUsername,toUsername,body,0);
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogger.Log("Interests.Send", ex);
                        }
                    }
                    if (Config.Users.NewEventNotification)
                    {
                        int imageID;
                        try
                        {
                            imageID = Photo.GetPrimary(senderUser.Username).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(senderUser.Gender);
                        }
                        string text = "You got a smile at Unona.net!".Translate();

                        string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        User.SendOnlineEventNotification(senderUser.Username, targetUser.Username, text, thumbnailUrl, "Mailbox.aspx?sel=sentint");
                    }

                    return true;
                case eStatus.Updated:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Interest Fetch(int id)
        {
            Interest[] interests = Fetch(id, null, null, false);
            if (interests.Length > 0)
            {
                return interests[0];
            }
            else
            {
                throw new NotFoundException
                    (Lang.Trans("The requested interest does not exist!"));
            }
        }

        /// <summary>
        /// Fetches the specified from username.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <returns></returns>
        public static Interest Fetch(string fromUsername, string toUsername)
        {
            Interest[] interests = Fetch(0, fromUsername, toUsername, false);
            if (interests.Length > 0)
            {
                return interests[0];
            }
            else
            {
                throw new NotFoundException
                    (Lang.Trans("The requested interest does not exist!"));
            }
        }

        public static Interest[] FetchReceived(string username, bool newInterests)
        {
            return Fetch(0, null, username, newInterests);
        }

        public static Interest[] FetchSent(string username)
        {
            return Fetch(0, username, null, false);
        }

        public static Interest[] Fetch(int id, string fromUsername, string toUsername, bool newInterests)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchInterests", id, fromUsername, toUsername, newInterests);

                List<Interest> lInterests = new List<Interest>();

                while (reader.Read())
                {
                    Interest interest = new Interest();

                    interest.id = (int) reader["Id"];
                    interest.fromUsername = (string) reader["FromUsername"];
                    interest.toUsername = (string) reader["ToUsername"];
                    interest.datePosted = (DateTime) reader["DatePosted"];
                    interest.deletedByFromUser = (bool) reader["DeletedByFromUser"];
                    interest.deletedByToUser = (bool) reader["DeletedByToUser"];

                    lInterests.Add(interest);
                }

                return lInterests.ToArray();
            }
        }

        private void DeleteBy(bool fromUser /*sent interest*/,
                              bool toUser /*received interest*/)
        {
            if (id > 0)
            {
                using (SqlConnection conn = Config.DB.Open())
                {
                    SqlHelper.ExecuteNonQuery(conn,
                                              "DeleteInterest", id, fromUser, toUser);
                }
            }
        }

        public void DeleteFromReceivedInterests()
        {
            DeleteBy(false, true);
        }

        public void DeleteFromSentInterests()
        {
            DeleteBy(true, false);
        }
    }
}