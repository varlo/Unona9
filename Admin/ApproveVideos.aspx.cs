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
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components.Profile;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for ApproveVideos.
    /// </summary>
    public partial class ApproveVideos : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Approve Videos".TranslateA();
            Description = "Use this section to approve or reject pending videos...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!Config.Misc.EnableVideoProfile)
                {
                    StatusPageMessage = Lang.TransA("Video profile option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                LoadStrings();
                PopulateDropDown();
                PopulateDataGrid();
            }
        }

        private void PopulateDataGrid()
        {
            dgPendingVideos.PageSize = Convert.ToInt32(dropVideosPerPage.SelectedValue);
            string[] usernames = VideoProfile.FetchNonApproved();

            if (usernames.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no videos waiting for approval!"), Misc.MessageType.Error);
                dgPendingVideos.Visible = false;
                pnlVideosPerPage.Visible = false;
            }
            else
            {
                bindUsernames(usernames);

                dgPendingVideos.Visible = true;
                pnlVideosPerPage.Visible = true;
            }
        }

        private void bindUsernames(string[] usernames)
        {
            dgPendingVideos.Columns[0].HeaderText = Lang.TransA("Username");
            dgPendingVideos.Columns[1].HeaderText = Lang.TransA("Video");

            DataTable dtUsernames = new DataTable("Usernames");
            dtUsernames.Columns.Add("Username");

            foreach (string username in usernames)
                dtUsernames.Rows.Add(new object[] { username });

            dtUsernames.DefaultView.Sort = "Username";

            dgPendingVideos.DataSource = dtUsernames;
            try
            {
                dgPendingVideos.DataBind();
            }
            catch (HttpException)
            {
                dgPendingVideos.CurrentPageIndex = 0;
                dgPendingVideos.DataBind();
            }
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropVideosPerPage.Items.Add(i.ToString());
            dropVideosPerPage.SelectedValue = 5.ToString();
        }

        private void LoadStrings()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.videoApproval;
            base.OnInit(e);
        }

        protected void gridPendingVideos_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            VideoRecorder vr = (VideoRecorder)e.Item.FindControl("VideoRecorder1");

            vr.AdminUser = true;
            vr.strTargetUsername = (string)DataBinder.Eval(e.Item.DataItem, "Username");

            Button btnApprove = (Button)e.Item.FindControl("btnApprove");
            btnApprove.Text = Lang.TransA("Approve");
            btnApprove.CommandName = "approve";
            btnApprove.CommandArgument = (string)DataBinder.Eval(e.Item.DataItem, "Username");

            DeleteButton deleteButton = (DeleteButton)e.Item.FindControl("DeleteButton");
            deleteButton.strTargetUsername = (string)DataBinder.Eval(e.Item.DataItem, "Username");

            if (!HasWriteAccess)
            {
                btnApprove.Enabled = false;
                deleteButton.Visible = false;
            }
        }

        protected void dgPendingVideos_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (e.CommandName == "approve")
            {
                VideoProfile.SetApproved((string)e.CommandArgument);
                PopulateDataGrid();
            }
        }

        protected void dgPendingVideos_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingVideos.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid();
        }

        protected void dropVideosPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingVideos.PageSize = Convert.ToInt32(dropVideosPerPage.SelectedValue);
            dgPendingVideos.CurrentPageIndex = 0;
            PopulateDataGrid();
        }
    }
}