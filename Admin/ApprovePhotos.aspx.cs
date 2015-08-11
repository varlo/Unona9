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
using System.Web;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ApprovePhotos : AdminPageBase
    {
        public ApprovePhotos()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Approve Photos".TranslateA();
            Description = "Use this section to approve or reject pending photos...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess) { }
                LoadStrings();
                PopulateDropDown();
                PopulateDataGrid(false);
            }
        }

        private void LoadStrings()
        {
            lblPhotosPerPage.Text = Lang.TransA("Photos per page");
            if (Config.CommunityModeratedSystem.EnableCommunityPhotoApproval)
            {
                cbShowManualApprovalPhotos.Text = Lang.TransA("Show only photos marked for manual approval");
                cbShowManualApprovalPhotos.Visible = true;
            }
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropPhotosPerPage.Items.Add(i.ToString());
            dropPhotosPerPage.SelectedValue = Config.AdminSettings.ApprovePhotos.PhotosPerPage.ToString();
        }

        private void PopulateDataGrid(bool manualApproved)
        {
            gridPendingApproval.PageSize = Convert.ToInt32(dropPhotosPerPage.SelectedValue);
            Photo[] photos = null;
            if (manualApproved)
            {
                photos = Photo.FetchManualApproved();
            }
            else if (!Config.CommunityFaceControlSystem.EnableCommunityFaceControl)
            {
                photos = Photo.FetchNonApproved();
            }
            else
            {
                photos = Photo.FetchNonApproved(true);
            }

            if (photos == null || photos.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no photos waiting for approval!"), Misc.MessageType.Error);
                gridPendingApproval.Visible = false;
                pnlPhotosPerPage.Visible = false;
            }
            else
            {
                bindPhotoDetails(photos);

                gridPendingApproval.Visible = true;
                pnlPhotosPerPage.Visible = true;
            }
        }

        private void bindPhotoDetails(Photo[] photos)
        {
            gridPendingApproval.Columns[0].HeaderText = Lang.TransA("Username");
            gridPendingApproval.Columns[1].HeaderText = Lang.TransA("Photo Name");
            gridPendingApproval.Columns[2].HeaderText = Lang.TransA("Photo Description");
            gridPendingApproval.Columns[3].HeaderText = Lang.TransA("Photo");

            DataTable dtPhotoDetails = new DataTable("PhotoDetails");
            dtPhotoDetails.Columns.Add("Username");
            dtPhotoDetails.Columns.Add("Name");
            dtPhotoDetails.Columns.Add("Description");
            dtPhotoDetails.Columns.Add("PhotoID");

            foreach (Photo photo in photos)
            {
                if (photo.Name == "")
                    photo.Name = Lang.TransA("N/A");

                if (photo.Description == "")
                    photo.Description = Lang.TransA("N/A");

                dtPhotoDetails.Rows.Add(new object[]
                                            {
                                                photo.User.Username,
                                                photo.Name,
                                                photo.Description.Length > 50
                                                    ? photo.Description.Substring(0, 50) + "..."
                                                    : photo.Description,
                                                photo.Id
                                            }
                    );
            }

            dtPhotoDetails.DefaultView.Sort = "Username";

            gridPendingApproval.DataSource = dtPhotoDetails;
            try
            {
                gridPendingApproval.DataBind();
            }
            catch (HttpException)
            {
                gridPendingApproval.CurrentPageIndex = 0;
                gridPendingApproval.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {

            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.photoApproval;
            base.OnInit(e);
        }

        protected void gridPendingApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            gridPendingApproval.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid(cbShowManualApprovalPhotos.Checked);
        }

        protected void dropPhotosPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridPendingApproval.PageSize = Convert.ToInt32(dropPhotosPerPage.SelectedValue);
            gridPendingApproval.CurrentPageIndex = 0;
            PopulateDataGrid(cbShowManualApprovalPhotos.Checked);
        }

        protected void cbShowManualApprovalPhotos_CheckedChanged(object sender, EventArgs e)
        {
            PopulateDataGrid(cbShowManualApprovalPhotos.Checked);
        }
    }
}