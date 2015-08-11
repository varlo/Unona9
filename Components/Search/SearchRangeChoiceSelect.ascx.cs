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
    public partial class SearchRangeChoiceSelect : UserControl, IProfileSearchComponent, ICascadeQuestionComponent
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

                ddFrom.Items.Add("");
                ddTo.Items.Add("");

                ProfileChoice[] choices = value.FetchChoices();
                if (choices != null)
                {
                    foreach (ProfileChoice choice in choices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                        {
                            ddFrom.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Id.ToString()));
                            ddTo.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Id.ToString()));
                        }
                        else
                        {
                            ddFrom.Items.Add(new ListItem(choice.Value, choice.Id.ToString()));
                            ddTo.Items.Add(new ListItem(choice.Value, choice.Id.ToString()));                            
                        }
                    }
                }

                if (ddFrom.Items.Count > 0)
                    ddFrom.SelectedIndex = 1;
                if (ddTo.Items.Count > 0)
                    ddTo.SelectedIndex = ddTo.Items.Count - 1;
            }
        }

        public ProfileAnswer[] Answers
        {
            get
            {
                List<ProfileAnswer> lAnswers = new List<ProfileAnswer>();
                if (ddFrom.SelectedIndex == 0 && ddTo.SelectedIndex == 0 ||
                    ddFrom.Items.Count > 1 && ddFrom.SelectedIndex == 1 && ddTo.SelectedIndex == ddTo.Items.Count - 1)
                {
                    return lAnswers.ToArray();
                }
                else if (ddFrom.SelectedIndex == 0 && ddTo.SelectedIndex != 0)
                {
                    ddFrom.SelectedIndex = 1;
                }
                else if (ddFrom.SelectedIndex != 0 && ddTo.SelectedIndex == 0)
                {
                    ddTo.SelectedIndex = ddTo.Items.Count - 1;
                }

                if (ddTo.SelectedIndex < ddFrom.SelectedIndex)
                {
                    int temp = ddFrom.SelectedIndex;
                    ddFrom.SelectedIndex = ddTo.SelectedIndex;
                    ddTo.SelectedIndex = temp;
                }

                for (int i = ddFrom.SelectedIndex; i <= ddTo.SelectedIndex; i++)
                {
                    ProfileAnswer answer = new ProfileAnswer();
                    answer.Question = new ProfileQuestion(
                        Convert.ToInt32(QuestionID));
                    if (ddFrom.Items[i].Value.Length > 0)
                        answer.Value = ProfileChoice.Fetch(Int32.Parse(ddFrom.Items[i].Value)).Value;//.Text;
                    else
                        answer.Value = String.Empty;
                    lAnswers.Add(answer);
                }

                return lAnswers.ToArray();
            }
        }

        public int[] ChoiceIds
        {
            get
            {
                List<int> lChoiceIds = new List<int>();

                if (ddFrom.SelectedIndex == 0 && ddTo.SelectedIndex == 0)
                {
                    return lChoiceIds.ToArray();
                }
                else if (ddFrom.SelectedIndex == 0 && ddTo.SelectedIndex != 0)
                {
                    ddFrom.SelectedIndex = 1;
                }
                else if (ddFrom.SelectedIndex != 0 && ddTo.SelectedIndex == 0)
                {
                    ddTo.SelectedIndex = ddTo.Items.Count - 1;
                }

                if (ddTo.SelectedIndex < ddFrom.SelectedIndex)
                {
                    int temp = ddFrom.SelectedIndex;
                    ddFrom.SelectedIndex = ddTo.SelectedIndex;
                    ddTo.SelectedIndex = temp;
                }

                for (int i = ddFrom.SelectedIndex; i <= ddTo.SelectedIndex; i++)
                {
                    lChoiceIds.Add(Convert.ToInt32(ddFrom.Items[i].Value));
                }

                return lChoiceIds.ToArray();
            }
            set
            {
                if (value.Length == 0)
                    return;

                List<int> lChoiceIds = new List<int>(value);
                lChoiceIds.Sort();

                for (int i = 0; i < lChoiceIds.Count; ++i)
                {
                    ListItem itemFrom = ddFrom.Items.FindByValue(lChoiceIds[i].ToString());
                    ListItem itemTo = ddTo.Items.FindByValue(lChoiceIds[i].ToString());

                    if (itemFrom != null)
                    {
                        for (int j = i; j < lChoiceIds.Count; ++j)
                        {
                            ListItem temp = ddTo.Items.FindByValue(lChoiceIds[j].ToString());
                            if (temp == null)
                                break;
                            else
                                itemTo = temp;
                        }

                        if (itemTo != null)
                        {
                            ddFrom.ClearSelection();
                            ddTo.ClearSelection();
                            itemFrom.Selected = true;
                            itemTo.Selected = true;
                            break;
                        }
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
        }

        public void GenerateJSForChildVisibility(Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices)
        {
            // this control cannot be parent
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$get('" + ddFrom.ClientID + "').selectedIndex = -1;" +
                  "$get('" + ddTo.ClientID + "').selectedIndex = -1;}";
            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID + "Reset", js);
        }

        public void ClearAnswers()
        {
            ddFrom.SelectedIndex = -1;
            ddTo.SelectedIndex = -1;
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