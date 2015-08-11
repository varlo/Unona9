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
    public partial class ViewMultiLine : UserControl, IProfileAnswerComponent
    {
        protected HtmlInputHidden hidQuestionId;

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
                    lblValue.Text = Lang.Trans("-- no answer --");
                else
                {
                    if (answer.Approved)
                    {
                        string sAnswer = answer.Value;

                        if (question.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceCheck ||
                            question.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceSelect ||
                            question.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceRadio ||
                            question.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceSelect)
                        {
                            if(Config.Misc.EnableProfileDataTranslation)
                                sAnswer = Lang.Trans(sAnswer);
                        }
                        
                        sAnswer = Server.HtmlEncode(Parsers.TrimLongWords(sAnswer, 30));
                        sAnswer = sAnswer.Replace("\n", "<br>");

                        lblValue.Text = sAnswer;
                    }
                    else
                        lblValue.Text = Lang.Trans("-- pending approval --");
                }
            }
            catch (NotFoundException)
            {
                lblValue.Text = Lang.Trans("-- no answer --");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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