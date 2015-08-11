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

namespace AspNetDating.Components.Profile
{
    /// <summary>
    ///		Summary description for ViewMultiChoiceBig.
    /// </summary>
    public partial class ViewMultiChoiceBig : UserControl, IProfileAnswerComponent
    {
        public void LoadAnswer(string Username, int QuestionID)
        {
            ProfileQuestion question = ProfileQuestion.Fetch(QuestionID);
            if (Config.Misc.EnableProfileDataTranslation)
                lblName.Text = Lang.Trans(question.AltName);
            else
                lblName.Text = question.AltName;

            try
            {
                ProfileAnswer answer = ProfileAnswer.Fetch(Username, QuestionID);
                if (answer.Value == String.Empty)
                {
                    pnlMessage.Visible = true;
                    pnlValues.Visible = false;
                    lblMessage.Text = Lang.Trans("-- no answer --");
                }
                else
                {
                    pnlMessage.Visible = false;
                    pnlValues.Visible = true;

                    string sAnswer = answer.Value;
                    sAnswer = sAnswer
                        .Replace("\n", "<br>");

                    string[] dataSource = sAnswer.Split(':');

                    if (question.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceCheck ||
                        question.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceSelect ||
                        question.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceRadio ||
                        question.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceSelect)
                    {
                        for (int i = 0; i < dataSource.Length; ++i)
                        {
                            if (Config.Misc.EnableProfileDataTranslation)
                                dataSource[i] = Lang.Trans(dataSource[i]);
                            
                            dataSource[i] = Server.HtmlEncode(dataSource[i]);
                        }
                    }
                    

                    rptValues.DataSource = dataSource;
                    rptValues.DataBind();
                }
            }
            catch (NotFoundException)
            {
                pnlMessage.Visible = true;
                pnlValues.Visible = false;
                lblMessage.Text = Lang.Trans("-- no answer --");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
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