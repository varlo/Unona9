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
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for ShowStatus.
    /// </summary>
    public partial class ShowStatus : PageBase
    {
        public ShowStatus()
        {
            RequiresAuthorization = false;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            MasterPage m = Page.Master;
            fbStatus.SkinID = StatusPageLinkSkindId;
            fbStatus.Text = StatusPageLinkText;
            if (StatusPageLinkURL != null)
                fbStatus.PostBackUrl = ResolveUrl(StatusPageLinkURL);
            fbStatus.Visible = !String.IsNullOrEmpty(fbStatus.PostBackUrl);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WideBoxStart1.Title = Lang.Trans("Status");

            if (Request.Params["adblock"] != null)
                lblMessage.Text = "Ad blocker was detected! Please disable the ad blocker and try again.".Translate();

            if (StatusPageMessage != null)
            {
                lblMessage.Text = StatusPageMessage;
                StatusPageMessage = null;
            }

            if (StatusPageLinkURL != "")
            {
                var hlnkNewDirection = new HyperLink
                                           {
                                               Text = StatusPageLinkText,
                                               NavigateUrl = StatusPageLinkURL
                                           };
//                try
//                {
//                    hlnkNewDirection.SkinID = StatusPageLinkSkindId;
//                }catch(ArgumentException){/*selected theme does not contain skin with such id*/}
//                
//                plhLinkContainer.Controls.Add(hlnkNewDirection);

                StatusPageLinkText = "";
                StatusPageLinkURL = "";
                StatusPageLinkSkindId = "";
            }

            //else
            //    hlnkNewDirection.Visible = false;
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