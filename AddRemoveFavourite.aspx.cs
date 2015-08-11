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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for AddRemoveFavourite.
    /// </summary>
    public partial class AddRemoveFavourite : PageBase
    {
        protected SmallBoxStart SmallBoxStart1;
        protected LargeBoxStart LargeBoxStart1;
        private string userID;
        private string source;
        private string command;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Users.EnableFavorites)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (!ValidateParameters())
                    return;

                source = (string) Request.Params["src"];
                command = (string) Request.Params["cmd"];
                userID = (string) Request.Params["uid"];

                LoadStrings();
                DoThings();
            }
            source = (string) Request.Params["src"];
            command = (string) Request.Params["cmd"];
            userID = (string) Request.Params["uid"];
        }

        private bool ValidateParameters()
        {
            if (Request.Params["src"] == null ||
                Request.Params["uid"] == null ||
                Request.Params["cmd"] == null)
                return false;

            string uid = (string) Request.Params["uid"];
            string cmd = (string) Request.Params["cmd"];
            string src = (string) Request.Params["src"];

            if (src != "profile" && src != "favorites" && src != "search")
                return false;
            if (cmd != "add" && cmd != "remove")
                return false;
            if (src == "favorites" && cmd != "remove")
                return false;

            return true;
        }

        private void LoadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Message");

            lnkFavourites.Text = Lang.Trans("Go to Favorites");
            if (source == "profile")
            {
                lnkBack.Text = Lang.Trans("Back to Profile");
                lnkBack.CommandArgument = "profile";
                pnlGoToFavourites.Visible = true;
            }
            else if (source == "favorites")
            {
                lnkBack.Text = Lang.Trans("Back to Favorites");
                lnkBack.CommandArgument = "favorites";
                pnlGoToFavourites.Visible = false;
            }
            else if (source == "search")
            {
                lnkBack.Text = Lang.Trans("Back to Search");
                lnkBack.CommandArgument = "search";
                pnlGoToFavourites.Visible = false;
            }
        }

        private void DoThings()
        {
            if (command == "add")
            {
                User.eAddFavouriteResult result =
                    CurrentUserSession.AddToFavourites(userID);
                switch (result)
                {
                    case Classes.User.eAddFavouriteResult.eAlreadyAdded:
                        lblMessage.CssClass = "alert text-danger";
                        lblMessage.Text = Lang.Trans("The user you selected already exists in your favourite list");
                        break;
                    case Classes.User.eAddFavouriteResult.eInvalidUsername:
                        lblMessage.CssClass = "alert text-danger";
                        lblMessage.Text = Lang.Trans("No such user!");
                        break;
                    case Classes.User.eAddFavouriteResult.eMaximumFavouritesReached:
                        lblMessage.CssClass = "alert text-danger";
                        lblMessage.Text =
                            String.Format(
                                Lang.Trans(
                                    "The maximum favourite users of {0} is reached! Please remove someone and try again!"),
                                Config.Users.MaxFavouriteUsers);
                        break;
                    case Classes.User.eAddFavouriteResult.eSuccess:
                        lblMessage.CssClass = "alert text-info";
                        lblMessage.Text = String.Format(Lang.Trans("{0} has been added to your favourite list"), userID);
                        
                        break;
                }
            }
            else if (command == "remove")
            {
                if (CurrentUserSession.IsUserInFavouriteList(userID))
                {
                    CurrentUserSession.RemoveFromFavourites(userID);

                    lblMessage.CssClass = "alert text-info";
                    lblMessage.Text = String.Format(Lang.Trans("{0} has been removed from your favourite list"), userID);
                }
                else
                {
                    lblMessage.CssClass = "alert text-danger";
                    lblMessage.Text = Lang.Trans("No such user!");
                }
            }
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
        }

        #endregion

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (lnkBack.CommandArgument == "profile")
                Response.Redirect(UrlRewrite.CreateShowUserUrl(userID));
            else if (lnkBack.CommandArgument == "favorites")
                Response.Redirect("~/Favorites.aspx");
            else if (lnkBack.CommandArgument == "search")
            {
                if (Config.BackwardCompatibility.UseClassicSearchPage)
                    Response.Redirect("~/Search.aspx");
                else
                    Response.Redirect("~/Search2.aspx");
            }
        }

        protected void lnkFavourites_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Favorites.aspx");
        }
    }
}