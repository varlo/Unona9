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
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Search
{
    public partial class SearchMultiChoiceCheck2 : UserControl, IProfileSearchComponent, ICascadeQuestionComponent
    {
        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        #region ICascadeQuestionComponent Members

        public void GenerateJSForChildVisibility(Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices)
        {
            bool translate = Config.Misc.EnableProfileDataTranslation;
            string js = "<script>";
            for (int i = 0; i < cbValues.Items.Count; i++)
            {
                js += "if ($get('" + cbValues.ClientID + "_" + i + "') != null) {";
                js += "$get('" + cbValues.ClientID + "_" + i + "')" + ".onclick = new Function(\"";

                foreach (var pair in dChildClientIDsWithParentQuestionChoices)
                {
                    string childClientID = pair.Key;
                    var parentQuestionChoices = (string[]) pair.Value[0];
                    var childQuestionsClientIDs = (List<string>) pair.Value[1];

                    js += "if(";

                    for (int j = 0; j < cbValues.Items.Count; j++)
                    {
                        js += "($get('" + cbValues.ClientID + "_" + j + "').checked && (";

                        for (int k = 0; k < parentQuestionChoices.Length; k++)
                        {
                            string parentQuestionChoice = parentQuestionChoices[k];
                            if (translate)
                                parentQuestionChoice =
                                    parentQuestionChoices[k].Translate().Replace(@"\", @"\\").Replace("'", @"\'").
                                        Replace("\"", "\\\"");
                            js += "$($get('" + cbValues.ClientID + "_" + j + "')).next('label').text() == '" +
                                  parentQuestionChoice + "'" +
                                  (k == parentQuestionChoices.Length - 1 ? String.Empty : " || ");
                        }

                        js += ")) " + (j == cbValues.Items.Count - 1 ? String.Empty : " || ");
                    }

                    js += ") {$get('" + childClientID + "').style.display = '';} else {$get('" + childClientID +
                          "').style.display = 'none';" + "Reset" + childClientID + "();";
                    foreach (string childQuestionClientID in childQuestionsClientIDs)
                    {
                        js += "$get('" + childQuestionClientID + "').style.display = 'none';" + "Reset" +
                              childQuestionClientID + "();";
                    }
                    js += "}";
                }

                js += "\");}";
            }

            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID, js);
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$('#" + cbValues.ClientID + " tr td input').each(function(){ this.checked = false;}); }";
            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID + "Reset", js);
        }

        #endregion

        #region IProfileSearchComponent Members

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();

                ltrName.Text = Config.Misc.EnableProfileDataTranslation
                                   ? Lang.Trans(value.AltName)
                                   : value.AltName;

                ProfileChoice[] profileChoices = value.FetchChoices();
                if (profileChoices != null)
                {
                    foreach (ProfileChoice choice in profileChoices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                            cbValues.Items.Add(
                                new ListItem(Lang.Trans(choice.Value), choice.Id.ToString()));
                        else
                            cbValues.Items.Add(
                                new ListItem(choice.Value, choice.Id.ToString()));
                    }
                }
            }
        }

        public ProfileAnswer[] Answers
        {
            get
            {
                var lAnswers = new List<ProfileAnswer>();
                foreach (ListItem item in cbValues.Items)
                {
                    if (item.Selected)
                    {
                        var answer = new ProfileAnswer
                                         {
                                             Question = ProfileQuestion.Fetch(Convert.ToInt32(QuestionID)),
                                             Value =
                                                 item.Value.Length > 0
                                                     ? ProfileChoice.Fetch(Int32.Parse(item.Value)).Value
                                                     : String.Empty
                                         };
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
                var lChoiceIds = new List<int>();
                foreach (ListItem item in cbValues.Items)
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
                    ListItem item = cbValues.Items.FindByValue(choiceId.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                        divExpandee.Style["display"] = ""; // Unhide
                    }
                }
            }
        }

        public HtmlGenericControl UserControlPanel
        {
            get { return pnlID; }
        }

        public void ClearAnswers()
        {
            foreach (ListItem item in cbValues.Items) item.Selected = false;
        }

        #endregion

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

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Unhide if something is chosen
            divExpandee.Style["display"] = Answers.Length > 0 ? "" : "none";

            base.OnPreRender(e);
        }
    }
}