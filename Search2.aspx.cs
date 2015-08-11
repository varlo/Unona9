using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class Search2 : PageBase
    {
        private List<ProfileQuestion> profileQuestions;
        private Dictionary<int, object> dicQuestions;


        private string selectedCountry = null;
        private string selectedState = null;
        private string selectedCity = null;
        private bool setDefault = false;

        public Search2()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserSession == null && Config.Users.RegistrationRequiredToSearch)
                Response.Redirect("Login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            if (!Page.IsPostBack)
            {
                if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasProfile && !CurrentUserSession.IsAdmin())
                {
                    Response.Redirect("Profile.aspx?err=profnotcompl");
                    return;
                }

                if (Config.Users.PhotoRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasPhotos && !CurrentUserSession.IsAdmin())
                {
                    Response.Redirect("Profile.aspx?err=nophoto");
                    return;
                }

                LoadStrings();
                LoadSavedSearches();
                PrepareDefaultFields();
                LoadLastSearch();
                if (!CheckForRequests())
                    btnSearch_Click(null, null);
            }
            else
            {
                PrepareCustomSearchFields();
            }

            Page.RegisterJQuery();
        }

        private void PrepareDefaultFields()
        {
            dropGender.Items.Clear();
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            dropDistance.Items.Clear();
            if (Config.Search.MeasureDistanceInKilometers)
            {
                dropDistance.Items.Add(new ListItem("Exact Location".Translate(), "-1"));
                dropDistance.Items.Add(new ListItem("5 km".Translate(), "5"));
                dropDistance.Items.Add(new ListItem("10 km".Translate(), "10"));
                dropDistance.Items.Add(new ListItem("25 km".Translate(), "25"));
                dropDistance.Items.Add(new ListItem("50 km".Translate(), "50"));
                dropDistance.Items.Add(new ListItem("75 km".Translate(), "75"));
                dropDistance.Items.Add(new ListItem("100 km".Translate(), "100"));
                dropDistance.Items.Add(new ListItem("150 km".Translate(), "150"));
                dropDistance.Items.Add(new ListItem("250 km".Translate(), "250"));
                dropDistance.Items.Add(new ListItem("500 km".Translate(), "500"));
                dropDistance.Items.Add(new ListItem("1000 km".Translate(), "1000"));
            }
            else
            {
                dropDistance.Items.Add(new ListItem("Exact Location".Translate(), "-1"));
                dropDistance.Items.Add(new ListItem("5 Miles".Translate(), "5"));
                dropDistance.Items.Add(new ListItem("10 Miles".Translate(), "10"));
                dropDistance.Items.Add(new ListItem("25 Miles".Translate(), "25"));
                dropDistance.Items.Add(new ListItem("50 Miles".Translate(), "50"));
                dropDistance.Items.Add(new ListItem("75 Miles".Translate(), "75"));
                dropDistance.Items.Add(new ListItem("100 Miles".Translate(), "100"));
                dropDistance.Items.Add(new ListItem("150 Miles".Translate(), "150"));
                dropDistance.Items.Add(new ListItem("250 Miles".Translate(), "250"));
                dropDistance.Items.Add(new ListItem("500 Miles".Translate(), "500"));
                dropDistance.Items.Add(new ListItem("1000 Miles".Translate(), "1000"));
            }
            if (Config.Users.DisableGenderInformation)
            {
                divGender.Visible = false;
                divInterestedIn.Visible = false;
                dropGender.SelectedIndex = 0;
            }
            else
            {
                if (!Config.Users.InterestedInFieldEnabled)
                {
                    divInterestedIn.Visible = false;

                    if (CurrentUserSession != null)
                    {
                        dropGender.SelectedValue = (CurrentUserSession.Gender == Classes.User.eGender.Female
                                                        ? (int)Classes.User.eGender.Male
                                                        : (int)Classes.User.eGender.Female).ToString();
                    }
                    else
                    {
                        dropGender.SelectedValue = ((int)Classes.User.eGender.Female).ToString();
                    }
                }
                else
                {
                    dropInterestedIn.Items.Clear();
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Male"), ((int) Classes.User.eGender.Male).ToString()));
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Female"), ((int) Classes.User.eGender.Female).ToString()));
                    if (Config.Users.CouplesSupport)
                    {
                        dropInterestedIn.Items.Add(
                            new ListItem(Lang.Trans("Couple"), ((int) Classes.User.eGender.Couple).ToString()));
                    }

                    if (CurrentUserSession != null)
                    {
                        dropInterestedIn.SelectedValue = ((int) CurrentUserSession.Gender).ToString();
                        dropGender.SelectedValue = ((int) CurrentUserSession.InterestedIn).ToString();
                    }
                }
            }

            if (Config.Users.DisableAgeInformation)
            {
                divAge.Visible = false;
            }
            else
            {
                txtAgeFrom.Text = Config.Users.MinAge.ToString();
                txtAgeTo.Text = Config.Users.MaxAge.ToString();
            }

            if (!Config.Users.LocationPanelVisible)
                divLocation.Visible = false;
        }

        private void PrepareCustomSearchFields()
        {
            PrepareCustomSearchFields(null, false);
        }

        private void LoadLastSearch()
        {
            PrepareCustomSearchFields(null, true);
        }

        private void PrepareCustomSearchFields(SavedSearch savedSearch)
        {
            PrepareCustomSearchFields(savedSearch, false);
        }

        /// <summary>
        /// Prepares the custom search fields.
        /// </summary>
        /// <param name="savedSearch">The saved search.</param>
        /// <param name="loadLastSearch">if set to <c>true</c> [load last search].</param>
        private void PrepareCustomSearchFields(SavedSearch savedSearch, bool loadLastSearch)
        {
            profileQuestions = new List<ProfileQuestion>();
            dicQuestions = new Dictionary<int, object>();
            plhCustomSearch.Controls.Clear();

            SavedSearch lastSearch = null;

            if (CurrentUserSession != null)
            {
                if (savedSearch != null)
                    lastSearch = savedSearch;
                else if (loadLastSearch)
                    lastSearch = SavedSearch.Load(CurrentUserSession.Username, "_lastsearch_");

                if (lastSearch != null)
                {
                    dropGender.SelectedValue = ((int) lastSearch.Gender).ToString();
                    if (Config.Users.LocationPanelVisible)
                    {
                        selectedCountry = lastSearch.Country;
                        selectedState = lastSearch.State;
                        selectedCity = lastSearch.City;
                        txtZip.Text = lastSearch.Zip;
                    }

                    dropGender.SelectedValue = Convert.ToInt32(lastSearch.Gender).ToString();
                    txtAgeFrom.Text = lastSearch.AgeFrom.ToString();
                    txtAgeTo.Text = lastSearch.AgeTo.ToString();
                    cbPhotoReq.Checked = lastSearch.PhotoRequired;
                    cbOnlineOnly.Checked = lastSearch.OnlineOnly;
                }
            }

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();

            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    #region Add boxes for topics

                    var divTopicContainer = new HtmlGenericControl("div");
                    plhCustomSearch.Controls.Add(divTopicContainer);

                    var ltrTopicHeading = new LiteralControl(
                        String.Format("<div class=\"panel-heading\"><h4 class=\"panel-title\"><a class=\"collapsed\" data-toggle=\"collapse\" href=\".topic_{1}\">{0}</a></h4></div>",
                                      Config.Misc.EnableProfileDataTranslation ? Lang.Trans(topic.Name) : topic.Name, topic.ID));

                    divTopicContainer.Controls.Add(ltrTopicHeading);

                    var ltrTopicBody = new HtmlGenericControl("div");
                    ltrTopicBody.Attributes.Add("class", "panel-collapse collapse topic_" + topic.ID);
                    //ltrTopicBody.Style.Add("display", "none");

                    divTopicContainer.Controls.Add(ltrTopicBody);

                    #endregion

                    ProfileQuestion[] questions = topic.FetchQuestions();
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
                                case ProfileQuestion.eSearchStyle.MultiChoiceSelect:
                                case ProfileQuestion.eSearchStyle.MultiChoiceCheck:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchMultiChoiceCheck2.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.RangeChoiceSelect:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchRangeChoiceSelect2.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.SingleChoice:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchSingleChoice2.ascx");
                                    break;

                                default:
                                    break;
                            }

                            if (cProfile == null) continue;

                            ((Control) cProfile).ID = question.Id.ToString();

                            cProfile.Question = question;

                            if (lastSearch != null)
                            {
                                cProfile.ChoiceIds = lastSearch.ChoiceIds;
                            }

                            if (!question.VisibleForPaidOnly ||
                                (CurrentUserSession != null && CurrentUserSession.Paid))
                            {
                                var divQuestionContainer = new HtmlGenericControl("div");
                                divQuestionContainer.Controls.Add((Control)cProfile);
                                ltrTopicBody.Controls.Add(divQuestionContainer);

                                dicQuestions.Add(question.Id, (Control) cProfile);

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
                                                                : "invisibleforcouple") + " panel-body";


                                    divQuestionContainer.Attributes["class"] = genderClasses.Trim();

                                    if ((Int32.Parse(dropGender.SelectedValue) == (int) Classes.User.eGender.Male
                                         && !question.VisibleForMale) ||
                                        (Int32.Parse(dropGender.SelectedValue) == (int) Classes.User.eGender.Female
                                         && !question.VisibleForFemale) ||
                                        (Int32.Parse(dropGender.SelectedValue) == (int) Classes.User.eGender.Couple
                                         && !question.VisibleForCouple))
                                    {
                                        divQuestionContainer.Style["display"] = "none";
                                    }
                                }
                            }

                            profileQuestions.Add(question);
                        }
                    }

                    #region Remove empty topics

                    if (ltrTopicBody.Controls.Count == 0)
                    {
                        plhCustomSearch.Controls.Remove(divTopicContainer);
                    }

                    if (questions != null)
                    {
                        string genderClasses =
                            (questions.Any(q => q.VisibleForMale) ? "visibleformale" : "invisibleformale") + " " +
                            (questions.Any(q => q.VisibleForFemale) ? "visibleforfemale" : "invisibleforfemale") + " " +
                            (questions.Any(q => q.VisibleForCouple) ? "visibleforcouple" : "invisibleforcouple") + " panel";

                        divTopicContainer.Attributes["class"] = genderClasses;

                        if ((Int32.Parse(dropGender.SelectedValue) == (int) Classes.User.eGender.Male
                             && !questions.Any(q => q.VisibleForMale)) ||
                            (Int32.Parse(dropGender.SelectedValue) == (int) Classes.User.eGender.Female
                             && !questions.Any(q => q.VisibleForFemale)) ||
                            (Int32.Parse(dropGender.SelectedValue) == (int) Classes.User.eGender.Couple
                             && !questions.Any(q => q.VisibleForCouple)))
                        {
                            divTopicContainer.Style["display"] = "none";
                        }
                    }

                    #endregion
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Expand topics where values are already selected
            var controls = new List<IProfileSearchComponent>(
                Misc.Select<IProfileSearchComponent>(plhCustomSearch));

            // Find all selections and unhide the topics
            foreach (var control in controls.Where(c => c.Answers.Length > 0))
            {
                var topicId = control.Answers[0].Question.TopicID;
                var topic = Misc.Select<HtmlGenericControl>(
                    plhCustomSearch,
                    c => ((HtmlGenericControl)c).Attributes["class"] != null &&
                        ((HtmlGenericControl)c).Attributes["class"].Contains("topic_" + topicId));
                if (topic == null || topic.Count() == 0) continue;
                topic.First().Style["display"] = "";
            }

            // Run cascade controls show/hide logic
            SetCascadeQuestions(profileQuestions.ToArray(), dicQuestions);

            cbSaveSearch.Checked = false;


            CascadingDropDown.SetupLocationControls(this, dropCountry, dropRegion, dropCity, setDefault,
                selectedCountry, selectedState, selectedCity);
            divLocationExpandee.Style["display"] = string.IsNullOrEmpty(dropCountry.SelectedValue()) && string.IsNullOrEmpty(selectedCountry)
                                                       ? "none"
                                                       : "";
        }

        private void LoadStrings()
        {
            SmallBoxStart1.Title = "Search Options".Translate();
            LargeBoxStart1.Title = "Search Results".Translate();
            cbPhotoReq.Text = "Photo is required".Translate();
            btnSearch.Text = "Search".Translate();
            cbOnlineOnly.Text = "Online users only".Translate();

            if (CurrentUserSession != null)
            {
                cbSaveSearch.Text = "Save this search".Translate();
                cbEmailSavedSearch.Text = "Use this search to send me new matching profiles".Translate();
                ddEmailFrequency.Items.Add(new ListItem(Lang.Trans("Weekly"), "7"));
                ddEmailFrequency.Items.Add(new ListItem(Lang.Trans("Semimonthly"), "14"));
                ddEmailFrequency.Items.Add(new ListItem(Lang.Trans("Monthly"), "30"));
            }
            else
            {
                cbSaveSearch.Visible = false;
            }
        }

        private void LoadSavedSearches()
        {
            if (CurrentUserSession != null)
            {
                SavedSearch[] savedSearches = SavedSearch.Load(CurrentUserSession.Username);

                var dtSavedSearches = new DataTable();
                dtSavedSearches.Columns.Add("ID", typeof(int));
                dtSavedSearches.Columns.Add("Name", typeof(string));

                foreach (SavedSearch savedSearch in savedSearches)
                {
                    if (savedSearch.Name == "_lastsearch_")
                    {
                        continue;
                    }
                    dtSavedSearches.Rows.Add(new object[] { savedSearch.Id, savedSearch.Name });
                }

                dlSavedSearches.DataSource = dtSavedSearches;
                dlSavedSearches.DataBind();

                lnkSavedSearches.Visible = savedSearches.Length > 1;
            }
        }

        private void SaveSavedSearch(string name, int[] lSearchIDs)
        {
            var gender = (User.eGender)Convert.ToInt32(dropGender.SelectedValue);

            string country = "";
            string state = "";
            string zip = "";
            string city = "";

            if (Config.Users.LocationPanelVisible)
            {
                country = selectedCountry ?? dropCountry.SelectedValue();
                state = selectedState ?? dropRegion.SelectedValue();
                city = selectedCity ?? dropCity.SelectedValue();
                zip = txtZip.Text;
            }

            int ageFrom = Config.Users.MinAge;
            int ageTo = Config.Users.MaxAge;

            try
            {
                ageFrom = Convert.ToInt32(txtAgeFrom.Text);
                ageTo = Convert.ToInt32(txtAgeTo.Text);
            }
            catch (FormatException)
            {
            }

            bool photoRequired = cbPhotoReq.Checked;
            bool emailMatches = false;
            int emailFrequency = 7;
            DateTime? nextEmailDate = null;

            if (cbSaveSearch.Checked && name != "_lastsearch_")
            {
                emailMatches = cbEmailSavedSearch.Checked;
                emailFrequency = Convert.ToInt32(ddEmailFrequency.SelectedValue);
                nextEmailDate = findNextFriday(DateTime.Now);
            }

            SavedSearch savedSearch = SavedSearch.Load(CurrentUserSession.Username, name);

            if (savedSearch == null)
                savedSearch = SavedSearch.Create(CurrentUserSession.Username, name,
                                                 gender, country, state, zip, city, ageFrom,
                                                 ageTo, photoRequired, lSearchIDs,
                                                 emailMatches, emailFrequency, nextEmailDate);
            else
            {
                savedSearch.Gender = gender;
                savedSearch.Country = country;
                savedSearch.State = state;
                savedSearch.Zip = zip;
                savedSearch.City = city;
                savedSearch.AgeFrom = ageFrom;
                savedSearch.AgeTo = ageTo;
                savedSearch.PhotoRequired = photoRequired;
                savedSearch.ChoiceIds = lSearchIDs;
                savedSearch.EmailMatches = emailMatches;
                savedSearch.EmailFrequency = emailFrequency;
                savedSearch.NextEmailDate = nextEmailDate;
            }

            savedSearch.OnlineOnly = cbOnlineOnly.Checked;
            savedSearch.Save();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            UserSearchResults results;
            var username = txtUsername.Text.Trim();

            if (username.Length > 0)
            {
                UsernameSearch usernameSearch = new UsernameSearch();

                usernameSearch.Username = username;
                results = usernameSearch.GetResults();

                SearchResults.ShowLastOnline = true;
                SearchResults.ShowViewedOn = false;
                SearchResults.ShowDistance = true;

                SearchResults.Results = results;
                return;
            }
            var search = new CustomSearch();
            
            if (Config.Users.RequireProfileToShowInSearch)
            {
                search.HasAnswer = true;
            }
            else
            {
                search.hasAnswer_isSet = false;
            }

            if (!Config.Users.DisableGenderInformation)
            {
                search.Gender = (User.eGender)
                                Convert.ToInt32(dropGender.SelectedValue);
                if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                    search.InterestedIn = (User.eGender)
                                          Convert.ToInt32(dropInterestedIn.SelectedValue);
            }

            if (!Config.Users.DisableAgeInformation)
            {
                try
                {
                    search.MinAge = Convert.ToInt32(txtAgeFrom.Text);
                    search.MaxAge = Convert.ToInt32(txtAgeTo.Text);
                }
                catch
                {
                }
            }

            if (Config.Users.LocationPanelVisible)
            {
                if (!IsPostBack && (CurrentUserSession == null || SavedSearch.Load(CurrentUserSession.Username, "_lastsearch_") == null))
                {
                    selectedCountry =
                        String.IsNullOrEmpty(Config.Users.ForceCountry) ? Config.Users.DefaultCountry : Config.Users.ForceCountry;
                    search.Country = selectedCountry;
                    setDefault = true;
                }
                else
                {
                    if (dropCountry != null)
                    {
                        search.Country = selectedCountry ?? dropCountry.SelectedValue();
                    }
                    if (dropRegion != null)
                    {
                        search.State = selectedState ?? dropRegion.SelectedValue();
                    }
                    if (dropCity != null)
                    {
                        search.City = selectedCity ?? dropCity.SelectedValue();
                    }
                }
                if (txtZip != null)
                {
                    search.Zip = txtZip.Text;
                }

                if (dropDistance.SelectedValue != "-1")
                {
                    search.FromLocation = Config.Users.GetLocation(search.Country, search.State, search.City);
                    //if (CurrentUserSession != null && CurrentUserSession.Latitude.HasValue && CurrentUserSession.Longitude.HasValue)
                    //{
                    //    search.FromLocation = new Location { Longitude = CurrentUserSession.Longitude.Value, Latitude = CurrentUserSession.Latitude.Value };
                    //    search.Distance = Int32.Parse(dropDistance.SelectedValue);
                    //}
                    var miles = Int32.Parse(dropDistance.SelectedValue);
                    search.Distance = Config.Search.MeasureDistanceInKilometers ? miles * 0.621371192 : miles;
                }
            }

            if (cbPhotoReq.Checked)
                search.HasPhoto = true;

            search.OnlineOnly = cbOnlineOnly.Checked;

            var lSearchTerms = new List<ProfileAnswer[]>();
            var lSearchIDs = new List<int>();

            var controls = new List<IProfileSearchComponent>(
                Misc.Select<IProfileSearchComponent>(plhCustomSearch));

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
                    lSearchIDs.AddRange(searchTerm.ChoiceIds);
                }
            }

            if (Page.IsPostBack && CurrentUserSession != null)
            {
                SaveSavedSearch("_lastsearch_", lSearchIDs.ToArray());

                if (cbSaveSearch.Checked)
                {
                    if (txtSavedSearchName.Text == "")
                    {
                        return;
                    }
                    SaveSavedSearch(txtSavedSearchName.Text, lSearchIDs.ToArray());
                    LoadSavedSearches();
                }
            }

            search.Answers = lSearchTerms.ToArray();


            results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            cbSaveSearch.Checked = false;
        }

        protected void dlSavedSearches_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SavedSearchView")
            {
                PrepareCustomSearchFields(SavedSearch.Load(Convert.ToInt32(e.CommandArgument)));
                txtSavedSearchName.Text = ((LinkButton)e.Item.FindControl("lnkSavedSearch")).Text;
                btnSearch_Click(null, null);
            }
            if (e.CommandName == "SavedSearchDelete")
            {
                txtSavedSearchName.Text = "";
                cbSaveSearch.Checked = false;
                int id = Convert.ToInt32(e.CommandArgument);
                SavedSearch.Delete(id);
                LoadSavedSearches();
            }
        }

        #region Cascade questions methods

        private void SetCascadeQuestions(ProfileQuestion[] questions, Dictionary<int, object> dicQuestions)
        {
            var lHiddenParentQuestions = new List<int>();

            foreach (ProfileQuestion question in questions)
            {
                ProfileQuestion[] childQuestions =
                    questions.Where(q => q.ParentQuestionID.HasValue
                                         && q.ParentQuestionID.Value == question.Id).ToArray();

                bool isParent = childQuestions.Length > 0;
                bool isChild = question.ParentQuestionID.HasValue;
                if (!dicQuestions.ContainsKey(question.Id)) // if current question is hidden
                    continue;
                var currentQuestionControl = (Control) dicQuestions[question.Id];

                if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                    ((ICascadeQuestionComponent) currentQuestionControl).GenerateResetValuesJS();

                if (isParent)
                {
                    var childClientIDsWithParentQuestionChoices = new Dictionary<string, object[]>();
                    GetChildrenClientIDs(question, questions, dicQuestions, childClientIDsWithParentQuestionChoices);

                    if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                        ((ICascadeQuestionComponent) currentQuestionControl).GenerateJSForChildVisibility(
                            childClientIDsWithParentQuestionChoices);
                    else
                        new Exception(String.Format("{0} control must implement ICascadeQuestionComponent",
                                                    currentQuestionControl.ID));
                }

                if (isChild)
                {
                    // if parent question is hidden hide the child
                    if (!dicQuestions.ContainsKey(question.ParentQuestionID.Value)
                        || lHiddenParentQuestions.Contains(question.ParentQuestionID.Value))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileSearchComponent) currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                           "display:none");
                        ((IProfileSearchComponent) currentQuestionControl).ClearAnswers();
                        continue;
                    }
                    var currentQuestionParentControl = (Control) dicQuestions[question.ParentQuestionID.Value];
                    var parentAnswers = new List<string>();
                    foreach (ProfileAnswer answer in ((IProfileSearchComponent) currentQuestionParentControl).Answers)
                    {
                        parentAnswers.Add(answer.Value);
                    }
                    if (
                        !question.ParentQuestionChoices.Split(':').Any(
                             parentChoice => parentAnswers.Contains(parentChoice)))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileSearchComponent) currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                           "display:none");
                        ((IProfileSearchComponent) currentQuestionControl).ClearAnswers();
                    }
                }
            }
        }

        private void GetChildrenClientIDs(ProfileQuestion question, ProfileQuestion[] questions,
                                          Dictionary<int, object> dicQuestions,
                                          Dictionary<string, object[]> childClientIDsWithParentQuestionChoices)
        {
            ProfileQuestion[] childQuestions =
                questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
            if (childQuestions.Length > 0)
            {
                foreach (ProfileQuestion childQuestion in childQuestions)
                {
                    var childClientIDs = new List<string>();

                    // child question is not visible so skip it
                    if (dicQuestions.ContainsKey(childQuestion.Id))
                    {
                        string childClientID =
                            ((IProfileSearchComponent) dicQuestions[childQuestion.Id]).UserControlPanel.ClientID;
                        string[] parentQuestionChoices =
                            childQuestion.ParentQuestionChoices.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);

                        childClientIDsWithParentQuestionChoices.Add(childClientID,
                                                                    new object[] {parentQuestionChoices, childClientIDs});
                    }

                    PopulateChildrenIDs(childQuestion, questions, dicQuestions, childClientIDs);
                }
            }
        }

        private void PopulateChildrenIDs(ProfileQuestion question, ProfileQuestion[] questions,
                                         Dictionary<int, object> dicQuestions, List<string> childClientIDs)
        {
            ProfileQuestion[] childQuestions =
                questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
            if (childQuestions.Length > 0)
            {
                foreach (ProfileQuestion childQuestion in childQuestions)
                {
                    // child question is not visible so skip it
                    if (dicQuestions.ContainsKey(childQuestion.Id))
                    {
                        string childClientID =
                            ((IProfileSearchComponent) dicQuestions[childQuestion.Id]).UserControlPanel.ClientID;

                        childClientIDs.Add(childClientID);
                    }

                    PopulateChildrenIDs(childQuestion, questions, dicQuestions, childClientIDs);
                }
            }
        }

        #endregion

        #region Helper methods

        private DateTime findNextFriday(DateTime date)
        {
            do date = date.AddDays(1);
            while (date.DayOfWeek != DayOfWeek.Friday);

            return date;
        }

        #endregion

        private bool CheckForRequests()
        {
            if (Session["BasicSearchRequest"] != null)
            {
                ProcessBasicSearchRequest();
                return true;
            }

            if (Session["CustomSearchRequest"] != null)
            {
                ProcessCustomSearchRequest();
                return true;
            }

            if (Session["UsernameSearchRequest"] != null)
            {
                ProcessUsernameSearchRequest();
                return true;
            }

            if (Session["NewUsersSearch"] != null)
            {
                Session.Remove("NewUsersSearch");
                ProcessNewUsersSearchRequest();
                return true;
            }

            if (Session["BroadcastingUsersSearch"] != null)
            {
                Session.Remove("BroadcastingUsersSearch");
                ProcessBroadcastingUsersSearchRequest();
                return true;
            }

            if (Session["OnlineUsersSearch"] != null)
            {
                Session.Remove("OnlineUsersSearch");
                ProcessOnlineSearchRequest();
                return true;
            }

            if (Session["ShowProfileViewers"] != null)
            {
                Session.Remove("ShowProfileViewers");
                ShowProfileViewers();
                return true;
            }

            if (Session["MutualVoteSearch"] != null)
            {
                Session.Remove("MutualVoteSearch");
                ShowMutualVotes();
                return true;
            }

            if (Session["MutualFriendsSearch"] != null)
            {
                ShowMutualFriends();
                return true;
            }

            return false;
        }

        private void ProcessBasicSearchRequest()
        {
            var search = (BasicSearch)Session["BasicSearchRequest"];
            Session.Remove("BasicSearchRequest");

            var results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        private void ProcessCustomSearchRequest()
        {
            var search = (CustomSearch)Session["CustomSearchRequest"];
            Session.Remove("CustomSearchRequest");

            var results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        private void ProcessUsernameSearchRequest()
        {
            var search = (UsernameSearch)Session["UsernameSearchRequest"];
            Session.Remove("UsernameSearchRequest");

            UserSearchResults results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        private void ProcessNewUsersSearchRequest()
        {
            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.PhotoReq = Config.Users.RequirePhotoToShowInNewUsers;
            nuSearch.ProfileReq = Config.Users.RequireProfileToShowInNewUsers;
            nuSearch.UsersSince = CurrentUserSession.PrevLogin;

            UserSearchResults nuResults = nuSearch.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = nuResults;
        }

        private void ProcessBroadcastingUsersSearchRequest()
        {
            VideoBroadcastingSearch search = new VideoBroadcastingSearch();

            UserSearchResults results = search.GetResults();
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        protected void ProcessOnlineSearchRequest()
        {
            OnlineSearch search = new OnlineSearch();

            UserSearchResults results = search.GetResults();
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        private void ShowProfileViewers()
        {
            if (CurrentUserSession == null) return;

            string[] profileViewersUsernames = Classes.User.FetchProfileViews(CurrentUserSession.Username,
                                                                              DateTime.MinValue);
            UserSearchResults pvResults = null;

            if (profileViewersUsernames != null)
            {
                pvResults = new UserSearchResults();
                pvResults.Usernames = profileViewersUsernames;
            }

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = true;
            SearchResults.ShowDistance = true;

            SearchResults.ShowViewedOnUsername = CurrentUserSession.Username;
            SearchResults.Results = pvResults;
        }

        private void ShowMutualVotes()
        {
            MutualVoteSearch mutualVoteSearch = new MutualVoteSearch();
            mutualVoteSearch.Username = CurrentUserSession.Username;

            UserSearchResults results = mutualVoteSearch.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        private void ShowMutualFriends()
        {
            var search = (MutualFriendsSearch)Session["MutualFriendsSearch"];
            UserSearchResults results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            Session.Remove("MutualFriendsSearch");

            if (results == null)
            {
                ShowFriendsConnection(search.Viewer, search.Viewed);
            }
        }

        private void ShowFriendsConnection(string viewer, string viewed)
        {
            var search = new FriendsConnectionSearch { Viewer = viewer, Viewed = viewed };
            UserSearchResults results = search.GetResults();

            SearchResults.ShowFriendsPath = true;
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;
            SearchResults.GridMode = false;

            SearchResults.Results = results;
        }
    }
}