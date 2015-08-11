using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class Login : PageBase
    {
        public Login()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            WideBoxStart1.Title = Lang.Trans("Log In or Register");


            if (fbRegister != null)
                fbRegister.Text = Lang.Trans("Register now");
                   
        }

          
        protected void imgbRegister_Click(object sender, ImageClickEventArgs e)
        {
            lnkRegister_Click(sender, null);
        }

        protected void lnkRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
     
    }
}
