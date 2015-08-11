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
using System.Collections;
using System.Web.UI;
using AspNetDating.Classes;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AspNetDating.Components
{
    public partial class Footer : UserControl
    {
        private int LanguageID
        {
            get { return ((PageBase) Page).LanguageId; }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        void AddSelectionCssClass(HtmlGenericControl control)
        {
            control.Attributes.Add("class", "active");
        }

        void SetSelectedLinkClass()
        {
            if (Page is ContentPage)
            {
                var li = rptPages.Items.Cast<RepeaterItem>().Select(i => i.FindControl("liContentPage")).Cast<HtmlGenericControl>().
                    Where(ctrl => ctrl.Attributes["data-id"] == Request.Params["id"]).FirstOrDefault();
                if (li != null)
                    AddSelectionCssClass(li);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Page is _default))
            {
                bvDefaultFooter.Visible = false;
            }

            loadPages();

            if (!IsPostBack)
                SetSelectedLinkClass();
        }

        private void loadPages()
        {
            ArrayList lContentPages = new ArrayList();

            Classes.ContentPage[] contentPages =
                Classes.ContentPage.FetchContentPages(Convert.ToInt32(LanguageID),
                                                      Classes.ContentPage.eSortColumn.FooterPosition);

            bool loggedOn = CurrentUserSession != null;
            bool isPaid = CurrentUserSession != null && Classes.User.IsPaidMember(CurrentUserSession.Username);

            foreach (Classes.ContentPage contentPage in contentPages)
            {
                if (contentPage.FooterPosition != null)
                {
                    if (
                        ((loggedOn && ((contentPage.VisibleFor & Classes.ContentPage.eVisibility.LoggedOnUsers) != 0 ||
                            contentPage.VisibleFor == Classes.ContentPage.eVisibility.Paid && isPaid ||
                            contentPage.VisibleFor == Classes.ContentPage.eVisibility.Unpaid && !isPaid)))
                        ||
                        ((!loggedOn && (contentPage.VisibleFor & Classes.ContentPage.eVisibility.NotLoggedOnUsers) != 0))
                       )
                        lContentPages.Add(contentPage);
                }
            }

            rptPages.DataSource = lContentPages.ToArray();
            rptPages.DataBind();
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}