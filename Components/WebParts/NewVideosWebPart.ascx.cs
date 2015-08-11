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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class NewVideosWebPart : WebPartUserControl
    {
        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Gender"), GenderProperty]
        public User.eGenderSearch Gender
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();

                if (user != null)
                {
                    if (ViewState["Gender"] == null)
                    {
                        if (Config.Users.DisableGenderInformation)
                            return User.eGenderSearch.All;
                        if (Config.Users.InterestedInFieldEnabled)
                        {
                            return (User.eGenderSearch)user.InterestedIn;
                        }
                        switch (user.Gender)
                        {
                            case User.eGender.Male:
                                return (User.eGenderSearch)User.eGender.Female;
                            case User.eGender.Female:
                                return (User.eGenderSearch)User.eGender.Male;
                            case User.eGender.Couple:
                                return User.eGenderSearch.All;
                        }
                        return User.eGenderSearch.All;
                    }
                    return (User.eGenderSearch)ViewState["Gender"];
                }
                return User.eGenderSearch.All;
            }
            set
            {
                ViewState["Gender"] = value;

            }
        }

        public int RepeatColumns
        {
            get
            {
                return dlNewVideos.RepeatColumns;
            }
            set
            {
                dlNewVideos.RepeatColumns = value;
            }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of rows")]
        public int LimitRows
        {
            get
            {
                return (int)(ViewState["NewVideosWebPart_LimitRows"] ??
                    Config.Photos.MaxRowsVideosOnMainPage);
            }
            set
            {
                ViewState["NewVideosWebPart_LimitRows"] = value < 10 ? value : 10;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            #region Load New Videos (thumbnails)
            DataTable dtNewVideos;

            string cacheKey = String.Format("NewVideos.ascx_{0}_{1}", Gender == User.eGenderSearch.All ? 
                "null" : Gender.ToString(), LimitRows);
            if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable && Session["theme"] == null)
            {
                dtNewVideos = (DataTable)Cache[cacheKey];
            }
            else
            {
                VideoUploadsSearch search = new VideoUploadsSearch();
                search.Approved = true;
                search.IsPrivate = false;
                search.SortColumn = VideoUploadsSearch.eSortColumn.ID;
                search.VideosCount = dlNewVideos.RepeatColumns * LimitRows;
                if (Gender != User.eGenderSearch.All)
                    search.Gender = (User.eGender)Gender;
                VideoUploadSearchResults nvResults = search.GetResults();
                dtNewVideos = new DataTable();
                dtNewVideos.Columns.Add("Username");
                dtNewVideos.Columns.Add("VideoId", typeof(int));
                dtNewVideos.Columns.Add("ThumbnailUrl");
                dtNewVideos.Columns.Add("Age");
                dtNewVideos.Columns.Add("Gender");
                if (nvResults != null && nvResults.Ids != null)
                {
                    foreach (int id in nvResults.Ids)
                    {
                        User user;
                        VideoUpload videoUpload;
                        try
                        {
                            videoUpload = VideoUpload.Load(id);
                            user = User.Load(videoUpload.Username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        string videoThumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                            user.Username, id);
                        string age = !Config.Users.DisableGenderInformation ? user.Age.ToString() : String.Empty;
                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        dtNewVideos.Rows.Add(
                            new object[]
                                {
                                    user.Username, id, videoThumbnail, age, gender
                                });
                    }

                    Cache.Add(cacheKey, dtNewVideos, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }
            
            if (dtNewVideos.Rows.Count > 0)
                mvNewVideos.SetActiveView(vNewVideos);
            else
                mvNewVideos.SetActiveView(vNoNewVideos);

            dlNewVideos.DataSource = dtNewVideos;
            dlNewVideos.DataBind();

            #endregion
        }

        protected void dlNewVideos_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlGenderAge");
                pnlGenderAge.Visible = false;
            }
            else if (Config.Users.DisableAgeInformation || Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlDelimiter");
                pnlDelimiter.Visible = false;
            }
        }
    }
}