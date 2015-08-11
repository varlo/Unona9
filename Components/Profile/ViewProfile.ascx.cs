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
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    public partial class ViewProfile : UserControl
    {
        protected bool blocked;
        protected bool hasBlog;
        protected bool hasPrivatePhoto;
        protected bool hasVideoProfile;
        private bool videoSelected = false;
        protected string joinDate = DateTime.Now.ToShortDateString();
        private bool loadComments;
        private bool loadFriends;
        protected bool messageHistoryExists;
        private User user;
        protected bool verifiedByAdmin;
        protected bool verifiedByUsers;
        protected User.eZodiacSign zodiacSign1 = 0;
        protected User.eZodiacSign? zodiacSign2 = null;

        //private bool IsVideoBroadcastLocked
        //{
        //    get
        //    {
        //        return Config.Credits.Required && Config.Credits.CreditsForVideoStream > 0 &&
        //           (CurrentUserSession == null ||
        //            (CurrentUserSession.Username != User.Username &&
        //             !(Config.Users.FreeForFemales && CurrentUserSession.Gender == User.eGender.Female) &&
        //             !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username,
        //                                                UnlockedSection.SectionType.VideoStream, null)));
        //    }
        //}

        protected bool hasFriends
        {
            get { return ViewState["HasFriends"] == null ? true : (bool)ViewState["HasFriends"]; }
            set { ViewState["HasFriends"] = value; }
        }

        protected string VideoUploadUrl
        {
            get { return ViewState["VideoUploadUrl"] as string; }
            set { ViewState["VideoUploadUrl"] = value; }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        public User User
        {
            set
            {
                user = value;
                if (value != null)
                    ViewState["Username"] = user.Username;
                ViewVideo1.User = value;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        private bool FirstLoad
        {
            get { return ViewState["FirstLoad"] == null ? true : false; }
            set { ViewState["FirstLoad"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        private bool CanAddComments()
        {
            if (ViewState["CanAddComments"] == null)
            {
                ViewState["CanAddComments"] =
                    (Config.Users.EnableProfileComments &&
                    (CurrentUserSession.CanAddComments() == PermissionCheckResult.Yes ||
                        (CurrentUserSession.Level != null && 
                        CurrentUserSession.Level.Restrictions.UserCanAddComments)
                     ) &&
                     Comment.Load(CurrentUserSession.Username, User.Username).Length < Config.Users.MaxComments);
            }

            return (bool)ViewState["CanAddComments"];
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            LoadProfile();

            if (FirstLoad)
            {
                loadComments = Config.Users.EnableProfileComments;
                loadFriends = true;
                FirstLoad = true;
            }

            lnkViewAllPhotos.HRef = UrlRewrite.CreateShowUserPhotosUrl(User.Username);

            if (Config.Misc.EnableProfileVideoBroadcast && VideoBroadcast.GetBroadcast(User.Username).HasValue
                && CurrentUserSession != null && !User.IsUserBlocked(CurrentUserSession.Username))
            {
                pnlVideoBroadcast.Visible = true;
                ViewVideoBroadcast1.User = User;
                ViewVideoBroadcast1.VideoGuid = VideoBroadcast.GetBroadcast(User.Username).Value;

                bool isVideoBroadCastUnlocked =
                    UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username,
                                                        UnlockedSection.SectionType.VideoStream, null);

                var permissionCheckResult = CurrentUserSession.CanViewStreamedVideo();

                if (permissionCheckResult == PermissionCheckResult.Yes || isVideoBroadCastUnlocked)
                {
                    //
                }
                else if (permissionCheckResult == PermissionCheckResult.No)
                {
                    pnlVideoBroadcast.Visible = false;
                }
                else
                {
                    pnlVideoStream.Visible = false;

                    if (permissionCheckResult == PermissionCheckResult.YesWithCredits ||
                        permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
                    {
                        pnlUnlockVideoStream.Visible = true;
                        btnUnlockVideoStream.OnClientClick = String.Format("return confirm(\"" + "Unlocking this section will subtract {0} credits from your balance.".Translate() + "\");",
                        CurrentUserSession.BillingPlanOptions.CanViewStreamedVideo.Credits);
                    }
                    else
                        pnlUnlockVideoStream.Visible = false;

                    if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                    {
                        pnlSubscribe.Visible = true;
                    }
                }

                //if (!CurrentUserSession.BillingPlanOptions.CanViewStreamedVideo)
                //{
                //    pnlSubscribe.Visible = true;
                //    pnlVideoStream.Visible = false;
                //}
                //else if (IsVideoBroadcastLocked)
                //{
                //    pnlVideoStream.Visible = false;
                //}

                //pnlUnlockVideoStream.Visible = IsVideoBroadcastLocked && pnlVideoBroadcast.Visible;
            }

            if (!Config.Ratings.EnableProfileRatings || User.IsOptionEnabled(eUserOptions.DisableProfileRating))
            {
                pnlRating.Visible = false;
            }
            else
            {
                pnlRating.Visible = true;
            }

            if (!Config.Ratings.EnableProfileVoting || User.IsOptionEnabled(eUserOptions.DisableProfileVoting) ||
                lblVotesScore.Text == "")
            {
                pnlVoting.Visible = false;
            }
            else
            {
                pnlVoting.Visible = true;
            }

            if (loadFriends && !User.IsOptionEnabled(eUserOptions.HideFriends))
                LoadFriends();

            if (loadComments && !User.IsOptionEnabled(eUserOptions.DisableProfileComments))
                LoadComments();

            if (!User.IsOptionEnabled(eUserOptions.HideFriends) && hasFriends)
            {
                divUserFriends.Visible = true;
            }
            else
            {
                divUserFriends.Visible = false;
            }

            if (!Config.Users.EnableProfileComments || User.IsOptionEnabled(eUserOptions.DisableProfileComments))
            {
                pnlUserComments.Visible = false;
            }
            else
            {
                pnlUserComments.Visible = true;
            }

            if (!User.IsOptionEnabled(eUserOptions.HideGroupMembership) && plhGroupsLinks.Controls.Count > 0)
            {
                divMemberOf.Visible = true;
            }
            else
            {
                divMemberOf.Visible = false;
            }

            ShowVideos();
            ShowAudioUploads();
        }

        private void ShowHideComments()
        {
            if (User.IsUserBlocked(User.Username, CurrentUserSession.Username) || !CanAddComments())
                spanAddNewComment.Visible = false;
            else
                spanAddNewComment.Visible = true;
        }

        private void LoadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("User Profile");

            if (!Config.Users.LocationPanelVisible || Config.Users.ForceCity != String.Empty)
                pnlLocation.Visible = false;

            if (!Config.Misc.EnableYouTubeVideos)
            {
                pnlVideoEmbed.Visible = false;
            }

            if (!Config.Misc.EnableVideoUpload)
            {
                pnlVideoUpload.Visible = false;
            }

            if (!Config.Misc.EnableAudioUpload)
            {
                pnlAudioHeaderLine.Visible = false;
                pnlAudioUploads.Visible = false;
            }

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
            pnlRelationship.Visible = Config.Users.EnableRelationshipStatus;

            btnRateUser.Text = Lang.Trans("Rate");
            btnSubmitNewComment.Text = Lang.Trans("Submit Comment");
            hlUserComments.Title = Lang.Trans("User Comments");
            hlUserFriends.Title = Lang.Trans("Friends");
            hlVideoBroadcast.Title = Lang.Trans("View Broadcast");
            hlAudio.Title = Lang.Trans("Audio Uploads");
            lnkViewAllComments.Text = Lang.Trans("View All Comments");
            lnkViewAllVideos.Text = Lang.Trans("View All Videos");
            lnkViewAllFriends.Text = Lang.Trans("View All Friends");
            btnUnlockVideo.Text = Lang.Trans("Unlock Video");
            btnUnlockVideoStream.Text = Lang.Trans("Unlock Video Stream");

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

        public void LoadProfile()
        {
            if (User == null)
            {
                return;
            }

            Photo primaryPhoto = null;
            bool hasPhoto = false;
            try
            {
                primaryPhoto = user.GetPrimaryPhoto();
                hasPhoto = primaryPhoto.Approved ? true : false;
            }
            catch (NotFoundException)
            {
            }

            if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                ltrPhoto.Text = String.Format("<a class=\"img-thumbnail\" href=\"{0}\">{1}</a>",
                    UrlRewrite.CreateShowUserPhotosUrl(User.Username),
                    ImageHandler.RenderImageTag(user.Gender, 120, 120, null, false, true));
            }
            else
            {
                if (Config.Photos.EnableJQueryPopeye && !Request.IsIE6())
                {
                    var popeyeCode = new StringBuilder();

                    bool hasViewPermission = Photo.HasViewPhotoPermission(CurrentUserSession, User, false);

                    foreach (var photo in Photo.Fetch(User.Username, primaryPhoto.PhotoAlbumID))
                    {
                        if (!hasViewPermission || !photo.Approved || photo.PrivatePhoto) continue;
                        var photoCode = String.Format("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\"></a></li>",
                                                        ImageHandler.CreateImageUrl(photo.Id, 450, 450, false, true,
                                                                                    false),
                                                        ImageHandler.CreateImageUrl(photo.Id, 200, 150, false, true,
                                                                                    true),
                                                        photo.Name);
                        if (photo.Id == primaryPhoto.Id)
                            popeyeCode.Insert(0, photoCode);
                        else
                            popeyeCode.AppendLine(photoCode);
                    }

                    if (popeyeCode.Length > 0)
                    {
                        popeyeCode.Insert(0, "<div class=\"ppy\"><ul class=\"ppy-imglist\">");
                        popeyeCode.Append(@"</ul><div class=""ppy-outer"">
                                            <div class=""ppy-stage"">
                                                <div class=""ppy-nav"">
                                                    <div class=""nav-wrap"">
                                                        <a class=""ppy-prev"" title=""Previous image"">Previous image</a>
                                                        <a class=""ppy-switch-enlarge"" title=""Enlarge"">Enlarge</a>
                                                        <a class=""ppy-switch-compact"" title=""Close"">Close</a>
                                                        <a class=""ppy-next"" title=""Next image"">Next image</a>
                                                    </div>
                                                </div>
                                                <div class=""ppy-counter"">
                                                    <strong class=""ppy-current""></strong> / <strong class=""ppy-total""></strong> 
                                                </div>
                                            </div>
                                        </div></div>");
                        ltrPhoto.Text = popeyeCode.ToString();

                        Page.RegisterJQuery();
                        ((Site)(Page.Master.Master ?? Page.Master)).ScriptManager.CompositeScript.Scripts.Add(
                            new ScriptReference("scripts/jquery.popeye.js"));
                        Page.Header.Controls.Add(new LiteralControl(
                                                        "<link href=\"images/jquery.popeye.css\" rel=\"stylesheet\" type=\"text/css\" />"));
                        ScriptManager.RegisterStartupScript(this, typeof(ViewProfile), "InitPopeye",
                                                            "$(document).ready(function () {var opt={opacity:0.7};" +
                                                            "$('.ppy').popeye(opt);});", true);
                    }
                    else
                    {
                        ltrPhoto.Text = String.Format("<a class=\"img-thumbnail\" href=\"{0}\">{1}</a><br />",
                                     UrlRewrite.CreateShowUserPhotosUrl(User.Username),
                                     ImageHandler.RenderImageTag(User.Gender, 120, 120, null, false, true));
                    }
                }
                else if (Config.Photos.EnablePhotoStack)
                {
                    ltrPhoto.Text = String.Format("<a href=\"{0}\">{1}</a><br />",
                                                  UrlRewrite.CreateShowUserPhotosUrl(User.Username),
                                                  ImageHandler.RenderImageStackTag(User.Username, 200, 150, 
                                                  ""));
                }
                else
                {
                    ltrPhoto.Text = String.Format("<a class=\"img-thumbnail\" href=\"{0}\">{1}</a><br />",
                                 UrlRewrite.CreateShowUserPhotosUrl(User.Username),
                                 ImageHandler.RenderImageTag(primaryPhoto.Id, 120, 120, null, false, true, true));
                }
            }

            lblUsername.Text = User.Username;

            if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple)
            {
                lblAge.Text = Lang.Trans("him") + " " +
                              (User.Age) +
                              ", " + Lang.Trans("her") + " " +
                              ((int)(DateTime.Now.Subtract(User.Birthdate2).TotalDays / 365.25));
            }
            else
            {
                lblAge.Text = User.Age.ToString();
            }

            lblGender.Text = Lang.Trans(User.Gender.ToString());

            #region Load icons

            #region load zodiac sign

            if (Config.Users.EnableZodiacSign)
            {
                zodiacSign1 = user.ZodiacSign1;
                zodiacSign2 = user.ZodiacSign2;
            }

            #endregion

            #region message history exists

            if (CurrentUserSession == null)
                messageHistoryExists = false;
            else
            {
                messageHistoryExists =
                    Message.MessagesExist(user.Username, CurrentUserSession.Username) ||
                    Message.MessagesExist(CurrentUserSession.Username, user.Username);
            }

            #endregion

            #region verified icons

            if (Config.Users.EnableRealPersonVerificationFunctionality)
                verifiedByUsers = User.IsUserVerified(user.Username, false);

            if (Config.Users.EnableRealPersonVerificationFunctionalityAdmin)
                verifiedByAdmin = User.IsUserVerified(user.Username, true);

            #endregion

            #region blocked icon

            blocked = CurrentUserSession != null && CurrentUserSession.IsUserBlocked(user.Username);

            #endregion

            #region video icon

            if ((Config.Misc.EnableVideoProfile && VideoProfile.HasVideoProfile(user.Username))
                || (Config.Misc.EnableVideoUpload && VideoUpload.HasVideoUpload(user.Username)))
                hasVideoProfile = true;

            #endregion

            #region private photo icon

            if (Config.Photos.EnablePrivatePhotos && user.HasPrivatePhotos())
                hasPrivatePhoto = true;

            #endregion

            #region blog icon

            if (Config.Misc.EnableBlogs && Classes.Blog.HasPosts(user.Username))
                hasBlog = true;

            #endregion

            #endregion

            #region Load Distance

            UserSession currentUserSession = CurrentUserSession;

            if (Config.Users.ShowDistanceFromOnlineUser && currentUserSession != null)
            {
                //Location from = Config.Users.GetLocation(currentUserSession);
                //ZipCode.DoLookupByZipCode(currentUserSession.ZipCode);
                //Location to = Config.Users.GetLocation(User);
                //ZipCode.DoLookupByZipCode(User.ZipCode);

                if (currentUserSession.Username != User.Username)
                {
                    double? distance = currentUserSession.GetDistanceFromUser(User, Config.Search.MeasureDistanceInKilometers);

                    if (distance.HasValue)
                    {
                        string measure = Config.Search.MeasureDistanceInKilometers
                                             ? Lang.Trans("kilometers")
                                             : Lang.Trans("miles");
                        lblDistance.Text = string.Format("{0:F1}", distance) + " " + measure;
                    }
                    else
                        pnlDistance.Visible = false;
                }
                else
                {
                    pnlDistance.Visible = false;
                }
            }
            else
            {
                pnlDistance.Visible = false;
            }

            #endregion

            lblLastOnline.Text = User.LastOnlineString;
            if (Config.Ratings.EnableProfileRatings)
                LoadRating(hasPhoto);
            LoadVotes(hasPhoto);

            #region Load Groups

            if (Config.Groups.EnableGroups && User.Username != "admin" &&
                !User.IsOptionEnabled(eUserOptions.HideGroupMembership))
            {
                Group[] groups = Group.FetchGroupsByUsername(User.Username, true);
                foreach (Group group in groups)
                {
                    if (!group.Approved) continue;

                    if (plhGroupsLinks.Controls.Count > 0)
                        plhGroupsLinks.Controls.Add(new LiteralControl(", "));

                    var anch = new HtmlAnchor
                                   {
                                       HRef = UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                       InnerHtml = Server.HtmlEncode(Parsers.TrimLongWords(group.Name, 20))
                                   };
                   // anch.Attributes.Add("class", "skinlinkcolor");
                    plhGroupsLinks.Controls.Add(anch);
                }
                if (plhGroupsLinks.Controls.Count > 0)
                    divMemberOf.Visible = true;
            }

            #endregion

            #region Load answers

            // Load slogan
            try
            {
                ProfileAnswer slogan = User.FetchSlogan();
                hlSlogan.Title = slogan.Approved
                                     ? Server.HtmlEncode(slogan.Value)
                                     : Lang.Trans("-- pending approval --");
            }
            catch (NotFoundException)
            {
                hlSlogan.Title = Lang.Trans("-- no headline --");
            }

            // Load skype
            try
            {
                ProfileAnswer skype = User.FetchSkype();
                skype.Value = skype.Value.Replace(" ", "");
                string encodedAnswer = Server.HtmlEncode(skype.Value);
                if (skype.Value != String.Empty && skype.Approved
                        && (!Config.Misc.SiteIsPaid
                                || CurrentUserSession != null && User.IsPaidMember(CurrentUserSession.Username)))
                    ltrSkype.Text =
                        String.Format(
                            "<a class=\"tooltip-go\" data-toggle=\"tooltip\" data-placement=\"bottom\" href=\"skype:{0}\" title=\"{2}\"><i class=\"fa fa-skype\"></i></a>",
                            encodedAnswer, encodedAnswer, "Skype me".Translate());
            }
            catch (NotFoundException)
            {
            }

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions == null)
                        continue;

                    bool topicHasQuestion = false;
                    foreach (ProfileQuestion question in questions)
                    {
                        if (question.ShowStyle != ProfileQuestion.eShowStyle.Slogan
                            && question.ShowStyle != ProfileQuestion.eShowStyle.SkypeLink)
                        {
                            topicHasQuestion = true;
                            break;
                        }
                    }

                    if (!topicHasQuestion)
                        continue;

                    var lbs = (HeaderLine)LoadControl("~/Components/HeaderLine.ascx");
                    lbs.Title = Config.Misc.EnableProfileDataTranslation
                                    ? "<h4>" + Lang.Trans(topic.Name) + "</h4>"
                                    : "<h4>" + topic.Name + "</h4>";
                    plhProfile.Controls.Add(lbs);

                    var divBoxcontent = new LiteralControl("<div class=\"form-horizontal\">");
                    plhProfile.Controls.Add(divBoxcontent);

                    ControlCollection ctlQuestions;
                    var tblColumns = new Table { CellPadding = 0, CellSpacing = 0, Width = Unit.Percentage(100) };
                    plhProfile.Controls.Add(tblColumns);
                    var tblRow = new TableRow();
                    tblColumns.Rows.Add(tblRow);
                    TableCell tblCell;
                    int questionNo = 0;
                    bool hasVisibleQuestions = false;

                    #region Adding Questions

                    List<int> lHiddenParentQuestions = new List<int>();

                    foreach (ProfileQuestion question in questions)
                    {
                        if (!question.IsVisible(User.Gender)
                            || question.ShowStyle == ProfileQuestion.eShowStyle.Hidden
                            || question.ShowStyle == ProfileQuestion.eShowStyle.Slogan
                            || question.ShowStyle == ProfileQuestion.eShowStyle.SkypeLink
                            || (question.VisibleForPaidOnly && (CurrentUserSession == null || !CurrentUserSession.Paid))
                            || (question.ParentQuestionID.HasValue && lHiddenParentQuestions.Contains(question.ParentQuestionID.Value)))
                        {
                            lHiddenParentQuestions.Add(question.Id);
                            continue;
                        }

                        if (question.ParentQuestionID.HasValue)
                        {
                            ProfileQuestion parentQuestion = ProfileQuestion.Fetch(question.ParentQuestionID.Value);
                            List<string> lParentQuestionChoices = new List<string>(question.ParentQuestionChoices.Split(':'));
                            string profileAnswerValue = String.Empty;
                            try
                            {
                                profileAnswerValue = parentQuestion.FetchAnswer(User.Username).Value;
                            }
                            catch (NotFoundException)
                            {
                            }
                            List<string> lChoices = new List<string>(profileAnswerValue.Split(':'));

                            if (!lParentQuestionChoices.Exists(parentChoice => lChoices.Contains(parentChoice)))
                            {
                                continue;
                            }
                        }

                        hasVisibleQuestions = true;

                        tblCell = new TableCell { VerticalAlign = VerticalAlign.Top };
                        ctlQuestions = tblCell.Controls;

                        IProfileAnswerComponent cProfile;

                        switch (question.ShowStyle)
                        {
                            case ProfileQuestion.eShowStyle.SingleChoice:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewSingleChoice.ascx");
                                break;
                            case ProfileQuestion.eShowStyle.SingleLine:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewSingleLine.ascx");
                                break;
                            case ProfileQuestion.eShowStyle.MultiLine:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewMultiLine.ascx");
                                break;
                            case ProfileQuestion.eShowStyle.MultiChoiceSmall:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewMultiChoiceSmall.ascx");
                                break;
                            case ProfileQuestion.eShowStyle.MultiChoiceBig:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewMultiChoiceBig.ascx");
                                break;
                            case ProfileQuestion.eShowStyle.Link:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewLink.ascx");
                                break;

                            default:
                                cProfile = (IProfileAnswerComponent)
                                           LoadControl("~/Components/Profile/ViewSingleChoice.ascx");
                                break;
                        }

                        if (cProfile == null)
                        {
                            continue;
                        }

                        cProfile.LoadAnswer(User.Username, question.Id);

                        ctlQuestions.Add((Control)cProfile);

                        tblRow.Cells.Add(tblCell);

                        if (topic.ViewColumns == 1 || ++questionNo % topic.ViewColumns == 0)
                        {
                            tblRow = new TableRow();
                            tblColumns.Rows.Add(tblRow);
                        }
                    }

                    #endregion

                    if (!hasVisibleQuestions)
                    {
                        plhProfile.Controls.Remove(divBoxcontent);
                        plhProfile.Controls.Remove(lbs);
                        plhProfile.Controls.Remove(tblColumns);
                        continue;
                    }

                    plhProfile.Controls.Add(new LiteralControl("</div>"));
                }
            }

            #endregion

            #region Load Status text

            if (Config.Users.EnableUserStatusText)
            {
                lblStatusText.Text = Server.HtmlEncode(Parsers.TrimLongWords(User.StatusText, 20));
                pnlStatusText.Visible = User.StatusText != null && (!Config.Misc.SiteIsPaid || User.IsPaidMember(User.Username));
            }

            #endregion

            #region Load Profile Match

            if (Config.Users.ShowMatchPercentage && CurrentUserSession != null && CurrentUserSession.HasProfile
                && (Config.Users.InterestedInFieldEnabled && User.Gender == CurrentUserSession.InterestedIn
                    || !Config.Users.InterestedInFieldEnabled
                        && (CurrentUserSession.Gender == User.eGender.Male && User.Gender == User.eGender.Female
                          || CurrentUserSession.Gender == User.eGender.Female && User.Gender == User.eGender.Male)))
            {
                string appeal;
                switch (User.Gender)
                {
                    case User.eGender.Male:
                        appeal = "His".Translate();
                        break;
                    case User.eGender.Female:
                        appeal = "Her".Translate();
                        break;
                    default:
                        appeal = "Their".Translate();
                        break;
                }
                int viewerPercentage;
                int viewedPercentage;
                User.CalculateMatchPercentage(CurrentUserSession.Username, User.Username, out viewerPercentage,
                                                      out viewedPercentage);
                lblMatchedPercentage.Text = String.Format("You {0}; {1} {2}".Translate(), viewerPercentage, appeal, viewedPercentage);
                pnlProfileMatch.Visible = viewerPercentage != -1 && viewerPercentage != -1;
            }
            else
            {
                pnlProfileMatch.Visible = false;
            }

            #endregion

            #region Load Relationship

            if (Config.Users.EnableRelationshipStatus)
            {
                spanRelationship.InnerText = "In relationship with".Translate();
                Relationship relationship = Relationship.Fetch(User.Username, null);
                if (relationship == null)
                    lblRelationship.Text = Relationship.GetRelationshipStatusString(Relationship.eRelationshipStatus.Single);
                else if (!relationship.Accepted) pnlRelationship.Visible = false;
                else if (relationship.Accepted)
                {
                    if (relationship.ToUsername == null) spanRelationship.InnerText = "In relationship".Translate();
                    lnkUsername.HRef = UrlRewrite.CreateShowUserUrl(relationship.ToUsername);
                    lnkUsername.InnerText = relationship.ToUsername;
                    lblRelationship.Text = "(" + Relationship.GetRelationshipStatusString(relationship.Type) + ")";
                }
            }
            

            #endregion

            #region Configure translation links

            if (Config.ThirdPartyServices.UseBingTranslate)
            {
                var headerValue = BingTranslator.GetHeaderValue();
                if (headerValue != null)
                {
                    Page.RegisterJQuery();
                    ((Site)(Page.Master.Master ?? Page.Master)).ScriptManager.CompositeScript.Scripts.Add(
                        new ScriptReference("scripts/jquery.sundaymorning.js"));
                    Page.Header.Controls.Add(new LiteralControl(
                        "<link href=\"images/jquery.sundaymorning.css\" rel=\"stylesheet\" type=\"text/css\" />"));

                    var translateScript = @"
$(function() {
    $('#btnTranslate').click(function(evt) {
        $.sundayMorning(null, {
            menuLeft: evt.pageX,
            menuTop: evt.pageY
        },
            function(response) {
                var dest = response;
                $('.translatable').each(function(i) {
                    var obj = $(this);
                    var text = obj.text();
                    $.sundayMorning(text, {
                        apiKey: '" + BingTranslator.GetHeaderValue() + @"',
                        destination: dest
                    },
                    function(res) {
                        obj.html(res.translation);
                    }
                );
                });
            }
        );
    });
});
";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "gtranslate",
                        translateScript, true);
                }
            }

            #endregion
        }

        private void ShowVideos()
        {
            hlVideos.Title = Lang.Trans("Videos");

            Classes.User ViewedUser = User;

            if (CurrentUserSession == null)
            {
                if (!ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewVideos))
                {
                    if (ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewVideos))
                        lblVideoPrivacySettingsError.Text = "Videos are visible for registered users only!".Translate();
                    else
                        lblVideoPrivacySettingsError.Text =
                            ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewVideos)
                                ? "Videos are visible for friends of friends!".Translate()
                                : "Videos are visible for friends only!".Translate();
                    pnlVideos.Visible = false;
                    return;
                }
            }
            else
            {
                if (ViewedUser.Username != CurrentUserSession.Username
                    && !ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewVideos)
                    && !ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewVideos))
                {
                    if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewVideos))
                    {
                        if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                        {
                            bool areFriends = false;
                            string[] friends = User.FetchMutuallyFriends(ViewedUser.Username);
                            foreach (string friend in friends)
                            {
                                if (User.IsUserInFriendList(friend, CurrentUserSession.Username))
                                {
                                    areFriends = true;
                                    break;
                                }
                            }
                            if (!areFriends)
                            {
                                lblVideoPrivacySettingsError.Text = "Videos are visible for friends of friends!".Translate();
                                pnlVideos.Visible = false;
                                return;
                            }
                        }
                    }
                    else if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                    {
                        lblVideoPrivacySettingsError.Text = "Videos are visible for friends only!".Translate();
                        pnlVideos.Visible = false;
                        return;
                    }
                }
            }

            var dtVideos = new DataTable();
            dtVideos.Columns.Add("Title", typeof(string));
            dtVideos.Columns.Add("ThumbnailUrl", typeof(string));
            dtVideos.Columns.Add("VideoUrl", typeof(string));
            int numberOfVideos = 0;

            if (Config.Misc.EnableVideoProfile &&
                VideoProfile.HasVideoProfile(User.Username))
            {
                if (User.HasPrivateVideo() &&
                    (CurrentUserSession == null || !User.HasUserAccessToPrivateVideo(CurrentUserSession.Username)))
                {
                }
                else
                {
                    string thumbnail = ResolveUrl("~/Images/recordedvideo.gif");
                    dtVideos.Rows.Add(new object[] { Lang.Trans("Recorded Video"), thumbnail, "%%recorded video%%" });
                    numberOfVideos++;
                }
            }

            if (Config.Misc.EnableVideoUpload)
            {
                List<VideoUpload> uploads = new List<VideoUpload>();
                if (ViewState["ViewProfile_ViewAllVideos"] == null)
                {
                    uploads = VideoUpload.Load(null, User.Username, true, true, null,
                                               numberOfVideos == 0
                                                   ? Config.Profiles.NumberOfProfileVideosToShow
                                                   : Config.Profiles.NumberOfProfileVideosToShow - numberOfVideos);
                }
                else uploads = VideoUpload.Load(null, User.Username, true, true, null, null);
                var videosUserHaveAccessTo = new List<string>();

                foreach (VideoUpload video in uploads)
                {
                    bool hasAccess = CurrentUserSession != null &&
                                     (CurrentUserSession.Username == User.Username ||
                                      User.HasUserAccessToPrivateVideo(User.Username, CurrentUserSession.Username));

                    if (!video.IsPrivate || hasAccess)
                    {
                        string videoUploadUrl = String.Format("{0}/UserFiles/{1}/video_{2}.flv", Config.Urls.Home,
                                                              User.Username, video.Id);

                        videosUserHaveAccessTo.Add(videoUploadUrl);

                        string thumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                         User.Username, video.Id);
                        if (!File.Exists(Server.MapPath(String.Format("~/UserFiles/{0}/video_{1}.png",
                                                                      User.Username, video.Id))))
                        {
                            thumbnail = ResolveUrl("~/Images/uploadedvideo.gif");
                        }
                        dtVideos.Rows.Add(new object[] { Lang.Trans("Uploaded Video"), thumbnail, videoUploadUrl });
                        numberOfVideos++;
                    }
                }

                if (videosUserHaveAccessTo.Count == 1)
                {
                    int id = GetIdFromVideoURL(videosUserHaveAccessTo[0]);

                    if (!CanViewVideo(id) && (ViewUserVideosPermission == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        ViewUserVideosPermission == PermissionCheckResult.YesWithCredits))
                    {
                        pnlVideoUnlock.Visible = true;
                        btnUnlockVideo.OnClientClick = String.Format("return confirm(\"" + "Unlocking videos will subtract {0} credits from your balance.".Translate() + "\");",
                        CurrentUserSession.BillingPlanOptions.CanViewUserVideos.Credits);
                    }
                    else
                    {
                        pnlVideoUnlock.Visible = false;
                    }
                    //if (IsVideosLocked(id))
                    //{
                    //    pnlVideoUnlock.Visible = true;
                    //}
                    //else
                    //{
                    //    pnlVideoUnlock.Visible = false;
                    //}

                    VideoUploadUrl = videosUserHaveAccessTo[0];
                }
            }

            if (Config.Misc.EnableYouTubeVideos)
            {
                List<VideoEmbed> embeds = new List<VideoEmbed>();
                if (ViewState["ViewProfile_ViewAllVideos"] == null)
                {
                    if (numberOfVideos == 0)
                        embeds = VideoEmbed.Load(null, User.Username, Config.Profiles.NumberOfProfileVideosToShow);
                    else if (Config.Profiles.NumberOfProfileVideosToShow - numberOfVideos > 0)
                    {
                        embeds = VideoEmbed.Load(null, User.Username,
                                                 Config.Profiles.NumberOfProfileVideosToShow - numberOfVideos);
                    }
                }
                else embeds = VideoEmbed.Load(null, User.Username, null);
                

                foreach (VideoEmbed video in embeds)
                {
                    dtVideos.Rows.Add(new object[] { video.Title, video.ThumbUrl, video.VideoUrl });
                    numberOfVideos++;
                }

                if (embeds.Count == 1)
                {
                    ltrVideoEmbed.Text =
                        String.Format(
                            "<object width=\"425\" height=\"350\"><param name=\"movie\" value=\"{0}\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"{0}\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed></object>",
                            embeds[0].VideoUrl);
                    lblVideoEmbedName.Text = embeds[0].Title;
                }
            }

            dlVideos.DataSource = dtVideos;
            dlVideos.DataBind();

            if (dtVideos.Rows.Count == 0)
            {
                pnlRecordedVideo.Visible = false;
                pnlVideoUpload.Visible = false;
                pnlVideoEmbed.Visible = false;
                pnlVideoUnlock.Visible = false;
            }

            if (dtVideos.Rows.Count == 1)
            {
                var videoURL = (string)dtVideos.Rows[0]["VideoUrl"];
                if (videoURL == "%%recorded video%%")
                {
                    hlVideos.Title = Lang.Trans("Recorded Video");
                    pnlRecordedVideo.Visible = true;
                    pnlVideoUpload.Visible = false;
                    pnlVideoEmbed.Visible = false;
                }
                else if (videoURL.EndsWith(".flv"))
                {
                    hlVideos.Title = Lang.Trans("User Video");

                    int id = GetIdFromVideoURL(videoURL);

                    if (/*IsVideosLocked(id)*/ !CanViewVideo(id))
                    {
                        pnlRecordedVideo.Visible = false;
                        pnlVideoUpload.Visible = false;
                        pnlVideoEmbed.Visible = false;

                        if (ViewUserVideosPermission == PermissionCheckResult.YesButMoreCreditsNeeded ||
                            ViewUserVideosPermission == PermissionCheckResult.YesWithCredits)
                        {
                            pnlVideoUnlock.Visible = true;
                            btnUnlockVideo.OnClientClick = String.Format("return confirm(\"" + "Unlocking videos will subtract {0} credits from your balance.".Translate() + "\");",
                            CurrentUserSession.BillingPlanOptions.CanViewUserVideos.Credits);
                        }
                        else
                        {
                            pnlVideoUnlock.Visible = false;
                        }
                    }
                    else
                    {
                        pnlRecordedVideo.Visible = false;
                        pnlVideoUpload.Visible = true;
                        pnlVideoEmbed.Visible = false;
                        pnlVideoUnlock.Visible = false;
                    }
                }
                else
                {
                    hlVideos.Title = Lang.Trans("Embedded Video");
                    pnlRecordedVideo.Visible = false;
                    pnlVideoUpload.Visible = false;
                    pnlVideoEmbed.Visible = true;
                    pnlVideoUnlock.Visible = false;
                }
            }
            else
            {
                if (!videoSelected)
                    pnlVideoUnlock.Visible = false;
            }

            UpdatePanelVideo.Visible = dtVideos.Rows.Count > 0;
            pnlVideosHeaderLine.Visible = dtVideos.Rows.Count > 0;
            divVideoThumbnails.Visible = dtVideos.Rows.Count > 1;
            pnlViewAllVideos.Visible = dtVideos.Rows.Count >= Config.Profiles.NumberOfProfileVideosToShow
                                       && ViewState["ViewProfile_ViewAllVideos"] == null;
        }

        private void ShowAudioUploads()
        {
            if (Config.Misc.EnableAudioUpload)
            {
                AudioUpload[] audioUploads = AudioUpload.Load(null, User.Username, true, null);
                int count = 0;

                foreach (AudioUpload audioUpload in audioUploads)
                {
                    bool hasAccess = CurrentUserSession != null &&
                                     (CurrentUserSession.Username == User.Username ||
                                      User.HasUserAccessToPrivateAudio(User.Username, CurrentUserSession.Username));

                    if (!audioUpload.IsPrivate || hasAccess) count++;
                }

                UpdatePanelAudioUploads.Visible = count > 0;
            }
        }

        private void LoadFriends()
        {
            Classes.User ViewedUser = User;

            if (CurrentUserSession == null)
            {
                if (!ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewFriends))
                {
                    if (ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewFriends))
                        lblFriendsPrivacySettingsError.Text = "Friends are visible for registered users only!".Translate();
                    else
                        lblFriendsPrivacySettingsError.Text =
                            ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewFriends)
                                ? "Friends are visible for friends of friends!".Translate()
                                : "Friends are visible for friends only!".Translate();
                    pnlFriends.Visible = false;
                    return;
                }
            }
            else
            {
                if (ViewedUser.Username != CurrentUserSession.Username
                    && !ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewFriends)
                    && !ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewFriends))
                {
                    if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewFriends))
                    {
                        if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                        {
                            bool areFriends = false;
                            string[] friends = User.FetchMutuallyFriends(ViewedUser.Username);
                            foreach (string friend in friends)
                            {
                                if (User.IsUserInFriendList(friend, CurrentUserSession.Username))
                                {
                                    areFriends = true;
                                    break;
                                }
                            }
                            if (!areFriends)
                            {
                                lblFriendsPrivacySettingsError.Text = "Friends are visible for friends of friends!".Translate();
                                pnlFriends.Visible = false;
                                return;
                            }
                        }
                    }
                    else if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                    {
                        lblFriendsPrivacySettingsError.Text = "Friends are visible for friends only!".Translate();
                        pnlFriends.Visible = false;
                        return;
                    }
                }
            }

            var dtFriends = new DataTable();
            dtFriends.Columns.Add("Username", typeof(string));
            dtFriends.Columns.Add("ImageId", typeof(int));

            int? countLimit = null;
            if (ViewState["ViewProfile_ViewAllFriends"] == null)
                countLimit = dlUserFriends.RepeatColumns * 2;

            string[] usernames = User.FetchMutuallyFriends(User.Username);
            if (usernames == null || usernames.Length == 0)
            {
                divUserFriends.Visible = false;
                hasFriends = false;
                return;
            }
            if (usernames.Length <= dlUserFriends.RepeatColumns * 2)
                divViewAllFriends.Visible = false;

            int count = 0;
            foreach (string username in usernames)
            {
                int imageId;
                try
                {
                    imageId = Photo.GetPrimary(username).Id;
                }
                catch (NotFoundException)
                {
                    try
                    {
                        User friendUser = User.Load(username);
                        imageId = ImageHandler.GetPhotoIdByGender(friendUser.Gender);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }
                }

                dtFriends.Rows.Add(new object[] { username, imageId });

                if (countLimit.HasValue && ++count >= countLimit) break;
            }

            dlUserFriends.DataSource = dtFriends;
            dlUserFriends.DataBind();
        }

        private void LoadComments()
        {
            var dtComments = new DataTable();
            dtComments.Columns.Add("Id", typeof(int));
            dtComments.Columns.Add("DatePosted", typeof(DateTime));
            dtComments.Columns.Add("FromUsername", typeof(string));
            dtComments.Columns.Add("CommentText", typeof(string));
            dtComments.Columns.Add("CanDelete", typeof(bool));

            int countLimit = -1;
            if (ViewState["ViewProfile_ViewAllComments"] == null)
                countLimit = Config.Profiles.NumberOfProfileCommentsToShow;
            Comment[] comments = Comment.Load(User.Username, countLimit);
            if (comments.Length < Config.Profiles.NumberOfProfileCommentsToShow) divViewAllComments.Visible = false;

            if (CurrentUserSession != null)
                ShowHideComments();
            else
                spanAddNewComment.Visible = false;

            foreach (Comment comment in comments)
            {
                bool canDelete = false;
                if (CurrentUserSession != null)
                {
                    if (comment.FromUsername == CurrentUserSession.Username || User.Username == CurrentUserSession.Username)
                    {
                        canDelete = true;
                    }
                }

                dtComments.Rows.Add(new object[]
                                        {
                                            comment.Id, comment.DatePosted, comment.FromUsername,
                                            Server.HtmlEncode(comment.CommentText), canDelete
                                        });
            }

            rptComments.DataSource = dtComments;
            rptComments.DataBind();
        }

        private void LoadRating(bool hasPhoto)
        {
            // Show rating
            if (hasPhoto)
            {
                // Show rating
                try
                {
                    UserRating userRating = UserRating.FetchRating(User.Username);

                    lblRatingAverage.Text = String.Format(
                        Lang.Trans("{0} ({1} votes)"),
                        userRating.AverageVote.ToString("0.00"), userRating.Votes);
                }
                catch (NotFoundException)
                {
                    lblRatingAverage.Text = Lang.Trans("no rating");
                }
            }
            else
            {
                lblRatingAverage.Text = Lang.Trans("no rating");
            }

            // Show voting form
            
            if (CurrentUserSession != null
                && CurrentUserSession.Username != User.Username
                && CurrentUserSession.CanVote
                && ((CurrentUserSession.Level != null &&
                                                      CurrentUserSession.Level.Restrictions.CanRateProfiles)
                                                    || CurrentUserSession.CanRateProfiles() == PermissionCheckResult.Yes))
            {
                try
                {
                    UserRating.FetchVote(CurrentUserSession.Username, User.Username);
                    pnlRateUser.Visible = false;
                }
                catch (NotFoundException)
                {
                    pnlRateUser.Visible = hasPhoto;

                    // select rating
                    try
                    {
                        int vote = UserRating.FetchVote(CurrentUserSession.Username, User.Username);
                        if (ddRating.Items.FindByValue(vote.ToString()) != null)
                        {
                            ddRating.SelectedValue = vote.ToString();
                        }
                    }
                    catch (NotFoundException)
                    {
                    }
                }
            }
            else
            {
                pnlRateUser.Visible = false;
            }
        }

        private void LoadVotes(bool hasPhoto)
        {
            if (hasPhoto)
            {
                lblVotesScore.Text = UserVotes.FetchVotesScore(User.Username).ToString();
                //var average = UserVotes.FetchBayesianAverage(User.Username, TimeSpan.FromDays(3650));
                //if (average != double.NaN)
                //    lblVotesScore.Text += String.Format(" ({0:F}%)", average * 100);
            }
            else
            {
                pnlVoting.Visible = false;
                return;
            }

            // Show voting form
            if (CurrentUserSession != null
                && CurrentUserSession.Username != User.Username)
            {
                try
                {
                    UserVotes.FetchVote(CurrentUserSession.Username, User.Username);
                    pnlVoteUser.Visible = false;
                }
                catch (NotFoundException)
                {
                    pnlVoteUser.Visible = true;
                }
            }
            else
            {
                pnlVoteUser.Visible = false;
            }
        }

        protected void btnRateUser_Click(object sender, EventArgs e)
        {
            try
            {
                UserRating.RateUser(
                    CurrentUserSession.Username,
                    User.Username,
                    Convert.ToInt32(ddRating.SelectedValue));

                LoadRating(true);
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

        protected void btnSubmitNewComment_Click(object sender, EventArgs e)
        {
            if (txtNewComment.Text.Trim() == "")
            {
                return;
            }

            if (CurrentUserSession != null)
            {
                string commentText = Config.Misc.EnableBadWordsFilterComments
                    ? Parsers.ProcessBadWords(txtNewComment.Text) : txtNewComment.Text;
                Comment comment = Comment.Create(CurrentUserSession.Username,
                                                 User.Username, commentText);
                comment.Save();

                #region Add NewProfileComment Event

                    var newEvent = new Event(User.Username) { Type = Event.eType.NewProfileComment };

                    var newProfileComment = new NewProfileComment();
                    newProfileComment.CommentID = comment.Id;
                    newEvent.DetailsXML = Misc.ToXml(newProfileComment);

                    newEvent.Save();

                    if (Config.Users.NewEventNotification)
                    {
                        string text = String.Format("User {0} has left a new comment on your profile".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>");
                        int imageID;
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

                User.AddScore(CurrentUserSession.Username,
                              Config.UserScores.LeftComment, "LeftComment");
                User.AddScore(User.Username, Config.UserScores.ReceivedComment, "ReceivedComment");
            }

            loadComments = true;
        }

        private void lnkViewAllComments_Click(object sender, EventArgs e)
        {
            divViewAllComments.Visible = false;
            ViewState["ViewProfile_ViewAllComments"] = true;

            loadComments = true;
        }

        private void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                Comment comment;
                try
                {
                    comment = Comment.Load(commentId);
                }
                catch (NotFoundException)
                {
                    return;
                }

                if (CurrentUserSession != null
                    && (comment.FromUsername == CurrentUserSession.Username
                        || User.Username == CurrentUserSession.Username))
                {
                    Comment.Delete(commentId);
                }

                loadComments = true;
            }
        }

        private void rptComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var lnkDeleteComment = (LinkButton)e.Item.FindControl("lnkDeleteComment");
            lnkDeleteComment.Attributes.Add("onclick",
                                            String.Format("javascript: return confirm('{0}')",
                                                          Lang.Trans("Do you really want to remove this comment?")));
        }

        protected void lnkViewAllFriends_Click(object sender, EventArgs e)
        {
            divViewAllFriends.Visible = false;
            ViewState["ViewProfile_ViewAllFriends"] = true;

            loadFriends = true;
        }

        protected void btnVote_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/Login.aspx");

            bool positive;

            if (sender == btnVoteDown)
                positive = false;
            else if (sender == btnVoteUp)
                positive = true;
            else
                return;

            if (CurrentUserSession != null) 
                UserVotes.VoteUser(CurrentUserSession.Username, User.Username, positive);

            LoadVotes(true);
        }

        protected void dlVideos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SelectVideo")
            {
                string videoUrl = e.CommandArgument.ToString().Split('|')[1];
                string title = e.CommandArgument.ToString().Split('|')[2];

                if (videoUrl == "%%recorded video%%")
                {
                    hlVideos.Title = Lang.Trans("Recorded Video");
                    pnlRecordedVideo.Visible = true;
                    pnlVideoUpload.Visible = false;
                    pnlVideoEmbed.Visible = false;
                }
                else if (videoUrl.EndsWith(".flv"))
                {
                    videoSelected = true;

                    int id = GetIdFromVideoURL(videoUrl);

                    hlVideos.Title = Lang.Trans("User Video");

                    VideoUploadUrl = videoUrl;

                    if (!CanViewVideo(id))
                    {
                        pnlRecordedVideo.Visible = false;
                        pnlVideoUpload.Visible = false;
                        pnlVideoEmbed.Visible = false;
                        
                        if (ViewUserVideosPermission == PermissionCheckResult.YesButMoreCreditsNeeded ||
                            ViewUserVideosPermission == PermissionCheckResult.YesWithCredits)
                        {
                            pnlVideoUnlock.Visible = true;
                            btnUnlockVideo.OnClientClick = String.Format("return confirm(\"" + "Unlocking videos will subtract {0} credits from your balance.".Translate() + "\");",
                                CurrentUserSession.BillingPlanOptions.CanViewUserVideos.Credits);
                        }
                        else
                        {
                            pnlVideoUnlock.Visible = false;
                        }
                    }
                    else
                    {
                        pnlRecordedVideo.Visible = false;
                        pnlVideoUpload.Visible = true;
                        pnlVideoEmbed.Visible = false;
                        pnlVideoUnlock.Visible = false;
                    }
                }
                else
                {
                    hlVideos.Title = Lang.Trans("Embedded Video");
                    pnlRecordedVideo.Visible = false;
                    pnlVideoUpload.Visible = false;
                    pnlVideoEmbed.Visible = true;
                    pnlVideoUnlock.Visible = false;

                    ltrVideoEmbed.Text =
                        String.Format(
                            "<object width=\"425\" height=\"350\"><param name=\"movie\" value=\"{0}\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"{0}\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed></object>",
                            videoUrl);

                    lblVideoEmbedName.Text = title;
                }
            }
        }

        protected void btnUnlockVideo_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
            {
                Response.Redirect("~/Register.aspx");
                return;
            }

            if (CurrentUserSession.Credits - CurrentUserSession.BillingPlanOptions.CanViewUserVideos.Credits < 0)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanViewUserVideos;
                Response.Redirect("~/Profile.aspx?sel=payment");
                return;
            }

            CurrentUserSession.Credits -= CurrentUserSession.BillingPlanOptions.CanViewUserVideos.Credits;
            CurrentUserSession.Update(true);

            CreditsHistory creditsHistory = new CreditsHistory(CurrentUserSession.Username);
            creditsHistory.Amount = CurrentUserSession.BillingPlanOptions.CanViewUserVideos.Credits;
            creditsHistory.Service = CreditsHistory.eService.ViewVideo;
            creditsHistory.Save();

            int id = GetIdFromVideoURL(VideoUploadUrl);
            UnlockedSection.UnlockSection(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Video,
                                          id, DateTime.Now.AddDays(Config.Credits.VideoUnlockPeriod));

            pnlVideoUpload.Visible = true;
        }

        protected void btnUnlockVideoStream_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
            {
                Response.Redirect("~/Register.aspx");
                return;
            }

            int credits = CurrentUserSession.BillingPlanOptions.CanViewStreamedVideo.Credits;
            if (CurrentUserSession.Credits - credits /*Config.Credits.CreditsForVideoStream*/ < 0)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanViewStreamedVideo;
                Response.Redirect("~/Profile.aspx?sel=payment");
                return;
            }

            CurrentUserSession.Credits -= credits /*Config.Credits.CreditsForVideoStream*/;
            CurrentUserSession.Update(true);

            CreditsHistory creditsHistory = new CreditsHistory(CurrentUserSession.Username);
            creditsHistory.Amount = credits;//Config.Credits.CreditsForVideoStream;
            creditsHistory.Service = CreditsHistory.eService.ViewVideoStream;
            creditsHistory.Save();

            UnlockedSection.UnlockSection(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.VideoStream,
                                          null, DateTime.Now.AddDays(Config.Credits.VideoStreamUnlockPeriod));
            pnlVideoStream.Visible = true;
        }

        private int GetIdFromVideoURL(string url)
        {
            string removedExtension = url.Remove(url.LastIndexOf(".flv"));
            return Convert.ToInt32(removedExtension.Substring(url.LastIndexOf("video_") + "video_".Length));
        }

        //private bool IsVideosLocked(int id)
        //{
        //    return Config.Credits.Required && Config.Credits.CreditsForUserVideo > 0 &&
        //        (CurrentUserSession == null ||
        //        (CurrentUserSession.Username != User.Username &&
        //         !(Config.Users.FreeForFemales && CurrentUserSession.Gender == User.eGender.Female) &&
        //         !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Video, id)));
        //}

        PermissionCheckResult? viewUserVideosPermission;
        private PermissionCheckResult ViewUserVideosPermission
        {
            get
            {
                if (!viewUserVideosPermission.HasValue)
                    viewUserVideosPermission = CurrentUserSession.CanViewUserVideos();

                return viewUserVideosPermission.Value;
            }
        }

        private bool CanViewVideo(int id)
        {
            if (CurrentUserSession == null)
            {
                return Config.Users.GetNonPayingMembersOptions().CanViewUserVideos.Value;
            }

            if (CurrentUserSession.Username == User.Username || ViewUserVideosPermission == PermissionCheckResult.Yes ||
                UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Video, id))
                return true;

            return false;
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
            this.lnkViewAllComments.Click += new EventHandler(lnkViewAllComments_Click);
            this.rptComments.ItemCreated +=
                new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptComments_ItemCreated);
            this.rptComments.ItemCommand +=
                new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptComments_ItemCommand);
        }

        #endregion

        protected void lnkViewAllVideos_Click(object sender, EventArgs e)
        {
            pnlViewAllVideos.Visible = false;
            ViewState["ViewProfile_ViewAllVideos"] = true;
        }
    }
}