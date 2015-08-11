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
using System.Drawing;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for Login.
    /// </summary>
    public partial class Login : AdminPageBase
    {
        //protected System.Web.UI.WebControls.Label lblError;
        protected MessageBox MessageBox;

        public Login()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        private void LoadStrings()
        {
            btnLogin.Text = Lang.TransA(" Sign In ");

            foreach (Language language in Language.FetchAll())
            {
                ddLanguage.Items.Add(new ListItem(Lang.TransA(language.Name), language.Id.ToString()));
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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please specify username!"), Misc.MessageType.Error);
                return;
            }
            if (txtPassword.Text.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please specify password!"), Misc.MessageType.Error);
                return;
            }

            Session["LanguageId"] = Convert.ToInt32(ddLanguage.SelectedValue);

            AdminSession admin = null;
            try
            {
                admin = new AdminSession(txtUsername.Text);
                admin.Authorize(txtPassword.Text, Session.SessionID);

                IPLogger.Log(admin.Username, Request.UserHostAddress, IPLogger.ActionType.AdminLoginSuccess);
            }
            catch (Exception err)
            {
                IPLogger.Log(txtUsername.Text, Request.UserHostAddress, IPLogger.ActionType.AdminLoginFailed);

                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return;
            }

            ((AdminPageBase) Page).CurrentAdminSession = admin;

            if (Request.Params["back_url"] != null
                && Request.Params["back_url"].Length > 0)
                Response.Redirect(Request.Params["back_url"]);
            else
                Response.Redirect("Home.aspx");
        }
    }
}