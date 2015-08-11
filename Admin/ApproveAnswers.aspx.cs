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
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for ApproveAnswers.
    /// </summary>
    public partial class ApproveAnswers : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Approve Answers".TranslateA();
            Description = "Use this section to approve or reject pending answers that require approval...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {

                }
                LoadStrings();
                PopulateDropDown();
                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            lblAnswersPerPage.Text = Lang.TransA("Answers per page");
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropAnswersPerPage.Items.Add(i.ToString());
            dropAnswersPerPage.SelectedValue = Config.AdminSettings.ApproveAnswers.AnswersPerPage.ToString();
        }

        private void PopulateDataGrid()
        {
            dgPendingApproval.PageSize = Convert.ToInt32(dropAnswersPerPage.SelectedValue);

            ProfileAnswer[] answers = ProfileAnswer.FetchNonApproved();

            if (answers.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no profile answers waiting for approval!"), Misc.MessageType.Error);
                dgPendingApproval.Visible = false;
                lblAnswersPerPage.Visible = false;
                dropAnswersPerPage.Visible = false;
            }
            else
            {
                bindAnswers(answers);

                //lblMessage.Visible = false;
                dgPendingApproval.Visible = true;
                lblAnswersPerPage.Visible = true;
                dropAnswersPerPage.Visible = true;
            }
        }

        private void bindAnswers(ProfileAnswer[] answers)
        {
            dgPendingApproval.Columns[0].HeaderText = Lang.TransA("Username");
            dgPendingApproval.Columns[1].HeaderText = Lang.TransA("Question");
            dgPendingApproval.Columns[2].HeaderText = Lang.TransA("Answer");

            DataTable dtAnswers = new DataTable("Answers");
            dtAnswers.Columns.Add("Username");
            dtAnswers.Columns.Add("Question");
            dtAnswers.Columns.Add("Answer");
            dtAnswers.Columns.Add("QuestionID");

            foreach (ProfileAnswer answer in answers)
            {
                dtAnswers.Rows.Add(new object[]
                                       {
                                           answer.User.Username,
                                           answer.Question.Name,
                                           answer.Value.Length > 300
                                               ? Server.HtmlEncode(answer.Value.Substring(0, 300)) + "..."
                                               : Server.HtmlEncode(answer.Value),
                                           answer.Question.Id
                                       }
                    );
            }

            dtAnswers.DefaultView.Sort = "Username";

            dgPendingApproval.DataSource = dtAnswers;
            try
            {
                dgPendingApproval.DataBind();
            }
            catch (HttpException)
            {
                dgPendingApproval.CurrentPageIndex = 0;
                dgPendingApproval.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.answerApproval;
            base.OnInit(e);
        }

        protected void dgPendingApproval_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (e.CommandName == "Approve")
            {
                string[] parameters = ((string)e.CommandArgument).Split(':');
                if (parameters.Length == 2)
                {
                    ProfileAnswer answer = ProfileAnswer.Fetch(parameters[0], Convert.ToInt32(parameters[1]));
                    answer.Approved = true;
                    answer.Save();

                    #region Add FriendUpdatedProfile Event

                    Event newEvent = new Event(parameters[0]);

                    newEvent.Type = Event.eType.FriendUpdatedProfile;
                    UpdatedProfile updatedProfile = new UpdatedProfile();
                    updatedProfile.QuestionIDs = new List<int>() { answer.Question.Id };
                    newEvent.DetailsXML = Misc.ToXml(updatedProfile);

                    newEvent.Save();

                    string[] usernames = Classes.User.FetchMutuallyFriends(parameters[0]);

                    foreach (string friendUsername in usernames)
                    {
                        if (Config.Users.NewEventNotification)
                        {
                            string text = String.Format("Your friend {0} has updated the \"{1}\" section in their profile".TranslateA(),
                                            "<b>" + parameters[0] + "</b>", answer.Question.Name);
                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(parameters[0]).Id;
                            }
                            catch (NotFoundException)
                            {
                                User user = null;
                                try
                                {
                                    user = Classes.User.Load(parameters[0]);
                                    imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                }
                                catch (NotFoundException)
                                {
                                    continue;
                                }
                            }
                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            Classes.User.SendOnlineEventNotification(parameters[0], friendUsername, text, thumbnailUrl,
                                                                     UrlRewrite.CreateShowUserUrl(parameters[0]));
                        }
                    }

                    #endregion

                    PopulateDataGrid();
                }
            }
            else if (e.CommandName == "Reject")
            {
                string[] parameters = ((string)e.CommandArgument).Split(':');
                if (parameters.Length == 2)
                {
                    ProfileAnswer.Delete(parameters[0], Convert.ToInt32(parameters[1]));
                    PopulateDataGrid();
                }
            }
        }

        protected void dgPendingApproval_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");

            lnkReject.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to reject this answer?")));
        }

        protected void dgPendingApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingApproval.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid();
        }

        protected void dropAnswersPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingApproval.PageSize = Convert.ToInt32(dropAnswersPerPage.SelectedValue);
            dgPendingApproval.CurrentPageIndex = 0;
            PopulateDataGrid();
        }

        protected void dgPendingApproval_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkApprove = (LinkButton)e.Item.FindControl("lnkApprove");
            LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");

            if (!HasWriteAccess)
            {
                lnkApprove.Enabled = false;
                lnkReject.Enabled = false;
            }
        }
    }
}