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
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;

namespace AspNetDating.Components.Search
{
    public partial class SearchBox : UserControl
    {
        protected SearchResults SearchResults;
        protected Button btnBasicSearchGo;
        protected LargeBoxStart LargeBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareCustomProfileQuestions();
        }

        private void PrepareCustomProfileQuestions()
        {
            var questions = ProfileQuestion.Fetch().Where(p=>p.VisibleInSearchBox);

            if (questions != null)
            {
                foreach (ProfileQuestion question in questions)
                {
                    if (question.SearchStyle == ProfileQuestion.eSearchStyle.Hidden ||
                        !(question.VisibleForMale || question.VisibleForFemale || question.VisibleForCouple))
                        continue;

                    IProfileSearchComponent cProfile = null;

                    switch (question.SearchStyle)
                    {
                        case ProfileQuestion.eSearchStyle.SingleChoice:
                        case ProfileQuestion.eSearchStyle.MultiChoiceSelect:
                        case ProfileQuestion.eSearchStyle.MultiChoiceCheck:
                            cProfile = (IProfileSearchComponent)
                                LoadControl("~/Components/Search/SearchBoxDropDown.ascx");
                            break;
                        default:
                            break;
                    }

                    if (cProfile == null) continue;

                    ((Control)cProfile).ID = question.Id.ToString();

                    cProfile.Question = question;


                    if (!question.VisibleForPaidOnly)
                    {
                        var divQuestionContainer = new HtmlGenericControl("div");
                        divQuestionContainer.Controls.Add((Control)cProfile);

                        var row = new HtmlGenericControl("div");
                        var td = new HtmlGenericControl("label");
                        td.Attributes.Add("class", "control-label col-sm-5");
                        var td2 = new HtmlGenericControl("div");
                        td2.Attributes.Add("class", "col-sm-7");
                        row.Controls.Add(td);
                        row.Controls.Add(td2);
                        td.InnerText = Config.Misc.EnableProfileDataTranslation
                            ? Lang.Trans(question.AltName) : question.AltName;

                        td2.Controls.Add(divQuestionContainer);
                        phProfileQuestions.Controls.Add(row);

                        if (!Config.Users.DisableGenderInformation)
                        {
                            string genderClasses = (cProfile.UserControlPanel.Attributes["class"] ?? "")
                                                   + " " +
                                                   (question.VisibleForMale
                                                        ? "visibleformale"
                                                        : "invisibleformale")
                                                   + " " +
                                                   (question.VisibleForFemale
                                                        ? "visibleforfemale"
                                                        : "invisibleforfemale")
                                                   + " " +
                                                   (question.VisibleForCouple
                                                        ? "visibleforcouple"
                                                        : "invisibleforcouple");


                            divQuestionContainer.Attributes["class"] = genderClasses.Trim();
                            row.Attributes["class"] = genderClasses.Trim() + " form-group";

                            if (dropGender.SelectedValue.Length > 0)
                            {
                                var selectedGender = (Classes.User.eGender)Int32.Parse(dropGender.SelectedValue);

                                if ((selectedGender == Classes.User.eGender.Male && !question.VisibleForMale) ||
                                    (selectedGender == Classes.User.eGender.Female && !question.VisibleForFemale) ||
                                    (selectedGender == Classes.User.eGender.Couple && !question.VisibleForCouple))
                                {
                                    divQuestionContainer.Style["display"] = "none";
                                    row.Style["display"] = "none";
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            LoadStrings();
            CascadingDropDown.SetupLocationControls(this.Page, dropCountry, dropRegion, dropCity, !IsPostBack);
            base.OnPreRender(e);
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

        private void LoadStrings()
        {
            if (LargeBoxStart1 != null)
                LargeBoxStart1.Title = Lang.Trans("Quick Search");

            if (!IsPostBack)
            {
                txtAgeFrom.Text = Config.Users.MinAge.ToString();
                txtAgeTo.Text = Config.Users.MaxAge.ToString();
            }

            dropGender.Items.Clear();
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)User.eGender.Couple).ToString()));
            }

            if (Config.Users.LocationPanelVisible)
                ShowLocation();
            else
                HideLocation();

            if (btnBasicSearchGo != null)
                btnBasicSearchGo.Text = Lang.Trans("Search");
            fbBasicSearchGo.Text = Lang.Trans("Search");

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
        }

        private void btnBasicSearchGo_Click(object sender, EventArgs e)
        {
            if (txtUsername != null && txtUsername.Text != String.Empty)
            {
                UsernameSearch search = new UsernameSearch();
                search.Username = txtUsername.Text;

                if (Config.Users.RequireProfileToShowInSearch)
                {
                    search.HasAnswer = true;
                }

                Session["UsernameSearchRequest"] = search;
            }
            else
            {
                var search = new CustomSearch();

                if (Config.Users.RequireProfileToShowInSearch)
                {
                    search.HasAnswer = true;
                }
                else
                {
                    search.hasAnswer_isSet = false;
                }

                search.Gender = (User.eGender)
                                Convert.ToInt32(dropGender.SelectedValue);
                try
                {
                    search.MinAge = Convert.ToInt32(txtAgeFrom.Text);
                    search.MaxAge = Convert.ToInt32(txtAgeTo.Text);
                }
                catch (ArgumentException)
                {
                }
                catch (FormatException)
                {
                }

                search.Country = dropCountry.SelectedValue().Trim();
                search.State = dropRegion.SelectedValue();
                search.City = dropCity.SelectedValue();


                var lSearchTerms = new List<ProfileAnswer[]>();

                var controls = new List<IProfileSearchComponent>(
                    Misc.Select<IProfileSearchComponent>(phProfileQuestions));

                foreach (IProfileSearchComponent searchTerm in controls)
                {
                    if (searchTerm.Answers != null && searchTerm.Answers.Length > 0)
                    {
                        ProfileQuestion question = ProfileQuestion.Fetch(searchTerm.Answers[0].Question.Id);

                        if (!Config.Users.DisableGenderInformation &&
                            (!question.VisibleForMale && search.Gender == Classes.User.eGender.Male
                             || !question.VisibleForFemale && search.Gender == Classes.User.eGender.Female
                             || !question.VisibleForCouple && search.Gender == Classes.User.eGender.Couple))
                            continue;

                        if (question.ParentQuestionID.HasValue)
                        {
                            var parentControl = controls.FirstOrDefault(
                                    c =>
                                    c != null && c.Answers.Length > 0 &&
                                    c.Answers[0].Question.Id == question.ParentQuestionID);
                            if (parentControl != null)
                            {
                                string[] parentAnswers = parentControl.Answers.Select(a => a.Value).ToArray();
                                if (!question.ParentQuestionChoices.Split(':').Any(parentChoice => parentAnswers.Contains(parentChoice)))
                                {
                                    continue;
                                }
                            }
                        }

                        lSearchTerms.Add(searchTerm.Answers);
                    }
                }

                search.Answers = lSearchTerms.ToArray();

                Session["CustomSearchRequest"] = search;
            }

            if (Config.BackwardCompatibility.UseClassicSearchPage)
                Response.Redirect("Search.aspx");
            else
                Response.Redirect("Search2.aspx");
        }

        protected void fbBasicSearchGo_Click(object sender, EventArgs e)
        {
            btnBasicSearchGo_Click(sender, null);
        }

        private void HideLocation()
        {
            if (trCountry != null)
            {
                trCountry.Visible = false;
            }
            if (trState != null)
            {
                trState.Visible = false;
            }
            if (trCity != null)
            {
                trCity.Visible = false;
            }
        }

        private void ShowLocation()
        {
            if (trCountry != null)
            {
                trCountry.Visible = true;
            }
            if (trState != null)
            {
                trState.Visible = true;
            }
            if (trCity != null)
            {
                trCity.Visible = true;
            }
        }
    }
}