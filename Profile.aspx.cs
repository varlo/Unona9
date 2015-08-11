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
using AspNetDating.Classes;
using AspNetDating.Components;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AspNetDating
{
    public partial class MemberProfile : PageBase
    {
        protected SmallBoxStart SmallBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            Settings1.SettingsSaved += Settings1_SettingsSaved;
            Page.RegisterJQuery();
            if (!Page.IsPostBack)
            {
                LoadStrings();
                LoadData();

                if (((PageBase)Page).CurrentUserSession != null &&
                    !((PageBase)Page).CurrentUserSession.HasPhotos && 
                    Request.Params["err"] == "nophoto")
                {
                    lnkUploadPhotos_Click(null, null);
                    Master.SetSelectedLink(lnkUploadPhotos.ClientID);
                    return;
                }

                switch (Request.Params["sel"])
                {
                    case "payment":
                        lnkSubscription_Click(null, null);
                        Master.SetSelectedLink(lnkSubscription.ClientID);
                        break;
                    case "photos":
                        lnkUploadPhotos_Click(null, null);
                        Master.SetSelectedLink(lnkUploadPhotos.ClientID);
                        break;
                    case "set":
                        lnkSettings_Click(null, null);
                        Master.SetSelectedLink(lnkSettings.ClientID);
                        break;
                    case "privacy":
                        lnkPrivacySettings_Click(null, null);
                        Master.SetSelectedLink(lnkPrivacySettings.ClientID);
                        break;
                    case "videouploads":
                        lnkUploadVideo_Click(null, null);
                        Master.SetSelectedLink(lnkUploadVideo.ClientID);
                        break;
                    default:
                        lnkEditProfile_Click(null, null);
                        Master.SetSelectedLink(lnkEditProfile.ClientID);
                        break;
                }
            }
        }

        void Settings1_SettingsSaved(object sender, EventArgs e)
        {
            pnlEditSkin.Visible = Config.Users.EnableProfileSkins && ((CurrentUserSession.Level != null &&
                                                      CurrentUserSession.Level.Restrictions.UserCanUseSkin)
                                                    || CurrentUserSession.CanUseSkin() == PermissionCheckResult.Yes)
                                   && ((CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanEditSkin)
                                   || CurrentUserSession.CanEditSkin() == PermissionCheckResult.Yes) && !String.IsNullOrEmpty(CurrentUserSession.ProfileSkin);
        }

        private void LoadData()
        {
            EditProfile1.User = CurrentUserSession;
            ViewProfile1.User = CurrentUserSession;
            EditPhotos1.User = CurrentUserSession;
            ViewPhotos1.User = CurrentUserSession;
            ViewEvents1.User = CurrentUserSession;
            PrivacySettings1.User = CurrentUserSession;
            Settings1.User = CurrentUserSession;
            Billing1.User = CurrentUserSession;
            EditSkin1.User = CurrentUserSession;
        }

        private void LoadStrings()
        {
            lnkEditProfile.Text = Lang.Trans("Manage Profile");
            lnkViewProfile.Text = Lang.Trans("View Profile");
            lnkUploadPhotos.Text = Lang.Trans("Manage Photos");
            lnkUploadSalutePhoto.Text = Lang.Trans("Upload Salute Photo");
            lnkViewPhotos.Text = Lang.Trans("View Photos");
            lnkViewEvents.Text = Lang.Trans("View Events");
            lnkPrivacySettings.Text = "Privacy Settings".Translate();
            lnkSettings.Text = Lang.Trans("Settings");
            lnkEditSkin.Text = Lang.Trans("Edit Skin");
            lnkUploadVideo.Text = Lang.Trans("Manage Video Files");
            lnkUploadAudio.Text = Lang.Trans("Manage Audio Files");

            if (Config.Misc.SiteIsPaid &&
                !(Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female))
            {
                if (lnkSubscription != null)
                {
                    lnkSubscription.Text = Lang.Trans("Billing");
                }
            }
            else if (trSubscription != null)
            {
                trSubscription.Visible = false;
            }

            if (!Config.Misc.EnableAudioUpload)
            {
                pnlAudioUpload.Visible = false;
            }


            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("Profile Management");

            if (!Config.Misc.EnableVideoProfile && !Config.Misc.EnableVideoUpload && !Config.Misc.EnableYouTubeVideos)
                pnlUploadVideo.Visible = false;

            if (Config.Photos.EnableSalutePhoto) pnlSalutePhoto.Visible = true;

            pnlEditSkin.Visible = Config.Users.EnableProfileSkins && ((CurrentUserSession.Level != null &&
                                                      CurrentUserSession.Level.Restrictions.UserCanUseSkin)
                                                    || CurrentUserSession.CanUseSkin() == PermissionCheckResult.Yes)
                                   && ((CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanEditSkin)
                                   || CurrentUserSession.CanEditSkin() == PermissionCheckResult.Yes) && !String.IsNullOrEmpty(CurrentUserSession.ProfileSkin);
        }

        private void HideControls()
        {
            EditPhotos1.Visible = false;
            EditProfile1.Visible = false;
            Billing1.Visible = false;
            UploadVideo1.Visible = false;
            UploadAudio1.Visible = false;
            PrivacySettings1.Visible = false;
            Settings1.Visible = false;
            ViewPhotos1.Visible = false;
            ViewEvents1.Visible = false;
            ViewProfile1.Visible = false;
            EditSkin1.Visible = false;
        }

        protected void lnkEditProfile_Click(object sender, EventArgs e)
        {
            HideControls();
            EditProfile1.Visible = true;
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            HideControls();
            ViewProfile1.Visible = true;
        }

        protected void lnkUploadPhotos_Click(object sender, EventArgs e)
        {
            HideControls();
            EditPhotos1.Visible = true;
        }

        protected void lnkViewPhotos_Click(object sender, EventArgs e)
        {
            HideControls();
            ViewPhotos1.Visible = true;
        }

        protected void lnkSettings_Click(object sender, EventArgs e)
        {
            HideControls();
            Settings1.Visible = true;
        }

        protected void lnkSubscription_Click(object sender, EventArgs e)
        {
            HideControls();
            Billing1.Visible = true;
        }

        protected void lnkUploadVideo_Click(object sender, EventArgs e)
        {
            HideControls();
            UploadVideo1.Visible = true;
            UploadVideo1.FirstLoad = true;
            UploadVideo1.InitControl();
        }


        protected void lnkUploadAudio_Click(object sender, EventArgs e)
        {
            HideControls();
            UploadAudio1.Visible = true;
        }

        protected void lnkEditSkin_Click(object sender, EventArgs e)
        {
            HideControls();
            EditSkin1.Visible = true;
        }

        protected void lnkPrivacySettings_Click(object sender, EventArgs e)
        {
            HideControls();
            PrivacySettings1.Visible = true;
        }

        protected void lnkViewEvents_Click(object sender, EventArgs e)
        {
            HideControls();
            ViewEvents1.Visible = true;
        }

        protected void lnkUploadSalutePhoto_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditSalutePhoto.aspx");
        }
    }
}