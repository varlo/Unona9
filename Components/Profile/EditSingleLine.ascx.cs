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
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    //using Microsoft.ApplicationBlocks.ExceptionManagement;

    public partial class EditSingleLine : UserControl, IProfileQuestionComponent, ICascadeQuestionComponent
    {

        private bool adminMode;

        public bool AdminMode
        {
            set
            {
                adminMode = value;
                if (adminMode)
                {
                    //lblDescription.CssClass = "font_css";
                    //lblHint.CssClass = "font_css";
                    //lblName.CssClass = "font_css";
                    //txtValue.CssClass = "font_css";
                    if (tdAdmin != null)
                        tdAdmin.Visible = true;
                    if (tdUser != null)
                        tdUser.Visible = false;
                    if (pnlAdmin != null)
                        pnlAdmin.Visible = true;
                    if (pnlUser != null)
                        pnlUser.Visible = false;
                }
                else
                {
                    if (tdAdmin != null)
                        tdAdmin.Visible = false;
                    if (tdUser != null)
                        tdUser.Visible = true;
                    if (pnlAdmin != null)
                        pnlAdmin.Visible = false;
                    if (pnlUser != null)
                        pnlUser.Visible = true;
                }
            }

            get { return adminMode; }
        }

        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        //public string Name
        //{
        //    get { return lblName.Text; }
        //    set { lblName.Text = value; }
        //}

        public string Description
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }

        public string Hint
        {
            get { return lblHint.Text; }
            set { lblHint.Text = value; }
        }

        public string Value
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }

        private User user;
        protected HtmlGenericControl pnlUser;
        protected HtmlGenericControl pnlAdmin;

        public User User
        {
            set { user = value; }
            get { return user; }
        }

        private bool required;

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();
                required = value.Required;
                if (Config.Misc.EnableProfileDataTranslation)
                {
                    hlName.Title = Lang.Trans(value.Name);
                    Description = Lang.Trans(value.Description);
                    Hint = Lang.Trans(value.Hint);
                }
                else
                {
                    hlName.Title = value.Name;
                    Description = value.Description;
                    Hint = value.Hint;
                }
                
                try
                {
                    ProfileAnswer answer =
                        value.FetchAnswer(User.Username);
                    Value = answer.Value;
                }
                catch (NotFoundException)
                {
                    // The user has't answered yet
                }
            }
        }

        public ProfileAnswer Answer
        {
            get
            {
                ProfileAnswer answer = new ProfileAnswer(
                    User.Username,
                    Convert.ToInt32(hidQuestionId.Value));

                if (Value.Trim().Length == 0)
                {
                    if (required && !adminMode)
                        throw new AnswerRequiredException(hlName.Title);
                    else
                    {
                        answer.Value = "";
                        return answer;
                    }
                }

                answer.Value = Value;
                return answer;
            }
        }

        public HtmlGenericControl UserControlPanel
        {
            get { return pnlID; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void GenerateJSForChildVisibility(System.Collections.Generic.Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices)
        {
            // this control cannot be parent
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$get('" + txtValue.ClientID + "').value = '';}";
            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID + "Reset", js);
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