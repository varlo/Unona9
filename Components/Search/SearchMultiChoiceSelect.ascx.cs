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
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Search
{
    /// <summary>
    ///		Summary description for SearchMultiChoiceSelect.
    /// </summary>
    public partial class SearchMultiChoiceSelect : UserControl, IProfileSearchComponent, ICascadeQuestionComponent
    {

        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();

                if (Config.Misc.EnableProfileDataTranslation)
                    lblName.Text = Lang.Trans(value.AltName);
                else
                    lblName.Text = value.AltName;

                ProfileChoice[] profileChoices = value.FetchChoices();
                if (profileChoices != null)
                {
                    lbValues.Items.Add("");
                    foreach (ProfileChoice choice in profileChoices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                            lbValues.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Id.ToString()));
                        else
                            lbValues.Items.Add(new ListItem(choice.Value, choice.Id.ToString()));
                    }
                }
            }
        }

        public ProfileAnswer[] Answers
        {
            get
            {
                List<ProfileAnswer> lAnswers = new List<ProfileAnswer>();
                foreach (ListItem item in lbValues.Items)
                {
                    if (item.Selected && item.Value != "")
                    {
                        ProfileAnswer answer = new ProfileAnswer();
                        answer.Question = new ProfileQuestion(
                            Convert.ToInt32(QuestionID));
                        if (item.Value.Length > 0)
                            answer.Value = ProfileChoice.Fetch(Int32.Parse(item.Value)).Value;//item.Text;
                        else
                            answer.Value = String.Empty;
                        lAnswers.Add(answer);
                    }
                }
                return lAnswers.ToArray();
            }
        }

        public int[] ChoiceIds
        {
            get
            {
                List<int> lChoiceIds = new List<int>();
                foreach (ListItem item in lbValues.Items)
                {
                    if (item.Selected)
                    {
                        lChoiceIds.Add(Convert.ToInt32(item.Value));
                    }
                }
                return lChoiceIds.ToArray();
            }
            set
            {
                foreach (int choiceId in value)
                {
                    ListItem item = lbValues.Items.FindByValue(choiceId.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        public HtmlGenericControl UserControlPanel
        {
            get { return pnlID; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
        }

        public void GenerateJSForChildVisibility(Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices)
        {
            bool translate = Config.Misc.EnableProfileDataTranslation;
            string js =
                String.Format("javascript: var selectedOptions = []; $('#{0} option:selected').each(function(i, selected){{selectedOptions[i] = $(selected).text();}});", lbValues.ClientID);

            js += "var match;";

            foreach (var pair in dChildClientIDsWithParentQuestionChoices)
            {
                string childClientID = pair.Key;
                string[] parentQuestionChoices = (string[]) pair.Value[0];
                List<string> childQuestionsClientIDs = (List<string>)pair.Value[1];

                js += "match = false; for (var j=0; j < selectedOptions.length; ++j){ if(";
                for (int i = 0; i < parentQuestionChoices.Length; ++i)
                {
                    string parentQuestionChoice = parentQuestionChoices[i];
                    if (translate)
                        parentQuestionChoice =
                            parentQuestionChoices[i].Translate().Replace(@"\", @"\\").Replace("'", @"\'").
                                Replace("\"", "\\\"");
                    js += "selectedOptions[j] == '" + parentQuestionChoice + "'" +
                          (i == parentQuestionChoices.Length - 1 ? String.Empty : " || ");
                }
                js += "){match = true; break;} } if(match) { $get('" + childClientID +
                      "').style.display = '';} else {$get('" + childClientID + "').style.display = 'none';" + "Reset" +
                      childClientID + "();";
                foreach (string childQuestionClientID in childQuestionsClientIDs)
                {
                    js += "$get('" + childQuestionClientID + "').style.display = 'none';" + "Reset" +
                          childQuestionClientID + "();";
                }

                js += "}";
            }

            lbValues.Attributes.Add("onclick", js);
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$get('" + lbValues.ClientID + "').selectedIndex = -1;}";
            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID + "Reset", js);
        }

        public void ClearAnswers()
        {
            lbValues.Items.Clear();
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