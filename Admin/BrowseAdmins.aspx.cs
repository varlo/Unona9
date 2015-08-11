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
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for BrowseAdmins.
    /// </summary>
    public partial class BrowseAdmins : AdminPageBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Admin Management".TranslateA();
            Subtitle = "Browse Admins".TranslateA();
            Description = "Use this section to browse, edit or delete administrators...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddAdmin.Enabled = false;
                }

                LoadStrings();
                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            btnAddAdmin.Text = "<i class=\"fa fa-plus\"></i>&nbsp;" + Lang.TransA("Add Admin");
            gridAdmins.Columns[0].HeaderText = Lang.TransA("Username");
            gridAdmins.Columns[1].HeaderText = Lang.TransA("Last login");
            gridAdmins.Columns[2].HeaderText = Lang.TransA("Actions");
        }

        private void PopulateDataGrid()
        {
            Classes.Admin[] admins = Classes.Admin.Fetch();

            if (admins.Length != 0)
            {
                DataTable dtAdmins = new DataTable("Admins");
                dtAdmins.Columns.Add("Username");
                dtAdmins.Columns.Add("LastLogin");

                foreach (Classes.Admin admin in admins)
                {
                    dtAdmins.Rows.Add(new object[]
                                          {
                                              admin.Username,
                                              admin.LastLogin
                                          }
                        );
                }

                dtAdmins.DefaultView.Sort = "Username";

                gridAdmins.DataSource = dtAdmins;
                gridAdmins.DataBind();
            }
            else
            {
                //no admins?!
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageAdmins;
            base.OnInit(e);
        }

        protected void gridAdmins_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect("EditAdmin.aspx?uid=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName == "Delete")
            {
                if (!HasWriteAccess)
                    return;

                string username = (string)e.CommandArgument;
                Classes.Admin.Delete(username);
                PopulateDataGrid();
            }
        }

        /// <summary>
        /// Handles the Click event of the AddAdmin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditAdmin.aspx");
        }

        protected void gridAdmins_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

            if (((Literal)e.Item.FindControl("litUsername")).Text == Config.Users.SystemUsername)
                lnkDelete.Visible = false;

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this account?")));
        }
    }
}