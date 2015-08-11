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
    /// Summary description for LostPassword.
    /// </summary>
    public partial class LostPassword : PageBase
    {
        public LostPassword()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here

            if (!Page.IsPostBack)
            {
                btnSubmit.Text = Lang.Trans("Submit");
                if (WideBoxStart1 != null)
                    WideBoxStart1.Title = Lang.Trans("Lost password");
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            User user;
            string email = txtEmail.Text;

            //email validation
            try
            {
                user = Classes.User.LoadUserByEmail(email);

                user.SendForgotPasswordEmail();
            }
            catch (Exception err) // Invalid e-mail address
            {
                lblError.Text = err.Message;
                return;
            }

            StatusPageMessage = Lang.Trans
                ("You will receive an e-mail shortly. In order "
                 + "to proceed to the password changing page, please click the "
                 + "confirmation link in the e-mail.");
            Response.Redirect("ShowStatus.aspx");
        }
    }
}