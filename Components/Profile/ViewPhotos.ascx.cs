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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    public partial class ViewPhotos : UserControl
    {
        protected LargeBoxStart LargeBoxStart;
        public bool loadComments;
        public bool loadPhotos;
        private User user;

        private bool FirstLoad
        {
            get { return ViewState["FirstLoad"] == null ? true : false; }
            set { ViewState["FirstLoad"] = value; }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected int CurrentPhotoId
        {
            get
            {
                if (ViewState["CurrentPhotoId"] != null)
                {
                    return (int)ViewState["CurrentPhotoId"];
                }
                return -1;
            }
            set { ViewState["CurrentPhotoId"] = value; }
        }

        protected int[] UserPhotosIDs
        {
            get
            {
                if (ViewState["UserPhotosIDs"] != null)
                {
                    return (int[])ViewState["UserPhotosIDs"];
                }
                return new int[0];
            }
            set { ViewState["UserPhotosIDs"] = value; }
        }

        public User User
        {
            set
            {
                user = value;
                ViewState["Username"] = user != null ? user.Username : null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        public int? PhotoAlbumID
        {
            get { return (int?)ViewState["PhotoAlbumID"]; }
            set { ViewState["PhotoAlbumID"] = value; }
        }

        public string BigPhotoImageTag
        {
            get { return ltrPhoto.Text; }
            set { ltrPhoto.Text = value; }
        }

        PermissionCheckResult? viewPhotosPermission;
        private PermissionCheckResult ViewPhotosPermission
        {
            get
            {
                if (!viewPhotosPermission.HasValue)
                    viewPhotosPermission = CurrentUserSession.CanViewPhotos();

                return viewPhotosPermission.Value;
            }
        }

        private bool CanViewPhoto
        {
            get
            {
                if (CurrentUserSession == null)
                {
                    return Config.Users.GetNonPayingMembersOptions().CanViewPhotos.Value;
                }

                if (CurrentUserSession.Username == User.Username || ViewPhotosPermission == PermissionCheckResult.Yes || 
                    UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos, null))
                    return true;

                return false;
            }
        }

        //private bool PhotosLocked
        //{
        //    get
        //    {
        //        return Config.Credits.Required && Config.Credits.CreditsForUserPhotos > 0 &&
        //            (CurrentUserSession == null ||
        //            (CurrentUserSession.Username != User.Username &&
        //             !(Config.Users.FreeForFemales && CurrentUserSession.Gender == User.eGender.Female) &&
        //             !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos, null)));
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportAbuse1.SendClickEvent += ReportAbuse1_SendClickEvent;
            ReportAbuse1.CancelClickEvent += ReportAbuse1_CancelClickEvent;

            if (!IsPostBack)
            {
                LoadStrings();
            }

            if (Config.Photos.EnablePhotoNotes && !Request.IsIE6())
            {
                Page.RegisterJQuery();
                ((Site) (Page.Master.Master ?? Page.Master)).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference("scripts/jquery.imgnotes.js"));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (CurrentUserSession != null && !CanViewPhoto)
            {
                if (ViewPhotosPermission == PermissionCheckResult.YesButMoreCreditsNeeded || ViewPhotosPermission == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanViewPhotos;
                    Response.Redirect("~/Profile.aspx?sel=payment");
                    return;
                }

                if (ViewPhotosPermission == PermissionCheckResult.No)
                {
                    ((PageBase)Page).StatusPageMessage = "You are not allowed to view photos!".Translate();
                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }
            }


            SetReportAbuseLink();

            if (FirstLoad)
            {
                loadPhotos = true;
                loadComments = Config.Photos.EnablePhotoComments;
                FirstLoad = true;
            }

            if (loadPhotos)
                LoadPhotos();

            if (dlPhotos.Items.Count > 0 && loadComments && !User.IsOptionEnabled(eUserOptions.DisablePhotoComments))
            {
                LoadComments();
                pnlUserComments.Visible = true;
            }
            else
            {
                pnlUserComments.Visible = false;
            }

            divSlideshowLink.Visible = !ReportAbuse1.Visible;

            if (dlPhotos.Items.Count == 0)
            {
                pnlReportAbuseLink.Visible = false;
                divSlideshowLink.Visible = false;
            }

            if (dlPhotos.Items.Count > 0 && !User.IsOptionEnabled(eUserOptions.DisablePhotoRating))
            {
                pnlPhotoRating.Visible = true;
            }
            else
            {
                pnlPhotoRating.Visible = false;
            }

            if (!CanViewPhoto)
            {
                pnlPhotoRating.Visible = false;
                pnlReportAbuseLink.Visible = false;
                divSlideshowLink.Visible = false;
                spanAddNewComment.Visible = false;
            }

            #region Prepare CoolIris link

            if (Config.Misc.EnableCoolIris)
            {
                var rssNews = new HtmlLink();
                rssNews.Attributes.Add("rel", "alternate");
                rssNews.Attributes.Add("type", "application/rss+xml");
                rssNews.Attributes.Add("title", "");
                rssNews.Attributes.Add("id", "gallery");
                var feedUrl = String.Format("CoolIris.ashx?feed=userphotos&username={0}",
                    User.Username);
                rssNews.Attributes.Add("href", feedUrl);
                Page.Header.Controls.Add(rssNews);
            }

            #endregion
        }

        private void ReportAbuse1_CancelClickEvent(object sender, EventArgs e)
        {
            loadComments = Config.Photos.EnablePhotoComments;
            pnlPhotos.Visible = true;
        }

        private void ReportAbuse1_SendClickEvent(object sender, EventArgs e)
        {
            pnlPhotos.Visible = true;
        }

        private void LoadStrings()
        {
            if (Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos)
            {
                btnRatePhoto.Text = Lang.Trans("Rate");
                lblRating.Text = Lang.Trans("Average Rating:");

                if (ddRating.Items.Count == 0)
                {
                    ddRating.Items.Clear();
                    ddRating.Items.Add("");
                    for (int i = Config.Ratings.MinRating; i <= Config.Ratings.MaxRating; i++)
                    {
                        ddRating.Items.Add(
                            new ListItem(i.ToString(), i.ToString()));
                    }
                }
            }
            else
            {
                lblRating.Visible = false;
                lblRatingAverage.Visible = false;
                pnlRatePhoto.Visible = false;
            }

            if (Page is MemberProfile && Config.Users.EnablePhotoAlbums)
            {
                ddPhotoAlbums.Items.Add(new ListItem(
                                            String.Format("{0}'s Photos".Translate(), CurrentUserSession.Username), "-1"));
                PhotoAlbum[] photoAlbums = PhotoAlbum.Fetch(CurrentUserSession.Username);
                foreach (PhotoAlbum photoAlbum in photoAlbums)
                {
                    ddPhotoAlbums.Items.Add(new ListItem(photoAlbum.Name, photoAlbum.ID.ToString()));
                }

                pnlPhotoAlbums.Visible = true;
            }

            LargeBoxStart.Title = Lang.Trans("User Photos");
            hlUserComments.Title = Lang.Trans("User Comments");
            btnSubmitNewComment.Text = Lang.Trans("Submit Comment");
            lnkViewAllComments.Text = Lang.Trans("View All Comments");
            lnkRunSlideshow.Text = Lang.Trans("View as a slideshow");
            btnUnlockPhotos.Text = Lang.Trans("Unlock Photos");
        }

        private void SetReportAbuseLink()
        {
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Username != User.Username)
                {
                    if (Config.AbuseReports.UserCanReportPhotoAbuse
                        && (CurrentUserSession.BillingPlanOptions.UserCanReportAbuse.Value
                        || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanReportAbuse))
                    {
                        pnlReportAbuseLink.Visible = true;
                        lnkReportAbuse.Text = Lang.Trans("Report Abuse");
                    }
                    else
                        pnlReportAbuseLink.Visible = false;
                }
                else
                    pnlReportAbuseLink.Visible = false;
            }
            else
                pnlReportAbuseLink.Visible = false;
        }

        public void LoadPhotos()
        {
            if (User == null) return;

            Photo[] photos = Photo.Fetch(User.Username, PhotoAlbumID);

            var dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoId", typeof(int));
            dtPhotos.Columns.Add("Name");
            dtPhotos.Columns.Add("Description");

            bool hasAccess = ((PageBase)Page).CurrentUserSession != null &&
                             (((PageBase)Page).CurrentUserSession.Username == User.Username ||
                              User.HasUserAccessToPrivatePhotos(User.Username,
                                                                ((PageBase)Page).CurrentUserSession.Username)
                             );

            if (photos != null && photos.Length > 0)
            {
                Photo primaryPhoto =
                    Array.Find(photos, p => p.Primary && p.Approved && (!p.PrivatePhoto || hasAccess)) ??
                    Array.Find(photos, p => p.Approved && (!p.PrivatePhoto || hasAccess));

                if (primaryPhoto != null && ltrPhoto.Text == "")
                {
                    CurrentPhotoId = primaryPhoto.Id;
                    string imageTag = ImageHandler.RenderImageTag(primaryPhoto.Id, 450, 450, "photoframe", false, true);
                    imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");
                    ltrPhoto.Text = imageTag;
                    lblName.Text = Server.HtmlEncode(primaryPhoto.Name);
                    lblDescription.Text = Server.HtmlEncode(primaryPhoto.Description);

                    if ((Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos) && !primaryPhoto.PrivatePhoto)
                        LoadRating(primaryPhoto.Id);

                    if (Config.Photos.EnablePhotoNotes && CanViewPhoto)
                        LoadPhotoNotes(primaryPhoto.Id);

                    if (primaryPhoto.PrivatePhoto)
                    {
                        lblRating.Visible = false;
                        lblRatingAverage.Visible = false;
                        pnlRatePhoto.Visible = false;
                    }
                }

                var lPhotosIDs = new List<int>();

                foreach (Photo photo in photos)
                {
                    if (!photo.Approved) continue;
                    if (photo.PrivatePhoto && !hasAccess)
                        continue;

                    lPhotosIDs.Add(photo.Id);

                    dtPhotos.Rows.Add(new object[]
                                          {
                                              photo.Id,
                                              photo.Name,
                                              photo.Description
                                          });
                }

                if (dtPhotos.Rows.Count > 0 && !CanViewPhoto)
                {
                    ltrPhoto.Text = String.Empty;
                    if (CurrentUserSession != null && ViewPhotosPermission == PermissionCheckResult.YesWithCredits)
                    {
                        btnUnlockPhotos.OnClientClick = String.Format("return confirm(\"" + "Unlocking photos will subtract {0} credits from your balance.".Translate() + "\");",
                        CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits);
                        pnlUnlockPhotos.Visible = true;
                    }

                    lblName.Text = String.Empty;
                    lblDescription.Text = String.Empty;
                }
                else
                {
                    pnlUnlockPhotos.Visible = false;
                }

                UserPhotosIDs = lPhotosIDs.ToArray();
            }
            else
            {
                ltrPhoto.Text = String.Empty;
                pnlUnlockPhotos.Visible = false;
            }

            if (dtPhotos.Rows.Count == 0)
            {
                lblError.Text = Lang.Trans("There are no photos!");
                ltrPhoto.Visible = false;
//                divUsersInPhoto.Visible = false;
            }
            else
            {
                ltrPhoto.Visible = true;
                lblError.Text = "";
//                divUsersInPhoto.Visible = true;
            }

            dlPhotos.DataSource = dtPhotos;
            dlPhotos.DataBind();
        }

        private void LoadPhotoNotes(int photoId)
        {
            var notesData = new List<string>();
            ltrUsersInPhoto.Text = "";
            foreach (var photoNote in PhotoNote.Load(null, photoId, null))
            {
                string notesText = photoNote.Notes ?? "";
                if (photoNote.Username != null)
                {
                    if (ltrUsersInPhoto.Text.Length > 0)
                        ltrUsersInPhoto.Text += ", ";

                    var linkHtml = Request.IsIE6() ? 
                        "<a href=\"{0}\" target=\"_blank\">{1}</a> (" +
                        "<a href=\"{2}\" target=\"_blank\">{3}</a>)" :
                        "<a href=\"{0}\" onmouseover=\"$('#imgPhoto').outlineUsername('{1}');\"" +
                        " onmouseout=\"$('#imgPhoto').removeOutlineUsername('{1}');\" target=\"_blank\">{1}</a> (" +
                        "<a href=\"{2}\" onmouseover=\"$('#imgPhoto').outlineUsername('{1}');\"" +
                        " onmouseout=\"$('#imgPhoto').removeOutlineUsername('{1}');\" target=\"_blank\">{3}</a>)";
                    string userLink = String.Format(linkHtml,
                        UrlRewrite.CreateShowUserUrl(photoNote.Username), photoNote.Username,
                        UrlRewrite.CreateShowUserPhotosUrl(photoNote.Username), "photos".Translate());
                    ltrUsersInPhoto.Text += userLink;
                }
                notesText = notesText.Replace("\"", "\\\"");

                notesData.Add(String.Format("{{\"x1\":\"{0}\",\"y1\":\"{1}\",\"height\":\"{2}\"," +
                    "\"width\":\"{3}\",\"note\":\"{4}\",\"username\":\"{5}\"}}",
                    photoNote.X, photoNote.Y, photoNote.Height, photoNote.Width, notesText,
                    photoNote.Username));
            }

            divUsersInPhoto.Visible = ltrUsersInPhoto.Text.Length > 0 && CanViewPhoto;

            var notesScript = String.Format(
                "notes = [{0}]; lastOffset = undefined;",
                String.Join(", ", notesData.ToArray()));

            // ReSharper disable PossibleNullReferenceException
            if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            // ReSharper restore PossibleNullReferenceException
            {
                notesScript += "$(window).load(function () {" +
                               "   $('#imgPhoto').imgNotes();" +
                               "});";
            }
            else
            {
                notesScript += "$('#imgPhoto').load(function () {" +
                               "   $('#imgPhoto').imgNotes();" +
                               "});";
            }

            notesScript += "$(window).resize(function () {" +
                           "   $('#imgPhoto').imgNotes();" +
                           "});";

            if (!Request.IsIE6())
            {
                ScriptManager.RegisterStartupScript(ltrPhoto, typeof(LiteralControl),
                    "imgNotesHandler", notesScript, true);
            }
        }

        private void LoadRating(int photoID)
        {
            hidcurrentPhotoID.Value = photoID.ToString();

            // Show rating
            try
            {
                var photoRating = new PhotoRating(photoID);

                lblRatingAverage.Text = String.Format(
                    Lang.Trans("{0} ({1} votes)"),
                    photoRating.AverageVote.ToString("0.00"), photoRating.Votes);
            }
            catch (NotFoundException)
            {
                lblRatingAverage.Text = Lang.Trans("no rating");
            }


            // Show voting form


            if (CurrentUserSession != null
                && CurrentUserSession.Username != User.Username
                && ((CurrentUserSession.Level != null &&
                                                      CurrentUserSession.Level.Restrictions.CanRatePhotos)
                                                    || CurrentUserSession.CanRatePhotos() == PermissionCheckResult.Yes)
                && Config.Ratings.EnablePhotoRatings)
            {
                try
                {
                    PhotoRating.FetchVote(((PageBase)Page).CurrentUserSession.Username, photoID);
                    pnlRatePhoto.Visible = false;
                }
                catch (NotFoundException)
                {
                    pnlRatePhoto.Visible = true;
                }
            }
            else
            {
                pnlRatePhoto.Visible = false;
            }
        }

        private void LoadComments()
        {
            if (CurrentPhotoId != -1)
            {
                var dtComments = new DataTable();
                dtComments.Columns.Add("ID", typeof(int));
                dtComments.Columns.Add("Date", typeof(DateTime));
                dtComments.Columns.Add("Username", typeof(string));
                dtComments.Columns.Add("Comment", typeof(string));
                dtComments.Columns.Add("CanDelete", typeof(bool));

                int? countLimit = null;
                if (ViewState["ViewPhotos_ViewAllComments"] == null) countLimit = 5;
                PhotoComment[] comments = countLimit.HasValue
                                              ? PhotoComment.FetchByPhotoID(CurrentPhotoId, countLimit.Value)
                                              : PhotoComment.FetchByPhotoID(CurrentPhotoId);
                if (comments.Length < 5) divViewAllComments.Visible = false;

                if (CurrentUserSession != null)
                    showHideComments();
                else
                    spanAddNewComment.Visible = false;

                foreach (PhotoComment comment in comments)
                {
                    bool canDelete = false;
                    if (CurrentUserSession != null)
                    {
                        if (comment.Username == CurrentUserSession.Username)
                        {
                            spanAddNewComment.Visible = false;
                            canDelete = true;
                        }
                        if (User.Username == CurrentUserSession.Username)
                        {
                            canDelete = true;
                        }
                    }

                    dtComments.Rows.Add(new object[]
                                            {
                                                comment.ID, comment.Date, comment.Username,
                                                Server.HtmlEncode(comment.Comment), canDelete
                                            });
                }

                rptComments.DataSource = dtComments;
                rptComments.DataBind();

                if (CurrentUserSession == null)
                {
                    pnlUserComments.Visible = dtComments.Rows.Count > 0;
                }
            }
            else
            {
                pnlUserComments.Visible = false;
            }
        }

        private void showHideComments()
        {
            if (User.IsUserBlocked(User.Username, CurrentUserSession.Username) || !CanAddComments())
                spanAddNewComment.Visible = false;
            else
                spanAddNewComment.Visible = true;
        }

        private bool CanAddComments()
        {
            if (ViewState["CanAddComments"] == null)
            {
                ViewState["CanAddComments"] =
                    (Config.Photos.EnablePhotoComments &&
                    (CurrentUserSession.CanAddComments() == PermissionCheckResult.Yes ||
                        (CurrentUserSession.Level != null && 
                        CurrentUserSession.Level.Restrictions.UserCanAddComments)
                     ) &&
                     PhotoComment.Fetch(CurrentPhotoId, CurrentUserSession.Username).Length < Config.Users.MaxComments);
            }

            return (bool)ViewState["CanAddComments"];
        }

        private void dlPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName != "ShowPhoto") return;

            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Photo photo = Photo.Fetch(id);

                CurrentPhotoId = id;
                if (CanViewPhoto)
                {
                    string imageTag = ImageHandler.RenderImageTag(photo.Id, 450, 450, null, false, true);

                    // HACK: On image load call the "SetHeight()" javascript function to fix the layout
                    imageTag = imageTag.Replace("/>", " onload=\"SetHeight();\" />");
                    imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");

                    ltrPhoto.Text = imageTag;
                }

                lblName.Text = CanViewPhoto? Server.HtmlEncode(photo.Name) :string.Empty;
                lblDescription.Text = CanViewPhoto ? Server.HtmlEncode(photo.Description) : String.Empty;
                if (Config.Photos.EnablePhotoNotes && CanViewPhoto)
                    LoadPhotoNotes(photo.Id);
                if (Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos)
                    LoadRating(photo.Id);

                ViewState["ViewPhotos_ViewAllComments"] = null;
                divViewAllComments.Visible = true;

                loadComments = Config.Photos.EnablePhotoComments;
            }
            catch (NotFoundException)
            {
                return;
            }
            catch (Exception err)
            {
                ExceptionLogger.Log(Request, err);
            }
        }

        private void btnRatePhoto_Click(object sender, EventArgs e)
        {
            try
            {
                PhotoRating.RatePhoto(
                    ((PageBase)Page).CurrentUserSession.Username,
                    Convert.ToInt32(hidcurrentPhotoID.Value),
                    Convert.ToInt32(ddRating.SelectedValue));

                LoadRating(Convert.ToInt32(hidcurrentPhotoID.Value));
            }
            catch (NullReferenceException)
            {
                Response.Redirect("default.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }
            catch (FormatException)
            {
                //do nothing
            }
            catch (Exception err)
            {
                ExceptionLogger.Log(Request, err);
            }
        }

        protected void lnkReportAbuse_Click(object sender, EventArgs e)
        {
            ReportAbuse1.Visible = true;
            pnlPhotos.Visible = false;

            ReportAbuse1.ReportedUser = User.Username;
            ReportAbuse1.ReportType = AbuseReport.ReportType.Photo;
            ReportAbuse1.TargetID = CurrentPhotoId;
            ReportAbuse1.Text = Lang.Trans("Please tell us why you are reporting this user photo");
        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                PhotoComment comment = PhotoComment.Fetch(commentId);

                if (comment != null)
                {
                    if (CurrentUserSession != null
                        && (comment.Username == CurrentUserSession.Username
                            || User.Username == CurrentUserSession.Username))
                    {
                        PhotoComment.Delete(commentId);
                    }

                    loadComments = true;
                }
            }
        }

        protected void lnkViewAllComments_Click(object sender, EventArgs e)
        {
            divViewAllComments.Visible = false;
            ViewState["ViewPhotos_ViewAllComments"] = true;

            loadComments = true;
        }

        protected void btnSubmitNewComment_Click(object sender, EventArgs e)
        {
            if (txtNewComment.Text.Trim() == "")
            {
                return;
            }

            if (CurrentUserSession != null)
            {
                var comment = new PhotoComment(CurrentPhotoId, CurrentUserSession.Username)
                                  {
                                      Comment = (Config.Misc.EnableBadWordsFilterComments
                                                     ? Parsers.ProcessBadWords(txtNewComment.Text)
                                                     : txtNewComment.Text)
                                  };

                comment.Save();

                User.AddScore(CurrentUserSession.Username,
                              Config.UserScores.LeftComment, "LeftPhotoComment");
                User.AddScore(User.Username, Config.UserScores.ReceivedComment, "ReceivedPhotoComment");

                Photo photo = null;
                try
                {
                    photo = Photo.Fetch(CurrentPhotoId);
                }
                catch (NotFoundException) { return; }

                if (!photo.PrivatePhoto)
                {
                    #region Add NewPhotoComment Event

                    var newEvent = new Event(User.Username) { Type = Event.eType.NewPhotoComment };
                    var newPhotoComment = new NewPhotoComment();
                    newPhotoComment.PhotoCommentID = comment.ID.Value;
                    newEvent.DetailsXML = Misc.ToXml(newPhotoComment);

                    newEvent.Save();

                    if (Config.Users.NewEventNotification)
                    {
                        string text = String.Format("User {0} has left a new comment on one of your photos".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>");
                        int imageID = 0;
                        try
                        {
                            imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                        }
                        string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        User.SendOnlineEventNotification(CurrentUserSession.Username, User.Username, text, thumbnailUrl,
                                                         "Comments.aspx");
                    }

                    #endregion
                }
            }

            loadComments = true;
        }

        protected void lnkRunSlideshow_Click(object sender, EventArgs e)
        {
            if (divSlideshow.Visible)
            {
                divSlideshow.Visible = false;
                pnlPhotos.Visible = true;
                lnkRunSlideshow.Text = Lang.Trans("View as a slideshow");
                loadComments = Config.Photos.EnablePhotoComments;
            }
            else
            {
                divSlideshow.Visible = true;
                pnlPhotos.Visible = false;
                lnkRunSlideshow.Text = Lang.Trans("Back to photos");
            }
        }

        protected void lnkPhoto_Click(object sender, EventArgs e)
        {
            int photoID = getNextPhotoID(UserPhotosIDs);
            Photo photo;
            try
            {
                photo = Photo.Fetch(photoID);
            }
            catch (NotFoundException)
            {
                return;
            }

            string imageTag = ImageHandler.RenderImageTag(photoID, 450, 450, null, false, true);
            imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");
            ltrPhoto.Text = imageTag;
            lblName.Text = Server.HtmlEncode(photo.Name);
            lblDescription.Text = Server.HtmlEncode(photo.Description);
            if (Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos)
                LoadRating(photo.Id);
            if (Config.Photos.EnablePhotoNotes && CanViewPhoto)
                LoadPhotoNotes(photo.Id);
            loadComments = Config.Photos.EnablePhotoComments;
        }

        private int getNextPhotoID(int[] photosIDs)
        {
            int currentPhotoIndex = Array.FindIndex(photosIDs, p => p == CurrentPhotoId);
            int id = currentPhotoIndex == photosIDs.Length - 1 ? photosIDs[0] : photosIDs[currentPhotoIndex + 1];
            CurrentPhotoId = id;
            return id;
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
            this.btnRatePhoto.Click += new EventHandler(btnRatePhoto_Click);
            this.dlPhotos.ItemCommand += new DataListCommandEventHandler(dlPhotos_ItemCommand);
        }

        #endregion

        protected void ddPhotoAlbums_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhotoAlbumID = ddPhotoAlbums.SelectedValue != "-1"
                               ? (int?)Convert.ToInt32(ddPhotoAlbums.SelectedValue)
                               : null;
            ltrPhoto.Text = String.Empty;
            loadPhotos = true;
            loadComments = Config.Photos.EnablePhotoComments;
        }

        protected void btnUnlockPhotos_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
            {
                Response.Redirect("~/Register.aspx");
                return;
            }

            if (UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos, null))
                return;

            if (CurrentUserSession.Credits - CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits < 0)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanViewPhotos;
                Response.Redirect("~/Profile.aspx?sel=payment");
                return;
            }

            CurrentUserSession.Credits -= CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits;
            CurrentUserSession.Update(true);

            CreditsHistory creditsHistory = new Classes.CreditsHistory(CurrentUserSession.Username);
            creditsHistory.Amount = CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits;
            creditsHistory.Service = Classes.CreditsHistory.eService.ViewPhotos;
            creditsHistory.Save();

            UnlockedSection.UnlockSection(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos,
                                          null, DateTime.Now.AddDays(Config.Credits.PhotoUnlockPeriod));

            loadPhotos = true;
            loadComments = Config.Photos.EnablePhotoComments;
            SetReportAbuseLink();
        }
    }
}