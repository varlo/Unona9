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
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using ionic.utils.zip;

namespace AspNetDating.Components.Profile
{
    public partial class GadgetsCtrl : UserControl
    {
        protected LargeBoxStart LargeBoxStart1;
        protected HeaderLine hlPersonalSettings;
        protected DatePicker datePicker1, datePicker2;
        protected DropDownList dropCity;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        private User user;

        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    ViewState["Username"] = user.Username;
                }
                else
                    ViewState["Username"] = null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        private void LoadStrings()
        {
            LargeBoxStart.Title = "Vista Sidebar Gadgets".Translate();
            hlGadgets.Title = "Download Gadgets".Translate();
            btnDownloadNewUsersGadget.Text = Lang.Trans("Download");
            btnDownloadQuickStatsGadget.Text = Lang.Trans("Download");
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        public Stream NewUsersGadgetParser(Stream file)
        {
            byte[] buff = new byte[file.Length];
            file.Read(buff, 0, buff.Length);
            string strFile = Encoding.UTF8.GetString(buff);

            string gender = "";
            if (Config.Users.InterestedInFieldEnabled)
            {
                gender = ((PageBase)Page).CurrentUserSession.InterestedIn.ToString();
            }
            else if (!Config.Users.DisableGenderInformation)
            {
                switch (((PageBase)Page).CurrentUserSession.Gender)
                {
                    case User.eGender.Male:
                        gender = User.eGender.Female.ToString();
                        break;
                    case User.eGender.Female:
                        gender = User.eGender.Male.ToString();
                        break;
                    case User.eGender.Couple:
                        //do nothing - gendersearch is disabled and shows all the genders
                        break;
                }
            }

            strFile = strFile.Replace("%%IFRAME_SRC%%", Config.Urls.Home + "/Gadgets/NewUsers/NewUsers.aspx?gender=" + gender);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(strFile));
            stream.Position = 0;
            return stream;
        }

        protected void btnDownloadNewUsersGadget_Click(object sender, EventArgs e)
        {
            string gadgetFile = Server.MapPath("~/Temp/" + Config.Misc.GadgetsPrefix + " " +
                ((PageBase)Page).CurrentUserSession.Username + " New Users.gadget");
            if (File.Exists(gadgetFile)) File.Delete(gadgetFile);

            using (var zip = new ZipFile(gadgetFile))
            {
                zip.BaseDir = Config.Directories.Home + @"\Gadgets\NewUsers\gadget\";
                string[] filenames = Directory.GetFiles(Config.Directories.Home + @"\Gadgets\NewUsers\gadget");
                foreach (String filename in filenames)
                {
                    ZipEntry.FileParserDelegate parser = null;

                    if (filename.Contains("gadget.htm"))
                        parser = NewUsersGadgetParser;

                    zip.AddFile(filename, false, parser);
                }

                zip.Save();
            }

            Response.Clear();
            Response.AppendHeader("content-disposition",
                                  String.Format("attachment; filename=\"{0}.gadget\"",
                                                string.Format("{0} New Users".Translate(), Config.Misc.GadgetsPrefix)));
            Response.TransmitFile(gadgetFile);
            Response.End();
        }

        public Stream QuickStatsGadgetParser(Stream file)
        {
            byte[] buff = new byte[file.Length];
            file.Read(buff, 0, buff.Length);
            string strFile = Encoding.UTF8.GetString(buff);

            string username = ((PageBase) Page).CurrentUserSession.Username;
            string encodedData = Convert.ToBase64String(Encoding.ASCII.GetBytes(username));
            string hash = Misc.HMACSHA1ToHex(encodedData, Properties.Settings.Default.SecretGadgetKey);

            strFile = strFile.Replace("%%IFRAME_SRC%%", Config.Urls.Home + "/Gadgets/QuickStats/QuickStats.aspx?username=" + username + "&hash=" + hash);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(strFile));
            stream.Position = 0;
            return stream;
        }

        protected void btnDownloadQuickStatsGadget_Click(object sender, EventArgs e)
        {
            string gadgetFile = Server.MapPath("~/Temp/" + Config.Misc.GadgetsPrefix + " " +
                ((PageBase)Page).CurrentUserSession.Username + " Quick Stats.gadget");
            if (File.Exists(gadgetFile)) File.Delete(gadgetFile);

            using (ZipFile zip = new ZipFile(gadgetFile))
            {
                zip.BaseDir = Config.Directories.Home + @"\Gadgets\QuickStats\gadget\";
                string[] filenames = Directory.GetFiles(Config.Directories.Home + @"\Gadgets\QuickStats\gadget");
                foreach (String filename in filenames)
                {
                    ZipEntry.FileParserDelegate parser = null;

                    if (filename.Contains("gadget.htm"))
                        parser = new ZipEntry.FileParserDelegate(QuickStatsGadgetParser);

                    zip.AddFile(filename, false, parser);
                }

                zip.Save();
            }

            Response.Clear();
            Response.AppendHeader("content-disposition",
                                  String.Format("attachment; filename=\"{0}.gadget\"",
                                                string.Format("{0} Quick Stats".Translate(), Config.Misc.GadgetsPrefix)));
            Response.TransmitFile(gadgetFile);
            Response.End();
        }
    }
}