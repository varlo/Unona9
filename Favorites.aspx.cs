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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for Favourites.
    /// </summary>
    public partial class Favourites : PageBase
    {
        #region properties

        protected bool showSlogan = false;

        public bool ShowSlogan
        {
            set { showSlogan = value; }
        }

        protected bool showLastOnline = true;

        public bool ShowLastOnline
        {
            set { showLastOnline = value; }
        }

        protected bool showRating = false;

        public bool ShowRating
        {
            set { showRating = value; }
        }

        #endregion

        protected LargeBoxStart LargeBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Users.EnableFavorites)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                LoadStrings();
                LoadFavourites();
            }
        }

        private void LoadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Favorites");
        }

        private void LoadFavourites()
        {
            DataTable dtFavourites = new DataTable();

            dtFavourites.Columns.Add("Username");
            dtFavourites.Columns.Add("PhotoId", typeof (int));
            dtFavourites.Columns.Add("Slogan");
            dtFavourites.Columns.Add("Age");
            dtFavourites.Columns.Add("LastOnlineString");
            dtFavourites.Columns.Add("Rating");
            dtFavourites.Columns.Add("AddedToFavouritesOn");

            User[] users = CurrentUserSession.FetchFavouriteUsers();

            if (users.Length > 0)
            {
                foreach (User user in users)
                {
                    Photo primaryPhoto = null;
                    try
                    {
                        primaryPhoto = user.GetPrimaryPhoto();
                    }
                    catch (NotFoundException)
                    {
                    }
                    string slogan = "";
                    try
                    {
                        ProfileAnswer sloganAnswer = user.FetchSlogan();
                        if (sloganAnswer.Approved)
                            slogan = sloganAnswer.Value;
                        else
                            slogan = Lang.Trans("-- pending approval --");
                    }
                    catch (NotFoundException)
                    {
                    }

                    string ratingString = "";
                    if (showRating)
                    {
                        try
                        {
                            UserRating userRating = UserRating.FetchRating(user.Username);
                            ratingString = String.Format(
                                Lang.Trans("{0} ({1} votes)"),
                                userRating.AverageVote.ToString("0.00"), userRating.Votes);
                        }
                        catch (NotFoundException)
                        {
                            ratingString = Lang.Trans("no rating");
                        }
                    }

                    DateTime date =
                        CurrentUserSession.FetchFavouriteTimeStamp(user.Username);

                    #region Check user.Gender and set photoId

                    int photoId = 0;

                    if (primaryPhoto == null || !primaryPhoto.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = primaryPhoto.Id;
                    }

                    #endregion

                    dtFavourites.Rows.Add(new object[]
                                              {
                                                  user.Username,
                                                  photoId,
                                                  slogan,
                                                  user.Age,
                                                  user.LastOnlineString,
                                                  ratingString,
                                                  date.ToShortDateString()
                                              });
                }
            }
            else
            {
                lblMessage.Text = Lang.Trans("No favorites found!");
            }

            dlFavourites.DataSource = dtFavourites;
            dlFavourites.DataBind();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlFavourites.ItemCreated += new DataListItemEventHandler(dlFavourites_ItemCreated);
        }

        #endregion

        protected void dlFavourites_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            HyperLink lnkSendMessage = (HyperLink) e.Item.FindControl("lnkSendMessage");
            HyperLink lnkRemoveFromFavourites = (HyperLink)e.Item.FindControl("lnkRemoveFromFavourites");

            lnkSendMessage.Attributes.Add("title", Lang.Trans("Send Message"));
            lnkRemoveFromFavourites.Attributes.Add("title", Lang.Trans("Remove from Favorites"));

            var pnlAge = (HtmlGenericControl)e.Item.FindControl("pnlAge");
            var pnlAgeValue = (HtmlGenericControl)e.Item.FindControl("pnlAgeValue");
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
            pnlAgeValue.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void dlFavourites_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            string username = (string) DataBinder.Eval(e.Item.DataItem, "Username");

            HyperLink lnkSendMessage = (HyperLink)e.Item.FindControl("lnkSendMessage");
            lnkSendMessage.NavigateUrl =
                String.Format("~/SendMessage.aspx?to_user={0}&src=favorites", username);

            HyperLink lnkRemoveFromFavourites = (HyperLink)e.Item.FindControl("lnkRemoveFromFavourites");
            lnkRemoveFromFavourites.NavigateUrl =
                String.Format("AddRemoveFavourite.aspx?uid={0}&cmd=remove&src=favorites", username);
        }
    }
}