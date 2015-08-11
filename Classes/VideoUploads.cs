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
using System.IO;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class VideoUpload
    {
        #region Fields

        private int? id;
        private string username;
        private bool processed;
        private bool approved;
        private bool isPrivate;

        #endregion

        #region Properties

        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool Processed
        {
            get { return processed; }
            set { processed = value; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        public bool IsPrivate
        {
            get { return isPrivate; }
            set { isPrivate = value; }
        }

        #endregion

        private VideoUpload()
        {
        }

        public VideoUpload(string username)
        {
            this.username = username;
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveVideoUpload", id, username, processed, approved, isPrivate);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }

        public static VideoUpload Load(int id)
        {
            List<VideoUpload> videoUploads = Load(id, null, null, null, null, null);
            return videoUploads.Count > 0 ? videoUploads[0] : null;
        }

        public static List<VideoUpload> Load(int? id, string username, bool? processed, bool? approved, bool? isPrivate, int? numberOfVideos)
        {
            List<VideoUpload> videoUploads = new List<VideoUpload>();
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "LoadVideoUploads", id,
                                                               username, processed, approved, isPrivate, numberOfVideos);

                while (reader.Read())
                {
                    VideoUpload videoUpload = new VideoUpload();

                    videoUpload.id = (int)reader["Id"];
                    videoUpload.username = (string)reader["Username"];
                    videoUpload.processed = (bool)reader["Processed"];
                    videoUpload.approved = (bool)reader["Approved"];
                    videoUpload.isPrivate = (bool)reader["Private"];
                    videoUploads.Add(videoUpload);
                }
            }

            return videoUploads;
        }

        public void Delete()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteVideoUpload", id);
            }

            string userFilesDir = Config.Directories.Home + @"\UserFiles\" + username;
            string videoFile = processed ? String.Format(@"{0}\video_{1}.flv", userFilesDir, id)
                : String.Format(@"{0}\video_{1}.original", userFilesDir, id);

            try
            {
                File.Delete(videoFile);
                File.Delete(videoFile.Replace(".flv", ".png"));
            }
            catch (Exception err)
            {
                Global.Logger.LogInfo("DeleteVideoUpload", err);
            }
        }

        public static void Delete(int id)
        {
            VideoUpload videoUpload = Load(id);
            if (videoUpload != null)
            {
                videoUpload.Delete();
            }
        }

        public static void ProcessVideoUpload(int id)
        {
            VideoUpload videoUpload = Load(id);
            ProcessVideoUpload(videoUpload);
        }

        public static void ProcessVideoUpload(VideoUpload videoUpload)
        {
            if (!VideoConverterPlugin.IsInstalled) return;

            string userFilesDir = Config.Directories.Home + @"\UserFiles\" + videoUpload.username;
            string sourceFile = String.Format(@"{0}\video_{1}.original", userFilesDir, videoUpload.Id);
            string targetFile = String.Format(@"{0}\video_{1}.flv", userFilesDir, videoUpload.Id);
            VideoConverterPlugin.ConvertVideo(sourceFile, targetFile);

            if (File.Exists(sourceFile))
            {
                File.Delete(sourceFile);
            }

            if (File.Exists(targetFile))
            {
                videoUpload.Processed = true;
                videoUpload.Save();
            }
        }

        public static bool HasVideoUpload(string username)
        {
            List<VideoUpload> uploads = Load(null, username, true, true, null, null);
            return uploads.Count > 0;
        }
    }
}