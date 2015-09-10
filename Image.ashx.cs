using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class ImageHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string Id = context.Request.Params["id"];

            int MaxWidth = 0, MaxHeight = 0;
            bool findFace = false;
            FaceFinderPlugin.FaceRegion faceRegion = null;
            try
            {
                if (context.Request.Params["width"] != null && context.Request.Params["height"] != null)
                {
                    MaxWidth = Convert.ToInt32(context.Request.Params["width"]);
                    MaxHeight = Convert.ToInt32(context.Request.Params["height"]);
                }

                if (!string.IsNullOrEmpty(context.Request.Params["findFace"]))
                {
                    findFace = true;
                    if (context.Request.Params["faceX"] != null)
                    {
                        try
                        {
                            faceRegion = new FaceFinderPlugin.FaceRegion
                                             {
                                                 X = int.Parse(context.Request.Params["faceX"]),
                                                 Y = int.Parse(context.Request.Params["faceY"]),
                                                 Width = int.Parse(context.Request.Params["faceW"]),
                                                 Height = int.Parse(context.Request.Params["faceH"])
                                             };
                        }
                        catch (ArgumentException)
                        {
                            faceRegion = null;
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                // Invalid paramerers; no need to log the error
            }

            #region Show image stack

            var username = context.Request.Params["username"];
            if (username != null && context.Request.Params["stack"] == "1")
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/stacks/photostack_{0}_{1}_{2}.png",
                                                username, MaxWidth, MaxHeight);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\stacks\photostack_{0}_{1}_{2}.png",
                                                username, MaxWidth, MaxHeight);
                string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);
                if (context.Cache[cacheKey] != null)
                {
                    context.Response.Redirect(cacheUrl, false);
                    return;
                }
                if (File.Exists(cacheFile))
                {
                    context.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                      Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), CacheItemPriority.NotRemovable,
                                      null);

                    context.Response.Redirect(cacheUrl, false);
                    return;
                }

                try
                {
                    ShowUserImageStack(context, username, MaxWidth, MaxHeight);
                }
                catch (NotFoundException)
                {
                    ShowAvatarImage(context, User.Load(username).Gender, MaxWidth, MaxHeight);
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err);
                    ShowAvatarImage(context, User.Load(username).Gender, MaxWidth, MaxHeight);
                }
                return;
            }

            #endregion

            if (Id == null || Id == "0") ShowNoImage(context, MaxWidth, MaxHeight);

            switch (Id)
            {
                case "-1":
                    ShowAvatarImage(context, User.eGender.Male, MaxWidth, MaxHeight);
                    return;
                case "-2":
                    ShowAvatarImage(context, User.eGender.Female, MaxWidth, MaxHeight);
                    return;
                case "-3":
                    ShowAvatarImage(context, User.eGender.Couple, MaxWidth, MaxHeight);
                    return;
            }

            int photoId;
            Int32.TryParse(Id, out photoId);

            if (context.Request.Params["cache"] != null && context.Request.Params["cache"] == "1")
            {
                string cacheKey = String.Format("Photos_ProcessRequest_{0}_{1}_{2}_{3}", Id, MaxWidth, MaxHeight, findFace);
                if (context.Cache[cacheKey] != null)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite((byte[])context.Cache[cacheKey]);
                    return;
                }
            }

            if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                                                photoId, MaxWidth, MaxHeight, photoId % 10, findFace ? "face" : "");
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\photo{4}_{0}_{1}_{2}.jpg",
                                                photoId, MaxWidth, MaxHeight, photoId % 10, findFace ? "face" : "");
                string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);
                if (context.Cache[cacheKey] != null)
                {
                    context.Response.Redirect(cacheUrl, false);
                    return;
                }
                if (File.Exists(cacheFile))
                {
                    context.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                      Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), CacheItemPriority.NotRemovable,
                                      null);

                    context.Response.Redirect(cacheUrl, false);
                    return;
                }
            }

            try
            {
                Image image;
                bool skipCaching;
                bool dispose = true;

                if (Id == "session")
                {
                    //image = ((Photo) context.Session["temp_photo"]).Image;
                    image = Image.FromFile((string)context.Session["temp_photo_fileName"]);
                    skipCaching = true;
                    dispose = false;
                }
                else
                {
                    if (context.Session["temp_photos"] != null)
                    {
                        skipCaching = true;
                        var collection = (SortedList)context.Session["temp_photos"];
                        if (collection[Id] != null)
                        {
                            image = ((Photo)collection[Id]).Image;
                            dispose = false;
                        }
                        else
                            image = Photo.Fetch(photoId).Image;
                    }
                    else
                    {
                        Photo photo = Photo.Fetch(photoId);
                        skipCaching = photo.ExplicitPhoto;

                        if (Config.Photos.EnableExplicitPhotos &&
                            photo.ExplicitPhoto &&
                            (PageBase.GetCurrentUserSession() == null
                                || PageBase.GetCurrentUserSession().Age < Config.Users.MinAgeForExplicitPhotos 
                                || PageBase.GetCurrentUserSession().CanViewExplicitPhotos() == PermissionCheckResult.YesButPlanUpgradeNeeded
                                || PageBase.GetCurrentUserSession().CanViewExplicitPhotos() == PermissionCheckResult.No) &&
                            context.Session["AdminSession"] == null)
                        {
                            ShowCensoredImage(context, MaxWidth, MaxHeight);
                            return;
                        }
                        if (findFace && faceRegion == null && photo.FaceCrop != null)
                        {
                            if (photo.FaceCrop.Trim().Length > 0)
                            {
                                try
                                {
                                    string[] coords = photo.FaceCrop.Split('|');
                                    faceRegion = new FaceFinderPlugin.FaceRegion
                                                     {
                                                         X = int.Parse(coords[0]),
                                                         Y = int.Parse(coords[1]),
                                                         Width = int.Parse(coords[2]),
                                                         Height = int.Parse(coords[3])
                                                     };
                                }
                                catch (Exception err)
                                {
                                    Global.Logger.LogWarning("FaceCrop " + photo.FaceCrop, err);
                                    findFace = false;
                                }
                            }
                            else
                            {
                                findFace = false;
                            }
                        }
                        image = photo.Image;
                    }
                }

                var expandCrop = context.Request.Params["exactCrop"] != "1";

                lock (image)
                {
                    ShowUserImage(image, context, MaxWidth, MaxHeight, photoId, skipCaching, findFace, expandCrop, faceRegion, dispose);
                }
            }
            catch (Exception err)
            {
                if (context.Request.Params["debug"] != null)
                    Global.Logger.LogError(err);
                ShowNoImage(context, MaxWidth, MaxHeight);
            }
        }

        private static void ShowUserImageStack(HttpContext context, string username, int width, int height)
        {
            var user = User.Load(username);
            try
            {
                //check if the user have primary photo
                user.GetPrimaryPhoto();

                using (var image = Photo.CreatePhotoStack(username, width, height))
                {
                    string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/stacks/photostack_{0}_{1}_{2}.png",
                        username, width, height);
                    string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\stacks\photostack_{0}_{1}_{2}.png",
                        username, width, height);
                    try
                    {
                        if (File.Exists(cacheFile))
                        {
                            try
                            {
                                File.Delete(cacheFile);
                            }
                            catch (Exception err)
                            {
                                Global.Logger.LogError(err);
                            }
                        }
                        if (!File.Exists(cacheFile))
                        {
                            image.Save(cacheFile, ImageFormat.Png);
                        }
                        context.Response.Redirect(cacheUrl, false);
                        return;
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError(err, cacheFile);
                    }

                    context.Response.Clear();
                    context.Response.ContentType = "image/png";

                    using (MemoryStream stream = new MemoryStream())
                    {
                        image.Save(stream, ImageFormat.Png);

                        stream.WriteTo(context.Response.OutputStream);
                    }
                }
            }
            catch (NotFoundException) //if no primary photo
            {
                ShowAvatarImage(context, user.Gender, width, height);
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private static void ShowNoImage(HttpContext context, int MaxWidth, int MaxHeight)
        {
            ShowUserImage(
                Image.FromFile(context.Server.MapPath("~/Images") + "/no_photo.png"), context,
                MaxWidth, MaxHeight, 0);
        }

        private static void ShowAvatarImage(HttpContext context, User.eGender gender, int MaxWidth, int MaxHeight)
        {
            switch (gender)
            {
                case User.eGender.Male:
                    ShowUserImage(
                        Image.FromFile(context.Server.MapPath("~/Images") + "\\no_photo.png"),
                        context, MaxWidth, MaxHeight, -1);
                    break;
                case User.eGender.Female:
                    ShowUserImage(
                        Image.FromFile(context.Server.MapPath("~/Images") + "\\no_photo.png"),
                        context, MaxWidth, MaxHeight, -2);
                    break;
                case User.eGender.Couple:
                    ShowUserImage(
                        Image.FromFile(context.Server.MapPath("~/Images") + "\\no_photo.png"),
                        context, MaxWidth, MaxHeight, -3);
                    break;
            }
        }

        private static void ShowCensoredImage(HttpContext context, int MaxWidth, int MaxHeight)
        {
            ShowUserImage(
                Image.FromFile(context.Server.MapPath("~/Images") + "\\censored_large.png"), context,
                MaxWidth, MaxHeight, -4);
        }

        private static void ShowUserImage(Image image, HttpContext context, int MaxWidth, int MaxHeight,
                                   int photoId)
        {
            ShowUserImage(image, context, MaxWidth, MaxHeight, photoId, false, false, true, null, true);
        }

        private static void ShowUserImage(Image image, HttpContext context, int MaxWidth, int MaxHeight,
                                   int photoId, bool skipCaching, bool findFace, bool expandCrop, 
            FaceFinderPlugin.FaceRegion faceRegion, bool disposeImage)
        {
            var format = image.RawFormat;
            if (findFace && faceRegion != null)
            {
                image = Photo.CropFace(image, faceRegion, MaxWidth, MaxHeight, expandCrop);
            }

            try
            {
                if (MaxWidth > 0 && MaxHeight > 0)
                    image = Photo.ResizeImage(image, MaxWidth, MaxHeight);
            }
            catch (Exception err)
            {
                ExceptionLogger.Log(context.Request, err);
                throw;
            }

            if (Config.Photos.DoWatermark
                && image.Width >= Config.Photos.MinWidthToWatermark
                && image.Height >= Config.Photos.MinHeightToWatermark)
            {
                Image watermark;
                if (context.Cache["Watermark_Image"] == null)
                {
                    string filename = context.Server.MapPath("~/Images") + "/Watermark.png";
                    watermark = Image.FromFile(filename);
                    context.Cache.Add("Watermark_Image", watermark, new CacheDependency(filename),
                                      Cache.NoAbsoluteExpiration, TimeSpan.FromHours(24),
                                      CacheItemPriority.NotRemovable, null);
                }
                else
                {
                    watermark = (Image)context.Cache["Watermark_Image"];
                }

                try
                {
                    lock (watermark)
                    {
                        Photo.ApplyWatermark(image, watermark, Config.Photos.WatermarkTransparency,
                                             Config.Photos.WatermarkPosition);
                    }
                }
                catch (Exception err)
                {
                    Global.Logger.LogWarning("Unable to apply watermark", err);
                }
            }

            context.Response.Clear();
            context.Response.ContentType = format.Guid == ImageFormat.Png.Guid ? "image/png" : "image/jpeg";

            if (context.Request.Params["cache"] != null && context.Request.Params["cache"] == "1")
            {
                string cacheKey = String.Format("Photos_ProcessRequest_{0}_{1}_{2}_{3}",
                    photoId, MaxWidth, MaxHeight, findFace);

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    image.Dispose();

                    context.Cache.Insert(cacheKey, ms.ToArray(), null, Cache.NoAbsoluteExpiration,
                                         TimeSpan.FromMinutes(15));
                    ms.WriteTo(context.Response.OutputStream);
                }
            }
            else if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1" && !skipCaching)
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                    photoId, MaxWidth, MaxHeight, Math.Abs(photoId % 10), findFace ? "face" : "");
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                    photoId, MaxWidth, MaxHeight, Math.Abs(photoId % 10), findFace ? "face" : "");
                try
                {
                    if (!File.Exists(cacheFile))
                    {
                        image.Save(cacheFile, format);
                        image.Dispose();
                    }
                    context.Response.Redirect(cacheUrl, false);
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err, cacheFile);
                    WriteToOutputStream(context, image, format);
                    image.Dispose();
                }
            }
            else
            {
                WriteToOutputStream(context, image, format);

                if (disposeImage)
                    image.Dispose();
            }
        }

        private static void WriteToOutputStream(HttpContext context, Image image, ImageFormat format)
        {
            if (format.Guid == ImageFormat.MemoryBmp.Guid)
                format = ImageFormat.Jpeg;
            if (format.Guid == ImageFormat.Png.Guid || image.RawFormat.Guid == ImageFormat.Png.Guid)
            {
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Png);

                    stream.WriteTo(context.Response.OutputStream);
                }
            }
            else
            {
                image.Save(context.Response.OutputStream, format);
            }
        }

        public static string RenderImageTag(User.eGender gender, int width, int height, string cssClass,
                                            bool useCache, bool diskCache)
        {
            int photoId = GetPhotoIdByGender(gender);

            return RenderImageTag(photoId, width, height, cssClass, useCache, diskCache);
        }

        public static string RenderImageTag(int imageId, int width, int height, string cssClass,
                                            bool useCache, bool diskCache)
        {
            return RenderImageTag(imageId, width, height, cssClass, useCache, diskCache, false);
        }

        public static string RenderImageTag(int imageId, int width, int height, string cssClass, bool useCache,
            bool diskCache, bool findFace)
        {
            string imageUrl = CreateImageUrl(imageId, width, height, useCache, diskCache, findFace);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string RenderImageStackTag(string username, int width, int height, string cssClass)
        {
            string imageUrl = CreateImageStackUrl(username, width, height);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string CreateImageStackUrl(string username, int width, int height)
        {
            string imageUrl = null;

            string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/stacks/photostack_{0}_{1}_{2}.png",
                username, width, height);
            string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/stacks/photostack_{0}_{1}_{2}.png",
                username, width, height);
            string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);

            if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
            {
                imageUrl = String.Format("{3}/stacks/photostack_{0}_{1}_{2}.png",
                    username, width, height, Config.Urls.ImagesCacheHome);
            }

            if (imageUrl != null && HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] == null)
            {
                HttpContext.Current.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                              Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1),
                                              CacheItemPriority.NotRemovable, null);
            }

            if (imageUrl == null)
            {
                imageUrl =
                    String.Format(
                        "{3}/Image.ashx?username={0}&width={1}&height={2}&stack=1",
                        username, width, height, Config.Urls.Home);
            }

            return imageUrl;
        }

        public static string CreateImageUrl(int imageId, int width, int height, bool useCache, bool diskCache,
            bool findFace)
        {
            string imageUrl = null;

            if (diskCache)
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                    imageId, width, height, imageId % 10, findFace ? "face" : "");
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                    imageId, width, height, imageId % 10, findFace ? "face" : "");
                string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);

                if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
                {
                    imageUrl = String.Format("{5}/{3}/photo{4}_{0}_{1}_{2}.jpg",
                        imageId, width, height, imageId % 10, findFace ? "face" : "", Config.Urls.ImagesCacheHome);
                }

                if (imageUrl != null && HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] == null)
                {
                    HttpContext.Current.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                                  Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1),
                                                  CacheItemPriority.NotRemovable, null);
                }
            }

            if (imageUrl == null)
            {
                string additionalParams = "";
                if (useCache)
                    additionalParams += "&cache=1";
                if (diskCache)
                    additionalParams += "&diskCache=1";
                if (findFace)
                    additionalParams += "&findFace=1";
                imageUrl =
                    String.Format(
                        "{4}/Image.ashx?id={0}&width={1}&height={2}{3}",
                        imageId, width, height, additionalParams, Config.Urls.Home);
            }

            return imageUrl;
        }

        public static int GetPhotoIdByGender(User.eGender gender)
        {
            int photoId;

            switch (gender)
            {
                case User.eGender.Male:
                    photoId = -1;
                    break;
                case User.eGender.Female:
                    photoId = -2;
                    break;
                case User.eGender.Couple:
                    photoId = -3;
                    break;
                default:
                    photoId = 0;
                    break;
            }

            return photoId;
        }

        public static void DeleteDiskCache(int imageId)
        {
            var cachePath = String.Format(@"{0}\images\cache\{1}",
                Config.Directories.Home, imageId % 10);
            var cacheMask = String.Format(@"photo_{0}_*.jpg", imageId);
            foreach (var file in Directory.GetFiles(cachePath, cacheMask))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception err)
                {
                    Global.Logger.LogInfo(err);
                }
            }
            cacheMask = String.Format(@"photoface_{0}_*.jpg", imageId);
            foreach (var file in Directory.GetFiles(cachePath, cacheMask))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception err)
                {
                    Global.Logger.LogInfo(err);
                }
            }
        }

        public static void DeleteImageStackCache(string username)
        {
            string cacheFolder = Config.Directories.ImagesCacheDirectory + "/stacks";

            foreach (var file in Directory.GetFiles(cacheFolder, @"photostack_" + username + "_*.png"))
            {
                File.Delete(file);
            }
        }

        public static string RenderImageTagUsername(string username, int width, int height, string cssClass, bool useCache, bool diskCache, bool findFace)
        {
            User user;
            int imageId;
            try
            {
                user = User.Load(username);
            }
            catch (NotFoundException) { return ""; }

            Photo photo = null;
            try
            {
                photo = user.GetPrimaryPhoto();
            }
            catch (NotFoundException)
            {
                //                return "";
            }

            if (photo == null || !photo.Approved)
            {
                imageId = GetPhotoIdByGender(user.Gender);
            }
            else
            {
                imageId = photo.Id;
            }

            string imageUrl = CreateImageUrl(imageId, width, height, useCache, diskCache, findFace);
            string imageTag = String.Format("<img src=\"{0}\" alt=\"photo\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }
    }
}