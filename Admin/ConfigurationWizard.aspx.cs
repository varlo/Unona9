﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using System.IO;

namespace AspNetDating.Admin
{
    public enum SiteType
    {
        DatingSite,
        CommunitySite,
        Custom
    }

    public enum SiteModel
    {
        SubscriptionBased,
        PerContact,
        Free
    }

    //public enum SiteLanguage
    //{
    //    English,
    //    Spanish,
    //    German,
    //    Italian,
    //    French
    //}

    public partial class ConfigurationWizard : AdminPageBase
    {
        public ConfigurationWizard()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
                LoadDefaults();
            }

            Page.RegisterJQuery();
        }

        private void LoadStrings()
        {
            //step 1 - Site Details
            rblSiteType.Items.Add(new ListItem("dating site".TranslateA(), ((int)SiteType.DatingSite).ToString()));
            rblSiteType.Items.Add(new ListItem("community site".TranslateA(), ((int)SiteType.CommunitySite).ToString()));
            rblSiteType.Items.Add(new ListItem("custom site".TranslateA(), ((int)SiteType.Custom).ToString()));

            //DECLARE @SpanishId INT
            //SET @SpanishId = 2
            //DECLARE @GermanId INT
            //SET @GermanId = 3
            //DECLARE @FrenchId INT
            //SET @FrenchId = 4
            //DECLARE @ItalianId INT
            //SET @ItalianId = 5
            cblSiteLanguages.Items.Add(new ListItem("English", 1.ToString()));
            cblSiteLanguages.Items.Add(new ListItem("Spanish", 2.ToString()));
            cblSiteLanguages.Items.Add(new ListItem("German", 3.ToString()));
            cblSiteLanguages.Items.Add(new ListItem("French", 4.ToString()));
            cblSiteLanguages.Items.Add(new ListItem("Italian", 5.ToString()));


            //step 2 - Site Settings

            cbEnableAjaxChatRoom.Text = "Enable ajax chat room".TranslateA();
            cbEnableMessenger.Text = "Enable 1-to-1 instant messenger (the AspNetAjaxChat software is required)".TranslateA();
            cbEnableCoolIris.Text = "Enable CoolIris support (http://www.cooliris.com)".TranslateA();
            cbEnablePhotoAlbums.Text = "Enable photo albums".TranslateA();
            cbEnableClassifiedAds.Text = "Enable classified ads".TranslateA();
            cbOnlyRegisteredUsersCanBrowseClassifiedAds.Text = "Only registered users can browse classified ads".TranslateA();
            cbAllowUsersToLeaveCommentsOnTheClassifiedAds.Text = "Allow users to leave comments on the classified ads".TranslateA();
            cbEnableBlogs.Text = "Enable blogs".TranslateA();
            cbEnableBlogPostApproval.Text = "Administrator should approve blog posts before they are visible".TranslateA();

            cbEnableCommunityGroups.Text = "Enable community groups".TranslateA();
            cbEnableAjaxChatRoomsInGroups.Text = "Enable ajax chat rooms in groups".TranslateA();
            cbAllowUsersToCreateGroups.Text = "Allow users to create groups".TranslateA();

            cbEnableVideoFileUploads.Text = "Enable video file uploads (the Video Converter plugin is required)".TranslateA();
            cbEnableYouTubeVideosEmbedding.Text = "Enable YouTube videos embedding".TranslateA();
            cbEnableMP3FileUploads.Text = "Enable MP3 file uploads".TranslateA();

            cbEnableLiveWebcamVideoStreaming.Text = "Enable live webcam video streaming (the Video Streamer plugin is required)".TranslateA();
            cbEnableGadgets.Text = "Enable Windows Vista® and Windows 7® compatible Sidebar gadgets".TranslateA();
            cbEnableAdBlockerBlocker.Text = "Enable ad-blocker blocker (blocks users using ad-blockers)".TranslateA();
            cbEnableSkypeIntegration.Text = "Enable Skype integration".TranslateA();
            cbEnableECards.Text = "Enable e-cards".TranslateA();
            cbEnableFriends.Text = "Enable friends".TranslateA();
            cbEnableFavorites.Text = "Enable favorites".TranslateA();
            cbEnableDistanceSearch.Text = "Enable distance search".TranslateA();

            //step 3 - User Settings
            cbPhotoApprovalRequiredByAdministrator.Text = "Administrator should approve photos before they are posted".TranslateA();
            cbAllowExplicitPhotos.Text = "Allow explicit photos".TranslateA();
            cbAlwaysMakeExplicitPhotosPrivate.Text = "Always make explicit photos private".TranslateA();
            cbAllowPrivatePhotos.Text = "Allow private photos".TranslateA();
            cbAllowUsersToCommentOnProfiles.Text = "Allow users to comment on profiles".TranslateA();
            cbAllowUsersToCommentOnPhotos.Text = "Allow users to comment on photos".TranslateA();
            cbAllowUsersToRateProfiles.Text = "Allow users to rate profiles".TranslateA();
            cbAllowUsersToRatePhotos.Text = "Allow users to rate photos".TranslateA();
            cbEnableHotOrNotStyle.Text = "Enable HOT or NOT style rating system".TranslateA();
            cbAllowUsersToSetStatus.Text = "Allow users to set status".TranslateA();
            cbAllowUsersToUseSkins.Text = "Allow users to use skins for their profile page".TranslateA();
            cbAllowUsersToCustomizeSkin.Text = "Allow users to customize the skin with their own images, colors, etc".TranslateA();
            cbAllowUsersToSpecifyRelationships.Text = "Allow users to specify relationships with other users".TranslateA();

            //step 4 - Anti-spam Settings
            //cbEnableSpamDetection.Text = "Enable spam detection".TranslateA();
            cbEnableCaptchaValidation.Text = "Enable CAPTCHA validation".TranslateA();
            cbRequireEmailConfirmation.Text = "Require user e-mail confirmation".TranslateA();
            cbEnableEmailAndPhoneNumberFiltering.Text = "Enable e-mail and phone number filtering in messages".TranslateA();
            cbBlockSendingMultipleSimilarMessages.Text = "Block users sending multiple similar messages until reviewed by administrator".TranslateA();
            cbManuallyApproveInitialUserMessages.Text = "Manually approve the initial user messages".TranslateA();
            cbAllowUsersToReportProfileAbuse.Text = "Allow users to report profile abuse".TranslateA();
            cbAllowUsersToReportPhotoAbuse.Text = "Allow users to report photo abuse".TranslateA();
            cbAllowUsersToReportMessageAbuse.Text = "Allow users to report message abuse".TranslateA();
        }

        private void LoadDefaults()
        {
            var billingPlanOptions = Config.Users.GetNonPayingMembersOptions();

            txtMaximumGroupsToJoin.Text = billingPlanOptions.MaxGroupsPerMember.Value.ToString();//Config.Groups.MaxGroupsPerMember.ToString();
            txtMaximumMessagesPerDay.Text = billingPlanOptions.MaxMessagesPerDay.Value.ToString();// Config.Users.MembersMaxMessagesPerDay.ToString();
            txtMaximumMP3FilesPerUser.Text = billingPlanOptions.MaxAudioUploads.Value.ToString();//Config.Misc.MaxAudioUploads.ToString();
            txtMaximumPhotosPerUser.Text = Config.Photos.MaxPhotos.ToString();
            txtMaximumUploadedVideosPerUser.Text = 3.ToString();// Config.Misc.MaxVideoUploads.ToString();
            txtMaximumUsersContactedPerDay.Text = Config.Users.MaxContactedUsersPerDay.ToString();
            txtMaximumYouTubeVideosPerUser.Text = 6.ToString();// Config.Misc.MaxYouTubeVideos.ToString();
            txtMessagesPerUserToBeApproved.Text = Config.Users.MessageVerificationsCount.ToString();
            txtMinimumAgeToSeeExplicitPhotos.Text = Config.Users.MinAgeForExplicitPhotos.ToString();
            txtNumberOfSimilarMessages.Text = Config.Misc.MaxSameMessages.ToString();
        }

        protected void wzWizard_ActiveStepChanged(object sender, EventArgs e)
        {
            
        }

        protected void wzWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (wzWizard.ActiveStep == wsSiteDetails)
            {
                if (fuLogo.HasFile)
                    fuLogo.PostedFile.SaveAs(Server.MapPath("~/Images/logo.png"));

                if (!ValidateStep1Input())
                {
                    e.Cancel = true;
                    return;
                }

                if (rblSiteType.SelectedValue == ((int)SiteType.DatingSite).ToString())
                {
                    TurnOnDatingFeatures();
                }
                else if (rblSiteType.SelectedValue == ((int)SiteType.CommunitySite).ToString())
                {
                    TurnOnCommunityFeatures();
                }
            }
            else if (wzWizard.ActiveStep == wsSiteSettings)
            {
                if (!ValidateStep2Input())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (wzWizard.ActiveStep == wsUserSettings)
            {
                if (!ValidateStep3Input())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (wzWizard.ActiveStep == wsAntiSpamSettings)
            {
                if (!ValidateStep4Input())
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private bool ValidateStep1Input()
        {
            try
            {
                Classes.User.ValidateEmail(txtSiteEmail.Text);
            }
            catch (Exception) { 
                MessageBox.Show("Invalid e-mail address!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (rblSiteType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select site type!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (cblSiteLanguages.SelectedIndex == -1 && (!cbOtherLanguage.Checked || txtOtherLanguage.Text.Trim().Length == 0))
            {
                MessageBox.Show("Please select at least one language to proceed!".TranslateA(), Misc.MessageType.Error);
                return false;                
            }

            return true;
        }

        private bool ValidateStep2Input()
        {
            int temp;
            if (!Int32.TryParse(txtMaximumGroupsToJoin.Text, out temp))
            {
                MessageBox.Show("Maximum group number is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMaximumUploadedVideosPerUser.Text, out temp))
            {
                MessageBox.Show("Maximum uploaded videos number is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMaximumYouTubeVideosPerUser.Text, out temp))
            {
                MessageBox.Show("Maximum uploaded videos number is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMaximumMP3FilesPerUser.Text, out temp))
            {
                MessageBox.Show("Maximum mp3 files number is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            return true;
        }

        private bool ValidateStep3Input()
        {
            int temp;

            if (!Int32.TryParse(txtMinimumAgeToSeeExplicitPhotos.Text, out temp))
            {
                MessageBox.Show("Minimum age is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMaximumPhotosPerUser.Text, out temp))
            {
                MessageBox.Show("Maximum photos number is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }
            
            return true;
        }

        private bool ValidateStep4Input()
        {
            int temp;

            if (!Int32.TryParse(txtNumberOfSimilarMessages.Text, out temp))
            {
                MessageBox.Show("Number of similar messages is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMessagesPerUserToBeApproved.Text, out temp))
            {
                MessageBox.Show("Number of message approvals per user is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMaximumMessagesPerDay.Text, out temp))
            {
                MessageBox.Show("Number of maximum messages per day is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            if (!Int32.TryParse(txtMaximumUsersContactedPerDay.Text, out temp))
            {
                MessageBox.Show("Number of maximum users contacted per day is invalid!".TranslateA(), Misc.MessageType.Error);
                return false;
            }

            return true;
        }

        private void TurnOnDatingFeatures()
        {
            cbEnableAjaxChatRoom.Checked = true;
            cbEnableECards.Checked = true;
            cbEnableFavorites.Checked = true;
            cbEnableDistanceSearch.Checked = true;

            TurnOnCommonFeatures();
        }
        private void TurnOnCommunityFeatures()
        {
            cbEnablePhotoAlbums.Checked = true;
            cbEnableClassifiedAds.Checked = true;
            cbEnableBlogs.Checked = true;
            cbEnableCommunityGroups.Checked = true;
            cbEnableSkypeIntegration.Checked = true;
            cbEnableFriends.Checked = true;

            TurnOnCommonFeatures();
        }
        private void TurnOnCommonFeatures()
        {
            cbEnableCoolIris.Checked = true;
            cbEnableYouTubeVideosEmbedding.Checked = true;
            cbEnableMP3FileUploads.Checked = true;
            cbEnableGadgets.Checked = true;
        }

        private void StoreAllSettings()
        {
            var billingPlanOptions = Config.Users.GetNonPayingMembersOptions();

            #region step 1
            Config.Misc.SiteTitle = txtSiteName.Text;
            Config.Misc.SiteEmail = txtSiteEmail.Text;

            //var siteModel = (SiteModel)Int32.Parse(rblSiteModel.SelectedValue);
            //switch(siteModel)
            //{
            //    case SiteModel.SubscriptionBased:
            //        Config.Users.PaymentRequired = true;
            //        Config.Credits.Required = false;
            //        break;
            //    case SiteModel.PerContact:
            //        Config.Credits.Required = true;
            //        Config.Users.PaymentRequired = false;
            //        break;
            //    case SiteModel.Free:
            //        Config.Credits.Required = false;
            //        Config.Users.PaymentRequired = false;
            //        break;
            //}

            #region activate selected languages
            foreach (ListItem item in cblSiteLanguages.Items)
            {
                var language = Language.Fetch(Int32.Parse(item.Value));

                if (item.Selected)
                {
                    language.Active = true;
                }
                else
                {
                    language.Active = false;
                }

                language.Save();
            }

            if (cbOtherLanguage.Checked && txtOtherLanguage.Text.Trim().Length > 0)
            {
                var language = Language.Create(txtOtherLanguage.Text, true);
                language.Save();
            }
            #endregion

            #endregion

            #region step 2

            Config.Misc.EnableAjaxChat = cbEnableAjaxChatRoom.Checked;
            billingPlanOptions.UserCanUseChat.Value = cbEnableAjaxChatRoom.Checked;

            Config.Misc.EnableIntegratedIM = cbEnableMessenger.Checked;
            Config.Misc.EnableCoolIris = cbEnableCoolIris.Checked;
            Config.Users.EnablePhotoAlbums = cbEnablePhotoAlbums.Checked;
            Config.Ads.Enable = cbEnableClassifiedAds.Checked;
            if (Config.Ads.Enable)
            {
                Config.Ads.OnlyRegisteredUsersCanBrowseClassifieds = cbOnlyRegisteredUsersCanBrowseClassifiedAds.Checked;
                Config.Ads.EnableAdComments = cbAllowUsersToLeaveCommentsOnTheClassifiedAds.Checked;
            }
            Config.Misc.EnableBlogs = cbEnableBlogs.Checked;
            if (Config.Misc.EnableBlogs)
            {
                Config.Misc.EnableBlogPostApproval = cbEnableBlogPostApproval.Checked;
            }

            Config.Groups.EnableGroups = cbEnableCommunityGroups.Checked;
            if (Config.Groups.EnableGroups)
            {
                Config.Groups.EnableAjaxChat = cbEnableAjaxChatRoomsInGroups.Checked;
                billingPlanOptions.CanCreateGroups.Value = cbAllowUsersToCreateGroups.Checked;
                billingPlanOptions.MaxGroupsPerMember.Value = Int32.Parse(txtMaximumGroupsToJoin.Text);
            }

            Config.Misc.EnableVideoUpload = cbEnableVideoFileUploads.Checked;
            if (Config.Misc.EnableVideoUpload)
            {
                billingPlanOptions.MaxVideoUploads.Value = Int32.Parse(txtMaximumUploadedVideosPerUser.Text);
            }

            Config.Misc.EnableYouTubeVideos = cbEnableYouTubeVideosEmbedding.Checked;
            if (Config.Misc.EnableYouTubeVideos)
            {
                billingPlanOptions.MaxVideos.Value = Int32.Parse(txtMaximumYouTubeVideosPerUser.Text);
                //Config.Misc.MaxYouTubeVideos = Int32.Parse(txtMaximumYouTubeVideosPerUser.Text);
            }

            Config.Misc.EnableAudioUpload = cbEnableMP3FileUploads.Checked;
            if (Config.Misc.EnableAudioUpload)
            {
                billingPlanOptions.MaxAudioUploads.Value = Int32.Parse(txtMaximumMP3FilesPerUser.Text);
            }

            Config.Misc.EnableProfileVideoBroadcast = cbEnableLiveWebcamVideoStreaming.Checked;
            Config.Misc.EnableGadgets = cbEnableGadgets.Checked;
            Config.Misc.StopUsersWithAdBlocker = cbEnableAdBlockerBlocker.Checked;
            if (cbEnableSkypeIntegration.Checked)
            {
                //create question
                ProfileQuestion question = new ProfileQuestion();
                question.Name = "Skype";
                question.AltName = String.Empty;
                question.Description = "Skype";
                question.Hint = String.Empty;
                question.EditStyle = ProfileQuestion.eEditStyle.SingleLine;
                question.ShowStyle = ProfileQuestion.eShowStyle.SkypeLink;
                question.SearchStyle = ProfileQuestion.eSearchStyle.Hidden;
                question.Required = false;
                question.TopicID = 2;//hardcoded value

                question.Save();
            }
            //Config.Users.EnableEcards = cbEnableECards.Checked;
            Config.Users.EnableFriends = cbEnableFriends.Checked;
            Config.Users.EnableFavorites = cbEnableFavorites.Checked;
            Config.Search.DistanceSearchEnabled = cbEnableDistanceSearch.Checked;
            #endregion

            #region step 3
            Config.Photos.AutoApprovePhotos = !cbPhotoApprovalRequiredByAdministrator.Checked;
            Config.Photos.EnableExplicitPhotos = cbAllowExplicitPhotos.Checked;
            if (Config.Photos.EnableExplicitPhotos)
            {
                Config.Photos.MakeExplicitPhotosPrivate = cbAlwaysMakeExplicitPhotosPrivate.Checked;
                Config.Users.MinAgeForExplicitPhotos = Int32.Parse(txtMinimumAgeToSeeExplicitPhotos.Text);
            }
            Config.Photos.EnablePrivatePhotos = cbAllowPrivatePhotos.Checked;
            Config.Photos.MaxPhotos = Int32.Parse(txtMaximumPhotosPerUser.Text);
            Config.Users.EnableProfileComments = cbAllowUsersToCommentOnProfiles.Checked;
            Config.Photos.EnablePhotoComments = cbAllowUsersToCommentOnPhotos.Checked;
            Config.Ratings.EnableProfileRatings = cbAllowUsersToRateProfiles.Checked;
            Config.Ratings.EnablePhotoRatings = cbAllowUsersToRatePhotos.Checked;
            if (Config.Ratings.EnablePhotoRatings)
            {
                Config.Ratings.EnableRatePhotos = cbEnableHotOrNotStyle.Checked;
            }
            Config.Users.EnableUserStatusText = cbAllowUsersToSetStatus.Checked;
            Config.Users.EnableProfileSkins = cbAllowUsersToUseSkins.Checked;
            if (Config.Users.EnableProfileSkins)
            {
                billingPlanOptions.UserCanUseSkin.Value = cbAllowUsersToUseSkins.Checked;
                billingPlanOptions.UserCanEditSkin.Value = cbAllowUsersToCustomizeSkin.Checked;
            }
            Config.Users.EnableRelationshipStatus = cbAllowUsersToSpecifyRelationships.Checked;
            #endregion

            #region step 4
            //Config.Misc.EnableSpamDetection = cbEnableSpamDetection.Checked;
            Config.Misc.EnableCaptcha = cbEnableCaptchaValidation.Checked;
            Config.Users.AutoActivateUsers = !cbRequireEmailConfirmation.Checked;
            Config.Misc.EnableMessageFilter = cbEnableEmailAndPhoneNumberFiltering.Checked;
            Config.Misc.EnableSpamDetection = cbBlockSendingMultipleSimilarMessages.Checked;
            if (Config.Misc.EnableSpamDetection)
            {
                Config.Misc.MaxSameMessages = Int32.Parse(txtNumberOfSimilarMessages.Text);
            }

            Config.Users.MessageVerificationEnabled = cbManuallyApproveInitialUserMessages.Checked;
            if (Config.Users.MessageVerificationEnabled)
            {
                Config.Users.MessageVerificationsCount = Int32.Parse(txtMessagesPerUserToBeApproved.Text);
            }

            //Config.Users.MembersMaxMessagesPerDay
            billingPlanOptions.MaxMessagesPerDay.Value = Int32.Parse(txtMaximumMessagesPerDay.Text);
            Config.Users.MaxContactedUsersPerDay = Int32.Parse(txtMaximumUsersContactedPerDay.Text);

            Config.AbuseReports.UserCanReportProfileAbuse = cbAllowUsersToReportProfileAbuse.Checked;
            Config.AbuseReports.UserCanReportPhotoAbuse = cbAllowUsersToReportPhotoAbuse.Checked;
            Config.AbuseReports.UserCanReportMessageAbuse = cbAllowUsersToReportMessageAbuse.Checked;
            #endregion

            Config.Users.SetNonPayingMembersOptions(billingPlanOptions);
        }

        protected void wzWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            StoreAllSettings();

            Config.Misc.EnableFirstRunWizard = false;

            Response.Redirect("~/Admin/ThemeManager.aspx");
        }
    }
}
