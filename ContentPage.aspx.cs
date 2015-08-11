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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for ContentPage.
    /// </summary>
    public partial class ContentPage : PageBase
    {
        public ContentPage()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.Params["id"];
                if (string.IsNullOrEmpty(id))
                {
                    return;
                }

                try
                {
                    int pageID = Convert.ToInt32(id);
                    Classes.ContentPage cp = Classes.ContentPage.FetchContentPage(pageID);
                    if (cp == null ||
                        (CurrentUserSession == null && (cp.VisibleFor == Classes.ContentPage.eVisibility.LoggedOnUsers
                            || cp.VisibleFor == Classes.ContentPage.eVisibility.Paid
                            || cp.VisibleFor == Classes.ContentPage.eVisibility.Unpaid))
                        || (CurrentUserSession != null
                                && ((cp.VisibleFor == Classes.ContentPage.eVisibility.Paid
                                    && !Classes.User.IsPaidMember(CurrentUserSession.Username))
                                    || (cp.VisibleFor == Classes.ContentPage.eVisibility.Unpaid
                                    && Classes.User.IsPaidMember(CurrentUserSession.Username)))))
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    Title = cp.Title;
                    divContent.InnerHtml = cp.Content;

                    Control[] controls = new Control[Page.Header.Controls.Count];
                    Page.Header.Controls.CopyTo(controls, 0);
                    var descriptionMetaTag = Array.Find(controls, c => c is HtmlMeta && ((HtmlMeta) c).Name == "description");
                    var keywordsMetaTag = Array.Find(controls, c => c is HtmlMeta && ((HtmlMeta) c).Name == "keywords");

                    if (cp.MetaDescription.Length != 0)
                    {
                        if (descriptionMetaTag != null)
                            Header.Controls.Remove(descriptionMetaTag);
                        HtmlMeta metaDesc = new HtmlMeta();
                        metaDesc.Name = "description";
                        metaDesc.Content = cp.MetaDescription;
                        Header.Controls.Add(metaDesc);
                    }

                    if (cp.MetaKeyword.Length != 0)
                    {
                        if (keywordsMetaTag != null)
                            Header.Controls.Remove(keywordsMetaTag);
                        HtmlMeta metaKeywords = new HtmlMeta();
                        metaKeywords.Name = "keywords";
                        metaKeywords.Content = cp.MetaKeyword;
                        Header.Controls.Add(metaKeywords);
                    }
                }
                catch (FormatException)
                {
                    return;
                }
            }

            base.OnPreRender(e);
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
    }
}