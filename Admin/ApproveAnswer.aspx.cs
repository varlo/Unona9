﻿/* ASPnetDating 
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
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for ApproveAnswer.
    /// </summary>
    public partial class ApproveAnswer : AdminPageBase
    {
        private int questionID;
        private string username;

        private List<int> QuestionIDs
        {
            get
            {
                if (ViewState["QuestionIDs"] == null)
                    ViewState["QuestionIDs"] = new List<int>();
                return (List<int>)ViewState["QuestionIDs"];
            }
            set { ViewState["QuestionIDs"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            username = Request.Params["uid"];
            if (username == null)
                return;

            try
            {
                questionID = Convert.ToInt32(Request.Params["qid"]);
            }
            catch (Exception)
            {
                return;
            }

            Title = "User Management".TranslateA();
            Subtitle = "Approve Answer".TranslateA();
            Description = "Use this section to approve pending answers...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnReject.Enabled = false;
                    btnSaveAndApprove.Enabled = false;
                }

                LoadStrings();

                btnReject.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to reject this answer?")));

                LoadPage();
            }
        }

        private void LoadStrings()
        {
            btnSaveAndApprove.Text = Lang.TransA("Save and Approve");
            btnReject.Text = Lang.TransA("Reject");
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void LoadPage()
        {
            ProfileAnswer answer = ProfileAnswer.Fetch(username, questionID);

            if (answer != null)
            {
                lblUsername.Text = answer.User.Username;
                lblQuestion.Text = answer.Question.Name;
                txtAnswer.Text = answer.Value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.answerApproval;
            base.OnInit(e);
        }

        protected void btnSaveAndApprove_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ProfileAnswer answer = new ProfileAnswer(username, questionID);

            if (txtAnswer.Text.Trim() == String.Empty)
                btnReject_Click(null, null);
            else
                answer.Value = txtAnswer.Text;
            answer.Approved = true;
            answer.Save();

            #region Add FriendUpdatedProfile Event

            Event newEvent = new Event(username);

            newEvent.Type = Event.eType.FriendUpdatedProfile;
            UpdatedProfile updatedProfile = new UpdatedProfile();
            updatedProfile.QuestionIDs = new List<int>() { answer.Question.Id };
            newEvent.DetailsXML = Misc.ToXml(updatedProfile);

            newEvent.Save();

            string[] usernames = Classes.User.FetchMutuallyFriends(username);

            foreach (string friendUsername in usernames)
            {
                if (Config.Users.NewEventNotification)
                {
                    string text = String.Format("Your friend {0} has updated the \"{1}\" section in their profile".TranslateA(),
                                            "<b>" + username + "</b>", answer.Question.Name);
                    int imageID = 0;
                    try
                    {
                        imageID = Photo.GetPrimary(username).Id;
                    }
                    catch (NotFoundException)
                    {
                        User user = null;
                        try
                        {
                            user = Classes.User.Load(username);
                            imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }
                        catch (NotFoundException)
                        {
                            break;
                        }
                    }
                    string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                    Classes.User.SendOnlineEventNotification(username, friendUsername, text, thumbnailUrl,
                                                             UrlRewrite.CreateShowUserUrl(username));
                }
            }

            #endregion

            Response.Redirect("ApproveAnswers.aspx");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ProfileAnswer.Delete(username, questionID);
            Response.Redirect("ApproveAnswers.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ApproveAnswers.aspx");
        }
    }
}