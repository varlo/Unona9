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
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for Cancel.
    /// </summary>
    public partial class Cancel : PageBase
    {
        public Cancel()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WideBoxStart1.Title = Lang.Trans("Status");
            var subscriptionCancelled = new MiscTemplates.SubscriptionCancelled(LanguageId);
            lblMessage.Text = subscriptionCancelled.Text;
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