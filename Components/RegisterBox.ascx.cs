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
using AspNetDating;
using AspNetDating.Classes;
using AspNetDating.Classes.MySpace;
using DataAvailability = AspNetDating.Classes.MySpace.DataAvailability;

namespace AspNetDating.Components
{
    public partial class RegisterBox : UserControl
    {
        protected Label lblNotRegisteredYet;
        protected SmallBoxStart SmallBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareStrings();
        }

        private void PrepareStrings()
        {

            if (lblNotRegisteredYet != null)
                lblNotRegisteredYet.Text = Lang.Trans("Not registered yet?<br>What are you waiting for?");

            if (fbRegister != null)
                fbRegister.Text = Lang.Trans("Register now");

  
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


        protected void fbRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

    }
}