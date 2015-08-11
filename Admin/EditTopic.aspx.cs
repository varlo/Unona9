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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditTopic.
    /// </summary>
    public partial class EditTopic : AdminPageBase
    {
        #region Properties

        private string TopicID
        {
            get
            {
                string param = Request.Params["tid"];

                if (param == null || param.Trim() == String.Empty)
                    return null;
                else
                {
                    foreach (char ch in param)
                        if (Char.IsLetter(ch))
                            return null;
                }

                return param;
            }
        }

        private DataTable DataSource
        {
            get { return ViewState["TopicQuestionsDataSource"] as DataTable; }
            set { ViewState["TopicQuestionsDataSource"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Edit Topics".TranslateA();
            Subtitle = "Edit Topic".TranslateA();
            Description = "Here you can add new questions for selected topic or modify existing ones...".TranslateA();

            if (TopicID == null)
                return;

            LoadStrings();
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddNewQuestion.Enabled = false;
                    btnDeleteSelectedQuestions.Enabled = false;
                    btnSave.Enabled = false;
                }

                PopulateDataFields();
                PopulateDataGrid();
            }

            if (Request.Params["new"] != null)
                ClearTempTopic();
            else
                LoadTempTopic();
        }

        private void LoadStrings()
        {
            btnAddNewQuestion.Text = "<i class=\"fa fa-plus\"></i>&nbsp;" + Lang.TransA("Add new question");
            btnCancel.Text = Lang.TransA("Cancel");
            btnSave.Text = Lang.TransA("Save");

            for (int i = 0; i < Config.Profiles.MaxTopicColumns; )
            {
                ++i;
                dropEditColumns.Items.Add(Convert.ToString(i));
                dropViewColumns.Items.Add(Convert.ToString(i));
            }

            dgQuestions.Columns[1].HeaderText = Lang.TransA("Name");
            dgQuestions.Columns[2].HeaderText = Lang.TransA("Description");
            dgQuestions.Columns[3].HeaderText = Lang.TransA("Required");
            dgQuestions.Columns[4].HeaderText = Lang.TransA("Order");

            btnDeleteSelectedQuestions.Text = "<i class=\"fa fa-trash-o\"></i>&nbsp;" + Lang.TransA("Delete selected questions");
            btnDeleteSelectedQuestions.Attributes.Add("onclick",
                                                      String.Format("javascript: return confirm('{0}')",
                                                                    Lang.TransA(
                                                                        "Do you really want to delete selected questions?")));
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.topicsQuestions;
            base.OnInit(e);
        }

        private void PopulateDataFields()
        {
            ProfileTopic topic;
            topic = ProfileTopic.Fetch(Convert.ToInt32(TopicID));
            txtTopicTitle.Text = topic.Name;
            dropEditColumns.SelectedValue = topic.EditColumns.ToString();
            dropViewColumns.SelectedValue = topic.ViewColumns.ToString();
        }

        private void PopulateDataGrid()
        {
            ProfileQuestion[] questions;

            int topicID = Convert.ToInt32(TopicID);
            questions = ProfileQuestion.FetchByTopicID(topicID);

            if (questions == null)
            {
                Master.MessageBox.Show(
                    Lang.TransA(
                        "There are no existing questions! Please click on \"Add new question\" to create new one."),
                    Misc.MessageType.Error);
                dgQuestions.Visible = false;
            }
            else
            {
                BindQuestionDetails(questions);
                dgQuestions.Visible = true;
            }
        }

        private void BindQuestionDetails(ProfileQuestion[] questions)
        {
            DataTable dtQuestions = new DataTable("Questions");
            dtQuestions.Columns.Add("QuestionID");
            dtQuestions.Columns.Add("Name");
            dtQuestions.Columns.Add("Description");
            dtQuestions.Columns.Add("Required", typeof(Boolean));

            foreach (ProfileQuestion question in questions)
            {
                dtQuestions.Rows.Add(new object[]
                                         {
                                             question.Id,
                                             question.Name,
                                             question.Description,
                                             question.Required
                                         }
                    );
            }

            DataSource = dtQuestions;

            dgQuestions.DataSource = dtQuestions;
            dgQuestions.DataBind();
        }

        protected void dgQuestions_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            //remove the upper arrow if the current item is the first one
            if (e.Item.ItemIndex == 0)
            {
                (e.Item.FindControl("lnkUp") as LinkButton).Visible = false;
            }

            //remove the lower arrow if the current item is the last one
            int lastItemIndex = DataSource.Rows.Count - 1;
            if (e.Item.ItemIndex == lastItemIndex)
                (e.Item.FindControl("lnkDown") as LinkButton).Visible = false;
        }

        protected void dgQuestions_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)e.Item.FindControl("cbSelect");
            int questionID = Convert.ToInt32(cbSelect.Value);

            switch (e.CommandName)
            {
                case "EditQuestion":
                    EditQuestion(e, questionID);
                    break;
                case "ChangeOrder":
                    if (!HasWriteAccess)
                        return;
                    ChangeOrder(e, questionID);
                    PopulateDataGrid();
                    break;
            }
        }

        private void EditQuestion(DataGridCommandEventArgs e, int questionID)
        {
            SaveTempTopic();
            string url = String.Format("EditQuestion.aspx?qid={0}", questionID);
            Response.Redirect(url);
        }

        private void SaveTempTopic()
        {
            ProfileTopic topic = new ProfileTopic(Convert.ToInt32(TopicID));
            topic.Name = txtTopicTitle.Text;
            topic.EditColumns = Convert.ToInt32(dropEditColumns.SelectedValue);
            topic.ViewColumns = Convert.ToInt32(dropViewColumns.SelectedValue);

            Session["TempTopic"] = topic;
        }

        private void LoadTempTopic()
        {
            ProfileTopic topic = Session["TempTopic"] as ProfileTopic;
            if (topic != null)
            {
                txtTopicTitle.Text = topic.Name;
                dropEditColumns.SelectedValue = topic.EditColumns.ToString();
                dropViewColumns.SelectedValue = topic.ViewColumns.ToString();
            }
        }

        private void ClearTempTopic()
        {
            Session["TempTopic"] = null;
        }

        private void ChangeOrder(DataGridCommandEventArgs e, int questionID)
        {
            string direction = (string)(e.CommandArgument);

            switch (direction)
            {
                case "Up":
                    ProfileQuestion.ChangeOrder(Convert.ToInt32(TopicID), questionID, eDirections.Up);
                    break;
                case "Down":
                    ProfileQuestion.ChangeOrder(Convert.ToInt32(TopicID), questionID, eDirections.Down);
                    break;
            }
        }

        protected void btnAddNewQuestion_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ProfileQuestion question = new ProfileQuestion();
            question.Name = "NewName";
            question.AltName = "New alternative name";
            question.Description = "New description";
            question.Hint = "New hint";
            question.EditStyle = ProfileQuestion.eEditStyle.Hidden;
            question.ShowStyle = ProfileQuestion.eShowStyle.Hidden;
            question.SearchStyle = ProfileQuestion.eSearchStyle.Hidden;
            question.Required = true;
            question.TopicID = Convert.ToInt32(TopicID);

            question.Save();
            PopulateDataGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ProfileTopic topic = new ProfileTopic(Convert.ToInt32(TopicID));
            topic.Name = txtTopicTitle.Text;
            topic.EditColumns = Convert.ToInt32(dropEditColumns.SelectedValue);
            topic.ViewColumns = Convert.ToInt32(dropViewColumns.SelectedValue);
            topic.Save();
            ClearTempTopic();
            Response.Redirect("EditTopics.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearTempTopic();
            Response.Redirect("EditTopics.aspx");
        }

        protected void btnDeleteSelectedQuestions_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            foreach (DataGridItem item in dgQuestions.Items)
            {
                HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)item.FindControl("cbSelect");
                if (cbSelect.Checked)
                {
                    int questionID = Convert.ToInt32(cbSelect.Value);
                    ProfileQuestion.Delete(questionID);

                    ProfileQuestion[] questions = null;
                    try
                    {
                        questions = ProfileTopic.Fetch(Convert.ToInt32(TopicID)).FetchQuestions();
                        if (questions != null)
                        {
                            ProfileQuestion[] childQuestions =
                                questions.Where(
                                    q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == questionID).ToArray();
                            if (childQuestions.Length > 0) // this question is parent
                            {
                                foreach (ProfileQuestion childQuestion in childQuestions)
                                {
                                    childQuestion.ParentQuestionID = null;
                                    childQuestion.ParentQuestionChoices = null;
                                    childQuestion.Save();
                                }
                            }
                        }
                    }
                    catch (NotFoundException)
                    {
                    }
                }
            }
            PopulateDataGrid();
        }

        protected void dgQuestions_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkUp = e.Item.FindControl("lnkUp") as LinkButton;
            LinkButton lnkDown = e.Item.FindControl("lnkDown") as LinkButton;

            if (!HasWriteAccess)
            {
                if (lnkUp != null)
                    lnkUp.Enabled = false;

                if (lnkDown != null)
                    lnkDown.Enabled = false;
            }
        }
    }
}