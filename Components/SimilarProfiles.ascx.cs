using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class SimilarProfiles : System.Web.UI.UserControl
    {        
        private string username;
        private User viewedUser;
        protected SmallBoxStart SmallBoxStart1;

        public string Username
        {
            set
            {
                username = value;
            }
            get
            {
                return username;
            }
        }

        public User ViewedUser
        {
            set
            {
                viewedUser = value;
                username = value.Username;
            }
            get
            {
                if (viewedUser != null)
                {
                    return viewedUser;
                }
                else if (username != null)
                {
                    return User.Load(username);
                }
                else 
                {
                    return null;
                }                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Config.Profiles.EnableSimilarProfiles)
            {
                Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                LoadStrings();
            }                                    
        }

        private void LoadStrings()
        {
            if (SmallBoxStart1 != null)
            {
                SmallBoxStart1.Title = Lang.Trans("Similar Profiles");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
            LoadSimilarProfiles();
        }
        
        protected void dlSimilarProfiles_ItemCreated(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlGenderAge");
                pnlGenderAge.Visible = false;
            }
            else if (Config.Users.DisableAgeInformation || Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlDelimiter");
                pnlDelimiter.Visible = false;
            }
        }

        private void LoadSimilarProfiles()
        {
            if (ViewedUser == null) {
                return;
            }

            DataTable dtNewUsers;
            var spSearch = new BasicSearch();

            if (Config.Users.RequireProfileToShowInSearch)
            {
                spSearch.HasAnswer = true;
            }
            else
            {
                spSearch.hasAnswer_isSet = false;
            }

            if (!Config.Users.DisableGenderInformation)
            {
                spSearch.Gender = ViewedUser.Gender;

                if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                {
                    spSearch.InterestedIn = ViewedUser.InterestedIn;
                }
            }

            if (!Config.Users.DisableAgeInformation)
            {
                spSearch.MinAge = (ViewedUser.Age - (int)Math.Round(ViewedUser.Age * 0.1));
                spSearch.MaxAge = (ViewedUser.Age + (int)Math.Round(ViewedUser.Age * 0.1));
            }

            if (Config.Users.LocationPanelVisible)
            {
                spSearch.City = ViewedUser.City;
                spSearch.Country = ViewedUser.Country;
                spSearch.Zip = ViewedUser.ZipCode;
                spSearch.State = ViewedUser.State;
                spSearch.Distance = 500;

                spSearch.FromLocation = Config.Users.GetLocation(ViewedUser.Country, ViewedUser.State, ViewedUser.City);
            }

            UserSearchResults nuResults = spSearch.GetResults();
            dtNewUsers = new DataTable();
            dtNewUsers.Columns.Add("Username");
            dtNewUsers.Columns.Add("ImageId", typeof(int));
            dtNewUsers.Columns.Add("Age");
            dtNewUsers.Columns.Add("Age2");
            dtNewUsers.Columns.Add("Gender");
            dtNewUsers.Columns.Add("City");

            if (nuResults != null && nuResults.Usernames != null)
            {
                nuResults.Usernames = nuResults.Usernames.Except(new string[] { ViewedUser.Username }).ToArray();

                if (nuResults.Usernames.Length > Config.Profiles.NumberOfSimilarProfilesToShow)
                {
                    Random random = new Random();
                    List<int> listRandIndexProfiles = new List<int>();

                    while (listRandIndexProfiles.Count < Config.Profiles.NumberOfSimilarProfilesToShow)
                    {
                        int index = random.Next(0, nuResults.Usernames.Length);
                        if (listRandIndexProfiles.Count == 0 || !listRandIndexProfiles.Contains(index))
                        {
                            listRandIndexProfiles.Add(index);
                        }
                    }

                    string[] randomizedUsersArr = new string[Config.Profiles.NumberOfSimilarProfilesToShow];

                    for (int i = 0; i < listRandIndexProfiles.Count; i++)
                    {
                        int randomIndex = listRandIndexProfiles[i];
                        randomizedUsersArr[i] = nuResults.Usernames[randomIndex];
                    }

                    nuResults.Usernames = randomizedUsersArr;
                }

                foreach (string username in nuResults.Usernames)
                {
                    int imageId;
                    User user;

                    try
                    {
                        user = User.Load(username);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }

                    try
                    {
                        imageId = Photo.GetPrimary(username).Id;
                    }
                    catch (NotFoundException)
                    {
                        imageId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    string age = !Config.Users.DisableAgeInformation ? user.Age.ToString() : String.Empty;
                    string age2 = !Config.Users.DisableAgeInformation ? user.Age2.ToString() : String.Empty;
                    string g = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                    dtNewUsers.Rows.Add(
                        new object[]
                            {
                                username, imageId, age, user.Gender == User.eGender.Couple ? age2 : String.Empty, g, user.City
                            });
                }
            }

            if (dtNewUsers.Rows.Count > 0)
            {
                dlSimilarProfiles.DataSource = dtNewUsers;
                dlSimilarProfiles.DataBind();
            }
            else
            {                
                Visible = false;
            }
        }
    }
}