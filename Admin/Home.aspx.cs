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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AspNetDating.Classes;
using System.Net;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for Home.
    /// </summary>
    public partial class Home : AdminPageBase
    {
        public Home()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Dashboard".TranslateA();
            Subtitle = String.Format("{0} Administrative Tool".TranslateA(), Config.Misc.SiteTitle);

            if (!Page.IsPostBack)
            {
                int errorCode;
                if (!int.TryParse(Request.Params["err"], out errorCode))
                    errorCode = -1;
                ShowError(errorCode);

                if (Config.Misc.EnableFirstRunWizard)
                    Response.Redirect("ConfigurationWizard.aspx");

                prepareStats();
                checkInstallation();
                loadPluginsStatus();
                loadNews();
            }
        }

        private void checkInstallation()
        {
            checkInstallationFolder(MapPath("~/UserFiles"));
            checkInstallationFolder(Config.Directories.ImagesCacheDirectory);
            checkInstallationFolder(MapPath("~/Logs"));
            checkInstallationFolder(MapPath("~/IpData"));
            checkInstallationFolder(MapPath("~/Temp"));

            for (int i = 0; i <= 9; i++)
                checkInstallationFolder(Config.Directories.ImagesCacheDirectory + "/" + i);

            if (Config.Photos.EnablePhotoStack)
                checkInstallationFolder(Config.Directories.ImagesCacheDirectory + "/stacks");

            if (Properties.Settings.Default.StorePhotosAsFiles)
                checkInstallationFolder(Config.Directories.UserImagesDirectory);

            try
            {
                HttpWebRequest request = (HttpWebRequest)
                    WebRequest.Create(Config.Urls.ChatHome + "/MessengerWindow.aspx");

                // execute the request
                HttpWebResponse response = (HttpWebResponse)
                    request.GetResponse();
            }
            catch (WebException ex)
            {
                Global.Logger.LogInfo(ex);
                if (ex.Response == null)
                {
                    Master.MessageBox.Show("Your HomeURL setting in the web.config file might be invalid or not accessible.", Misc.MessageType.Error);
                }
                else if (((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.NotFound)
                {
                    Master.MessageBox.Show("Your AjaxChat folder is not configured as application", Misc.MessageType.Error);
                }
            }
            catch (UriFormatException)
            {
                Master.MessageBox.Show("Your ChatHomeUrl is invalid. Please specify absolute Uri or leave it blank in order to use the default AjaxChat path", Misc.MessageType.Error);
            }
        }

        private void checkInstallationFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception)
                {
                    Master.MessageBox.Show("Unable to create folder! Please manually create " +
                        folder, Misc.MessageType.Error);
                    return;
                }
            }

            Guid guid = Guid.NewGuid();
            var testFile = folder + @"\" + guid + ".temp";
            try
            {
                TextWriter tw = File.CreateText(testFile);
                tw.Close();
                File.Delete(testFile);
            }
            catch (Exception)
            {
                Master.MessageBox.Show("The application does not have read/write permissions to " +
                    folder + ". Please grant the necessary permissions!", Misc.MessageType.Error);
                return;
            }
        }

        private void prepareStats()
        {
            #region New registrations chart

            ChartNewRegistrations.Series["Default"]["DrawingStyle"] = "Cylinder";
            ChartNewRegistrations.Titles["Title1"].Text =
                Lang.TransA("At-a-glance: New registrations for the past 30 days");
            ChartNewRegistrations.Series["Default"].ToolTip = String.Format("#VALX\n#VALY {0}",
                                                                            "Registrations".TranslateA());
            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.ShowInvisible = true;
            nuSearch.ProfileReq = false;
            nuSearch.UsersSince = DateTime.Now.AddDays(-31);
            UserSearchResults nuResults = nuSearch.GetResults();

            List<string> xValues = new List<string>();
            List<int> yValues = new List<int>();
            List<int> newUsersCount = new List<int>(30);
            newUsersCount.AddRange(new int[30]);

            Dictionary<long, int> usersCount = new Dictionary<long, int>();

            if (nuResults != null)
            {
                foreach (string username in nuResults.Usernames)
                {
                    User user = Classes.User.Load(username);

                    long key = user.UserSince.Date.Ticks;
                    if (usersCount.ContainsKey(key))
                        usersCount[key]++;
                    else
                        usersCount.Add(key, 1);
                }

                for (int i = 0; i < 30; i++)
                {
                    var key = DateTime.Now.AddDays(i - 30).Date.Ticks;
                    DateTime date = new DateTime(key);
                    xValues.Add(date.ToString("d MMM"));

                    if (usersCount.ContainsKey(key))
                    {
                        yValues.Add(usersCount[key]);
                    }
                    else
                    {
                        yValues.Add(0);
                    }
                }
            }

            ChartNewRegistrations.Series["Default"].Points.DataBindXY(xValues, yValues);

            #endregion

            #region Total users chart

            if (Config.Users.DisableGenderInformation)
            {
                ChartTotalRegistrations.Visible = false;
            }
            else
            {
                ChartTotalRegistrations.Titles["Title1"].Text = Lang.TransA("At-a-glance: Total Registrations");
                ChartTotalRegistrations.Series["Default"]["PieLabelStyle"] = "Outside";

                BasicSearch search = new BasicSearch();
                search.hasAnswer_isSet = false;
                search.hasPhoto_isSet = false;
                search.interestedIn_isSet = false;
                search.Gender = Classes.User.eGender.Male;
                UserSearchResults results = search.GetResults();
                int count = results == null ? 0 : results.Usernames.Length;

                xValues = new List<string>();
                yValues = new List<int>();

                xValues.Add(String.Format(Lang.TransA("{0} Males"), count));
                yValues.Add(count);

                search.Gender = Classes.User.eGender.Female;
                results = search.GetResults();
                count = results == null ? 0 : results.Usernames.Length;
                xValues.Add(String.Format(Lang.TransA("{0} Females"), count));
                yValues.Add(count);

                if (Config.Users.CouplesSupport)
                {
                    search.Gender = Classes.User.eGender.Couple;
                    results = search.GetResults();
                    count = results == null ? 0 : results.Usernames.Length;
                    if (count != 0)
                    {
                        xValues.Add(String.Format(Lang.TransA("{0} Couples"), count));
                        yValues.Add(count);
                    }
                }

                ChartTotalRegistrations.Series["Default"].Points.DataBindXY(xValues, yValues);
            }

            #endregion
        }

        private void loadPluginsStatus()
        {
            var dtPlugins = new DataTable();
            dtPlugins.Columns.Add("Icon");
            dtPlugins.Columns.Add("Name");

            if (File.Exists(MapPath("~/") + "AjaxChat/MessengerWindow.aspx"))
            {
                dtPlugins.Rows.Add("plugin", "AspNetAjaxChat loaded!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "AspNetAjaxChat is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=2291092\" target=\"_blank\">Buy now</a> to enable private chats, file transfers, smilies, 1-to-1 instant messenger, audio/video chats and more.");
            }

            if (FaceFinderPlugin.IsInstalled)
            {
                if (Config.Photos.FindFacesForThumbnails)
                    dtPlugins.Rows.Add("plugin", "Face finder plugin loaded!");
                else
                    dtPlugins.Rows.Add("plugin_error", "Face finder plugin is installed but the 'Find faces in photos' option is not enabled in 'Settings'!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "Face finder plugin is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=1850386\" target=\"_blank\">Buy now</a> to enable automatic face recognition " +
                    " and photo thumbnail cropping.");
            }

            if (VideoConverterPlugin.IsInstalled)
            {
                if (Config.Misc.EnableVideoUpload)
                    dtPlugins.Rows.Add("plugin", "Video converter plugin loaded!");
                else
                    dtPlugins.Rows.Add("plugin_error", "Video converter plugin is installed but the 'Enable Video Upload' option is not enabled in 'Settings'!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "Video converter plugin is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=1850390\" target=\"_blank\">Buy now</a> to enable video file uploads.");
            }

            if (VideoStreamerPlugin.IsInstalled)
            {
                if (Config.Misc.EnableProfileVideoBroadcast)
                    dtPlugins.Rows.Add("plugin", "Video streamer plugin loaded!");
                else
                    dtPlugins.Rows.Add("plugin_error", "Video streamer plugin is installed but the 'Enable Video Broadcast' option is not enabled in 'Settings'!");
            }
            else
            {
                dtPlugins.Rows.Add("plugin_disabled", "Video streamer plugin is not installed! " +
                    "<a href=\"https://www.plimus.com/jsp/buynow.jsp?contractId=1850466\" target=\"_blank\">Buy now</a> to enable live web cam video streaming to profile (with FMS or RED5 streaming server).");
            }

            rptPlugins.DataSource = dtPlugins;
            rptPlugins.DataBind();
        }

        private void loadNews()
        {
            try
            {
                XDocument xdoc = XDocument.Load("http://feeds.feedburner.com/AspnetdatingHelpdesk-LatestTopics");

                var topics = from item in xdoc.Descendants("item")
                             select new
                             {
                                 // ReSharper disable PossibleNullReferenceException
                                 Title = item.Element("title").Value,
                                 Category = item.Element("category").Value,
                                 Description = item.Element("description").Value,
                                 Link = item.Element("link").Value,
                                 PubDate = item.Element("pubDate").Value
                                 // ReSharper restore PossibleNullReferenceException
                             };

                rptHelpdeskTopics.DataSource = topics;
                rptHelpdeskTopics.DataBind();
            }
            catch (Exception err)
            {
                Global.Logger.LogInfo(err);
            }
        }

        private void ShowError(int errorCode)
        {
            switch (errorCode)
            {
                case 1:
                    Master.MessageBox.Show(Lang.TransA("Access denied!"), Misc.MessageType.Error);
                    break;
                case -1:
                default:
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = Classes.Admin.eAccess.ReadWrite;
            base.OnInit(e);
        }
    }
}