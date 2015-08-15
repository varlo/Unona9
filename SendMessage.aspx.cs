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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;
//using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace AspNetDating
{
    public partial class SendMessage : PageBase
    {
        protected SmallBoxStart SmallBoxStart1;
        protected LargeBoxStart LargeBoxStart1;

        protected string MessageBodyClientId
        {
            get { return txtMessageBody.ClientID; }
        }

        public DataList dlSmilies;

        public int CurrentSmiliesPage
        {
            set
            {
                ViewState["CurrentSmiliesPage"] = value;
            }
            get
            {
                return (int) (ViewState["CurrentSmiliesPage"] ?? 0);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.SetSuppressLinkSelection();
                checkRequestLegit();
                loadStrings();
                preparePage();
                loadSmileys();
            }
        }

        private void loadSmileys()
        {
            var lUsedSmilies = new List<string>();
            var dtSmilies = new DataTable();
            dtSmilies.Columns.Add("Key");
            dtSmilies.Columns.Add("Image");
            dtSmilies.Columns.Add("Description");
            dtSmilies.Columns.Add("IsSecondary");

            foreach (string key in Smilies.dSmileys.Keys)
            {
                Smiley smiley = Smilies.dSmileys[key];
                if (lUsedSmilies.IndexOf(smiley.Image) >= 0) continue;
                lUsedSmilies.Add(smiley.Image);

                var row = dtSmilies.NewRow();
                row.ItemArray = new object[]
                                 {
                                     smiley.Key, Config.Urls.Home + "/Smilies/" + smiley.Image,
                                     smiley.Description, smiley.Secondary
                                 };
                
                dtSmilies.Rows.Add(row);
            }

            dtSmilies.DefaultView.Sort = "IsSecondary";

            var pagedSource = new PagedDataSource
                                  {
                                      AllowPaging = true,
                                      PageSize = 41,
                                      DataSource = dtSmilies.DefaultView,
                                  };
            if (CurrentSmiliesPage < 0) CurrentSmiliesPage = pagedSource.PageCount - 1;
            if (CurrentSmiliesPage >= pagedSource.PageCount) CurrentSmiliesPage = 0;
            pagedSource.CurrentPageIndex = CurrentSmiliesPage;

            dlSmilies.DataSource = pagedSource;
            dlSmilies.DataBind();
        }

        private void checkMessagesLimitReached(out bool shouldPayWithCredits)
        {
            shouldPayWithCredits = false;

            if (CurrentUserSession.Username == Config.Users.SystemUsername)
                return;

            if (Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female)
                return;

            int maxBillingPlanMessages = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Value;
            int maxLevelMessages = 0;
            if (CurrentUserSession.Level != null)
                maxLevelMessages = CurrentUserSession.Level.Restrictions.MaxMessagesPerDay;

            if (maxBillingPlanMessages == -1 || maxLevelMessages == -1)
                return;

            int maxMessages = Math.Max(maxBillingPlanMessages, maxLevelMessages);

            //if (CurrentUserSession.Username == Config.Users.SystemUsername
            //    || Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female
            //    || CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Value == -1 /*Config.Users.MembersMaxMessagesPerDay == -1*/)
            //    return;

            Message[] messages =
                Message.FetchSentMessagesForLast24Hours(CurrentUserSession.Username);


            if (messages.Length >= maxMessages)
            {
                if (!CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment &&
                    !CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.UpgradableToNextPlan)
                {
                    StatusPageMessage = Lang.Trans("You've exceeded the number of messages you can send per day!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }
                else if (CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment &&
                    CurrentUserSession.Credits >= CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits)
                {
                    shouldPayWithCredits = true;
                    // the user has enough credits
                }
                else
                {
                    string toUsername = Request.Params["to_user"];
                    bool toUsernamePaid = Classes.User.IsPaidMember(toUsername);

                    if (!(Config.Users.NonPaidMembersCanRespondToPaidMembers && toUsernamePaid && Message.MessagesExist(toUsername, CurrentUserSession.Username)))
                    {
                        Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                        Response.Redirect("Profile.aspx?sel=payment");
                        return;
                    }
                }

            }

            //if (Classes.User.IsNonPaidMember(CurrentUserSession.Username))
            //{
            //    if (messages.Length >= Config.Users.UnpaidMembersMaxMessagesPerDay)
            //    {
            //        Response.Redirect("Profile.aspx?sel=payment");
            //    }
            //}

            //if (messages.Length >= Config.Users.MembersMaxMessagesPerDay)
            //{
            //    StatusPageMessage = Lang.Trans("You've exceeded the number of messages you can send per day!");
            //    Response.Redirect("ShowStatus.aspx");
            //}

            #region checkContactedUsersLimitReached

            List<string> uniqueUsers = new List<string>();
            
            foreach (Message message in messages)
                AddUniqueItems(uniqueUsers, message.ToUser.Username);

            if (uniqueUsers.Count >= Config.Users.MaxContactedUsersPerDay)
            {
                StatusPageMessage = Lang.Trans("You've exceeded the number of users you can contact per day!");
                Response.Redirect("ShowStatus.aspx");
            }

            #endregion
        }

        private List<string> AddUniqueItems(List<string> list, string aItem)
        {
            bool found = false;
            foreach (string item in list)
            {
                if (item == aItem)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                list.Add(aItem);

            return list;
        }


        //private void ShowPreWrittenResponses()
        //{
        //    pnlUserResponse.Visible = false;
        //    ddPrewrittenMessages.Visible = false;
        //    MiscTemplates.PreWrittenMessageResponses prewrittenMessageResponsesTemplate =
        //        new MiscTemplates.PreWrittenMessageResponses(CurrentUserSession.LanguageId);
        //    string[] responses =
        //        prewrittenMessageResponsesTemplate.MessageResponses.Split('\n');

        //    foreach (string response in responses)
        //        dropPreWrittenResponses.Items.Add(response);
        //}
        
        //private void PopulatePrewrittenMessagesDropDown()
        //{
        //    MiscTemplates.PreWrittenMessageResponses prewrittenMessageResponsesTemplate =
        //        new MiscTemplates.PreWrittenMessageResponses(CurrentUserSession.LanguageId);
        //    string[] responses =
        //        prewrittenMessageResponsesTemplate.MessageResponses.Split('\n');

        //    ddPrewrittenMessages.Items.Add(new ListItem(Lang.Trans("Quick notes"), "-1"));
        //    foreach (string response in responses)
        //        ddPrewrittenMessages.Items.Add(response.Trim());
        //}

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
            this.lnkBack.Click += new EventHandler(this.lnkBack_Click);
            this.btnSend.Click += new EventHandler(this.btnSend_Click);
        }

        #endregion

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Message");
            btnSend.Text = Lang.Trans("Send Message");
            //lnkUpgradeNow.Text = Lang.Trans("Upgrade Now to Respond to this email");

            if (Request.Params["src"] != null && Request.Params["src"] == "message")
            {
                lnkBack.Text = Lang.Trans("Back to Message");
                lnkBack.CommandArgument = "message";
            }
            else if (Request.Params["src"] == "profile")
            {
                lnkBack.Text = Lang.Trans("Back to Profile");
                lnkBack.CommandArgument = "profile";
            }
            else if (Request.Params["src"] == "favorites")
            {
                lnkBack.Text = Lang.Trans("Back to Favorites");
                lnkBack.CommandArgument = "favorites";
            }
            else if (Request.Params["src"] == "friends")
            {
                lnkBack.Text = Lang.Trans("Back to Friends");
                lnkBack.CommandArgument = "friends";
            }
            else if (Request.Params["src"] == "search")
            {
                lnkBack.Text = Lang.Trans("Back to Search");
                lnkBack.CommandArgument = "search";                
            }
        }

        private void checkRequestLegit()
        {
            #region Check if message request is legit

            if (Request.Params["to_user"] == null
                || Request.Params["to_user"] == "")
            {
                Response.Redirect("~/MailBox.aspx");
                return;
            }

            string toUsername = Request.Params["to_user"];
            User toUser = null;
            try
            {
                toUser = Classes.User.Load(toUsername);
                if (toUser.Deleted)
                {
                    ((PageBase) Page).StatusPageMessage = Lang.Trans("The user no longer exists");
                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }
            }
            catch (NotFoundException)
            {
                ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists");
                Response.Redirect("~/ShowStatus.aspx");
                return;
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception err)
            {
                Log(err);
                Response.Redirect("~/MailBox.aspx");
                return;
            }
            ViewState["SendMessage_ToUsername"] = toUser.Username;

            #endregion

            #region Check can current user send message

            User fromUser = null;
            try
            {
                fromUser = Classes.User.Load(CurrentUserSession.Username);
            }
            catch(NotFoundException)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            string reason = "";
            if (!Message.MessagesExist(toUser.Username, fromUser.Username)
                && !Classes.User.CanSendMessage(fromUser, toUser, out reason)
                && !CurrentUserSession.IsAdmin())
            {
                if (!Classes.User.IsGroupOwner(fromUser, toUser))
                {
                    StatusPageMessage = reason;

                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }
            }

            #endregion
        }

        private void preparePage()
        {
            //if (Config.Users.EnablePrewrittenMessages)
            //{
            //    PopulatePrewrittenMessagesDropDown();
            //}
            //else
            //{
            //    ddPrewrittenMessages.Visible = false;
            //}
            
            string toUsername = Request.Params["to_user"];

            bool shouldPayWithCredits;
            checkMessagesLimitReached(out shouldPayWithCredits);

            if (shouldPayWithCredits)
            {
                btnSend.OnClientClick =
                    String.Format("return confirm(\"" + "Sending this message will subtract {0} credits from your balance.".Translate() + "\");",
                        CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits);
            }

            #region PaidMemberCheck


            //string username = CurrentUserSession.Username;

            //if (Classes.User.IsNonPaidMember(username))
            //{
            //    if (Classes.User.CanRespondToMail(username, toUsername))
            //    {
            //        if (Config.Users.NonPaidMembersCanUsePrewrittenResponsesOnly)
            //        {
            //            //show prewritten responses and hide message textbox
            //            ShowPreWrittenResponses();
            //        }
            //        else
            //            pnlPreWrittenResponse.Visible = false;
            //    }
            //    else
            //    {
            //        Response.Redirect("Profile.aspx?sel=payment");
            //    }
            //}
            //else if (!Config.Users.PaymentRequired && Config.Credits.Required  
            //    && Config.Users.NonPaidMembersCanUsePrewrittenResponsesOnly 
            //    && CurrentUserSession.Credits < Config.Credits.CreditsPerMessage)
            //{
            //    ShowPreWrittenResponses();
            //}
            //else pnlPreWrittenResponse.Visible = false;

            #endregion

            #region Set photo and nicknames

            User toUser = null;

            try
            {
                toUser = Classes.User.Load(toUsername);
            }
            catch (NotFoundException)
            {
                ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists");
                Response.Redirect("~/ShowStatus.aspx");
            
            }
            
            Photo primaryPhoto = null;
            try
            {
                primaryPhoto = toUser.GetPrimaryPhoto();
            }
            catch (NotFoundException)
            {
            }
            catch (Exception err)
            {
                Log(err);
            }

          /*  if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(toUser.Gender, 50, 50, "photoframe", false, true);
            }
            else
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(primaryPhoto.Id, 50, 50, "photoframe", false, true);
            }*/

            lblFromUsername.Text = CurrentUserSession.Username;
            lblToUsername.Text = toUser.Username;

            #endregion

            #region Load previous messages

            int repliedTo = -1;
            try
            {
                repliedTo = Convert.ToInt32(Request.Params["src_id"]);
            }
            catch (InvalidCastException)
            {
            }

            if (repliedTo > 0)
            {
                DataTable dtPrevMessages = new DataTable();
                dtPrevMessages.Columns.Add("Username");
                dtPrevMessages.Columns.Add("Message");

                int messageId = repliedTo;
                int counter = 0;

                while (messageId != -1 && messageId != 0 && counter <= 10)
                {
                    counter++;
                    Message message;
                    try
                    {
                        message = Message.Fetch(messageId);
                    }
                    catch (NotFoundException)
                    {
                        break;
                    }
                    if (message.fromUsername != CurrentUserSession.Username
                        && message.toUsername != CurrentUserSession.Username)
                    {
                        break;
                    }
                    if ((message.fromUsername == CurrentUserSession.Username
                         && message.FromFolder == Message.eFolder.Deleted)
                        || (message.toUsername == CurrentUserSession.Username
                            && message.ToFolder == Message.eFolder.Deleted))
                    {
                        break;
                    }

                    string body = message.Body;
                    body = Server.HtmlEncode(body);
                    body = body.Replace("\n", "<br>");
                    Smilies.Process(ref body);

                    dtPrevMessages.Rows.Add(new object[] {message.fromUsername, body});

                    messageId = message.RepliedTo;
                }

                rptPreviousMessages.DataSource = dtPrevMessages;
                rptPreviousMessages.DataBind();

                pnlPreviousMessages.Visible = true;
            }

            #endregion
        }

        /// <summary>
        /// Replaces the email.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        private static string ReplaceEmail(string input, string replacement)
        {
            string pattern = @"(([^<>()[\]\\.,;:\s@\""]+"
                               + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                               + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                               + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                               + @"[a-zA-Z]{2,}))";
            
            return Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// Replaces the phone.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        private static string ReplacePhone(string input, string replacement)
        {
            string result = input;
            string[] patterns = {
                                @"[2-9]\d{2}-\d{3}-\d{4}",                                         //Matches: 800-555-5555|||333-444-5555|||212-666-1234
                                @"((\(\d{3}\) ?)|(\d{3}[- \.]))?\d{3}[- \.]\d{4}(\s(x\d+)?){0,1}", //Matches: (123) 456-7890|||(123) 456-7890 x123
                                @"(\(\d{3}\)[- ]?|\d{3}[- ])?\d{3}[- ]\d{4}"                       //Matches: (555)555-5555|||(555) 555-5555|||555-5555
                                };
            
            foreach (string pattern in patterns)
            {
                result = Regex.Replace(result, pattern, replacement);
            }

            return result;
        }
        
        private void btnSend_Click(object sender, EventArgs e)
        {
            #region Check can current user send message

            Classes.User toUser = null;
            User fromUser = null;
            try
            {
                toUser = Classes.User.Load((string)ViewState["SendMessage_ToUsername"]);
                fromUser = Classes.User.Load(CurrentUserSession.Username);
            }
            catch (NotFoundException)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            string reason;
            if (!Message.MessagesExist(toUser.Username, fromUser.Username) &&
                    !Classes.User.CanSendMessage(fromUser, toUser, out reason) && !CurrentUserSession.IsAdmin())
            {
                if (!Classes.User.IsGroupOwner(fromUser, toUser))
                {
                    StatusPageMessage = reason;
                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }
            }

            #endregion

            try
            {
                var toUsername = (string) ViewState["SendMessage_ToUsername"];

                if (Classes.User.IsUserBlocked(toUsername, CurrentUserSession.Username))
                {
                    StatusPageMessage =
                        String.Format(Lang.Trans("You are currently blocked from sending messages to {0}"), toUsername);
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                var msg = new Message(CurrentUserSession.Username,
                                          toUsername)
                              {
                                  Body =
                                      (Config.Misc.EnableBadWordsFilterMessage
                                           ? Parsers.ProcessBadWords(txtMessageBody.Text.Trim())
                                           : txtMessageBody.Text.Trim())
                              };

                if (Config.Misc.EnableMessageFilter)
                {
                    msg.Body = ReplaceEmail(msg.Body, "***");
                    msg.Body = ReplacePhone(msg.Body, "***");    
                }

                //if (pnlPreWrittenResponse != null && pnlPreWrittenResponse.Visible)
                //{
                //    txtMessageBody.Text = dropPreWrittenResponses.SelectedItem.Text;
                //    msg.Body = dropPreWrittenResponses.SelectedItem.Text;
                //}

                if (txtMessageBody.Text.Trim().Length == 0)
                {
                    return;
                }

                if (Request.Params["src"] != null && Request.Params["src"] == "message"
                    && Request.Params["src_id"] != null)
                {
                    try
                    {
                        msg.RepliedTo = Convert.ToInt32(Request.Params["src_id"]);
                    }
                    catch (InvalidCastException)
                    {
                    }
                }

                bool shouldPayWithCredits;
                checkMessagesLimitReached(out shouldPayWithCredits);
                //if (Config.Credits.Required)
                //{
                //    if (!Config.Users.FreeForFemales || 
                //        CurrentUserSession.Gender != Classes.User.eGender.Female)
                //    {
                shouldPayWithCredits = true;
                if (shouldPayWithCredits)
                {
                    int creditsLeft = fromUser.Credits - CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits; //Config.Credits.CreditsPerMessage;
                    int price = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits;
                    Int32.TryParse(Request["txtPrice"], out price);
                    if (!Config.Credits.ChargeOneTimePerMember) // charge every time
                    {
                        if (creditsLeft < 0)
                        {
                            Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                            Response.Redirect("~/Profile.aspx?sel=payment");
                            return;
                        }

                        fromUser.Credits -= price; //CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits; //Config.Credits.CreditsPerMessage;
                        fromUser.Update(true);
                        CurrentUserSession.Credits = fromUser.Credits;
                    }
                    else
                    {
                        bool isCharged = EstablishedCommunication.Fetch(fromUser.Username,
                            toUser.Username) != null;

                        if (!isCharged)
                        {
                            if (creditsLeft >= 0)
                            {
                                var establishedCommunication =
                                    new EstablishedCommunication(fromUser.Username, toUser.Username);

                                establishedCommunication.Save();

                                fromUser.Credits -= price; //CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits;
                                fromUser.Update(true);
                                CurrentUserSession.Credits = fromUser.Credits;
                            }
                            else
                            {
                                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                                Response.Redirect("~/Profile.aspx?sel=payment");
                                return;
                            }
                        }
                    }
                }
                //    }
                //}

                if (Config.Misc.EnableSpamDetection)
                {
                    if (fromUser.SpamSuspected)
                    {
                        StatusPageMessage =
                            Lang.Trans(
                                "This account is flagged for review! You will not be able to send messages untill the review is completed!");
                        Response.Redirect("ShowStatus.aspx");
                        return;
                    } 

                    string message = Regex.Replace(msg.Body, @"[^\w]*", "").ToLower();

                    if (message.Length > 100)
                    {
                        message = message.Substring(0, 100);
                    }

                    int sentMessages = MessagesSandBox.Save(CurrentUserSession.Username, message);

                    if (sentMessages > Config.Misc.MaxSameMessages)
                    {
                        fromUser.SpamSuspected = true;
                        fromUser.Update();

                        StatusPageMessage =
                            Lang.Trans(
                                "This account is flagged for review! You will not be able to send messages until the review is completed!");
                        Response.Redirect("ShowStatus.aspx");
                        return;
                    }
                }

                msg.Send();

                Classes.User.AddScore(toUsername, Config.UserScores.ReceivedMessage, "ReceivedMessage");
                if (msg.RepliedTo > 0)
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.RepliedToMessage,
                        "RepliedToMessage");
                else
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.SentMessage,
                        "SentMessage");

                StatusPageMessage = Lang.Trans
                    ("<b>Your message has been sent successfully!</b>");
            }
            catch (ThreadAbortException)
            {
            }
            catch (NotFoundException)
            {
                StatusPageMessage = Lang.Trans("The message could not be delivered because the target user was deleted");
            }
            catch (ArgumentException ex)
            {
                StatusPageMessage = ex.Message;
            }
            catch (Exception err)
            {
                StatusPageMessage = Lang.Trans("Message could not be delivered!");
                Log(err);
            }

            Response.Redirect("ShowStatus.aspx");
        }

        private void lnkBack_Click(object sender, EventArgs e)
        {
            switch (lnkBack.CommandArgument)
            {
                case "profile":
                    Response.Redirect(UrlRewrite.CreateShowUserUrl((string)ViewState["SendMessage_ToUsername"])); 
                    break;
                case "favorites":
                    Response.Redirect("~/Favorites.aspx");
                    break;
                case "friends":
                    Response.Redirect("~/Friends.aspx");
                    break;
                case "search":
                    Response.Redirect(Config.BackwardCompatibility.UseClassicSearchPage
                                          ? "~/Search.aspx"
                                          : "~/Search2.aspx");
                    break;
                default:
                case "message":
                    if (Request.Params["src"] != null && Request.Params["src"] == "message"
                        && Request.Params["src_id"] != null)
                    {
                        try
                        {
                            int mid = Convert.ToInt32(Request.Params["src_id"]);
                            Response.Redirect("~/ShowMessage.aspx?mid=" + mid);
                        }
                        catch (InvalidCastException)
                        {
                            Response.Redirect("~/Mailbox.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Mailbox.aspx");
                    }
                    break;
            }
        }

        //private void lnkUpgradeNow_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("Profile.aspx?sel=payment");
        //}

        //protected void ddPrewrittenMessages_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddPrewrittenMessages.SelectedValue != "-1")
        //    {
        //        txtMessageBody.Text = ddPrewrittenMessages.SelectedItem.Text;
        //    }
        //}

        protected void ibtnPrevSmilies_Click(object sender, ImageClickEventArgs e)
        {
            CurrentSmiliesPage--;
            loadSmileys();
        }

        protected void ibtnNextSmilies_Click(object sender, ImageClickEventArgs e)
        {
            CurrentSmiliesPage++;
            loadSmileys();
        }
    }
}