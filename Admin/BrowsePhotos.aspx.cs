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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BrowsePhotos : AdminPageBase
    {
        private int PhotosPerPage
        {
            get { return Convert.ToInt32(dropPhotosPerPage.SelectedValue); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Browse Photos".TranslateA();
            Description = "Use this section to browse user photos...".TranslateA();

            if (!Page.IsPostBack)
            {
                LoadStrings();
                PopulateFilters();
                PreparePaginator();
            }
        }

        private void PopulateFilters()
        {
            for (int i = 5; i <= 50; i += 5)
                dropPhotosPerPage.Items.Add(i.ToString());
            dropPhotosPerPage.SelectedValue = Config.AdminSettings.BrowsePhotos.PhotosPerPage.ToString();

            ddPrimary.Items.Add(Lang.TransA("Yes"));
            ddPrimary.Items.Add(Lang.TransA("No"));
            ddPrivate.Items.Add(Lang.TransA("Yes"));
            ddPrivate.Items.Add(Lang.TransA("No"));
            ddExplicit.Items.Add(Lang.TransA("Yes"));
            ddExplicit.Items.Add(Lang.TransA("No"));

            trPrivatePhotosSearch.Visible = Config.Photos.EnablePrivatePhotos;
            trExplicitPhotosSearch.Visible = Config.Photos.EnableExplicitPhotos;
        }

        private void PopulateGrid()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browsePhotos;
            base.OnInit(e);
        }

        protected void dropPhotosPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        private void LoadStrings()
        {
            btnSearch.Text = Lang.TransA("Search");

            dgPhotos.Columns[0].HeaderText = Lang.TransA("Username");
            dgPhotos.Columns[1].HeaderText = Lang.TransA("Photo");
            dgPhotos.Columns[2].HeaderText = Lang.TransA("Description");
            dgPhotos.Columns[3].HeaderText = Lang.TransA("Primary");
            dgPhotos.Columns[4].HeaderText = Lang.TransA("Private");
            dgPhotos.Columns[4].Visible = Config.Photos.EnablePrivatePhotos;
            dgPhotos.Columns[5].HeaderText = Lang.TransA("Explicit");
            dgPhotos.Columns[5].Visible = Config.Photos.EnableExplicitPhotos;

            lnkFirst.Text = "<i class=\"fa fa-angle-double-left\"></i>";
            lnkPrev.Text = "<i class=\"fa fa-angle-left\"></i>";
            lnkNext.Text = "<i class=\"fa fa-angle-right\"></i>";
            lnkLast.Text = "<i class=\"fa fa-angle-double-right\"></i>";
        }

        public PhotoSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (PhotoSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                PreparePaginator();
                LoadResultsPage();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                    return 1;
                else
                    return (int)ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            set { pnlPaginator.Visible = value; }
        }

        private void PreparePaginator()
        {
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(PhotosPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Photos.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * PhotosPerPage + 1;
                int toUser = CurrentPage * PhotosPerPage;
                if (Results.Photos.Length < toUser)
                    toUser = Results.Photos.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Photos.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void LoadResultsPage()
        {
            PreparePaginator();

            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("Id");
            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("Description");
            dtResults.Columns.Add("Primary", typeof(bool));
            dtResults.Columns.Add("Private", typeof(bool));
            dtResults.Columns.Add("Explicit", typeof(bool));

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                Photo[] photos = Results.GetPage(CurrentPage, PhotosPerPage);

                if (photos != null && photos.Length > 0)
                    foreach (Photo photo in photos)
                    {
                        dtResults.Rows.Add(new object[]
                                               {
                                                   photo.Id,
                                                   photo.Username,
                                                   photo.Name,
                                                   photo.Description,
                                                   photo.Primary,
                                                   photo.PrivatePhoto,
                                                   photo.ExplicitPhoto
                                               });
                    }

                tblHideSearch.Visible = true;
            }
            else
            {
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            dgPhotos.DataSource = dtResults;
            dgPhotos.DataBind();
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;

            LoadResultsPage();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            LoadResultsPage();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(PhotosPerPage))
                CurrentPage++;

            LoadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(PhotosPerPage))
                CurrentPage = Results.GetTotalPages(PhotosPerPage);

            LoadResultsPage();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string username = null;
            if (txtUsername.Text.Trim().Length > 0)
                username = txtUsername.Text.Trim();
            object primary = null, privatePhoto = null, explicitPhoto = null;
            if (ddPrimary.SelectedIndex > 0)
                primary = ddPrimary.SelectedIndex == 1;
            if (ddPrivate.SelectedIndex > 0)
                privatePhoto = ddPrivate.SelectedIndex == 1;
            if (ddExplicit.SelectedIndex > 0)
                explicitPhoto = ddExplicit.SelectedIndex == 1;

            /*
			search.SortColumn = SortField;
			search.SortAsc = SortAsc;
			*/

            if (Results == null) Results = new PhotoSearchResults();
            Results.Photos = Photo.Search(-1, username, -1, true, primary, explicitPhoto, privatePhoto);
            CurrentPage = 1;
            //LoadResultsPage();
        }

        protected void dgPhotos_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this photo?")));
        }

        protected void dgPhotos_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (!HasWriteAccess)
                    return;

                int photoId = Convert.ToInt32(e.CommandArgument);
                Photo.Delete(photoId);
                PerformSearch();
            }
            else if (e.CommandName == "RemoveExplicit")
            {
                if (!HasWriteAccess)
                    return;

                int photoId = Convert.ToInt32(e.CommandArgument);
                Photo photo = Photo.Fetch(photoId);
                photo.ExplicitPhoto = false;
                photo.PrivatePhoto = false;
                photo.Save(false);
                PerformSearch();
            }
        }

        public string SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return "LastOnline";
                }
                else
                {
                    return (string)ViewState["sortField"];
                }
            }
            set { ViewState["sortField"] = value; }
        }

        public bool SortAsc
        {
            get
            {
                if (ViewState["sortAsc"] == null)
                {
                    return true;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["sortAsc"]);
                }
            }
            set { ViewState["sortAsc"] = value; }
        }

        protected void dgPhotos_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField)
                {
                    SortAsc = !SortAsc;
                }

                SortField = e.SortExpression;
            }

            PerformSearch();
        }
    }
}