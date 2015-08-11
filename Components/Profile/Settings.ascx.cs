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
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using oAuth;

namespace AspNetDating.Components.Profile
{
    public partial class SettingsCtrl : UserControl
    {
        protected LargeBoxStart LargeBoxStart;
        protected LargeBoxStart LargeBoxStart1;
        protected HeaderLine hlPersonalSettings;
        protected DatePicker datePicker1, datePicker2;
        protected HeaderLine hlAdditionalSettings;

        private string selectedCountry = null;
        private string selectedState = null;
        private string selectedCity = null;

        protected bool IsbtnDeleteClicked
        {
            get
            {
                if (ViewState["IsbtnDeleteClicked"] == null) return false;
                return (bool) ViewState["IsbtnDeleteClicked"];
            }
            set { ViewState["IsbtnDeleteClicked"] = value; }
        }

        public event EventHandler SettingsSaved;

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (ViewState["Settings_ControlsPopulated"] == null)
            {
                ViewState["Settings_ControlsPopulated"] = true;
                PopulateControls();
            }

            CascadingDropDown.SetupLocationControls(this.Page, dropCountry, dropRegion, dropCity, false,
                selectedCountry, selectedState, selectedCity);
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
            pnlLocation.Visible = Config.Users.LocationPanelVisible;

            LargeBoxStart.Title = Lang.Trans("Settings");
            hlPersonalSettings.Title = Lang.Trans("Personal Settings");
            hlAdditionalSettings.Title = Lang.Trans("Additional Settings");
            cbReceiveEmails.Text = Lang.Trans("I'd like to receive email notifications");
            cbProfileVisible.Text = Lang.Trans("Make my profile visible to other members");
            cbPhotoRequired.Text = Lang.Trans("Only members with photo can send messages to me");
            cbDisableProfileRating.Text = Lang.Trans("Disable profile rating");
            cbDisableVoting.Text = Lang.Trans("Disable voting");
            cbDisableProfileComments.Text = Lang.Trans("Disable profile comments");
            cbDisablePhotoComments.Text = Lang.Trans("Disable photo comments");
            cbDisablePhotoRating.Text = Lang.Trans("Disable photo rating");
            cbDisableBlogComments.Text = Lang.Trans("Disable blog comments");

            if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
            {
                trInterestedIn.Visible = true;

                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Male"), ((int) User.eGender.Male).ToString()));
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Female"), ((int) User.eGender.Female).ToString()));

                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Couple"), ((int) User.eGender.Couple).ToString()));
                }
            }
            else
                trInterestedIn.Visible = false;

            if (Config.Users.DisableAgeInformation)
            {
                pnlBirthdate.Visible = false;
            }

            if (!Config.Users.DisableGenderInformation)
            {
                ddMessagesFrom.Items.Add(new ListItem(Lang.Trans("Men"),
                                                      ((int) IncomingMessagesRestrictions.eMessagesFrom.Men).ToString()));
                ddMessagesFrom.Items.Add(new ListItem(Lang.Trans("Women"),
                                                      ((int) IncomingMessagesRestrictions.eMessagesFrom.Women).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    ddMessagesFrom.Items.Add(
                        new ListItem(Lang.Trans("Couples"),
                                     ((int) IncomingMessagesRestrictions.eMessagesFrom.Couples).ToString()));
                }
            }
            
            ddMessagesFrom.Items.Add(new ListItem(Lang.Trans("Favorites"), ((int)IncomingMessagesRestrictions.eMessagesFrom.Favorites).ToString()));
            ddMessagesFrom.Items.Add(new ListItem(Lang.Trans("All"), ((int)IncomingMessagesRestrictions.eMessagesFrom.All).ToString()));

            btnSaveChanges.Text = Lang.Trans(" Save Changes ");

            LargeBoxStart1.Title = Lang.Trans("Delete Your Account");
            btnDelete.Text = Lang.Trans("Delete Account");

            if (!Config.Ratings.EnableProfileRatings)
            {
                pnlDisableProfileRating.Visible = false;
            }

            if (!Config.Ratings.EnableProfileVoting)
            {
                pnlDisableVoting.Visible = false;
            }

            if (!Config.Users.EnableProfileComments)
            {
                pnlDisableProfileComments.Visible = false;
            }

            if (!Config.Photos.EnablePhotoComments)
            {
                pnlDisablePhotoComments.Visible = false;
            }

            if (!Config.Ratings.EnablePhotoRatings && !Config.Ratings.EnableRatePhotos)
            {
                pnlDisablePhotoRating.Visible = false;
            }

            if (Config.Users.DisableAgeInformation)
            {
                pnlAge.Visible = false;
            }

            if (!Config.Misc.EnableBlogs)
            {
                pnlDisableBlogComments.Visible = false;
            }
            else if (CurrentUserSession == null)
            {
                pnlDisableBlogComments.Visible = false;
            }
            else if (CurrentUserSession != null)
            {
                var permissionCheckResult = CurrentUserSession.CanCreateBlogs();
                if (CurrentUserSession.Level != null)
                {
                    if (!CurrentUserSession.Level.Restrictions.CanCreateBlogs && 
                        (permissionCheckResult == PermissionCheckResult.No ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded))
                    {
                        pnlDisableBlogComments.Visible = false;
                    }
                }
                else if ((permissionCheckResult == PermissionCheckResult.No ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded))
                {
                    pnlDisableBlogComments.Visible = false;
                }
            }

            if (Config.Users.EnableProfileSkins && ((CurrentUserSession.Level != null && 
                                                      CurrentUserSession.Level.Restrictions.UserCanUseSkin)
                                                    || CurrentUserSession.CanUseSkin() == PermissionCheckResult.Yes))
            {
                pnlProfileSkin.Visible = true;
                dropProfileSkin.Items.Add(new ListItem("", ""));
                dropProfileSkin.Items.Add(new ListItem("Default skin".Translate(), "-"));
                foreach (var skinFile in ProfileSkin.AvailableSkins)
                {
                    string skinName = skinFile.Remove(skinFile.LastIndexOf('/'));
                    skinName = skinName.Substring(skinName.LastIndexOf('/') + 1);
                    dropProfileSkin.Items.Add(new ListItem(skinName, skinFile));
                }
                if (User.ProfileSkin != null 
                    && dropProfileSkin.Items.FindByValue(user.ProfileSkin) != null)
                    dropProfileSkin.SelectedValue = User.ProfileSkin;
            }

            if (Config.Users.EnableRelationshipStatus)
            {
                pnlRelationshipStatus.Visible = true;

                ddRelationshipStatus.Items.Add(new ListItem("Single".Translate(),
                                                            ((int) Classes.Relationship.eRelationshipStatus.Single).ToString()));
                ddRelationshipStatus.Items.Add(new ListItem("In a relationship".Translate(),
                                                            ((int) Classes.Relationship.eRelationshipStatus.InRelationship).
                                                                ToString()));
                ddRelationshipStatus.Items.Add(new ListItem("Engaged".Translate(),
                                                            ((int) Classes.Relationship.eRelationshipStatus.Engaged).ToString()));
                ddRelationshipStatus.Items.Add(new ListItem("Married".Translate(),
                                                            ((int) Classes.Relationship.eRelationshipStatus.Married).ToString()));
                ddRelationshipStatus.Items.Add(new ListItem("It's complicated".Translate(),
                                                            ((int) Classes.Relationship.eRelationshipStatus.ItIsComplicated).
                                                                ToString()));
                ddRelationshipStatus.Items.Add(new ListItem("In an open relationship".Translate(),
                                                            ((int) Classes.Relationship.eRelationshipStatus.InAnOpenRelationship)
                                                                .ToString()));

            }

            if (Config.Misc.EnableTwitterIntegration)
            {
                LargeBoxStart3.Title = "Twitter Integration".Translate();
                //btnSaveTwitterSettings.Text = "Save Twitter Credentials >>".Translate();
                //btnRemoveTwitterCredentials.Text = "Remove Twitter Credentials".Translate();
                btnLinkToYourTwitterAccount.Text = "Link to your Twitter account".Translate();
                btnUnlinkFromYourTwitterAccount.Text = "Unlink from your Twitter account".Translate();

                if (Twitter.HasCredentials(CurrentUserSession.Username))
                {
                    divEnterTwitterCredentials.Visible = false;
                    divTwitterStatus.Visible = true;
                    lblTwitterStatus.Text =
                        "Your Twitter credentials are saved and status updates are automatically posted".Translate();
                }
                else
                {
                    divEnterTwitterCredentials.Visible = true;
                    divTwitterStatus.Visible = false;
                    lblTwitterStatus.Text =
                        Lang.Trans("Link to your Twitter account if you would like to synchronize your status");
                }
            }
            else
            {
                divTwitter.Visible = false;
            }
        }

        private void PopulateControls()
        {
            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry != null && User.Country != String.Empty)
                {
                    selectedCountry = User.Country;
                }
                if (dropRegion != null && User.State != String.Empty)
                {
                    selectedState = User.State;
                }
                if (dropCity != null && User.City != String.Empty)
                {
                    selectedCity = User.City;
                }
                if (txtZipCode != null)
                {
                    txtZipCode.Text = User.ZipCode;
                }
            }
            txtName.Text = User.Name;
            datePicker1.SelectedDate = User.Birthdate;
            if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple && !Config.Users.DisableAgeInformation)
            {
                datePicker2.SelectedDate = User.Birthdate2;
                trBirthdate2.Visible = true;
            }
            else
            {
                trBirthdate2.Visible = false;
            }
            if (Config.Users.InterestedInFieldEnabled)
            {
                dropInterestedIn.SelectedValue = ((int) User.InterestedIn).ToString();
            }
            txtEmail.Text = User.Email;
            cbReceiveEmails.Checked = User.ReceiveEmails;
            cbProfileVisible.Checked = User.ProfileVisible;

            ddMessagesFrom.SelectedValue = ((int) User.IncomingMessagesRestrictions.MessagesFrom).ToString();

            cbPhotoRequired.Checked = User.IncomingMessagesRestrictions.PhotoRequired;
            txtAgeFrom.Text = User.IncomingMessagesRestrictions.AgeFrom.ToString();
            txtAgeTo.Text = User.IncomingMessagesRestrictions.AgeTo.ToString();


            if (Config.Users.EnableRelationshipStatus)
            {
                string[] friends = User.FetchMutuallyFriends(CurrentUserSession.Username);

                ddInRelationshipWith.Items.Add(new ListItem(String.Empty));
                foreach (string friend in friends)
                {
                    ddInRelationshipWith.Items.Add(new ListItem(friend));
                }

                Relationship relationship = Relationship.Fetch(CurrentUserSession.Username, null);
                ddRelationshipStatus.SelectedValue = relationship != null
                                                         ? ((int) relationship.Type).ToString()
                                                         : ((int) Relationship.eRelationshipStatus.Single).ToString();
                if (relationship != null)
                {
                    ddInRelationshipWith.Visible = true;
                    if (ddInRelationshipWith.Items.Contains(new ListItem(relationship.ToUsername ?? String.Empty)))
                        ddInRelationshipWith.SelectedValue = relationship.ToUsername ?? String.Empty;
                }
            }

            #region load user options

            User user = User;

            cbDisableBlogComments.Checked = user.IsOptionEnabled(eUserOptions.DisableBlogComments);
            cbDisablePhotoRating.Checked = user.IsOptionEnabled(eUserOptions.DisablePhotoRating);
            cbDisableProfileComments.Checked = user.IsOptionEnabled(eUserOptions.DisableProfileComments);
            cbDisablePhotoComments.Checked = user.IsOptionEnabled(eUserOptions.DisablePhotoComments);
            cbDisableProfileRating.Checked = user.IsOptionEnabled(eUserOptions.DisableProfileRating);
            cbDisableVoting.Checked = user.IsOptionEnabled(eUserOptions.DisableProfileVoting);
            #endregion

        }

        private bool ValidateData()
        {
            lblError.CssClass = "alert text-danger";

            try
            {
                if (txtEmail.Text.Length == 0)
                {
                    lblError.Text =
                        Lang.Trans("Please specify e-mail address!");
                    return false;
                }

                if (Config.Users.CheckForDuplicateEmails && User.Email != txtEmail.Text && User.IsEmailUsedByAnotherMember(txtEmail.Text.ToLower()))
                {
                    lblError.Text =
                        Lang.Trans("E-mail address is already in use by another user!");
                    return false;
                }
            }
            catch (ArgumentException err) // Invalid e-mail address
            {
                lblError.Text = err.Message;
                return false;
            }

            // Validate passwords
            bool AllBlank = (txtCurrentPassword.Text.Trim() == "" &&
                             txtNewPassword.Text.Trim() == "" &&
                             txtConfirmNewPassword.Text.Trim() == "");

            bool AllFilledIn = (txtCurrentPassword.Text.Trim() != "" &&
                                txtNewPassword.Text.Trim() != "" &&
                                txtConfirmNewPassword.Text.Trim() != "");

            if (!(AllBlank || AllFilledIn))
            {
                lblError.Text = Lang.Trans("Please fill in all password fields or leave them blank!");
                return false;
            }

            if (AllFilledIn && txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                lblError.Text = Lang.Trans("New password fields do not match!");
                return false;
            }

            if (AllFilledIn && !User.IsPasswordIdentical(txtCurrentPassword.Text))
            {
                lblError.Text = Lang.Trans("The specified current password is wrong!");
                return false;
            }

            // Validate name
            if (txtName.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter your name!");
                return false;
            }

            // Validate birthdate
		    if (!datePicker1.ValidDateEntered)
			{
				lblError.Text = Lang.Trans("Please select your birthdate!");
				return false;
			}
			if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple
				&& !datePicker2.ValidDateEntered)
			{
				lblError.Text = Lang.Trans("Please select your birthdate!");
				return false;
			}

            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry != null && String.IsNullOrEmpty(dropCountry.SelectedValue()))
                {
                    lblError.Text = Lang.Trans("Please select your country!");
                    return false;
                }

                if (dropRegion != null && String.IsNullOrEmpty(dropRegion.SelectedValue()))
                {
                    lblError.Text = Lang.Trans("Please select your state!");
                    return false;
                }

                if (txtZipCode != null && txtZipCode.Text == String.Empty)
                {
                    lblError.Text = Lang.Trans("Please enter your Zip/Postal Code");
                    return false;
                }

                if (dropCity != null && String.IsNullOrEmpty(dropCity.SelectedValue()))
                {
                    lblError.Text = Lang.Trans("Please select your city!");
                    return false;
                }
            }

            return true;
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
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnSaveChanges.Click += new EventHandler(this.btnSaveChanges_Click);
            //this.btnSaveTwitterSettings.Click += new EventHandler(btnSaveTwitterSettings_Click);
            //this.btnRemoveTwitterCredentials.Click += new EventHandler(btnRemoveTwitterCredentials_Click);
        }

        #endregion

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                try
                {
                    if (Config.Users.LocationPanelVisible)
                    {
                        if (dropCountry != null)
                        {
                            User.Country = dropCountry.SelectedValue();
                        }
                        if (dropRegion != null)
                        {
                            User.State = dropRegion.SelectedValue();
                        }
                        if (txtZipCode != null)
                        {
                            User.ZipCode = txtZipCode.Text;
                        }
                        if (dropCity != null)
                        {
                            User.City = dropCity.SelectedValue();
                        }

                        Location loc = Config.Users.GetLocation(User.Country, User.State, User.City);

                        if (loc != null)
                        {
                            User.Longitude = loc.Longitude;
                            User.Latitude = loc.Latitude;
                        }
                        else
                        {
                            User.Longitude = null;
                            User.Latitude = null;
                        }
                    }

                    User.Name = Config.Misc.EnableBadWordsFilterProfile ? Parsers.ProcessBadWords(txtName.Text) : txtName.Text;
                    User.Birthdate = datePicker1.SelectedDate;

                    if (!Config.Users.DisableGenderInformation)
                    {
                        if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple)
                        {
                            User.Birthdate2 = datePicker2.SelectedDate;
                        }

                        if (Config.Users.InterestedInFieldEnabled)
                        {
                            User.InterestedIn = (User.eGender)Convert.ToInt32(dropInterestedIn.SelectedValue);
                        }
                    }

                    if (Config.Users.EnableRelationshipStatus)
                    {
                        if (ddRelationshipStatus.SelectedIndex > 0) // other than "Single" is selected
                        {
                            string toUsername = ddInRelationshipWith.SelectedValue.Length == 0
                                                    ? null
                                                    : ddInRelationshipWith.SelectedValue;
                            Relationship.eRelationshipStatus selectedStatus =
                                (Relationship.eRelationshipStatus) Convert.ToInt32(ddRelationshipStatus.SelectedValue);
                            Relationship relationship = Relationship.Fetch(CurrentUserSession.Username, null);

                            if (relationship == null)
                            {
                                relationship = new Relationship(CurrentUserSession.Username, toUsername);
                                relationship.Type = selectedStatus;
                            }
                            else if (relationship.ToUsername != toUsername)
                            {
                                if (relationship.Accepted && relationship.ToUsername != null)
                                {
                                    AddRemovedFriendRelationshipEvent(relationship.FromUsername, relationship.ToUsername, relationship.Type);
                                    AddRemovedFriendRelationshipEvent(relationship.ToUsername, relationship.FromUsername, relationship.Type);
                                }

                                Relationship.Delete(relationship.FromUsername, relationship.ToUsername, null);
                                Relationship.Delete(relationship.ToUsername, relationship.FromUsername, null);

                                relationship = new Relationship(CurrentUserSession.Username, toUsername);
                                relationship.Type = selectedStatus;
                            }
                            else if (relationship.Type != selectedStatus)
                            {
                                if (relationship.Accepted)
                                    relationship.PendingType = selectedStatus;
                                else relationship.Type = selectedStatus;
                            }

                            if (toUsername == null) relationship.Accepted = true;
                            relationship.Save();
                        }
                        else
                        {
                            Relationship relationship = Relationship.Fetch(CurrentUserSession.Username, null);
                            if (relationship != null && relationship.Accepted && relationship.ToUsername != null)
                            {
                                AddRemovedFriendRelationshipEvent(CurrentUserSession.Username, relationship.ToUsername, relationship.Type);
                                AddRemovedFriendRelationshipEvent(relationship.ToUsername, CurrentUserSession.Username, relationship.Type);
                            }
                            Relationship.Delete(CurrentUserSession.Username, null, null);
                            Relationship.Delete(null, CurrentUserSession.Username, null);
                        }
                    }

                    User.Email = txtEmail.Text.Trim();

                    if (txtNewPassword.Text != "")
                    {
                        User.Password = txtNewPassword.Text;
                    }

                    User.ReceiveEmails = cbReceiveEmails.Checked;
                    User.ProfileVisible = cbProfileVisible.Checked;

                    int ageFrom;
                    int ageTo;
                    try
                    {
                        ageFrom = Convert.ToInt32(txtAgeFrom.Text);
                        ageTo = Convert.ToInt32(txtAgeTo.Text);
                    }
                    catch
                    {
                        lblError.Text = Lang.Trans("Please enter valid age!");
                        return;
                    }

                    if (ageFrom > ageTo)
                    {
                        lblError.Text = Lang.Trans("Please enter valid age!");
                        return;
                    }

                    var incommingMessagesRestrictions = new IncomingMessagesRestrictions
                                                            {
                                                                AgeFrom = ageFrom,
                                                                AgeTo = ageTo,
                                                                MessagesFrom =
                                                                    ((IncomingMessagesRestrictions.eMessagesFrom)
                                                                     (Convert.ToInt32(ddMessagesFrom.SelectedValue))),
                                                                PhotoRequired = cbPhotoRequired.Checked
                                                            };

                    User.IncomingMessagesRestrictions = incommingMessagesRestrictions;

                    #region set user options and privacy settings

                    ulong options = ((PageBase) Page).CurrentUserSession.Options;

                    options &= ~(ulong)eUserOptions.DisableBlogComments;
                    options &= ~(ulong)eUserOptions.DisablePhotoRating;
                    options &= ~(ulong)eUserOptions.DisableProfileComments;
                    options &= ~(ulong)eUserOptions.DisablePhotoComments;
                    options &= ~(ulong)eUserOptions.DisableProfileRating;
                    options &= ~(ulong)eUserOptions.DisableProfileVoting;

                    if (cbDisableBlogComments.Checked) options |= (ulong) eUserOptions.DisableBlogComments;
                    if (cbDisablePhotoRating.Checked) options |= (ulong) eUserOptions.DisablePhotoRating;
                    if (cbDisableProfileComments.Checked) options |= (ulong) eUserOptions.DisableProfileComments;
                    if (cbDisablePhotoComments.Checked) options |= (ulong)eUserOptions.DisablePhotoComments;
                    if (cbDisableProfileRating.Checked) options |= (ulong) eUserOptions.DisableProfileRating;
                    if (cbDisableVoting.Checked) options |= (ulong)eUserOptions.DisableProfileVoting;

                    User.Options = options;

                    #endregion

                    if (dropProfileSkin.SelectedIndex > 0)
                        User.ProfileSkin = dropProfileSkin.SelectedIndex == 1
                                           ? null
                                           : dropProfileSkin.SelectedValue;

                    User.Update();
                    CurrentUserSession.Country = User.Country;
                    CurrentUserSession.State = User.State;
                    CurrentUserSession.City = User.City;
                    CurrentUserSession.Longitude = User.Longitude;
                    CurrentUserSession.Latitude = User.Latitude;
                    CurrentUserSession.Options = options;
                    CurrentUserSession.IncomingMessagesRestrictions = User.IncomingMessagesRestrictions;
                    CurrentUserSession.ProfileSkin = User.ProfileSkin;

                    lblError.CssClass = "alert text-info";
                    lblError.Text = Lang.Trans("Your account has been successfully updated!");

                    if (SettingsSaved != null)
                    {
                        SettingsSaved(this, new EventArgs());
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    return;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!IsbtnDeleteClicked)
            {
                IsbtnDeleteClicked = true;
                pnlConfirmPasswordForDeletion.Visible = true;
                btnDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.Trans(
                                                           "Do you really want to delete your account?Remember,this action is IRREVERSIBLE!")));
                return;
            }

            if (CurrentUserSession.IsPasswordIdentical(txtConfirmPasswordForDeletion.Text.Trim()))
            {
                string username = CurrentUserSession.Username;
                Subscription subscription = Subscription.FetchActiveSubscription(username);
                if (subscription != null && !subscription.CancellationRequested)
                {
                    lblErrorConfirmPasswordForDeletion.Text = Lang.Trans("You have to cancel your subscription first");
                    return;
                }

                User.Delete(username, String.Format(Lang.Trans("The user deleted their account on {0}"),
                    DateTime.Now.ToShortDateString()));

                try
                {
                    IPLogger.Log(username, Request.UserHostAddress, IPLogger.ActionType.DeleteProfile);
                }
                catch (Exception err)
                {
                    Global.Logger.LogError("SettingsPage IP Logger", err);
                }

                Session.Abandon();
                Response.Redirect("Default.aspx");
            }
            else
            {
                lblErrorConfirmPasswordForDeletion.Text = "The provided password is invalid!".Translate();
            }
        }

        //void btnSaveTwitterSettings_Click(object sender, EventArgs e)
        //{
        //    Twitter.SaveCredentials(CurrentUserSession.Username, txtTwitterUsername.Text, 
        //        txtTwitterPassword.Text);

        //    lblError.CssClass = "message";
        //    lblError.Text = Lang.Trans("Your Twitter credentials have been saved!");

        //    divEnterTwitterCredentials.Visible = false;
        //    divTwitterStatus.Visible = true;
        //}

        //void btnRemoveTwitterCredentials_Click(object sender, EventArgs e)
        //{
        //    Twitter.RemoveCredentials(CurrentUserSession.Username);

        //    lblError.CssClass = "message";
        //    lblError.Text = Lang.Trans("Your Twitter credentials have been removed!");

        //    divEnterTwitterCredentials.Visible = true;
        //    divTwitterStatus.Visible = false;
        //}

        protected void dropProfileSkin_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropProfileSkin.SelectedIndex > 1)
            {
                lnkPreviewSkin.HRef = String.Format("{0}/ShowUser.aspx?uid={1}&skin={2}",
                    Config.Urls.Home, User.Username, Server.UrlEncode(dropProfileSkin.SelectedValue));
                lnkPreviewSkin.Visible = true;
            }
            else
            {
                lnkPreviewSkin.Visible = false;
            }
            updatePanelProfileSkin.Update();
        }

        protected void ddRelationshipStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddInRelationshipWith.Visible = ddRelationshipStatus.SelectedIndex > 0;
        }

        private void AddRemovedFriendRelationshipEvent(string fromUsername, string toUsername, Relationship.eRelationshipStatus type)
        {
            Event newEvent = new Event(fromUsername);

            newEvent.Type = Event.eType.RemovedFriendRelationship;
            RemovedFriendRelationship removedFriendRelationship = new RemovedFriendRelationship();
            removedFriendRelationship.Username = toUsername;
            removedFriendRelationship.Type = type;
            newEvent.DetailsXML = Misc.ToXml(removedFriendRelationship);

            newEvent.Save();

            if (Config.Users.NewEventNotification)
            {
                string[] usernames = User.FetchMutuallyFriends(fromUsername);

                string text = String.Format("{0} {1} are no longer in relationship ({2})".Translate(),
                                            "<b>" + fromUsername + "</b>", toUsername,
                                            Relationship.GetRelationshipStatusString(type));
                int imageID = 0;
                try
                {
                    imageID = Photo.GetPrimary(fromUsername).Id;
                }
                catch (NotFoundException)
                {
                    User user = null;
                    try
                    {
                        user = User.Load(fromUsername);
                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    catch (NotFoundException) { return; }
                }
                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);

                foreach (string friendUsername in usernames)
                {
                    if (toUsername == friendUsername) continue;

                    User.SendOnlineEventNotification(fromUsername, friendUsername, text, thumbnailUrl,
                                                         UrlRewrite.CreateShowUserUrl(toUsername));
                }
            }
        }

        protected void btnLinkToYourTwitterAccount_Click(object sender, EventArgs e)
        {
            oAuthTwitter oAuth = new oAuthTwitter();

            oAuth.CallBackUrl = Config.Urls.Home.Trim('/') + "/TwitterCallback.ashx";
            Response.Redirect(oAuth.AuthorizationLinkGet());
        }

        protected void btnUnlinkFromYourTwitterAccount_Click(object sender, EventArgs e)
        {
            Twitter.RemoveCredentials(CurrentUserSession.Username);

            lblError.CssClass = "alert text-info";
            lblError.Text = Lang.Trans("Your Twitter credentials have been removed!");

            divEnterTwitterCredentials.Visible = true;
            divTwitterStatus.Visible = false;
        }
    }
}