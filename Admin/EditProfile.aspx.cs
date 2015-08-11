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
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditProfile.
    /// </summary>
    public partial class EditProfile : AdminPageBase
    {
        protected User user;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "User Management".TranslateA();
            Subtitle = "Edit Profile".TranslateA();
            Description = "Use this section to edit profile...".TranslateA();

            if (!Page.IsPostBack)
            {
                if (Request.Params["username"] != null)
                {
                    user = Classes.User.Load(Request.Params["username"]);
                    EditProfileCtrl1.User = user;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.userAccounts;
            base.OnInit(e);
        }
    }
}