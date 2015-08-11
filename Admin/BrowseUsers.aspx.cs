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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for BrowseUsers.
    /// </summary>
    public partial class BrowseUsers : AdminPageBase
    {
        private int UsersPerPage
        {
            get { return Convert.ToInt32(dropUsersPerPage.SelectedValue); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Browse Members".TranslateA();
            Description = "Use this section to browse, edit or delete members of your site...".TranslateA();

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
                dropUsersPerPage.Items.Add(i.ToString());
            dropUsersPerPage.SelectedValue = Config.AdminSettings.BrowseUsers.UsersPerPage.ToString();

            if (!Config.Users.DisableGenderInformation)
            {
                ddGender.Items.Add(
                new ListItem(Lang.TransA("Male"), ((int)Classes.User.eGender.Male).ToString()));
                ddGender.Items.Add(
                    new ListItem(Lang.TransA("Female"), ((int)Classes.User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    ddGender.Items.Add(
                        new ListItem(Lang.TransA("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }
            }

            ddPhoto.Items.Add(Lang.TransA("Yes"));
            ddPhoto.Items.Add(Lang.TransA("No"));
            ddActive.Items.Add(Lang.TransA("Yes"));
            ddActive.Items.Add(Lang.TransA("No"));
            ddDeleted.Items.Add(Lang.TransA("Yes"));
            ddDeleted.Items.Add(Lang.TransA("No"));
            ddPaid.Items.Add(Lang.TransA("Yes"));
            ddPaid.Items.Add(Lang.TransA("No"));
        }

        private void PopulateGrid()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.userAccounts;
            base.OnInit(e);
        }

        protected void dropUsersPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        /*-------------------------------------*/

        private void LoadStrings()
        {
            btnSearch.Text = Lang.TransA("Search");
            btnGetCSV.Text = "<i class=\"fa fa-file-excel-o\"></i>&nbsp;" + Lang.TransA("Download as CSV");

            dgUsers.Columns[0].HeaderText = Lang.TransA("Username") + "&nbsp;<i class=\"fa fa-sort\"></i>";
            dgUsers.Columns[1].HeaderText = Lang.TransA("Name") + "&nbsp;<i class=\"fa fa-sort\"></i>";
            dgUsers.Columns[2].HeaderText = Lang.TransA("Gender") + "&nbsp;<i class=\"fa fa-sort\"></i>";
            dgUsers.Columns[3].HeaderText = Lang.TransA("Birthdate") + "&nbsp;<i class=\"fa fa-sort\"></i>";
            dgUsers.Columns[4].HeaderText = Lang.TransA("Signup Date") + "&nbsp;<i class=\"fa fa-sort\"></i>";
            dgUsers.Columns[5].HeaderText = Lang.TransA("Signup IP") + "&nbsp;<i class=\"fa fa-sort\"></i>";
            dgUsers.Columns[6].HeaderText = Lang.TransA("Email") + "&nbsp;<i class=\"fa fa-sort\"></i>";

            lnkFirst.Text = "<i class=\"fa fa-angle-double-left\"></i>";
            lnkPrev.Text = "<i class=\"fa fa-angle-left\"></i>";
            lnkNext.Text = "<i class=\"fa fa-angle-right\"></i>";
            lnkLast.Text = "<i class=\"fa fa-angle-double-right\"></i>";

            if (Config.Users.DisableGenderInformation)
            {
                dgUsers.Columns[2].Visible = false;
                pnlGender.Visible = false;
            }
        }

        public UserSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (UserSearchResults)
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(UsersPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Usernames.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * UsersPerPage + 1;
                int toUser = CurrentPage * UsersPerPage;
                if (Results.Usernames.Length < toUser)
                    toUser = Results.Usernames.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Usernames.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void LoadResultsPage()
        {
            PreparePaginator();

            DataTable dtResults;
            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);
                User[] users = Results.GetPage(CurrentPage, UsersPerPage);
                dtResults = FetchResultsDataTable(users, false);
                tblHideSearch.Visible = true;
            }
            else
            {
                dtResults = new DataTable();
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            dgUsers.DataSource = dtResults;
            dgUsers.DataBind();
        }

        private static DataTable FetchResultsDataTable(User[] users, bool isCSV)
        {
            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("Gender");
            dtResults.Columns.Add("Birthdate");
            dtResults.Columns.Add("SignupDate");
            dtResults.Columns.Add("SignupIP");
            dtResults.Columns.Add("Email");
            dtResults.Columns.Add("CountryCode");
            dtResults.Columns.Add("CountryName");

            if (users != null && users.Length > 0)
                foreach (User user in users)
                {
                    string countryCode = null;
                    string countryName = null;
                    if (!isCSV)
                    {
                        if (user.SignupIp != "127.0.0.1" && user.SignupIp != "0.0.0.0"
                            && !user.SignupIp.StartsWith("192.168."))
                        {
                            countryCode = IPToCountry.GetCountry(user.SignupIp);
                        }

                        if (countryCode != null)
                        {
                            countryName = Config.Users.GetCountryByCode(countryCode);
                        }
                    }
                    else
                    {
                        countryName = user.Country;
                    }

                    dtResults.Rows.Add(new object[]
                                               {
                                                   user.Username,
                                                   user.Name,
                                                   Lang.TransA(user.Gender.ToString()),
                                                   user.Birthdate.ToShortDateString(),
                                                   user.UserSince.ToShortDateString(),
                                                   user.SignupIp,
                                                   user.Email,
                                                   countryCode,
                                                   countryName
                                               });
                }

            return dtResults;
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
            if (CurrentPage < Results.GetTotalPages(UsersPerPage))
                CurrentPage++;

            LoadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(UsersPerPage))
                CurrentPage = Results.GetTotalPages(UsersPerPage);

            LoadResultsPage();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            BasicSearch search = new BasicSearch();
            search.deleted_isSet = false;
            search.active_isSet = false;
            search.hasAnswer_isSet = false;
            search.visible_isSet = false;
            search.FaceControlApproved = null;

            search.SortColumn = SortField;
            search.SortAsc = SortAsc;

            if (txtUsername.Text.Trim().Length > 0)
                search.Username = txtUsername.Text.Trim();
            if (txtName.Text.Trim().Length > 0)
                search.Name = txtName.Text.Trim();
            if (ddGender.SelectedIndex > 0)
                search.Gender = (User.eGender)Convert.ToInt32(ddGender.SelectedValue);

            try
            {
                search.MinAge = Convert.ToInt32(txtAgeFrom.Text.Trim());
                search.MaxAge = Convert.ToInt32(txtAgeTo.Text.Trim());
            }
            catch
            {
            }

            if (txtCountry.Text.Trim().Length > 0)
                search.Country = txtCountry.Text.Trim();
            if (txtRegion.Text.Trim().Length > 0)
                search.State = txtRegion.Text.Trim();
            if (txtCity.Text.Trim().Length > 0)
                search.City = txtCity.Text.Trim();

            if (txtEmail.Text.Trim().Length > 0)
                search.Email = txtEmail.Text.Trim();
            if (txtIP.Text.Trim().Length > 0)
                search.IP = txtIP.Text.Trim();
            if (ddPhoto.SelectedIndex > 0)
                search.HasPhoto = ddPhoto.SelectedIndex == 1;
            if (ddActive.SelectedIndex > 0)
                search.Active = ddActive.SelectedIndex == 1;
            if (ddDeleted.SelectedIndex > 0)
                search.Deleted = ddDeleted.SelectedIndex == 1;
            if (ddPaid.SelectedIndex > 0)
                search.Paid = ddPaid.SelectedIndex == 1;

            Results = search.GetResults();

            btnGetCSV.Visible = Results != null;

            LoadResultsPage();
        }

        protected void dgUsers_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
        }

        protected void dgUsers_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Manage")
            {
                string username = (string)e.CommandArgument;
                Response.Redirect(String.Format("ManageUser.aspx?uid={0}", username));
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

        protected void dgUsers_SortCommand(object source, DataGridSortCommandEventArgs e)
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

        protected void btnGetCSV_Click(object sender, EventArgs e)
        {
            if (Results != null)
            {
                Response.Clear();
                Response.ContentType = "application/text";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = Encoding.UTF8.EncodingName;
                Response.AppendHeader("content-disposition", "attachment; filename=results.csv");
                Response.Write(Parsers.ParseCSV(FetchResultsDataTable(Results.Get(), true)));
                Response.End();
            }
        }
    }
}