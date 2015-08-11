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
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using ionic.utils.zip;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for UploadLogo.
    /// </summary>
    public partial class GenerateGadgets : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Extras".TranslateA();
            Subtitle = "Sideshow Gadgets".TranslateA();
            Description = "Use this section to generate and download Vista Sideshow Gadgets".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                    btnGenerateAdminStatsGadget.Enabled = false;
                LoadStrings();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.generateGadgets;
            base.OnInit(e);
        }

        private void LoadStrings()
        {
            btnGenerateAdminStatsGadget.Text = Lang.TransA("Generate Gadget");
        }

        public Stream FileParser(Stream file)
        {
            byte[] buff = new byte[file.Length];
            file.Read(buff, 0, buff.Length);
            string strFile = Encoding.UTF8.GetString(buff);

            strFile = strFile.Replace("%%IFRAME_SRC%%", Config.Urls.Home + "/Gadgets/AdminStats/AdminStats.aspx");

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(strFile));
            stream.Position = 0;
            return stream;
        }

        protected void btnGenerateAdminStatsGadget_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            string gadgetFile = Server.MapPath("~/Temp/" + Config.Misc.GadgetsPrefix + " Admin Stats.gadget");
            if (File.Exists(gadgetFile)) File.Delete(gadgetFile);

            using (ZipFile zip = new ZipFile(gadgetFile))
            {
                zip.BaseDir = Config.Directories.Home + @"\Gadgets\AdminStats\gadget\";
                string[] filenames = Directory.GetFiles(Config.Directories.Home + @"\Gadgets\AdminStats\gadget");
                foreach (String filename in filenames)
                {
                    ZipEntry.FileParserDelegate parser = null;

                    if (filename.Contains("gadget.htm"))
                        parser = new ZipEntry.FileParserDelegate(FileParser);

                    zip.AddFile(filename, false, parser);
                }

                zip.Save();
            }

            Response.Clear();
            Response.AppendHeader("content-disposition",
                                  String.Format("attachment; filename=\"{0}.gadget\"",
                                                Config.Misc.GadgetsPrefix + " Admin Stats"));
            Response.TransmitFile(gadgetFile);
            Response.End();
        }
    }
}