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
using System.Globalization;
using System.IdentityModel.Claims;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;
using Microsoft.IdentityModel.TokenProcessor;
using Newtonsoft.Json.Linq;
using Photo = AspNetDating.Classes.Photo;
using System.Linq;
using System.Web.UI;

namespace AspNetDating
{
    public partial class Register : PageBase
    {
        protected DatePicker datePicker1, datePicker2;
        protected LargeBoxStart LargeBoxStart1;

        private string selectedCountry = null;
        private string selectedState = null;
        private string selectedCity = null;


        private long? FacebookID
        {
            get
            {
                return ViewState["FacebookID"] as long?;
            }

            set
            {
                ViewState["FacebookID"] = value;
            }
        }

        private string PrimaryPhotoURL
        {
            get
            {
                return ViewState["PrimaryPhotoURL"] as string;
            }

            set
            {
                ViewState["PrimaryPhotoURL"] = value;
            }
        }

        private long[] FacebookFriendIDs
        {
            get
            {
                return ViewState["FacebookFriendIDs"] as long[];
            }

            set
            {
                ViewState["FacebookFriendIDs"] = value;
            }
        }

        private string[] FriendIDs
        {
            get
            {
                return ViewState["FriendIDs"] as string[];
            }

            set
            {
                ViewState["FriendIDs"] = value;
            }
        }

        public Register()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterJQuery();

            if (CurrentUserSession != null
                && CurrentUserSession.IsAuthorized)
            {
                Response.Redirect("Home.aspx");
            }

            if (!Page.IsPostBack)
            {
                PrepareStrings();
                if (Request.Params["token"] == "1")
                    LoadTokenData();


                if (Config.Misc.EnableFacebookIntegration &&
                    !String.IsNullOrEmpty(Context.Request.QueryString["facebook"]))
                {
                    if (!String.IsNullOrEmpty(Context.Request.QueryString["login"]))
                        divLogin.Visible = true;

                    PopulateUserDataUsingFacebook();
                }
            }

            if (Config.Users.DisableAgeInformation || dropGender.SelectedValue != ((int)Classes.User.eGender.Couple).ToString())
                trBirthday2.Style.Add("display", "none");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            CascadingDropDown.SetupLocationControls(this, dropCountry, dropRegion, dropCity, !IsPostBack,
                selectedCountry, selectedState, selectedCity);
        }

        private void LoadTokenData()
        {
            Token token = Session["Token"] as Token;
            if (token == null) return;

            if (Classes.User.GetUsernameByTokenUniqueId(token.UniqueID) != null)
            {
                lblError.Text = Lang.Trans("The presented information card is already used for another account!");
                return;
            }

            selectedCountry = token.Claims[ClaimTypes.Country];
            selectedState = token.Claims[ClaimTypes.StateOrProvince];
            selectedCity = token.Claims[ClaimTypes.Locality];

            txtZipCode.Text = token.Claims[ClaimTypes.PostalCode];
            txtName.Text = token.Claims[ClaimTypes.GivenName] + " " + token.Claims[ClaimTypes.Surname];
            dropGender.SelectedValue = ((int)Enum.Parse(typeof(User.eGender), token.Claims[ClaimTypes.Gender])).ToString();
            datePicker1.SelectedDate = DateTime.Parse(token.Claims[ClaimTypes.DateOfBirth]);
            txtEmail.Text = token.Claims[ClaimTypes.Email];
            ViewState["TokenUniqueId"] = token.UniqueID;
        }

        private void PrepareStrings()
        {
            WideBoxStart1.Title = Lang.Trans("Registration details");

            if (Config.Users.LocationPanelVisible)
                ShowLocation();
            else
                HideLocation();

            lblUsername.Text = Lang.Trans("Username");
            lblEmail.Text = Lang.Trans("E-Mail");
            lblPassword.Text = Lang.Trans("Password");
            lblPassword2.Text = Lang.Trans("Confirm password");
            lblName.Text = Lang.Trans("Name");
            lblGender.Text = Lang.Trans("Gender");
            lblBirthdate.Text = Lang.Trans("Birthdate");
            lblInterestedIn.Text = Lang.Trans("Interested in");
            lblInvitationCode.Text = Lang.Trans("Invitation Code");
            btnLogin.Text = "Login";

            dropGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            if (Config.Users.DisableGenderInformation)
                dropGender.SelectedValue = ((int)Classes.User.eGender.Male).ToString();

            if (Config.Users.InterestedInFieldEnabled)
            {
                trInterestedIn.Visible = !Config.Users.DisableGenderInformation;

                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));

                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }

                if (Config.Users.DisableGenderInformation)
                    dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
            }
            else
                trInterestedIn.Visible = false;

            if (Config.Users.InvitationCode == String.Empty)
                trInvitationCode.Visible = false;

            if (Config.Misc.EnableCaptcha)
            {
                lblCaptcha.Text = Lang.Trans("Enter the code shown above");
                trCaptcha.Visible = true;
            }

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlBirthdate.Visible = !Config.Users.DisableAgeInformation;

            if (Config.Users.DisableAgeInformation)
                datePicker1.SelectedDate = new DateTime(DateTime.Now.Year - Config.Users.MinAge, 1, 1);

            divFacebook.Visible = Config.Misc.EnableFacebookIntegration;

            btnRegister.Text = Lang.Trans("Register");
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            #region Validate username

            try
            {
                if (txtUsername.Text.Length == 0)
                {
                    lblError.Text =
                        Lang.Trans("Please specify username!");
                    return;
                }

                if (Classes.User.IsUsernameTaken(txtUsername.Text))
                {
                    lblError.Text =
                        Lang.Trans("Username is already taken!");
                    return;
                }

                foreach (string reservedUsername in Config.Users.ReservedUsernames)
                {
                    if (reservedUsername == txtUsername.Text.ToLower())
                    {
                        lblError.Text = Lang.Trans("Username is reserved!");
                        return;
                    }
                }
            }
            catch (ArgumentException err) // Invalid username
            {
                lblError.Text = err.Message;
                return;
            }

            #endregion

            #region Validate e-mail address

            try
            {
                if (txtEmail.Text.Length == 0)
                {
                    lblError.Text =
                        Lang.Trans("Please specify e-mail address!");
                    return;
                }

                if (txtEmail.Text.ToLower().EndsWith("@mail.bg"))
                {
                    lblError.Text =
                        Lang.Trans("E-mails from mail.bg are not accepted!");
                    return;
                }

                if (Config.Users.CheckForDuplicateEmails && Classes.User.IsEmailUsed(txtEmail.Text))
                {
                    lblError.Text =
                        Lang.Trans("E-mail address is already in use!");
                    return;
                }
            }
            catch (ArgumentException err) // Invalid e-mail address
            {
                lblError.Text = err.Message;
                return;
            }

            #endregion

            #region Validate passwords

            if (txtPassword.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please specify password!");
                return;
            }
            if (txtPassword2.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please verify password!");
                return;
            }
            if (txtPassword.Text != txtPassword2.Text)
            {
                lblError.Text = Lang.Trans("Passwords do not match!");
                return;
            }

            #endregion

            #region Validate name

            if (txtName.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter your name!");
                return;
            }

            #endregion

            #region Validate gender

            if (dropGender.SelectedIndex == 0)
            {
                lblError.Text = Lang.Trans("Please select your gender!");
                return;
            }

            #endregion

            #region Validate InterestedIn

            if (Config.Users.InterestedInFieldEnabled)
            {
                if (dropInterestedIn.SelectedIndex == 0)
                {
                    lblError.Text = Lang.Trans("Please select who are you interested in!");
                    return;
                }
            }

            #endregion

            #region Validate birthdate1

            if (!datePicker1.ValidDateEntered)
            {
                lblError.Text = Lang.Trans("Please select your birthdate!");
                return;
            }

            #endregion

            #region Validate birthdate2

            if ((User.eGender)Convert.ToInt32(dropGender.SelectedValue) == Classes.User.eGender.Couple
                && !datePicker2.ValidDateEntered)
            {
                lblError.Text = Lang.Trans("Please select your birthdate!");
                return;
            }

            #endregion

            #region Validate agreement

            if (!cbAgreement.Checked)
            {
                lblError.Text = Lang.Trans("You must accept the agreement to proceed!");
                return;
            }

            #endregion

            #region Validate location

            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry != null && String.IsNullOrEmpty(dropCountry.SelectedValue()))
                {
                    lblError.Text = Lang.Trans("Please select your country!");
                    return;
                }

                if (dropRegion.Items.Count > 1 && String.IsNullOrEmpty(dropRegion.SelectedValue()))
                {
                    lblError.Text = Lang.Trans("Please select your state!");
                    return;
                }

                if (txtZipCode != null && txtZipCode.Text == String.Empty)
                {
                    lblError.Text = Lang.Trans("Please enter your Zip/Postal Code");
                    return;
                }

                if (dropCity != null && String.IsNullOrEmpty(dropCity.SelectedValue()))
                {
                    lblError.Text = Lang.Trans("Please select your city!");
                    return;
                }
            }

            #endregion

            #region Validate Invitation Code

            if (Config.Users.InvitationCode != String.Empty)
            {
                if (Config.Users.InvitationCode != txtInvitationCode.Text)
                {
                    lblError.Text = Lang.Trans("Invalid Invitation Code!");
                    return;
                }
            }

            #endregion

            #region Validate captcha

            if (Config.Misc.EnableCaptcha &&
                (Session["Captcha_RandomCode"] == null || (string)Session["Captcha_RandomCode"] != txtCaptcha.Text))
            {
                lblError.Text = Lang.Trans("Invalid verification code!");
                return;
            }

            #endregion

            #region Validate IP address

            if (Properties.Settings.Default.BannedCountries.Count > 0)
            {
                foreach (string countryCode in Properties.Settings.Default.BannedCountries)
                {
                    if (IPToCountry.GetCountry(Request.UserHostAddress) == countryCode.Trim())
                    {
                        lblError.Text = Lang.Trans("Registration is not allowed for your country!");
                        return;
                    }
                }
            }

            #endregion

            try
            {
                User newUser = new User(txtUsername.Text);

                #region Save location

                if (Config.Users.LocationPanelVisible)
                {
                    if (dropCountry != null)
                    {
                        newUser.Country = dropCountry.SelectedValue();
                    }
                    if (dropRegion != null)
                    {
                        newUser.State = dropRegion.SelectedValue();
                    }
                    if (txtZipCode != null)
                    {
                        newUser.ZipCode = txtZipCode.Text;
                    }
                    if (dropCity != null)
                    {
                        newUser.City = dropCity.SelectedValue();
                    }

                    Location loc = Config.Users.GetLocation(newUser.Country, newUser.State, newUser.City);

                    if (loc != null)
                    {
                        newUser.Longitude = loc.Longitude;
                        newUser.Latitude = loc.Latitude;
                    }
                }

                #endregion

                newUser.Password = txtPassword.Text;
                newUser.Email = txtEmail.Text;
                newUser.Name = txtName.Text;
                newUser.Gender = (User.eGender)Convert.ToInt32(dropGender.SelectedValue);
                newUser.Birthdate = datePicker1.SelectedDate;
                newUser.LanguageId = LanguageId;
                if (newUser.Gender == Classes.User.eGender.Couple)
                {
                    newUser.Birthdate2 = datePicker2.SelectedDate;
                }

                if (Config.Users.InterestedInFieldEnabled)
                {
                    newUser.InterestedIn = (User.eGender)Convert.ToInt32(dropInterestedIn.SelectedValue);
                }
                else
                {
                    if (Config.Users.DisableGenderInformation)
                        newUser.InterestedIn = Classes.User.eGender.Male;
                    else
                        newUser.InterestedIn = newUser.Gender == Classes.User.eGender.Male
                                                   ?
                                                       Classes.User.eGender.Female
                                                   : Classes.User.eGender.Male;
                }
                newUser.ReceiveEmails = Config.Users.EmailNotificationsDefault;
                if (ViewState["TokenUniqueId"] is string)
                    newUser.TokenUniqueId = (string)ViewState["TokenUniqueId"];

                #region Set and Delete invitedBy cookie

                if (Request.Cookies["invitedBy"] != null)
                {
                    newUser.InvitedBy = Server.HtmlEncode(Request.Cookies["invitedBy"].Value);

                    HttpCookie cookie = new HttpCookie("invitedBy");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                #endregion

                #region Set and Delete affiliateID cookie

                if (Request.Cookies["affiliateID"] != null)
                {
                    newUser.AffiliateID = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["affiliateID"].Value));

                    HttpCookie cookie = new HttpCookie("affiliateID");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                #endregion

               
                if (Config.Misc.EnableFacebookIntegration)
                    newUser.FacebookID = FacebookID;

                newUser.Create(Request.UserHostAddress);

                if (Config.Misc.EnableFacebookIntegration)
                {
                    StorePrimaryPhoto(newUser);
                    AddFriends(newUser);
                }

                if (Config.Users.SmsConfirmationRequired)
                {
                    Response.Redirect("SmsConfirm.aspx?username=" + newUser.Username);
                    return;
                }

                if (Config.Users.AutoActivateUsers)
                {
                    if (Config.Users.SendWelcomeMessage)
                    {
                        Message.SendWelcomeMessage(newUser);
                    }

                    UserSession userSession = null;
                    try
                    {
                        userSession = new UserSession(newUser.Username);
                        userSession.Authorize(Session.SessionID);

                        ((PageBase)Page).CurrentUserSession = userSession;

                        #region Autojoin to group

                        Group[] autojoinGroups = Group.Fetch(true);

                        if (autojoinGroups.Length > 0)
                        {
                            var groups =
                                autojoinGroups.Where(
                                    g => g.Approved &&
                                    (g.AutojoinCountry == null || g.AutojoinCountry == userSession.Country) &&
                                    (g.AutojoinRegion == null || g.AutojoinRegion == userSession.State) &&
                                    (g.AutojoinCity == null || g.AutojoinCity == userSession.City));
                            foreach (Group group in groups)
                            {
                                GroupMember groupMember = new GroupMember(group.ID, userSession.Username);
                                groupMember.Active = true;
                                groupMember.Type = GroupMember.eType.Member;
                                groupMember.Save();
                                group.ActiveMembers++;
                                group.Save();
                            }
                        }

                        #endregion

                        Response.Redirect("Profile.aspx");
                    }
                    catch (Exception err)
                    {
                        StatusPageMessage = err.Message;
                    }
                }
                else
                    StatusPageMessage = Lang.Trans
                        ("<b>Your account has been created successfully!</b><br><br>"
                         + "You will receive a confirmation e-mail shortly. In order "
                         + "to finish your registration you'll have to click the "
                         + "activation link in the e-mail.");
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (ArgumentException err)
            {
                lblError.Text = err.Message;
                return;
            }
            catch (Exception err)
            {
                lblError.Text = Lang.Trans
                    ("Unknown error has occured while trying to create "
                     + "your account! Please try again later.");
                Log(err);
                return;
            }
            Response.Redirect("ShowStatus.aspx");
        }

        private void HideLocation()
        {
            if (trCountry != null)
            {
                trCountry.Visible = false;
            }
            if (trState != null)
            {
                trState.Visible = false;
            }
            if (trZipCode != null)
            {
                trZipCode.Visible = false;
            }
            if (trCity != null)
            {
                trCity.Visible = false;
            }
        }

        private void ShowLocation()
        {
            if (trCountry != null)
            {
                trCountry.Visible = true;
            }
            if (trState != null)
            {
                trState.Visible = true;
            }
            if (trZipCode != null)
            {
                trZipCode.Visible = true;
            }
            if (trCity != null)
            {
                trCity.Visible = true;
            }
        }


        private void StorePrimaryPhoto(User user)
        {
            if (PrimaryPhotoURL != null)
            {
                var request = (HttpWebRequest)WebRequest.Create(PrimaryPhotoURL);
                request.Method = "GET";
                using (var stream = request.GetResponse().GetResponseStream())
                {
                    Photo photo = new Photo();
                    System.Drawing.Image image;
                    try
                    {
                        image = System.Drawing.Image.FromStream(stream);
                        photo.Image = image;

                        photo.Primary = true;
                        photo.Name = String.Empty;
                        photo.Description = String.Empty;
                        photo.User = user;
                        photo.Approved = Config.Photos.AutoApprovePhotos;
                        photo.Save(true);
                        photo.Image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                }
            }
        }

        private void AddFriends(User user)
        {
            List<string> usernames = new List<string>();


            if (FacebookFriendIDs != null && FacebookFriendIDs.Length > 0)
            {
                usernames.AddRange(Classes.User.FetchUsernamesByFacebookID(FacebookFriendIDs));
            }

            foreach (var username in usernames)
            {
                try
                {
                    User friend = Classes.User.Load(username);
                    user.AddToFriends(username);
                    friend.AddToFriends(user.Username);
                }
                catch (NotFoundException) { }
            }
        }

        private void PopulateUserDataUsingFacebook()
        {
            var oAuth = new oAuthFacebook
                            {
                                CallBackUrl = Config.Urls.Home.Trim('/') + "/Register.aspx?facebook=1&login=1",
                                Scope = "user_birthday,email"
                            };

            if (Request["code"] == null)
            {
                //Redirect the user back to Facebook for authorization.
                Response.Redirect(oAuth.AuthorizationLinkGet());
            }
            else
            {
                //Get the access token and secret.
                oAuth.AccessTokenGet(Request["code"]);

                if (oAuth.Token.Length > 0)
                {
                    string url = string.Format("https://graph.facebook.com/me?access_token={0}", oAuth.Token);
                    //Response.Write(url);
                    string json = oAuth.WebRequest(oAuthFacebook.Method.GET, url, String.Empty);
                    var userInfo = (JContainer)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    var userId = Convert.ToInt64(userInfo["id"].Value<string>());

                    #region log on the user if she is already registered
                    string[] usernames = Classes.User.FetchUsernamesByFacebookID(new[] { userId  });
                    if (usernames.Length > 0)
                    {
                        UserSession userSession;
                        try
                        {
                            userSession = new UserSession(usernames[0]);
                            Classes.User.AuthorizeByFacebookID(userId);
                            userSession.Authorize(Session.SessionID);
                        }
                        catch (NotFoundException)
                        {
                            goto populatedata;
                        }
                        catch (AccessDeniedException err)
                        {
                            StatusPageMessage = err.Message;
                            Response.Redirect(Config.Urls.Home + "/ShowStatus.aspx");
                            return;
                        }
                        catch (SmsNotConfirmedException)
                        {
                            Response.Redirect("SmsConfirm.aspx?username=" + usernames[0]);
                            return;
                        }
                        catch (ArgumentException err)
                        {
                            StatusPageMessage = err.Message;
                            Response.Redirect(Config.Urls.Home + "/ShowStatus.aspx");
                            return;
                        }
                        catch (Exception err)
                        {
                            Global.Logger.LogWarning(err);
                            StatusPageMessage = err.Message;
                            Response.Redirect(Config.Urls.Home + "/ShowStatus.aspx");
                            return;
                        }

                        CurrentUserSession = userSession;
                        CurrentUserSession.LoggedInThroughFacebook = true;
                        Response.Redirect("Home.aspx");
                    }
                    #endregion

                populatedata:

                    FacebookID = userId;
                    Session["FacebookID"] = userId;

                    DateTime birthday;
                    if (userInfo["birthday"] != null &&
                        DateTime.TryParse(userInfo["birthday"].Value<string>(), 
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday))
                    {
                        datePicker1.SelectedDate = birthday;
                    }

                    if (userInfo["email"] != null)
                        txtEmail.Text = userInfo["email"].Value<string>();
                    if (userInfo["username"] != null)
                        txtUsername.Text = userInfo["username"].Value<string>();

                    User.eGender? gender = null;

                    if (Config.Users.DisableGenderInformation)
                    {
                        dropGender.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                        if (Config.Users.InterestedInFieldEnabled)
                            dropInterestedIn.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                    }
                    else
                    {
                        if (userInfo["gender"] != null)
                        {
                            switch (userInfo["gender"].Value<string>().ToLower(CultureInfo.InvariantCulture))
                            {
                                case "male":
                                    gender = Classes.User.eGender.Male;
                                    break;
                                case "female":
                                    gender = Classes.User.eGender.Female;
                                    break;
                            }
                        }

                        if (gender.HasValue)
                        {
                            dropGender.SelectedValue = ((int) gender).ToString();

                            if (Config.Users.InterestedInFieldEnabled)
                            {
                                switch (gender)
                                {
                                    case Classes.User.eGender.Male:
                                        dropInterestedIn.SelectedValue = ((int) Classes.User.eGender.Female).ToString();
                                        break;
                                    case Classes.User.eGender.Female:
                                        dropInterestedIn.SelectedValue = ((int) Classes.User.eGender.Male).ToString();
                                        break;
                                }
                            }
                        }
                    }

                    if (userInfo["username"] != null)
                        PrimaryPhotoURL = string.Format("https://graph.facebook.com/{0}/picture?type=large",
                                                        userInfo["username"].Value<string>());

                    if (userInfo["name"] != null)
                        txtName.Text = userInfo["name"].Value<string>();

                    divFacebook.Visible = false;
                }
            }
        }

        //protected void dropGender_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    trBirthday2.Visible = dropGender.SelectedValue == ((int)Classes.User.eGender.Couple).ToString();
        //}

        protected void btnUseFacebook_Click(object sender, EventArgs e)
        {
            PopulateUserDataUsingFacebook();
        }
    }
}
