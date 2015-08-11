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
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class NewGroupsWebPart : WebPartUserControl
    {
        public int RepeatColumns
        {
            get { return dlNewGroups.RepeatColumns; }
            set { dlNewGroups.RepeatColumns = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of rows")]
        public int LimitRows
        {
            get
            {
                return (int) (ViewState["NewGroupsWebPart_LimitRows"] ??
                              Config.Groups.NumberOfNewGroupsRows);
            }
            set { ViewState["NewGroupsWebPart_LimitRows"] = value < 10 ? value : 10; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Config.Groups.EnableGroups)
            {
                #region Load New Groups (thumbnails)

                DataTable dtNewGroups;

                string cacheKey = String.Format("NewGroupsWebPart_{0}", LimitRows);
                if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable)
                {
                    dtNewGroups = (DataTable) Cache[cacheKey];
                }
                else
                {
                    Group[] groups =
                        Group.Fetch(RepeatColumns*LimitRows, true, Group.eSortColumn.DateCreated);
                    dtNewGroups = new DataTable();
                    dtNewGroups.Columns.Add("GroupID");
                    dtNewGroups.Columns.Add("Name");
                    dtNewGroups.Columns.Add("AccessLevel");

                    foreach (Group group in groups)
                    {
                        string accessLevel = null;

                        switch (group.AccessLevel)
                        {
                            case Group.eAccessLevel.Public:
                                accessLevel = Lang.Trans("Public Group");
                                break;
                            case Group.eAccessLevel.Moderated:
                                accessLevel = Lang.Trans("Moderated Group");
                                break;
                            case Group.eAccessLevel.Private:
                                accessLevel = Lang.Trans("Private Group");
                                break;
                        }
                        dtNewGroups.Rows.Add(
                            new object[]
                                {
                                    group.ID, group.Name, accessLevel
                                });
                    }

                    Cache.Add(cacheKey, dtNewGroups, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }

                dlNewGroups.DataSource = dtNewGroups;
                dlNewGroups.DataBind();

                if (dtNewGroups.Rows.Count == 0)
                    Visible = false;
                else
                    Visible = true;

                #endregion
            }
            else Visible = false;
        }
    }
}