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

namespace AspNetDating
{
    /// <summary>
    /// Summary description for ChangeForgottenPassword.
    /// </summary>
    public partial class ChangeLostPassword : PageBase
    {

        private string username;
        protected RequiredFieldValidator RequiredFieldValidator1;
        protected RegularExpressionValidator RegularExpressionValidator1;
        private string guid;

        public ChangeLostPassword()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here

            username = Request.Params["username"];
            guid = Request.Params["guid"];

            if (!Classes.User.IsValidPendingGuid(username, guid) || username == null)
            {
                StatusPageMessage = Lang.Trans("<b>Wrong confirmation URL!</b><br><br>");
                Response.Redirect("ShowStatus.aspx");
            }

            btnSubmit.Text = Lang.Trans("Submit");
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
            try
            {
                User user = Classes.User.Load(username);

                if (txtNewPassword.Value == txtConfirmedPassword.Value)
                {
                    user.Password = txtNewPassword.Value;
                    user.Update();
                    Classes.User.RemovePendingGuid(guid);
                    Response.Redirect("home.aspx");
                }
                else
                {
                    lblError.Text = Lang.Trans("The passwords don't match!");
                    txtNewPassword.Value = "";
                    txtConfirmedPassword.Value = "";
                }
            }
            catch (ArgumentException ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}