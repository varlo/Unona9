﻿/* ASPnetDating 
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
using System.Data;
using System.IO;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ExportPhotos : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Export photos".TranslateA();
            Subtitle = "Export photos".TranslateA();
            Description = "Use this page to export photos to files...".TranslateA();
        }

        private static string message = "";
        private static string exportDirectory;

        protected void btnStartExport_Click(object sender, EventArgs e)
        {
            exportDirectory = Server.MapPath("~/images/export");
            ThreadPool.QueueUserWorkItem(ExportPhotosAsync);
        }

        private static void ExportPhotosAsync(object state)
        {
            int[] photoIds = Photo.Search(-1, null, -1, null, null, null, null);
            int i = 0;
            var dateStart = DateTime.Now;
            foreach (var photoId in photoIds)
            {
                TimeSpan timeElapsed = DateTime.Now.Subtract(dateStart);
                message = String.Format("Exporting {0} out of {1}. Time elapsed {2}. Total time estimated {3}".TranslateA(),
                    ++i, photoIds.Length, timeElapsed,
                    TimeSpan.FromSeconds(timeElapsed.TotalSeconds * photoIds.Length / i));

                var subDirectory = exportDirectory + "/" + photoId % 100;
                if (!Directory.Exists(subDirectory))
                    Directory.CreateDirectory(subDirectory);
                var exportFile = subDirectory + "/" + photoId + ".jpg";
                if (File.Exists(exportFile))
                    continue;

                var photo = Photo.Fetch(photoId);
                photo.Image.Save(subDirectory + "/" + photoId + ".jpg");
            }

            message = "Done!";
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            MessageBox1.Show(message, Misc.MessageType.Success);
        }
    }
}