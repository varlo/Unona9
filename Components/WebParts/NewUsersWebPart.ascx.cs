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
using System.Collections.Generic;
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
    public partial class NewUsersWebPart : WebPartUserControl
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

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Minimum Age"), AgeProperty]
        public int MinAge
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();
                if (user != null)
                {
                    if (ViewState["NewUsers_MinAge"] == null)
                        return Math.Max(user.IncomingMessagesRestrictions.AgeFrom, Config.Users.MinAge);

                    return (int)(ViewState["NewUsers_MinAge"] ?? Config.Users.MinAge);
                }
                else return Config.Users.MinAge;
            }
            set
            {
                ViewState["NewUsers_MinAge"] = value;
            }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Maximum Age"), AgeProperty]
        public int MaxAge
        {
            get
            {
                UserSession user = PageBase.GetCurrentUserSession();

                if (user != null)
                {
                    if (ViewState["NewUsers_MaxAge"] == null)
                        return Math.Min(user.IncomingMessagesRestrictions.AgeTo, Config.Users.MaxAge);

                    return (int)(ViewState["NewUsers_MaxAge"] ?? Config.Users.MaxAge);
                }
                else return Config.Users.MinAge;
            }
            set
            {
                ViewState["NewUsers_MaxAge"] = value;
            }
        }

        //[Personalizable(PersonalizationScope.User), WebBrowsable(true), WebDisplayName("RepeatColumns")]
        public int RepeatColumns
        {
            get
            {
                return dlNewMembers.RepeatColumns;
            }
            set
            {
                dlNewMembers.RepeatColumns = value;
            }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of rows")]
        public int LimitRows
        {
            get
            {
                return (int)(ViewState["NewUsers_LimitRows"] ?? Config.Photos.MaxRowsPhotosOnMainPage);
            }
            set
            {
                ViewState["NewUsers_LimitRows"] = value < 10 ? value : 10;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            #region Load New Users (thumbnails)

            DataTable dtNewUsers;

            string cacheKey = String.Format("NewUsersWebPart_{0}_{1}_{2}_{3}",
                Gender == User.eGenderSearch.All ? "null" : Gender.ToString(), MinAge, MaxAge, LimitRows);
            if (Cache[cacheKey] != null && Cache[cacheKey] is DataTable && Session["theme"] == null)
            {
                dtNewUsers = (DataTable)Cache[cacheKey];
            }
            else
            {
                NewUsersSearch nuSearch = new NewUsersSearch();
                nuSearch.PhotoReq = Config.Users.RequirePhotoToShowInNewUsers;
                nuSearch.ProfileReq = Config.Users.RequireProfileToShowInNewUsers;
                nuSearch.UsersCount = dlNewMembers.RepeatColumns * LimitRows;
                nuSearch.MinAge = MinAge;
                nuSearch.MaxAge = MaxAge;
                if (Gender != User.eGenderSearch.All) 
                    nuSearch.Gender = (User.eGender) Gender;
                UserSearchResults nuResults = nuSearch.GetResults();
                dtNewUsers = new DataTable();
                dtNewUsers.Columns.Add("Username");
                dtNewUsers.Columns.Add("ImageId", typeof(int));
                dtNewUsers.Columns.Add("Age");
                dtNewUsers.Columns.Add("Age2");
                dtNewUsers.Columns.Add("Gender");
                dtNewUsers.Columns.Add("City");
                if (nuResults != null && nuResults.Usernames != null)
                {
                    foreach (string username in nuResults.Usernames)
                    {
                        int imageId = 0;
                        User user = null;

                        try
                        {
                            user = User.Load(username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        try
                        {
                            imageId = Photo.GetPrimary(username).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageId = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }

                        string age = !Config.Users.DisableAgeInformation ? user.Age.ToString() : String.Empty;
                        string age2 = !Config.Users.DisableAgeInformation ? user.Age2.ToString() : String.Empty;
                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        dtNewUsers.Rows.Add(
                            new object[]
                                {
                                    username, imageId, age, user.Gender == User.eGender.Couple ? age2 : String.Empty, gender, user.City
                                });
                    }

                    Cache.Add(cacheKey, dtNewUsers, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }

            dlNewMembers.DataSource = dtNewUsers;
            dlNewMembers.DataBind();

            #endregion
        }

        protected void dlNewMembers_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlGenderAge = (HtmlGenericControl) e.Item.FindControl("pnlGenderAge");
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