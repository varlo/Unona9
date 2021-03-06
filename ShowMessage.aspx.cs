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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    public partial class ShowMessage : PageBase
    {
        protected SmallBoxStart SmallBoxStart1;
        protected LargeBoxStart LargeBoxStart1;

        public ShowMessage()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.SetSuppressLinkSelection();
                ReportAbuse1.Visible = false;
                loadMessage();
                loadStrings();

                if (Config.ThirdPartyServices.UseBingTranslate)
                {
                    var headerValue = BingTranslator.GetHeaderValue();
                    if (headerValue != null)
                    {
                        Page.RegisterJQuery();
                        Master.ScriptManager.CompositeScript.Scripts.Add(
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
                        apiKey: '" + headerValue + @"',
                        destination: dest
                    },
                    function(response) {
                        obj.html(response.translation);
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
                        divTranslate.Visible = true;
                    }
                }
            }
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Message");
            lnkReply.Text = Lang.Trans("Reply");
            lnkBack.Text = Lang.Trans("Back to Mailbox");
            lnkBlockUser.Text = Lang.Trans("Block this User");
            lnkUnblockUser.Text = Lang.Trans("Unblock this User");
            lnkGrantAccess.Text = Lang.Trans("Grant Access to my Private Photos");
            lnkDenyAccess.Text = Lang.Trans("Revoke Access to my Private Photos");
            lnkReportAbuse.Text = Lang.Trans("Report Abuse");
            lnkDelete.Text = "Delete".Translate();
        }

        private void loadMessage()
        {

            #region Check if message request is legit

            if (Request.Params["mid"] == null
                || Request.Params["mid"] == "")
            {
                Response.Redirect("~/MailBox.aspx");
            }

            int messageId = 0;
            try
            {
                messageId = Convert.ToInt32(Request.Params["mid"]);
            }
            catch
            {
                // Invalid Message ID
                Response.Redirect("~/MailBox.aspx");
            }

            Message message = null;
            try
            {
                message = Message.Fetch(messageId);
            }
            catch (NotFoundException)
            {
                // Message does not exist
                Response.Redirect("~/MailBox.aspx");
            }
            catch (Exception err)
            {
                Log(err);
                Response.Redirect("~/MailBox.aspx");
            }

            if (message.FromUser.Username != CurrentUserSession.Username
                && message.ToUser.Username != CurrentUserSession.Username)
            {
                // User is neither the sender nor the recipient
                Response.Redirect("~/MailBox.aspx");
            }

            #endregion

            if (!message.IsRead && 
                message.fromUsername != Config.Users.SystemUsername &&
                message.fromUsername != CurrentUserSession.Username)
            {
                var result = CurrentUserSession.CanReadEmail();

                if (result == PermissionCheckResult.YesButMoreCreditsNeeded ||
                    result == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanReadEmail;
                    Response.Redirect("~/Profile.aspx?sel=payment");
                    return;
                }

                if (result == PermissionCheckResult.No)
                {
                    StatusPageMessage = "You are not allowed to read emails".Translate();
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                if (result == PermissionCheckResult.YesWithCredits)
                {
                    var toUser = message.ToUser;
                    toUser.Credits -= CurrentUserSession.BillingPlanOptions.CanReadEmail.Credits;
                    toUser.Update(true);
                    CurrentUserSession.Credits = toUser.Credits;
                }
            }

            #region Disable reply button if the sender is a system user

            foreach (string systemUser in Config.Users.ReservedUsernames)
            {
                if (message.FromUser.Username.ToLower() == systemUser)
                    lnkReply.Enabled = false;
            }

            #endregion

            #region Show message

            Photo primaryPhoto = null;
            try
            {
                primaryPhoto = message.ToUser.Username == CurrentUserSession.Username
                                   ?
                               message.FromUser.GetPrimaryPhoto()
                                   : message.ToUser.GetPrimaryPhoto();
            }
            catch (NotFoundException)
            {
            }
            catch (Exception err)
            {
                Log(err);
            }

            /*if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                var gender = message.ToUser.Username == CurrentUserSession.Username ? message.FromUser.Gender : message.ToUser.Gender;
                ltrPhoto.Text =
                    ImageHandler.RenderImageTag(gender, 50, 50, "photoframe", false, true);
            }
            else
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(primaryPhoto.Id, 50, 50, "photoframe", false, true);
            }*/

            ViewState["ShowMessage_ReplyTo"] = message.ToUser.Username == CurrentUserSession.Username
                                                   ?
                                               message.FromUser.Username
                                                   : message.ToUser.Username;
            ViewState["ShowMessage_MessageId"] = message.Id;
            lblFromUsername.Text = message.FromUser.Username;
            lblToUsername.Text = message.ToUser.Username;
            lblMessageTime.Text = message.Timestamp.Add(Config.Misc.TimeOffset).ToString();
           /* lnkShowUser.HRef = UrlRewrite.CreateShowUserUrl(message.ToUser.Username == CurrentUserSession.Username
                                                 ? message.FromUser.Username
                                                 : message.ToUser.Username); */

            string body = message.Body;
            Parsers.ProcessMessage(ref body);
            lblMessage.Text = body;

            #endregion

            #region Load previous messages

            int repliedTo = message.RepliedTo;
            
            if (repliedTo > 0)
            {
                DataTable dtPrevMessages = new DataTable();
                dtPrevMessages.Columns.Add("Username");
                dtPrevMessages.Columns.Add("Message");

                int messId = repliedTo;
                int counter = 0;

                while (messId != -1 && messId != 0 && counter <= 10)
                {
                    counter++;
                    Message msg;
                    try
                    {
                        msg = Message.Fetch(messId);
                    }
                    catch (NotFoundException)
                    {
                        break;
                    }
                    if (msg.fromUsername != CurrentUserSession.Username
                        && msg.toUsername != CurrentUserSession.Username)
                    {
                        break;
                    }
                    if ((msg.fromUsername == CurrentUserSession.Username
                         && msg.FromFolder == Message.eFolder.Deleted)
                        || (msg.toUsername == CurrentUserSession.Username
                            && msg.ToFolder == Message.eFolder.Deleted))
                    {
                        break;
                    }

                    string messageBody = msg.Body;
                    messageBody = Server.HtmlEncode(messageBody);
                    messageBody = messageBody.Replace("\n", "<br>");
                    Smilies.Process(ref messageBody);

                    dtPrevMessages.Rows.Add(new object[] { msg.fromUsername, messageBody });
                   
                    messId = msg.RepliedTo;
                }

                rptPreviousMessages.DataSource = dtPrevMessages;
                rptPreviousMessages.DataBind();

                pnlPreviousMessages.Visible = true;
            }


            #endregion

            #region Mark message as read if necessary

            if (!message.IsRead && message.ToUser.Username == CurrentUserSession.Username)
            {
                message.IsRead = true;
            }

            #endregion

            #region Set "block/unlblock user" links

            if (CurrentUserSession.IsUserBlocked((string) ViewState["ShowMessage_ReplyTo"]))
                pnlBlockUser.Visible = false;
            else
                pnlUnblockUser.Visible = false;

            #endregion

            #region set "allow/disallow user to view your private photos" links

            if (Config.Photos.EnablePrivatePhotos &&
                CurrentUserSession != null &&
                CurrentUserSession.HasPrivatePhotos())
            {
                if (CurrentUserSession.HasUserAccessToPrivatePhotos((string) ViewState["ShowMessage_ReplyTo"]))
                    pnlGrantAccessToPrivatePhotos.Visible = false;
                else
                    pnlDenyAccessToPrivatePhotos.Visible = false;
            }
            else
            {
                pnlGrantAccessToPrivatePhotos.Visible = false;
                pnlDenyAccessToPrivatePhotos.Visible = false;
            }

            #endregion

            #region show/hide ReportAbuse link
            if (Config.AbuseReports.UserCanReportMessageAbuse
                && (CurrentUserSession.BillingPlanOptions.UserCanReportAbuse.Value
                || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanReportAbuse))
                lnkReportAbuse.Visible = true;
            else
                lnkReportAbuse.Visible = false;
            #endregion
        }

        protected void lnkReply_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SendMessage.aspx?to_user=" + ViewState["ShowMessage_ReplyTo"]
                              + "&src=message&src_id=" + ViewState["ShowMessage_MessageId"]);
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx");
        }

        protected void lnkBlockUser_Click(object sender, EventArgs e)
        {
            CurrentUserSession.BlockUser((string) ViewState["ShowMessage_ReplyTo"]);
            pnlBlockUser.Visible = false;
            pnlUnblockUser.Visible = true;
        }

        protected void lnkUnblockUser_Click(object sender, EventArgs e)
        {
            CurrentUserSession.UnblockUser((string) ViewState["ShowMessage_ReplyTo"]);
            pnlBlockUser.Visible = true;
            pnlUnblockUser.Visible = false;
        }

        protected void lnkGrantAccess_Click(object sender, EventArgs e)
        {
            CurrentUserSession.SetAccessToPrivatePhotos((string) ViewState["ShowMessage_ReplyTo"], true);

            pnlDenyAccessToPrivatePhotos.Visible = true;
            pnlGrantAccessToPrivatePhotos.Visible = false;
        }

        protected void lnkDenyAccess_Click(object sender, EventArgs e)
        {
            CurrentUserSession.SetAccessToPrivatePhotos((string) ViewState["ShowMessage_ReplyTo"], false);

            pnlDenyAccessToPrivatePhotos.Visible = false;
            pnlGrantAccessToPrivatePhotos.Visible = true;
        }

        protected void lnkReportAbuse_Click(object sender, EventArgs e)
        {
            ReportAbuse1.Visible = true;
            ReportAbuse1.ReportedUser = (string) ViewState["ShowMessage_ReplyTo"];
            ReportAbuse1.ReportType = AbuseReport.ReportType.Message;
            ReportAbuse1.TargetID = (int?)ViewState["ShowMessage_MessageId"];
            ReportAbuse1.Text = Lang.Trans("Please tell us why you are reporting this user message");
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            #region Check if message request is legit

            if (Request.Params["mid"] == null
                || Request.Params["mid"] == "")
            {
                Response.Redirect("~/MailBox.aspx");
            }

            int messageId = 0;
            try
            {
                messageId = Convert.ToInt32(Request.Params["mid"]);
            }
            catch
            {
                // Invalid Message ID
                Response.Redirect("~/MailBox.aspx");
            }

            Message message = null;
            try
            {
                message = Message.Fetch(messageId);
            }
            catch (NotFoundException)
            {
                // Message does not exist
                Response.Redirect("~/MailBox.aspx");
            }
            catch (Exception err)
            {
                Log(err);
                Response.Redirect("~/MailBox.aspx");
            }

            if (message.FromUser.Username != CurrentUserSession.Username
                && message.ToUser.Username != CurrentUserSession.Username)
            {
                // User is neither the sender nor the recipient
                Response.Redirect("~/MailBox.aspx");
            }

            #endregion

            if (message.FromUser.Username == CurrentUserSession.Username)
            {
                message.FromFolder = message.FromFolder == Message.eFolder.Trash
                                         ? Message.eFolder.Deleted
                                         : Message.eFolder.Trash;
            }
            if (message.ToUser.Username == CurrentUserSession.Username)
            {
                message.ToFolder = message.ToFolder == Message.eFolder.Trash
                                       ? Message.eFolder.Deleted
                                       : Message.eFolder.Trash;
            }

            Response.Redirect("~/Mailbox.aspx");
        }
    }
}