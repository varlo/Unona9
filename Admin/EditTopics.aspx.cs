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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditTopics.
    /// </summary>
    public partial class EditTopics : AdminPageBase
    {
        #region Properties

        private DataTable DataSource
        {
            get { return ViewState["ProfileTopicsDataSource"] as DataTable; }
            set { ViewState["ProfileTopicsDataSource"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Site Management".TranslateA();
            Subtitle = "Edit Topics".TranslateA();
            Description = "In this section you can create new topics or modify existing ones...".TranslateA();

            LoadStrings();
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddNewTopic.Enabled = false;
                    btnDeleteSelectedTopics.Enabled = false;
                }

                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            dgTopics.Columns[1].HeaderText = Lang.TransA("Title");
            dgTopics.Columns[2].HeaderText = Lang.TransA("EditColumns");
            dgTopics.Columns[3].HeaderText = Lang.TransA("ViewColumns");
            dgTopics.Columns[4].HeaderText = Lang.TransA("Order");

            btnAddNewTopic.Text = "<i class=\"fa fa-plus\"></i>&nbsp;" + Lang.TransA("Add new topic");
            btnDeleteSelectedTopics.Text = "<i class=\"fa fa-trash-o\"></i>&nbsp;" + Lang.TransA("Delete selected topic");
            btnDeleteSelectedTopics.Attributes.Add("onclick",
                                                   String.Format("javascript: return confirm('{0}')",
                                                                 Lang.TransA(
                                                                     "Do you really want to delete selected topics?")));
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.topicsQuestions;
            base.OnInit(e);
        }

        private void PopulateDataGrid()
        {
            ProfileTopic[] topics = ProfileTopic.Fetch();

            if (topics == null)
            {
                Master.MessageBox.Show(
                    Lang.TransA("There are no existing topics! Please click on \"Add new topic\" to create new one."),
                    Misc.MessageType.Error);
                dgTopics.Visible = false;
            }
            else
            {
                BindTopicDetails(topics);

                dgTopics.Visible = true;
            }
        }

        private void BindTopicDetails(ProfileTopic[] topics)
        {
            DataTable dtTopics = new DataTable("Topics");
            dtTopics.Columns.Add("TopicID");
            dtTopics.Columns.Add("Title");
            dtTopics.Columns.Add("EditColumns", typeof(int));
            dtTopics.Columns.Add("ViewColumns", typeof(int));

            foreach (ProfileTopic topic in topics)
            {
                dtTopics.Rows.Add(new object[]
                                      {
                                          topic.ID,
                                          topic.Name,
                                          topic.EditColumns,
                                          topic.ViewColumns
                                      }
                    );
            }

            DataSource = dtTopics;

            dgTopics.DataSource = dtTopics;
            dgTopics.DataBind();
        }

        protected void dgTopics_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)e.Item.FindControl("cbSelect");
            int topicID = Convert.ToInt32(cbSelect.Value);

            switch (e.CommandName)
            {
                case "EditTopic":
                    EditTopic(e, topicID);
                    break;
                case "ChangeOrder":
                    if (!HasWriteAccess)
                        return;
                    ChangeOrder(e, topicID);
                    PopulateDataGrid();
                    break;
            }
        }

        private void EditTopic(DataGridCommandEventArgs e, int topicID)
        {
            string url = String.Format("EditTopic.aspx?tid={0}&new=1", topicID);
            Response.Redirect(url);
        }

        private void ChangeOrder(DataGridCommandEventArgs e, int topicID)
        {
            string direction = (string)(e.CommandArgument);

            switch (direction)
            {
                case "Up":
                    ProfileTopic.ChangeOrder(topicID, eDirections.Up);
                    break;
                case "Down":
                    ProfileTopic.ChangeOrder(topicID, eDirections.Down);
                    break;
            }
        }

        protected void dgTopics_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            //remove the upper arrow if the current item is the first one
            if (e.Item.ItemIndex == 0)
            {
                (e.Item.FindControl("lnkUp") as LinkButton).Visible = false;
                //return;
            }

            //remove the lower arrow if the current item is the last one
            int lastItemIndex = DataSource.Rows.Count - 1;
            if (e.Item.ItemIndex == lastItemIndex)
                (e.Item.FindControl("lnkDown") as LinkButton).Visible = false;
        }

        protected void btnAddNewTopic_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            ProfileTopic topic = new ProfileTopic();
            topic.Name = "NewTopic";
            topic.EditColumns = 1;
            topic.ViewColumns = 1;
            topic.Save();
            PopulateDataGrid();
        }

        protected void btnDeleteSelectedTopics_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            foreach (DataGridItem item in dgTopics.Items)
            {
                HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)item.FindControl("cbSelect");
                if (cbSelect.Checked)
                {
                    int topicID = Convert.ToInt32(cbSelect.Value);
                    ProfileTopic.Delete(topicID);
                }
            }
            PopulateDataGrid();
        }

        protected void dgTopics_ItemDataBound(object sender, DataGridItemEventArgs e)
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