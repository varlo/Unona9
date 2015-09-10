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
using System.Collections;
using System.Web.UI.WebControls;
using AspNetDating.Components.Groups;

namespace AspNetDating.Classes
{
    /// <summary>
    /// Summary description for Templates.
    /// </summary>
    /// 
    public class HtmlEditor : TextBox
    {
        public string Content { get { return Text; } set { Text = value; } }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            TextMode = TextBoxMode.MultiLine;

            Page.RegisterJQuery();
            Page.RegisterCKEditor();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ckeditor" + ClientID, "$('#" + ClientID.EscapeJqueryChars() + "').ckeditor();", true);
        }
    }

    public class EmailTemplates
    {
        public interface IEmailTemplate : ITemplate
        {
            string Subject { get; set; }
            string Body { get; set; }
            string Description { get; }
        }

        public interface ITemplate
        {
            int LanguageId { get; set; }
        }


        [Reflection.DescriptionAttribute("New password confirmation template")]
        public class NewPasswordConfirmation : IEmailTemplate
        {
            public NewPasswordConfirmation()
            {
            }

            public NewPasswordConfirmation(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultSubject = "Forgot your password?";

            private string DefaultBody = "Hi <b>%%RECIPIENT%%</b>,<br>\r\n"
                                         + "<br>\r\n"
                                         +
                                         "In order to change your password please click on the following link:<br>\r\n"
                                         + "<a href=\"%%CONFIRM_URL%%\">%%CONFIRM_URL%%</a><br>\r\n"
                                         + "<br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";

            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get
                {
                    return DBSettings.Get("NewPasswordConfirmation_Subject_" + LanguageId.ToString(),
                                          DefaultSubject);
                }
                set { DBSettings.Set("NewPasswordConfirmation_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof (HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            
            public string Body
            {
                get { return DBSettings.Get("NewPasswordConfirmation_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("NewPasswordConfirmation_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%RECIPIENT%% where you want to specify the name of the recipient and %%CONFIRM_URL%% where you want to place the confirmation link".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Registration confirmation template")]
        public class RegistrationConfirmation : IEmailTemplate
        {
            public RegistrationConfirmation()
            {
            }

            public RegistrationConfirmation(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultSubject = "Complete your registration";

            private string DefaultBody = "Hi <b>%%RECIPIENT%%</b>,<br>\r\n"
                                         + "<br>\r\n"
                                         +
                                         "In order to confirm your registration please click on the following link:<br>\r\n"
                                         + "<a href=\"%%CONFIRM_URL%%\">%%CONFIRM_URL%%</a><br>\r\n"
                                         + "<br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";


            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("RegistrationConfirmation_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("RegistrationConfirmation_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            
            public string Body
            {
                get { return DBSettings.Get("RegistrationConfirmation_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("RegistrationConfirmation_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%RECIPIENT%% where you want to specify the username of the recipient and %%CONFIRM_URL%% where you want to place the confirmation link".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Reject profile template")]
        public class RejectProfile : IEmailTemplate
        {
            public RejectProfile()
            {
            }

            public RejectProfile(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultSubject = "Your account has been rejected";

            private string DefaultBody = "Hi <b>%%RECIPIENT%%</b>,<br>\r\n"
                                         + "<br>\r\n"
                                         +
                                         "We regret to inform you that your account has been rejected.";

            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("RejectProfile_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("RejectProfile_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]

            public string Body
            {
                get { return DBSettings.Get("RejectProfile_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("RejectProfile_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%RECIPIENT%% where you want to specify the username of the recipient".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Message from member template")]
        public class MessageFromMember : IEmailTemplate
        {
            public MessageFromMember()
            {
            }

            public MessageFromMember(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultSubject = "New Message from %%SENDER%%";

            private string DefaultBody = "Hi <b>%%RECIPIENT%%</b>,<br>\r\n"
                                         + "<br>\r\n"
                                         + "You have new message from %%SENDER%%:<br>\r\n"
                                         + "<div color=\"#333333\"><i>%%MESSAGE%%</i></div>\r\n"
                                         + "<br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";

            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("MessageFromMember_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("MessageFromMember_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            
            public string Body
            {
                get { return DBSettings.Get("MessageFromMember_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("MessageFromMember_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%SENDER%% where you want to put the username of the message sender, %%RECIPIENT%% for the recipient of the message and %%MESSAGE%% for the actual message".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Message to non paid member template")]
        public class MessageToNonPaidMember : IEmailTemplate
        {
            public MessageToNonPaidMember()
            {
            }

            public MessageToNonPaidMember(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultSubject = "New Message from %%SENDER%%";

            private string DefaultBody = "Hi <b>%%RECIPIENT%%</b>,<br>\r\n"
                                         + "<br>\r\n"
                                         + "You have new message from %%SENDER%%:<br>\r\n"
                                         + "<div color=\"#333333\"><i>%%MESSAGE%%</i></div>\r\n"
                                         + "<br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";

            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("MessageToNonPaidMember_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("MessageToNonPaidMember_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]

            public string Body
            {
                get { return DBSettings.Get("MessageToNonPaidMember_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("MessageToNonPaidMember_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%SENDER%% where you want to put the username of the message sender, %%RECIPIENT%% for the recipient of the message and %%MESSAGE%% for the actual message".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Invite a friend template")]
        public class InviteFriend : IEmailTemplate
        {
            public InviteFriend()
            {
            }

            public InviteFriend(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultSubject = "Invitation from %%SENDER%%";

            private string DefaultBody = "Hi,<br>\r\n"
                                         + "<br>\r\n"
                                         + "Check out this online dating website "
                                         + "<a href=\"" + Config.Urls.Home + "/default.aspx?invitedBy=%%USERNAME%%\">" + Config.Misc.SiteTitle +
                                         "</a>. I am sure you would like it.<br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "%%SENDER%%";

            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("InviteFriend_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("InviteFriend_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("InviteFriend_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("InviteFriend_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%USERNAME%% where you want to put username and %%SENDER%% where you want to put the first and the last name of the sender".TranslateA();
                }
            }

            public string GetFormattedBody(string senderName)
            {
                return Body.Replace("%%SENDER%%", senderName);
            }

            public string GetFormattedSubject(string senderName)
            {
                return Subject.Replace("%%SENDER%%", senderName);
            }
        }

        [Reflection.Description("Automatic birthday e-mail template")]
        public class HappyBirthday : IEmailTemplate
        {
            public HappyBirthday() { }
            public HappyBirthday(int languageId) { this.languageId = languageId; }

            public int languageId = Config.Misc.DefaultLanguageId;
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }
            private const string DefaultSubject = "Happy FriendBirthday!";
            private string DefaultBody = "Happy FriendBirthday %%RECIPIENT%%!<br>\r\n"
                + "<br>\r\n"
                + "-- Cool birthday greeting goes here -- <br><br>"
                + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";

            [Reflection.Description("Email subject")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("HappyBirthday_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("HappyBirthday_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("Email body")]
            [Reflection.Control(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("HappyBirthday_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("HappyBirthday_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("N/A")]
            public string Description
            {
                get { return "Description: Use %%RECIPIENT%% where you want to put the first and the last name of the recipient".TranslateA(); }
            }

            public string GetFormattedBody(string recipientUsername)
            {
                return Body.Replace("%%RECIPIENT%%", recipientUsername);
            }

            public string GetFormattedSubject(string recipientUsername)
            {
                return Subject.Replace("%%RECIPIENT%%", recipientUsername);
            }
        }

        [Reflection.DescriptionAttribute("Send to a friend template")]
        public class SendProfileToFriend : IEmailTemplate
        {
            public SendProfileToFriend()
            {
            }

            public SendProfileToFriend(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultSubject = "Your friend %%SENDER%% wants you to meet someone";

            private string DefaultBody = "Hi,<br>\r\n"
                                         + "%%RECIPIENTNAME%%<br>"
                                         + "<br />"
                                         + "<br />"
                                         + "<img alt=\"\" src=\"%%PHOTOURL%%\" /> %%USERNAME%%<br />"
                                         + "<br />"
                                         + "Age:%%AGE%%<br />"
                                         + "<br />"
                                         + "<a href=\"%%PROFILEURL%%\">View Profile</a> <br />"
                                         + "%%MESSAGE%%<br>"
                                         + "Check out this online dating website "
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle +
                                         "</a>. I am sure you would like it.<br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "%%SENDER%%";

            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("SendProfileToFriend_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("SendProfileToFriend_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("SendProfileToFriend_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("SendProfileToFriend_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%USERNAME%% for username, %%SENDER%% where you want to put the first and the last name of the sender, %%RECIPIENTNAME%% for recipient's name, %%PHOTOURL%% for photo's URL, %%AGE%% for age".TranslateA();
                }
            }

            public string GetFormattedBody(string senderUsername, string recipientName, string message, string sentUsername)
            {
                User sentUser = null;
                Photo primaryPhoto = null;
                string body = Body;

                try
                {
                    sentUser = User.Load(sentUsername);
                }
                catch (NotFoundException)
                {
                    body = "";
                }

                if (sentUser != null && !sentUser.Deleted)
                {
                    body = body.Replace("%%USERNAME%%", sentUser.Username);
                    body = body.Replace("%%RECIPIENTNAME%%", recipientName);
                    body = body.Replace("%%MESSAGE%%", message);
                    body = body.Replace("%%SENDER%%", senderUsername);
                    body = body.Replace("%%AGE%%", sentUser.Age.ToString());

                    try
                    {
                        primaryPhoto = sentUser.GetPrimaryPhoto();
                        body = body.Replace("%%PHOTOURL%%", ImageHandler.CreateImageUrl(primaryPhoto.Id, 450, 450, false, true, false));
                        body = body.Replace("%%PHOTOID%%", primaryPhoto.Id.ToString());
                    }
                    catch (NotFoundException)
                    {
                        body = body.Replace("%%PHOTOURL%%", ImageHandler.CreateImageUrl(ImageHandler.GetPhotoIdByGender(sentUser.Gender), 90, 90, false, true, false));
                        body = body.Replace("%%PHOTOID%%", "0");
                    }

                    body =
                        body.Replace("%%PROFILEURL%%",
                                     Config.Urls.Home + "/ShowUser.aspx?uid=" + sentUser.Username);
                }
                
                return body;
            }

            public string GetFormattedSubject(string senderUsername)
            {
                return Subject.Replace("%%SENDER%%", senderUsername);
            }
        }

        [Reflection.Description("Automatic matches e-mail template")]
        public class SavedSearchMatches : IEmailTemplate
        {
            public SavedSearchMatches() { }
            public SavedSearchMatches(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.Description("Rows of the table")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("Width", 20)]
            [Reflection.Property("CssClass", "font_css")]
            public int Rows
            {
                get
                {
                    return DBSettings.Get("SavedSearchMatches_Rows_" + LanguageId.ToString(), 3);
                }
                set { DBSettings.Set("SavedSearchMatches_Rows_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("Columns of the table")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("Width", 20)]
            [Reflection.Property("CssClass", "font_css")]
            public int Columns
            {
                get
                {
                    return DBSettings.Get("SavedSearchMatches_Columns_" + LanguageId.ToString(), 3);
                }
                set { DBSettings.Set("SavedSearchMatches_Columns_" + LanguageId.ToString(), value); }
            }

            public int NumberOfMatchesToMail
            {
                get { return Rows * Columns; }
            }

            private const string DefaultSubject = "Your Matches";
            private string DefaultBody = "Your matches %%RECIPIENT%%!<br />"
                + "<br />"
                + "<br />"
                + "%%USERSPROFILES%%"
                + "<br />"
                + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";

            [Reflection.Description("Email subject")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("Width", 400)]
            [Reflection.Property("CssClass", "font_css")]
            public string Subject
            {
                get { return DBSettings.Get("SavedSearchMatches_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("SavedSearchMatches_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("Email body")]
            [Reflection.Control(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", 200)]
            [Reflection.Property("Width", "100%")]
            public string Body
            {
                get { return DBSettings.Get("SavedSearchMatches_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("SavedSearchMatches_Body_" + LanguageId.ToString(), value); }
            }

            private string DefaultItemTemplate = "<div align=\"center\" style=\"padding-right: 3px; padding-left: 3px; padding-bottom: 3px; margin: 3px; padding-top: 3px; background-color: #ebebeb\">"                                               
                                                 + "<img alt=\"\" src=\"%%PHOTOURL%%\" /><br /><br />%%USERNAME%%<br />"
                                                 + "<br />"
                                                 + "Age:%%AGE%%<br />"
                                                 + "<br />"
                                                 + "<a href=\"%%PROFILEURL%%\">View Profile</a>"
                                                 + "</div>";

            [Reflection.Description("User profile template")]
            [Reflection.Control(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", 200)]
            [Reflection.Property("Width", "100%")]
            public string ItemTemplate
            {
                get { return DBSettings.Get("SavedSearchMatches_ItemTemplate_" + LanguageId.ToString(), DefaultItemTemplate); }
                set { DBSettings.Set("SavedSearchMatches_ItemTemplate_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%RECIPIENT%% where you want to put the first and the last name of the recipient, %%USERSPROFILES%% where you want to put users profiles. Use %%USERNAME%% for username, %%AGE%% for age, %%PHOTOURL%% for photo's URL, %%PROFILEURL%% for profile URL".TranslateA();
                }
            }

            public string GetFormattedBody(SavedSearchMatches savedSearchTemplate, string recipientName, string[] usernameMatches)
            {
                string body = Body;

                body = body.Replace("%%USERSPROFILES%%", generateContentTable(savedSearchTemplate.Rows, savedSearchTemplate.Columns, usernameMatches));
                body = body.Replace("%%RECIPIENT%%", recipientName);

                return body;
            }

            public string GetFormattedSubject(string recipientUsername)
            {
                return Subject.Replace("%%RECIPIENT%%", recipientUsername);
            }

            private string generateContentTable(int rows, int columns, string[] usernameMatches)
            {
                ArrayList alUsernameMatches = new ArrayList(usernameMatches);

                string result = "<table>";

                for (int i = 0; i < rows; i++)
                {
                    result += "<tr>";

                    for (int j = 0; j < columns; j++)
                    {
                        string username = (string)alUsernameMatches[0];
                        result += "<td>" + generateItemTemplate(username, ItemTemplate) + "</td>";
                        alUsernameMatches.Remove(username);
                    }

                    result += "</tr>";
                }

                return result + "</table>";
            }

            private string generateItemTemplate(string username, string itemTemplate)
            {
                User matchedUser = null;
                Photo primaryPhoto = null;

                try
                {
                    matchedUser = User.Load(username);
                    itemTemplate = itemTemplate.Replace("%%USERNAME%%", matchedUser.Username);
                }
                catch (NotFoundException)
                {
                    return "";
                }

                itemTemplate = itemTemplate.Replace("%%AGE%%", matchedUser.Age.ToString());

                try
                {
                    primaryPhoto = matchedUser.GetPrimaryPhoto();
                    itemTemplate = itemTemplate.Replace("%%PHOTOURL%%", ImageHandler.CreateImageUrl(primaryPhoto.Id, 90, 90, false, true, true));
                }
                catch (NotFoundException)
                {
                    itemTemplate = itemTemplate.Replace("%%PHOTOURL%%", ImageHandler.CreateImageUrl(ImageHandler.GetPhotoIdByGender(matchedUser.Gender), 90, 90, false, true, false));
                }

                itemTemplate =
                    itemTemplate.Replace("%%PROFILEURL%%",
                                 Config.Urls.Home + "/ShowUser.aspx?uid=" + matchedUser.Username);

                return itemTemplate;
            }
        }

        [Reflection.Description("Last visit e-mail template")]
        public class VisitSiteReminder : IEmailTemplate
        {
            public VisitSiteReminder() { }
            public VisitSiteReminder(int languageId) { this.languageId = languageId; }

            public int languageId = Config.Misc.DefaultLanguageId;
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }
            private const string DefaultSubject = "We miss you!";

            private string DefaultBody = "Hey %%RECIPIENT%%!<br>\r\n"
                                         + "<br>\r\n"
                                         +
                                         String.Format(
                                             "It's been {0} days since you last visited <a href=\"" + Config.Urls.Home +
                                             "\">" + Config.Misc.SiteTitle + "</a>!<br><br>",
                                             Config.Misc.NotVisitedSiteDays);

            [Reflection.Description("Email subject")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("VisitSiteReminder_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("VisitSiteReminder_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("Email body")]
            [Reflection.Control(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("VisitSiteReminder_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("VisitSiteReminder_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("N/A")]
            public string Description
            {
                get { return "Description: Use %%RECIPIENT%% where you want to put the first and the last name of the recipient".TranslateA(); }
            }

            public string GetFormattedBody(string recipientUsername)
            {
                return Body.Replace("%%RECIPIENT%%", recipientUsername);
            }

            public string GetFormattedSubject(string recipientUsername)
            {
                return Subject.Replace("%%RECIPIENT%%", recipientUsername);
            }
        }

        [Reflection.DescriptionAttribute("Send e-card message")]
        public class SendEcard
        {
            public SendEcard()
            { }

            public SendEcard(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private readonly string DefaultSubject = String.Format("You have received an e-card in {0}", Config.Misc.SiteTitle);

            private string DefaultBody = "%%USER%% has sent you an e-card!<br>\r\n"
                + "<br>\r\n"
                + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a>";

            [Reflection.Description("Email subject")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("SendEcard_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("SendEcard_Subject_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("Email body")]
            [Reflection.Control(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("SendEcard_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("SendEcard_Body_" + LanguageId.ToString(), value); }
            }

            [Reflection.Description("N/A")]
            public string Description
            {
                get { return "Description: You can use %%USER%% to specify the username of the sender".TranslateA(); }
            }

            public string GetFormattedBody(string senderUsername)
            {
                return Body.Replace("%%USER%%", senderUsername);
            }

            public string GetFormattedSubject(string senderUsername)
            {
                return Subject.Replace("%%USER%%", senderUsername);
            }
        }

        [Reflection.Description("Show interest message")]
        public class ShowInterest : IEmailTemplate
        {
            public ShowInterest() { }
            public ShowInterest(int languageId) { _languageId = languageId; }

            private int _languageId = Config.Misc.DefaultLanguageId;
            public int LanguageId
            {
                get { return _languageId; }
                set { _languageId = value; }
            }

            private const string DefaultSubject = "You got a smile from %%SENDER%%";

            private readonly string _defaultBody = String.Format(SendAnnouncement.EMAIL_TEMPLATE, "<a href=\"" + Config.Urls.Home + "/ShowUser.aspx?uid=%%SENDER%%\">%%IMAGE_SENDER%%</a><br/>\r\n"
                + "<a href=\"" + Config.Urls.Home + "/ShowUser.aspx?uid=%%SENDER%%\">%%SENDER%%</a> is sending a smile to you!<br/>"
                + "<p>Please "
                + "<a href=\"" + Config.Urls.Home + "\">" + "log in" + "</a>"
                + " to your account to check the profile. Keep the interest going by sending a smile or even better - send a kind message to get a relationship started.<br>\r\n"
                + " Do not procrastinate, your dream might be just a click away. Remember - She is Waiting for You. Greetings,<br/>\r\n"
                + "Unona Dating Customer Support</p>");

            [Reflection.Description("Email subject")]
            [Reflection.Property("TextMode", TextBoxMode.SingleLine)]
            [Reflection.Property("CssClass", "tsingleline")]
            public string Subject
            {
                get { return DBSettings.Get("ShowInterest_Subject_" + LanguageId, DefaultSubject); }
                set { DBSettings.Set("ShowInterest_Subject_" + LanguageId, value); }
            }

            [Reflection.Description("Email body")]
            [Reflection.Control(typeof(HtmlEditor), "Value")]

            public string Body
            {
                get { return String.Format(SendAnnouncement.EMAIL_TEMPLATE, DBSettings.Get("ShowInterest_Body_" + LanguageId, _defaultBody)); }
                set { DBSettings.Set("ShowInterest_Body_" + LanguageId, value); }
            }

            [Reflection.Description("N/A")]
            public string Description
            {
                get { return Lang.Trans("Description: Use %%RECIPIENT%% where you want to put the first and the last name of the recipient"); }
            }

            public string GetFormattedBody(string recipientUsername, string senderUsername, string senderImage)
            {
                string formattedBody = Body;

                formattedBody = formattedBody.Replace("%%IMAGE_SENDER%%", senderImage);
                formattedBody = formattedBody.Replace("%%RECIPIENT%%", recipientUsername);
                formattedBody = formattedBody.Replace("%%SENDER%%", senderUsername);

                return formattedBody;
            }

            public string GetFormattedSubject(string senderUsername)
            {
                return Subject.Replace("%%SENDER%%", senderUsername);
            }
        }
    }

    public class MiscTemplates
    {
        [Reflection.DescriptionAttribute("Welcome message")]
        public class WelcomeMessage
        {
            public WelcomeMessage()
            {
            }

            public WelcomeMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = "Dear %%USERNAME%%, welcome to %%SITETITLE%%.";

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("WelcomeMessage_Message_" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("WelcomeMessage_Message_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%USERNAME%% where you want to put the username and %%SITETITLE%% for the site title".TranslateA();
                }
            }

            public string GetFormattedMessage(string username, string siteTitle)
            {
                return Message.Replace("%%USERNAME%%", username).Replace("%%SITETITLE%%", siteTitle);
            }
        }

        [Reflection.DescriptionAttribute("Automatic friend birthday e-mail template")]
        public class FriendBirthday
        {
            public FriendBirthday()
            {
            }

            public FriendBirthday(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultMessage = "Your friend %%USERNAME%% has birthday today!";

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("FriendBirthday_Message_" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("FriendBirthday_Message_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get { return "Description: You can use %%USERNAME%% where you want to put friend username".TranslateA(); }
            }
        }

        [Reflection.DescriptionAttribute("Approve profile message")]
        public class ApproveProfileMessage
        {
            public ApproveProfileMessage()
            {
            }

            public ApproveProfileMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApproveProfileMessage_Message_" + LanguageId.ToString(),
                                       "Your profile has been approved!");
                }
                set { DBSettings.Set("ApproveProfileMessage_Message_" + LanguageId.ToString(), value); }
            }
        }

        [Reflection.DescriptionAttribute("Approve photo message")]
        public class ApprovePhotoMessage
        {
            public ApprovePhotoMessage()
            {
            }

            public ApprovePhotoMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApprovePhotoMessage_Message_" + LanguageId.ToString(),
                                       "Your photo has been approved!");
                }
                set { DBSettings.Set("ApprovePhotoMessage_Message_" + LanguageId.ToString(), value); }
            }
        }

        [Reflection.DescriptionAttribute("Reject photo message")]
        public class RejectPhotoMessage
        {
            public RejectPhotoMessage()
            {
            }

            public RejectPhotoMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultWithNoReasonMessage = "Your photo has been rejected!";

            private string DefaultWithReasonMessage = "Your photo has been rejected for the following reason:\r\n"
                                                      + "%%REASON%%";

            [Reflection.DescriptionAttribute("no reason message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string WithNoReasonMessage
            {
                get
                {
                    return
                        DBSettings.Get("RejectPhotoMessage_WithNoReasonMessage_" + LanguageId.ToString(),
                                       DefaultWithNoReasonMessage);
                }
                set { DBSettings.Set("RejectPhotoMessage_WithNoReasonMessage_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("message with reason")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string WithReasonMessage
            {
                get
                {
                    return
                        DBSettings.Get("RejectPhotoMessage_WithReasonMessage_" + LanguageId.ToString(),
                                       DefaultWithReasonMessage);
                }
                set { DBSettings.Set("RejectPhotoMessage_WithReasonMessage_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%REASON%% in the \"with reason\" template message to specify reason's location".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Approve video message")]
        public class ApproveVideoMessage
        {
            public ApproveVideoMessage()
            {
            }

            public ApproveVideoMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApproveVideoMessage_Message_" + LanguageId.ToString(),
                                       "Your video has been approved!");
                }
                set { DBSettings.Set("ApproveVideoMessage_Message_" + LanguageId.ToString(), value); }
            }
        }

        [Reflection.DescriptionAttribute("Reject video message")]
        public class RejectVideoMessage
        {
            public RejectVideoMessage()
            {
            }

            public RejectVideoMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("RejectVideoMessage_Message_" + LanguageId.ToString(),
                                       "Your video has been rejected!");
                }
                set { DBSettings.Set("RejectVideoMessage_Message_" + LanguageId.ToString(), value); }
            }
        }

        [Reflection.DescriptionAttribute("Approve audio message")]
        public class ApproveAudioMessage
        {
            public ApproveAudioMessage()
            {
            }

            public ApproveAudioMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApproveAudioMessage_Message_" + LanguageId.ToString(),
                                       "Your audio upload has been approved!");
                }
                set { DBSettings.Set("ApproveAudioMessage_Message_" + LanguageId.ToString(), value); }
            }
        }

        [Reflection.DescriptionAttribute("Reject audio message")]
        public class RejectAudioMessage
        {
            public RejectAudioMessage()
            {
            }

            public RejectAudioMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("RejectAudioMessage_Message_" + LanguageId.ToString(),
                                       "Your audio upload has been rejected!");
                }
                set { DBSettings.Set("RejectAudioMessage_Message_" + LanguageId.ToString(), value); }
            }
        }

        //[Reflection.DescriptionAttribute("Pre-Written Message Responses")]
        //public class PreWrittenMessageResponses
        //{
        //    public PreWrittenMessageResponses()
        //    {
        //    }

        //    public PreWrittenMessageResponses(int languageId)
        //    {
        //        this.languageId = languageId;
        //    }

        //    public int languageId = Config.Misc.DefaultLanguageId;

        //    public int LanguageId
        //    {
        //        get { return languageId; }
        //        set { languageId = value; }
        //    }

        //    private const string DefaultResponses =
        //        "Thank You for the Message!\n" +
        //        "When I upgrade my account I will contact you!\n" +
        //        "I am interested in getting to know you better!\n" +
        //        "I enjoyed reading your profile!";

        //    [Reflection.DescriptionAttribute("Message Responses")]
        //    [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
        //    [Reflection.PropertyAttribute("CssClass", "tmultiline")]
        //    public string MessageResponses
        //    {
        //        get
        //        {
        //            return
        //                DBSettings.Get("PreWrittenMessageResponses_MessageResponses_" + LanguageId.ToString(),
        //                               DefaultResponses);
        //        }
        //        set { DBSettings.Set("PreWrittenMessageResponses_MessageResponses_" + LanguageId.ToString(), value); }
        //    }

        //    [Reflection.DescriptionAttribute("N/A")]
        //    public string Description
        //    {
        //        get { return "Description: Each line represents single pre-written message response".TranslateA(); }
        //    }
        //}

        [Reflection.DescriptionAttribute("Approve classified message")]
        public class ApproveAdMessage
        {
            public ApproveAdMessage()
            {
            }

            public ApproveAdMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = @"Your ""%%SUBJECT%%"" classified has been approved";

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApproveAdMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("ApproveAdMessage_Message" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%SUBJECT%% to specify ad's subject".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Reject ad message")]
        public class RejectAdMessage
        {
            public RejectAdMessage()
            {
            }

            public RejectAdMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultWithNoReasonMessage = @"Your ""%%SUBJECT%%"" classified has been rejected!";

            private string DefaultWithReasonMessage = "Your \"%%SUBJECT%%\" ad has been rejected for the following reason:\r\n"
                                                      + "%%REASON%%";

            [Reflection.DescriptionAttribute("no reason message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string WithNoReasonMessage
            {
                get
                {
                    return
                        DBSettings.Get("RejectAdMessage_WithNoReasonMessage_" + LanguageId.ToString(),
                                       DefaultWithNoReasonMessage);
                }
                set { DBSettings.Set("RejectAdMessage_WithNoReasonMessage_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("message with reason")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string WithReasonMessage
            {
                get
                {
                    return
                        DBSettings.Get("RejectAdMessage_WithReasonMessage_" + LanguageId.ToString(),
                                       DefaultWithReasonMessage);
                }
                set { DBSettings.Set("RejectAdMessage_WithReasonMessage_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%REASON%% in the \"with reason\" template message to specify reason's location and %%SUBJECT%% to specify ad's subject".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Approve group message")]
        public class ApproveGroupMessage
        {
            public ApproveGroupMessage()
            {
            }

            public ApproveGroupMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = @"Your ""%%GROUP%%"" group has been approved";

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApproveGroupMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("ApproveGroupMessage_Message" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Reject group message")]
        public class RejectGroupMessage
        {
            public RejectGroupMessage()
            {
            }

            public RejectGroupMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultWithNoReasonMessage = @"Your ""%%GROUP%%"" group has been rejected!";

            private string DefaultWithReasonMessage = "Your \"%%GROUP%%\" group has been rejected for the following reason:\r\n"
                                                      + "%%REASON%%";

            [Reflection.DescriptionAttribute("no reason message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string WithNoReasonMessage
            {
                get
                {
                    return
                        DBSettings.Get("RejectGroupMessage_WithNoReasonMessage_" + LanguageId.ToString(),
                                       DefaultWithNoReasonMessage);
                }
                set { DBSettings.Set("RejectGroupMessage_WithNoReasonMessage_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("message with reason")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string WithReasonMessage
            {
                get
                {
                    return
                        DBSettings.Get("RejectGroupMessage_WithReasonMessage_" + LanguageId.ToString(),
                                       DefaultWithReasonMessage);
                }
                set { DBSettings.Set("RejectGroupMessage_WithReasonMessage_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%REASON%% in the \"with reason\" template message to specify reason's location and %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Approve group member message")]
        public class ApproveGroupMemberMessage
        {
            public ApproveGroupMemberMessage()
            {
            }

            public ApproveGroupMemberMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = @"Your request for ""%%GROUP%%"" group has been approved";

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("ApproveGroupMemberMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("ApproveGroupMemberMessage_Message" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Reject group member message")]
        public class RejectGroupMemberMessage
        {
            public RejectGroupMemberMessage()
            {
            }

            public RejectGroupMemberMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = @"Your request for ""%%GROUP%%"" has been rejected";

            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>The message.</value>
            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("RejectGroupMemberMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("RejectGroupMemberMessage_Message" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Delete group member message")]
        public class DeleteGroupMemberMessage
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DeleteGroupMemberMessage"/> class.
            /// </summary>
            public DeleteGroupMemberMessage()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DeleteGroupMemberMessage"/> class.
            /// </summary>
            /// <param name="languageId">The language id.</param>
            public DeleteGroupMemberMessage(int languageId)
            {
                this.languageId = languageId;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = @"You have been erased from ""%%GROUP%%"" group";

            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>The message.</value>
            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("DeleteGroupMemberMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("DeleteGroupMemberMessage_Message" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Transfer group ownership message")]
        public class TransferGroupOwnerMessage
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TransferGroupOwnerMessage"/> class.
            /// </summary>
            public TransferGroupOwnerMessage()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TransferGroupOwnerMessage"/> class.
            /// </summary>
            /// <param name="languageId">The language id.</param>
            public TransferGroupOwnerMessage(int languageId)
            {
                this.languageId = languageId;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = @"Congratulations you are owner of ""%%GROUP%%"" group now.";

            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>The message.</value>
            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("TransferGroupOwnerMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("TransferGroupOwnerMessage_Message" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Invite group member message")]
        public class InviteGroupMemberMessage
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InviteGroupMemberMessage"/> class.
            /// </summary>
            public InviteGroupMemberMessage()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="InviteGroupMemberMessage"/> class.
            /// </summary>
            /// <param name="languageId">The language id.</param>
            public InviteGroupMemberMessage(int languageId)
            {
                this.languageId = languageId;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = "%%SENDER%% has invited you to the \"%%GROUP%%\" group.";

            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>The message.</value>
            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("InviteGroupMemberMessage_Message" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("InviteGroupMemberMessage_Message" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% to specify group's location".TranslateA();
                }
            }
        }

        [Reflection.DescriptionAttribute("Warn group member message")]
        public class WarnGroupMemberMessage
        {
            public WarnGroupMemberMessage()
            {
            }

            public WarnGroupMemberMessage(int languageId)
            {
                this.languageId = languageId;
            }

            public int languageId = Config.Misc.DefaultLanguageId;

            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private string DefaultMessage = "You have been warned in the %%GROUP%% group for the following reason:\r\n %%REASON%%";

            [Reflection.DescriptionAttribute("Message")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.MultiLine)]
            [Reflection.PropertyAttribute("CssClass", "tmultiline")]
            public string Message
            {
                get
                {
                    return
                        DBSettings.Get("WarnGroupMemberMessage_Message_" + LanguageId.ToString(),
                                       DefaultMessage);
                }
                set { DBSettings.Set("WarnGroupMemberMessage_Message_" + LanguageId.ToString(), value); }
            }

            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: You can use %%GROUP%% where you want to put the group name and %%REASON%% to specify reason's location".TranslateA();
                }
            }

            public string GetFormattedMessage(string groupName, string reason)
            {
                return Message.Replace("%%GROUP%%", groupName).Replace("%%REASON%%", reason);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Subscription Completed Text")]
        public class SubscriptionCompleted
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionCompleted"/> class.
            /// </summary>
            public SubscriptionCompleted()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionCompleted"/> class.
            /// </summary>
            /// <param name="languageId">The language id.</param>
            public SubscriptionCompleted(int languageId)
            {
                this.languageId = languageId;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultText = "<br>Thank you for upgrading your account!<br>\n<br>\n<br>";

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            [Reflection.DescriptionAttribute("Text")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Text
            {
                get { return DBSettings.Get("SubscriptionCompleted_Text_" + LanguageId.ToString(), DefaultText); }
                set { DBSettings.Set("SubscriptionCompleted_Text_" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get { return "Description: This text appears when a member completes the subscription process".TranslateA(); }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Subscription Cancelled Text")]
        public class SubscriptionCancelled
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionCancelled"/> class.
            /// </summary>
            public SubscriptionCancelled()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionCancelled"/> class.
            /// </summary>
            /// <param name="languageId">The language id.</param>
            public SubscriptionCancelled(int languageId)
            {
                this.languageId = languageId;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultText = "<br>Your subscription has been cancelled!<br>\n<br>\n<br>";

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            [Reflection.DescriptionAttribute("Text")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Text
            {
                get { return DBSettings.Get("SubscriptionCancelled_Text_" + LanguageId.ToString(), DefaultText); }
                set { DBSettings.Set("SubscriptionCancelled_Text_" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get { return "Description: This text appears when a member cancelled his/her subscription".TranslateA(); }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Subscription Charge Text")]
        public class SubscriptionCharge
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionCharge"/> class.
            /// </summary>
            public SubscriptionCharge()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionCharge"/> class.
            /// </summary>
            /// <param name="languageID">The language ID.</param>
            public SubscriptionCharge(int languageID)
            {
                this.languageId = languageID;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultSubject = "You have been charged by %%SENDER%%";

            private string DefaultBody = "Hi %%RECIPIENTUSERNAME%%,<br>\r\n"
                                         + "This email is a notification that your recurring order has been successfully billed.<br>\r\n"
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a><br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "%%SENDER%%";

            /// <summary>
            /// Gets or sets the subject.
            /// </summary>
            /// <value>The subject.</value>
            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("SubscriptionCharge_Subject_" + LanguageId.ToString(), DefaultSubject); }
                set { DBSettings.Set("SubscriptionCharge_Subject_" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets or sets the body.
            /// </summary>
            /// <value>The body.</value>
            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("SubscriptionCharge_Body_" + LanguageId.ToString(), DefaultBody); }
                set { DBSettings.Set("SubscriptionCharge_Body_" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%SENDER%% where you want to put the site title.".TranslateA();
                }
            }

            /// <summary>
            /// Gets the formatted body.
            /// </summary>
            /// <param name="senderUsername">The sender username.</param>
            /// <param name="recipientUsername">The recipient username.</param>
            /// <returns></returns>
            public string GetFormattedBody(string senderUsername, string recipientUsername)
            {
                string body = Body;
                body = body.Replace("%%SENDER%%", senderUsername);
                body = body.Replace("%%RECIPIENTUSERNAME%%", recipientUsername);
                return body;
            }

            /// <summary>
            /// Gets the formatted subject.
            /// </summary>
            /// <param name="senderUsername">The sender username.</param>
            /// <returns></returns>
            public string GetFormattedSubject(string senderUsername)
            {
                return Subject.Replace("%%SENDER%%", senderUsername);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Pay by Check Text")]
        public class PayByCheck
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PayByCheck"/> class.
            /// </summary>
            public PayByCheck()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PayByCheck"/> class.
            /// </summary>
            /// <param name="languageId">The language id.</param>
            public PayByCheck(int languageId)
            {
                this.languageId = languageId;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultText = "This is the information you need in order to pay by check ...";

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            [Reflection.DescriptionAttribute("Text")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Text
            {
                get { return DBSettings.Get("PayByCheck_Text_" + LanguageId.ToString(), DefaultText); }
                set { DBSettings.Set("PayByCheck_Text_" + LanguageId.ToString(), value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get { return "Description: This text appears when a member choose to pay by check".TranslateA(); }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Affiliate Request Payment Text")]
        public class AffiliateRequestPayment
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AffiliateRequestPayment"/> class.
            /// </summary>
            public AffiliateRequestPayment()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AffiliateRequestPayment"/> class.
            /// </summary>
            /// <param name="languageID">The language ID.</param>
            public AffiliateRequestPayment(int languageID)
            {
                this.languageId = languageID;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultSubject = "Request payment from %%AFFILIATE%%";

            private string DefaultBody = "Dear administrator,<br>\r\n"
                                         + "%%AFFILIATE%% has requested a payment.<br>\r\n";

            /// <summary>
            /// Gets or sets the subject.
            /// </summary>
            /// <value>The subject.</value>
            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("AffiliateRequestPayment_Subject_" + LanguageId, DefaultSubject); }
                set { DBSettings.Set("AffiliateRequestPayment_Subject_" + LanguageId, value); }
            }

            /// <summary>
            /// Gets or sets the body.
            /// </summary>
            /// <value>The body.</value>
            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("AffiliateRequestPayment_Body_" + LanguageId, DefaultBody); }
                set { DBSettings.Set("AffiliateRequestPayment_Body_" + LanguageId, value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%AFFILIATE%% where you want to put the affiliate username.".TranslateA();
                }
            }

            /// <summary>
            /// Gets the formatted body.
            /// </summary>
            /// <param name="senderUsername">The sender username.</param>
            /// <param name="recipientUsername">The recipient username.</param>
            /// <returns></returns>
            public string GetFormattedBody(string senderUsername, string recipientUsername)
            {
                string body = Body;
                body = body.Replace("%%AFFILIATE%%", senderUsername);
                return body;
            }

            /// <summary>
            /// Gets the formatted subject.
            /// </summary>
            /// <param name="senderUsername">The sender username.</param>
            /// <returns></returns>
            public string GetFormattedSubject(string senderUsername)
            {
                return Subject.Replace("%%AFFILIATE%%", senderUsername);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Reflection.DescriptionAttribute("Affiliate Payment Sent Text")]
        public class AffiliatePaymentSent
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AffiliatePaymentSent"/> class.
            /// </summary>
            public AffiliatePaymentSent()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AffiliatePaymentSent"/> class.
            /// </summary>
            /// <param name="languageID">The language ID.</param>
            public AffiliatePaymentSent(int languageID)
            {
                languageId = languageID;
            }

            /// <summary>
            /// 
            /// </summary>
            public int languageId = Config.Misc.DefaultLanguageId;

            /// <summary>
            /// Gets or sets the language id.
            /// </summary>
            /// <value>The language id.</value>
            public int LanguageId
            {
                get { return languageId; }
                set { languageId = value; }
            }

            private const string DefaultSubject = "Your payment from %%SENDER%% was sent";

            private string DefaultBody = "Hi %%RECIPIENTUSERNAME%%,<br>\r\n"
                                         + "This email is a notification that your payment has been successfully made.<br>\r\n"
                                         + "<a href=\"" + Config.Urls.Home + "\">" + Config.Misc.SiteTitle + "</a><br>\r\n"
                                         + "Greetings,<br>\r\n"
                                         + "%%SENDER%%";

            /// <summary>
            /// Gets or sets the subject.
            /// </summary>
            /// <value>The subject.</value>
            [Reflection.DescriptionAttribute("Email subject")]
            [Reflection.PropertyAttribute("TextMode", TextBoxMode.SingleLine)]
            [Reflection.PropertyAttribute("CssClass", "form-control")]
            public string Subject
            {
                get { return DBSettings.Get("AffiliatePaymentSent_Subject_" + LanguageId, DefaultSubject); }
                set { DBSettings.Set("AffiliatePaymentSent_Subject_" + LanguageId, value); }
            }

            /// <summary>
            /// Gets or sets the body.
            /// </summary>
            /// <value>The body.</value>
            [Reflection.DescriptionAttribute("Email body")]
            [Reflection.ControlAttribute(typeof(HtmlEditor), "Content")]
            [Reflection.Property("Height", "200px")]
            [Reflection.Property("Width", "98%")]
            public string Body
            {
                get { return DBSettings.Get("AffiliatePaymentSent_Body_" + LanguageId, DefaultBody); }
                set { DBSettings.Set("AffiliatePaymentSent_Body_" + LanguageId, value); }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Reflection.DescriptionAttribute("N/A")]
            public string Description
            {
                get
                {
                    return "Description: Use %%SENDER%% where you want to put the site title.".TranslateA();
                }
            }

            /// <summary>
            /// Gets the formatted body.
            /// </summary>
            /// <param name="senderUsername">The sender username.</param>
            /// <param name="recipientUsername">The recipient username.</param>
            /// <returns></returns>
            public string GetFormattedBody(string senderUsername, string recipientUsername)
            {
                string body = Body;
                body = body.Replace("%%SENDER%%", senderUsername);
                body = body.Replace("%%RECIPIENTUSERNAME%%", recipientUsername);
                return body;
            }

            /// <summary>
            /// Gets the formatted subject.
            /// </summary>
            /// <param name="senderUsername">The sender username.</param>
            /// <returns></returns>
            public string GetFormattedSubject(string senderUsername)
            {
                return Subject.Replace("%%SENDER%%", senderUsername);
            }
        }        
    }
}