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
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Xml.Serialization;
using AspNetDating.Model;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// This class handles the user accounts
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// The result when you try to add a favourite
        /// </summary>
        public enum eAddFavouriteResult
        {
            /// <summary>
            /// Invalid username
            /// </summary>
            eInvalidUsername = 0,
            /// <summary>
            /// Success
            /// </summary>
            eSuccess = 1,
            /// <summary>
            /// Already added
            /// </summary>
            eAlreadyAdded = 2,
            /// <summary>
            /// The maximum number of favories has been reached
            /// </summary>
            eMaximumFavouritesReached = 3
        }

        public enum ePrivacyLevel
        {
            Everyone,
            RegisteredUsersOnly,
            FriendsOnly,
            FriendsOfFriends
        }

        public enum eAddFriendResult
        {
            /// <summary>
            /// Invalid username
            /// </summary>
            eInvalidUsername = 0,
            /// <summary>
            /// Success
            /// </summary>
            eSuccess = 1,
            /// <summary>
            /// Already added
            /// </summary>
            eAlreadyAdded = 2,
            /// <summary>
            /// The maximum number of friends has been reached
            /// </summary>
            eMaximumFriendsReached = 3
        }

        #region Properties

        private string username;

        /// <summary>
        /// The username for the user account
        /// </summary>
        /// <value>The username.</value>
        /// <exception cref="ArgumentException">Throwed is username is invalid</exception>
        public string Username
        {
            get { return username; }
            set
            {
                try
                {
                    ValidateUsername(value);
                }
                catch (ArgumentNullException)
                {
                    throw new ArgumentException
                        (Lang.Trans("Invalid username!"));
                }

                username = value;
            }
        }

        private string password;

        /// <summary>
        /// The password for the user account.
        /// The property is write-only.
        /// </summary>
        /// <value>The password.</value>
        /// <exception cref="ArgumentException">Throwed if the password is invalid</exception>
        public string Password
        {
            get { return password; }
            set
            {
                try
                {
                    ValidatePassword(value);
                }
                catch
                {
                    throw new ArgumentException
                        (Lang.Trans("Invalid password!") /*, "Password"*/);
                }

                password = FormsAuthentication
                    .HashPasswordForStoringInConfigFile(value, "sha1");
            }
        }

        private string email;

        /// <summary>
        /// The e-mail address for the user account.
        /// </summary>
        /// <value>The email address.</value>
        /// <exception cref="ArgumentException">Throwed if the e-mail address is invalid</exception>
        public string Email
        {
            get { return email; }
            set
            {
                try
                {
                    ValidateEmail(value);
                }
                catch
                {
                    throw new ArgumentException
                        (Lang.Trans("Invalid e-mail address!"));
                }

                email = value;
            }
        }

        private string name;

        /// <summary>
        /// The first and last name of the user
        /// </summary>
        /// <value>The name.</value>
        /// <exception cref="ArgumentException">Throwed if no name is specified</exception>
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException
                        ("Please specify name!");
                }
                name = value;
            }
        }

        /// <summary>
        /// The gender of the user
        /// </summary>
        public enum eGender
        {
            /// <summary>
            /// Male
            /// </summary>
            Male = 1,
            /// <summary>
            /// Female
            /// </summary>
            Female = 2,
            /// <summary>
            /// Couple
            /// </summary>
            Couple = 3
        }

        /// <summary>
        /// The gender of the search results
        /// </summary>
        public enum eGenderSearch
        {
            /// <summary>
            /// All genders
            /// </summary>
            All = 0,
            /// <summary>
            /// Only males
            /// </summary>
            Male = 1,
            /// <summary>
            /// Only females
            /// </summary>
            Female = 2,
            /// <summary>
            /// Only couples
            /// </summary>
            Couple = 3
        }

        private eGender gender;

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        /// <summary>
        /// The zodiac signs
        /// </summary>
        public enum eZodiacSign
        {
            /// <summary>
            /// 
            /// </summary>
            Aries = 1, //Mar 21- Apr 20
            /// <summary>
            /// 
            /// </summary>
            Taurus = 2, //Apr 21 - May 21
            /// <summary>
            /// 
            /// </summary>
            Gemini = 3, //May 22 - Jun 21
            /// <summary>
            /// 
            /// </summary>
            Cancer = 4, //Jun 22 - July 23
            /// <summary>
            /// 
            /// </summary>
            Leo = 5, //Jul 24 - Aug 23
            /// <summary>
            /// 
            /// </summary>
            Virgo = 6, //Aug 24 - Sep 23
            /// <summary>
            /// 
            /// </summary>
            Libra = 7, //Sep 24 - Oct 22
            /// <summary>
            /// 
            /// </summary>
            Scorpio = 8, //Oct 23 - Nov 22
            /// <summary>
            /// 
            /// </summary>
            Sagittarius = 9, //Nov 23 - Dec 21
            /// <summary>
            /// 
            /// </summary>
            Capricorn = 10, //Dec 22 - Jan 20
            /// <summary>
            /// 
            /// </summary>
            Aquarius = 11, //Jan 21 - Feb 19
            /// <summary>
            /// 
            /// </summary>
            Pisces = 12 //Feb 20 - Mar 20
        }

        private eZodiacSign zodiacSign1;
        private eZodiacSign zodiacSign2;

        /// <summary>
        /// Gets the zodiac sign.
        /// </summary>
        /// <value>The zodiac sign.</value>
        public eZodiacSign ZodiacSign1
        {
            get
            {
                if (birthdate == default(DateTime))
                    throw new Exception("Birthdate must be set before obtaining zodiac sign!");

                if (zodiacSign1 == 0)
                {
                    GetZodiacSign(birthdate, out zodiacSign1);
                }
                return zodiacSign1;
            }
        }

        /// <summary>
        /// Gets the zodiac sign.
        /// </summary>
        /// <value>The zodiac sign.</value>
        public eZodiacSign? ZodiacSign2
        {
            get
            {
                if (birthdate2 == DateTime.MinValue)
                    return null;

                if (zodiacSign2 == 0)
                {
                    GetZodiacSign(birthdate2, out zodiacSign2);
                }
                return zodiacSign2;
            }
        }

        private eGender interestedIn;

        /// <summary>
        /// Gets or sets the interested in.
        /// </summary>
        /// <value>The interested in.</value>
        public eGender InterestedIn
        {
            get { return interestedIn; }
            set { interestedIn = value; }
        }

        private DateTime birthdate;

        /// <summary>
        /// Gets or sets the birthdate.
        /// </summary>
        /// <value>The birthdate.</value>
        public DateTime Birthdate
        {
            get { return birthdate; }
            set
            {
                if (DateTime.Now.Subtract(value)
                        .TotalDays < Config.Users.MinAge*365.25)

                {
                    throw new ArgumentException(
                        String.Format(
                            Lang.Trans("You must be at least {0} years old to register!"),
                            Config.Users.MinAge));
                }
                else if (DateTime.Now.Subtract(value)
                             .TotalDays > Config.Users.MaxAge*365.25)

                {
                    throw new ArgumentException(
                        String.Format(
                            Lang.Trans("You must be at most {0} years old to register!"),
                            Config.Users.MaxAge));
                }
                else
                {
                    birthdate = value;
                }
            }
        }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <value>The age.</value>
        public int Age
        {
            get { return (int) (DateTime.Now.Subtract(Birthdate).TotalDays/365.25); }
        }

        public int Age2
        {
            get { return (int)(DateTime.Now.Subtract(Birthdate2).TotalDays / 365.25); }
        }
        private DateTime birthdate2 = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the birthdate (only for couples).
        /// </summary>
        /// <value>The birthdate.</value>
        public DateTime Birthdate2
        {
            get { return birthdate2; }
            set
            {
                if (DateTime.Now.Subtract(value)
                        .TotalDays < Config.Users.MinAge*365.25)

                {
                    throw new ArgumentException(
                        String.Format(
                            Lang.Trans("You must be at least {0} years old to register!"),
                            Config.Users.MinAge));
                }
                else if (DateTime.Now.Subtract(value)
                             .TotalDays > Config.Users.MaxAge*365.25)

                {
                    throw new ArgumentException(
                        String.Format(
                            Lang.Trans("You must be at most {0} years old to register!"),
                            Config.Users.MaxAge));
                }
                else
                {
                    birthdate2 = value;
                }
            }
        }

        private DateTime userSince = DateTime.Now;

        /// <summary>
        /// The date and time when the user account was created.
        /// The property is read-only.
        /// </summary>
        /// <value>The date and time.</value>
        public DateTime UserSince
        {
            get { return userSince; }
        }

        private bool active;

        /// <summary>
        /// The activation status for the user account.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private bool smsConfirmed;

        /// <summary>
        /// Gets or sets a value indicating whether the user is SMS confirmed.
        /// </summary>
        /// <value><c>true</c> if the user is SMS confirmed; otherwise, <c>false</c>.</value>
        public bool SmsConfirmed
        {
            get { return smsConfirmed; }
            set { smsConfirmed = value; }
        }

        private DateTime prevLogin = DateTime.MinValue;

        /// <summary>
        /// The date and time when the user last logged in.
        /// The property is read-only.
        /// </summary>
        /// <value>The prev login date and tune.</value>
        public DateTime PrevLogin
        {
            get { return prevLogin; }
        }

        private DateTime lastLogin = DateTime.MinValue;

        /// <summary>
        /// The date and time when the user last logged in.
        /// The property is read-only.
        /// </summary>
        /// <value>The last login date and time.</value>
        public DateTime LastLogin
        {
            get { return lastLogin; }
        }

        private DateTime lastOnline = DateTime.MinValue;

        /// <summary>
        /// The date and time when the user was last online.
        /// The property is read-only.
        /// </summary>
        /// <value>The last online date and time.</value>
        public DateTime LastOnline
        {
            get
            {
                var cacheKey = "LastOnline_" + Username;
                return HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null
                           ? (DateTime)HttpContext.Current.Cache[cacheKey]
                           : lastOnline;
            }
        }

        /// <summary>
        /// Gets the last online time formated nicely (e.g. "5 hours ago")
        /// </summary>
        /// <value>The last online string.</value>
        public string LastOnlineString
        {
            get
            {
                TimeSpan diff = DateTime.Now.Subtract(LastOnline);
                return (TimespanToString(diff));
            }
        }

        /// <summary>
        /// Determines whether this instance is online.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is online; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOnline()
        {
            TimeSpan diff = DateTime.Now.Subtract(LastOnline);
            return (diff.Days == 0 && diff.Hours == 0 && diff.Minutes <= 1);
        }

        public static bool IsOnline(string username)
        {
            var cacheKey = "LastOnline_" + username;
            var lastOnline = HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null
                       ? (DateTime)HttpContext.Current.Cache[cacheKey]
                       : DateTime.MinValue;
            TimeSpan diff = DateTime.Now.Subtract(lastOnline);
            return (diff.Days == 0 && diff.Hours == 0 && diff.Minutes <= 1);
        }

        public static bool IsUsingNotifier(string username)
        {
            var cacheKey = "LastNotifierCheck_" + username;
            var lastOnline = HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null
                       ? (DateTime)HttpContext.Current.Cache[cacheKey]
                       : DateTime.MinValue;
            TimeSpan diff = DateTime.Now.Subtract(lastOnline);
            return (diff.Days == 0 && diff.Hours == 0 && diff.Minutes <= 5);
        }

        /// <summary>
        /// Converts timespan to string.
        /// </summary>
        /// <param name="diff">The timespan.</param>
        /// <returns></returns>
        public static string TimespanToString(TimeSpan diff)
        {
            if (diff.Days == 0 && diff.Hours == 0 && diff.Minutes <= 1)
            {
                return Lang.Trans("online now");
            }
            if (diff.Days == 0 && diff.Hours == 0)
            {
                return String.Format((diff.Minutes == 1)
                                         ?
                                             Lang.Trans("one minute ago")
                                         :
                                             Lang.Trans("{0} minutes ago"), diff.Minutes);
            }
            if (diff.Days == 0)
            {
                return String.Format((diff.Hours == 1)
                                         ?
                                             Lang.Trans("one hour ago")
                                         :
                                             Lang.Trans("{0} hours ago"), diff.Hours);
            }
            if (diff.Days > 31)
                return "More than a month ago".Translate();

            return String.Format((diff.Days == 1)
                                     ?
                                         Lang.Trans("one day ago")
                                     :
                                         Lang.Trans("{0} days ago"), diff.Days);
        }

        private int loginCount = 0;

        /// <summary>
        /// Gets the login count.
        /// </summary>
        /// <value>The login count.</value>
        public int LoginCount
        {
            get { return loginCount; }
        }

        private int profileViews = 0;

        /// <summary>
        /// Gets the profile views.
        /// </summary>
        /// <value>The profile views.</value>
        public int ProfileViews
        {
            get { return profileViews; }
        }

        private bool receiveEmails = Config.Users.EmailNotificationsDefault;

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to receive notification emails.
        /// </summary>
        /// <value><c>true</c> if the user wants to receive notification emails; otherwise, <c>false</c>.</value>
        public bool ReceiveEmails
        {
            get { return receiveEmails; }
            set { receiveEmails = value; }
        }

        private bool profileVisible = true;

        /// <summary>
        /// Gets or sets a value indicating whether profile is visible.
        /// </summary>
        /// <value><c>true</c> if profile is visible; otherwise, <c>false</c>.</value>
        public bool ProfileVisible
        {
            get { return profileVisible; }
            set { profileVisible = value; }
        }

        private string country = "";

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country
        {
            get
            {
                //try
                //{
                //    int index = Config.Users.CountriesHash.IndexOfValue(country);
                //    return (string) Config.Users.CountriesHash.GetKey(index);
                //}
                //catch (Exception)
                //{
                //    return String.Empty;
                //}
                return country;
            }
            set
            {
                //country = (string) Config.Users.CountriesHash[value];
                country = value;
            }
        }

        private string state = "";

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State
        {
            get
            {
                //try
                //{
                //    int index = Config.Users.StateHash.IndexOfValue(state);
                //    return (string) Config.Users.StateHash.GetKey(index);
                //}
                //catch (Exception)
                //{
                //    return String.Empty;
                //}
                return state;
            }
            set
            {
                //state = (string) Config.Users.StateHash[value];
                state = value;
            }
        }

        private string zipCode = "";

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        private string city = "";

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        /// <summary>
        /// Gets the location string.
        /// </summary>
        /// <value>The location string.</value>
        public string LocationString
        {
            get
            {
                List<string> lLocation = new List<string>(3);

                if (city.Trim() != String.Empty && Config.Users.ForceCity == String.Empty)
                    lLocation.Add(city);
                if (state.Trim() != String.Empty && Config.Users.ForceRegion == String.Empty)
                    lLocation.Add(state);
                if (country.Trim() != String.Empty && Config.Users.ForceCountry == String.Empty)
                    lLocation.Add(country);

                return String.Join(", ", lLocation.ToArray());                 
            }
        }
        
        private bool deleted;

        /// <summary>
        /// Gets a value indicating whether this <see cref="User"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        private bool paid = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="User"/> is paid.
        /// </summary>
        /// <value><c>true</c> if paid; otherwise, <c>false</c>.</value>
        public bool Paid
        {
            get { return paid; }
            set { paid = value; }
        }

        private string lastSessionID = String.Empty;

        /// <summary>
        /// Gets the last session ID.
        /// </summary>
        /// <value>The last session ID.</value>
        public string LastSessionID
        {
            get { return lastSessionID; }
        }

        /// <summary>
        /// Gets or sets the message verifications left.
        /// </summary>
        /// <value>The message verifications left.</value>
        public int MessageVerificationsLeft
        {
            get { return messageVerificationsLeft; }
            set { messageVerificationsLeft = value; }
        }

        private int messageVerificationsLeft = 0;

        private string signupIp;

        /// <summary>
        /// Gets the signup ip.
        /// </summary>
        /// <value>The signup ip.</value>
        public string SignupIp
        {
            get { return signupIp; }
        }

        private int languageId;

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>The language id.</value>
        public int LanguageId
        {
            get { return languageId; }
            set { languageId = value; }
        }

        private bool stealthMode;
        /// <summary>
        /// Gets or sets a value indicating whether [stealth mode].
        /// </summary>
        /// <value><c>true</c> if [stealth mode]; otherwise, <c>false</c>.</value>
        public bool StealthMode
        {
            get { return stealthMode; }
            set { stealthMode = value; }
        }

        private string _billingDetails;

        private string billingDetails
        {
            set { _billingDetails = value; }
            get
            {
                if (billingDetailsDeserialized == null)
                {
                    if (_billingDetails != null)
                    {
                        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_billingDetails));
                        XmlSerializer xmls = new XmlSerializer(typeof (BillingDetails));
                        billingDetailsDeserialized = (BillingDetails) xmls.Deserialize(ms);
                        return _billingDetails;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    XmlSerializer xmls = new XmlSerializer(typeof (BillingDetails));
                    xmls.Serialize(ms, billingDetailsDeserialized);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        private BillingDetails billingDetailsDeserialized = null;

        /// <summary>
        /// Gets or sets the billing details.
        /// </summary>
        /// <value>The billing details.</value>
        public BillingDetails BillingDetails
        {
            get
            {
                if (billingDetailsDeserialized == null && billingDetails != null)
                {
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(billingDetails));
                    XmlSerializer xmls = new XmlSerializer(typeof (BillingDetails));
                    billingDetailsDeserialized = (BillingDetails) xmls.Deserialize(ms);
                }
                return billingDetailsDeserialized;
            }
            set
            {
                billingDetailsDeserialized = value;
                MemoryStream ms = new MemoryStream();
                XmlSerializer xmls = new XmlSerializer(typeof (BillingDetails));
                xmls.Serialize(ms, value);
                billingDetails = Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private string _incomingMessagesRestrictions;

        private IncomingMessagesRestrictions incomingMessagesRestrictionDeserialized = null;

        private string incomingMessagesRestrictions
        {
            get
            {
                if (incomingMessagesRestrictionDeserialized == null)
                {
                    if (_incomingMessagesRestrictions != null)
                    {
                        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_incomingMessagesRestrictions));
                        XmlSerializer xmls = new XmlSerializer(typeof(IncomingMessagesRestrictions));
                        incomingMessagesRestrictionDeserialized = (IncomingMessagesRestrictions) xmls.Deserialize(ms);
                        return _incomingMessagesRestrictions;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    XmlSerializer xmls = new XmlSerializer(typeof(IncomingMessagesRestrictions));
                    xmls.Serialize(ms, incomingMessagesRestrictionDeserialized);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            set { _incomingMessagesRestrictions = value; }
        }

        /// <summary>
        /// Gets or sets the incoming messages restrictions.
        /// </summary>
        /// <value>The incoming messages restrictions.</value>
        public IncomingMessagesRestrictions IncomingMessagesRestrictions
        {
            get
            {
                if (_incomingMessagesRestrictions == null)
                {
                    return new IncomingMessagesRestrictions();
                }

                if (incomingMessagesRestrictionDeserialized == null && incomingMessagesRestrictions != null)
                {
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(incomingMessagesRestrictions));
                    XmlSerializer xmls = new XmlSerializer(typeof(IncomingMessagesRestrictions));
                    incomingMessagesRestrictionDeserialized = (IncomingMessagesRestrictions) xmls.Deserialize(ms);
                }
                return incomingMessagesRestrictionDeserialized;
            }

            set
            {
                incomingMessagesRestrictionDeserialized = value;
                MemoryStream ms = new MemoryStream();
                XmlSerializer xmls = new XmlSerializer(typeof(IncomingMessagesRestrictions));
                xmls.Serialize(ms, value);
                incomingMessagesRestrictions = Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private ulong options
            = (ulong)eUserOptions.VisitorsCanViewProfile |
              (ulong)eUserOptions.UsersCanViewProfile |
              (ulong)eUserOptions.VisitorsCanViewPhotos |
              (ulong)eUserOptions.UsersCanViewPhotos |
              (ulong)eUserOptions.VisitorsCanViewFriends |
              (ulong)eUserOptions.UsersCanViewFriends |
              (ulong)eUserOptions.VisitorsCanViewVideos |
              (ulong)eUserOptions.UsersCanViewVideos |
              (ulong)eUserOptions.VisitorsCanViewBlog |
              (ulong)eUserOptions.UsersCanViewBlog;

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        public ulong Options
        {
            get { return options; }
            set { options = value; }
        }

        private string invitedBy = null;

        /// <summary>
        /// Gets or sets the invited by.
        /// </summary>
        /// <value>The invited by.</value>
        public string InvitedBy
        {
            get { return invitedBy; }
            set { invitedBy = value; }
        }

        private string deleteReason = null;

        /// <summary>
        /// Gets the delete reason.
        /// </summary>
        /// <value>The delete reason.</value>
        public string DeleteReason
        {
            get { return deleteReason; }
            //set { deleteReason = value; }
        }

        private int? affiliateID = null;

        /// <summary>
        /// Gets or sets the affiliate ID.
        /// </summary>
        /// <value>The affiliate ID.</value>
        public int? AffiliateID
        {
            get { return affiliateID; }
            set { affiliateID = value; }
        }

        private double? longitude;

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double? Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private double? latitude;

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double? Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private int score;

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>The score.</value>
        public int Score
        {
            get { return score; }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public UserLevel Level
        {
            get
            {
                if (Config.UserScores.EnableUserLevels)
                    return UserLevel.GetLevelByScore(score);
                else
                    return null;
            }
        }

        private string tokenUniqueId;

        /// <summary>
        /// Gets or sets the token unique id.
        /// </summary>
        /// <value>The token unique id.</value>
        public string TokenUniqueId
        {
            get { return tokenUniqueId; }
            set { tokenUniqueId = value; }
        }

        private string _personalizationInfo;

        private string personalizationInfo
        {
            set { _personalizationInfo = value; }
            get
            {
                if (personalizationInfoDeserialized == null)
                {
                    if (_personalizationInfo != null)
                    {
                        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_personalizationInfo));
                        XmlSerializer xmls = new XmlSerializer(typeof(PersonalizationInfo));
                        personalizationInfoDeserialized = (PersonalizationInfo)xmls.Deserialize(ms);
                        return _personalizationInfo;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    XmlSerializer xmls = new XmlSerializer(typeof(PersonalizationInfo));
                    xmls.Serialize(ms, personalizationInfoDeserialized);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        private PersonalizationInfo personalizationInfoDeserialized = null;

        /// <summary>
        /// Gets or sets the personalization info.
        /// </summary>
        /// <value>The personalization info.</value>
        public PersonalizationInfo PersonalizationInfo
        {
            get
            {
                if (personalizationInfoDeserialized == null && personalizationInfo != null)
                {
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(personalizationInfo));
                    XmlSerializer xmls = new XmlSerializer(typeof(PersonalizationInfo));
                    personalizationInfoDeserialized = (PersonalizationInfo)xmls.Deserialize(ms);
                }
                return personalizationInfoDeserialized;
            }
            set
            {
                personalizationInfoDeserialized = value;
                MemoryStream ms = new MemoryStream();
                XmlSerializer xmls = new XmlSerializer(typeof(PersonalizationInfo));
                xmls.Serialize(ms, value);
                personalizationInfo = Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private int credits = 0;

        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        /// <value>The credits.</value>
        public int Credits
        {
            get { return credits; }
            set { credits = value; }
        }

        private int moderationScores = 0;

        public int ModerationScores
        {
            get { return moderationScores; }
            set { moderationScores = value; }
        }

        public bool SpamSuspected { get; set; }

        private bool faceControlApproved = !Config.CommunityFaceControlSystem.EnableCommunityFaceControl;
        public bool FaceControlApproved 
        {
            get { return faceControlApproved; }
            set { faceControlApproved = value; }
        }

        public string ProfileSkin
        {
            get; set;
        }

        private string statusText = null;
        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; }
        }

        private bool isFeaturedMember = false;
        public bool IsFeaturedMember
        {
            get { return isFeaturedMember; }
            set { isFeaturedMember = value; }
        }

        private string mySpaceID = null;
        public string MySpaceID
        {
            get { return mySpaceID; }
            set { mySpaceID = value; }
        }

        private long? facebookID;
        public long? FacebookID
        {
            get { return facebookID; }
            set { facebookID = value; }
        }

        private ulong? eventsSettings;
        public ulong? EventsSettings
        {
            get { return eventsSettings; }
            set { eventsSettings = value; }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            languageId = Config.Misc.DefaultLanguageId;
        }

        /// <summary>
        /// Default constructor.
        /// Used for creating of new or virtual user accounts.
        /// </summary>
        /// <param name="Username">Username for the user account.</param>
        public User(string Username) : this()
        {
            this.Username = Username;
        }

        /// <summary>
        /// Validates user credentials against the DB
        /// </summary>
        /// <param name="Username">Username for the user account.</param>
        /// <param name="Password">Password for the user account.</param>
        /// <param name="sessionID">The session ID.</param>
        /// <exception cref="ArgumentException">Username of Password is invalid.</exception>
        /// <exception cref="NotFoundException">Username was not found.</exception>
        /// <exception cref="AccessDeniedException">Password is invalid.</exception>
        public static void Authorize(string Username, string Password,
                                     string sessionID)
        {
            ValidateUsername(Username);
            ValidatePassword(Password);

            User user = Load(Username);

            if (!user.IsPasswordIdentical(Password))
            {
                throw new AccessDeniedException
                    (Lang.Trans("The provided password is invalid!"));
            }

            if (!user.active)
            {
                if (!user.smsConfirmed && Config.Users.SmsConfirmationRequired)
                {
                    throw new SmsNotConfirmedException
                        (Lang.Trans("This account is not yet SMS confirmed!"));
                }

                throw new AccessDeniedException
                    (Lang.Trans("This account is not yet activated!"));
            }

            if (user.deleted)
            {
                if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                    throw new AccessDeniedException
                        (Lang.Trans("This user has been deleted!"));
                else
                    throw new AccessDeniedException
                        (String.Format(Lang.Trans("This user has been deleted ({0})"), user.DeleteReason));
            }

            if (sessionID != null)
            {
                user.updateLastLogin(sessionID);
            }
        }

        /// <summary>
        /// Validates user credentials against the DB
        /// </summary>
        /// <param name="tokenUniqueId">Token unique Id.</param>
        /// <exception cref="NotFoundException">Information card was not found.</exception>
        public static void AuthorizeByToken(string tokenUniqueId)
        {
            string username = GetUsernameByTokenUniqueId(tokenUniqueId);
            if (username == null)
                throw new NotFoundException(
                    Lang.Trans("The presented information card is not associated with any account"));

            User user = Load(username);

            if (!user.active)
            {
                if (!user.smsConfirmed && Config.Users.SmsConfirmationRequired)
                {
                    throw new SmsNotConfirmedException
                        (Lang.Trans("This account is not yet SMS confirmed!"));
                }

                throw new AccessDeniedException
                    (Lang.Trans("This account is not yet activated!"));
            }

            if (user.deleted)
            {
                if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                    throw new AccessDeniedException
                        (Lang.Trans("This user has been deleted!"));
                else
                    throw new AccessDeniedException
                        (String.Format(Lang.Trans("This user has been deleted ({0})"), user.DeleteReason));
            }
        }

        public static void AuthorizeByMySpaceID(string mySpaceID)
        {
            string[] usernames = FetchUsernamesByMySpaceID(new[] { mySpaceID });

            if (usernames.Length == 0)
                throw new NotFoundException(
                    "There is no user associated with your MySpace account!".Translate());

            User user = Load(usernames[0]);

            if (!user.active)
            {
                if (!user.smsConfirmed && Config.Users.SmsConfirmationRequired)
                {
                    throw new SmsNotConfirmedException
                        (Lang.Trans("This account is not yet SMS confirmed!"));
                }

                throw new AccessDeniedException
                    (Lang.Trans("This account is not yet activated!"));
            }

            if (user.deleted)
            {
                if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                    throw new AccessDeniedException
                        (Lang.Trans("This user has been deleted!"));
                else
                    throw new AccessDeniedException
                        (String.Format(Lang.Trans("This user has been deleted ({0})"), user.DeleteReason));
            }            
        }

        public static void AuthorizeByFacebookID(long facebookID)
        {
            string[] usernames = FetchUsernamesByFacebookID(new[] { facebookID });

            if (usernames.Length == 0)
                throw new NotFoundException(
                    "There is no user associated with your Facebook account!".Translate());

            User user = Load(usernames[0]);

            if (!user.active)
            {
                if (!user.smsConfirmed && Config.Users.SmsConfirmationRequired)
                {
                    throw new SmsNotConfirmedException
                        (Lang.Trans("This account is not yet SMS confirmed!"));
                }

                throw new AccessDeniedException
                    (Lang.Trans("This account is not yet activated!"));
            }

            if (user.deleted)
            {
                if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                    throw new AccessDeniedException
                        (Lang.Trans("This user has been deleted!"));
                else
                    throw new AccessDeniedException
                        (String.Format(Lang.Trans("This user has been deleted ({0})"), user.DeleteReason));
            }
        }

        /// <summary>
        /// Updates the last login to the current date and time.
        /// </summary>
        protected void updateLastLogin()
        {
            updateLastLogin(null);
        }

        /// <summary>
        /// Updates the last login to the current date and time.
        /// </summary>
        /// <param name="sessionID">The session ID.</param>
        public void updateLastLogin(string sessionID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UpdateLastLogin",
                                          Username, sessionID);
            }

            prevLogin = lastLogin;
            lastLogin = DateTime.Now;
            loginCount++;
        }

        /// <summary>
        /// Loads user account data from DB.
        /// Throws "NotFoundException" exception. 
        /// </summary>
        /// <param name="username">Username identifying the user</param>
        /// <returns>User object</returns>
        /// <exception cref="NotFoundException">Username was not found.</exception>
        public static User Load(string username)
        {
            string cachekey = String.Format("User_Load_{0}", username);
            User user;

            if (HttpContext.Current == null)
            {
                user = new User(username);
                user.Load();
                return user;
            }
            else if (HttpContext.Current.Items[cachekey] == null)
            {
                user = new User(username);
                user.Load();
                HttpContext.Current.Items.Add(cachekey, user);
                return user;
            }
            else
            {
                return HttpContext.Current.Items[cachekey] as User;
            }
        }

        /// <summary>
        /// Loads the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static User LoadUserByEmail(string email)
        {
            User user = new User();
            user.Email = email;
            user.LoadByEmail();

            return user;
        }

        /// <summary>
        /// Loads user account data from DB.
        /// Throws NotFoundException.
        /// </summary>
        public void Load()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "LoadUser", username);

                if (reader.Read())
                {
                    password = (string) reader["Password"];
                    email = (string) reader["Email"];
                    username = (string) reader["Username"];
                    name = (string) reader["Name"];
                    gender = (eGender) reader["Gender"];
                    if (!(reader["InterestedIn"] is Int32) ||
                        ((int) reader["InterestedIn"] == 0)
                        )
                    {
                        switch (gender)
                        {
                            case eGender.Male:
                                interestedIn = eGender.Female;
                                break;
                            case eGender.Female:
                                interestedIn = eGender.Male;
                                break;
                            case eGender.Couple:
                                interestedIn = eGender.Couple;
                                break;
                        }
                    }
                    else
                    {
                        interestedIn = (eGender) reader["InterestedIn"];
                    }
                    birthdate = (DateTime) reader["Birthdate"];
                    if (reader["Birthdate2"] is DateTime)
                    {
                        birthdate2 = (DateTime) reader["Birthdate2"];
                    }
                    active = (bool) reader["Active"];
                    userSince = (DateTime) reader["UserSince"];
                    prevLogin = (DateTime) reader["PrevLogin"];
                    lastLogin = (DateTime) reader["LastLogin"];
                    loginCount = (int) reader["LoginCount"];
                    lastOnline = (DateTime) reader["LastOnline"];
                    profileViews = (int) reader["ProfileViews"];
                    receiveEmails = (bool) reader["ReceiveEmails"];
                    profileVisible = (bool) reader["ProfileVisible"];
                    country = (string) reader["Country"];
                    state = (string) reader["State"];
                    ZipCode = (string) reader["ZipCode"];
                    city = (string) reader["City"];
                    deleted = (bool) reader["Deleted"];
                    paid = (bool) reader["Paid"];
                    lastSessionID = (string) reader["LastSessionID"];
                    if (reader["SignupIP"] != DBNull.Value)
                        signupIp = (string) reader["SignupIP"];
                    smsConfirmed = (bool)reader["SmsConfirmed"];
                    messageVerificationsLeft = (int)reader["MessageVerificationsLeft"];
                    languageId = (int) reader["LanguageId"];
                    if (reader["BillingDetails"] is string)
                        billingDetails = (string) reader["BillingDetails"];
                    if (reader["InvitedBy"] is string)
                        invitedBy = (string) reader["InvitedBy"];
                    incomingMessagesRestrictions = reader["IncomingMessagesRestrictions"] as string;
                    if (reader["DeleteReason"] is string)
                        deleteReason = (string) reader["DeleteReason"];
                    affiliateID = reader["AffiliateID"] != DBNull.Value ? (int?) reader["AffiliateID"] : null;
                    options = Convert.ToUInt64(reader["Options"]);
                    if (reader["Longitude"] is double)
                        longitude = (double)reader["Longitude"];
                    if (reader["Latitude"] is double)
                        latitude = (double) reader["Latitude"];
                    score = (int) reader["Score"];
                    tokenUniqueId = reader["TokenUniqueId"] as string;
                    personalizationInfo = reader["PersonalizationInfo"] as string;
                    credits = (int) reader["Credits"];
                    moderationScores = (int) reader["ModerationScores"];
                    SpamSuspected = (bool) reader["SpamSuspected"];
                    faceControlApproved = (bool) reader["FaceControlApproved"];
                    ProfileSkin = reader["ProfileSkin"] as string;
                    statusText = reader["StatusText"] != DBNull.Value ? (string) reader["StatusText"] : null;
                    isFeaturedMember = (bool) reader["FeaturedMember"];
                    mySpaceID = reader["MySpaceID"] as string;
                    facebookID = reader["FacebookID"] as long?;
                    eventsSettings = reader["EventsSettings"] == DBNull.Value
                                         ? Int64.MaxValue // all events are switched on. Do not change it to UInt64.MaxValue
                                         : (ulong?) Convert.ToInt64(reader["EventsSettings"]);
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The specified account does not exist!"));
                }
            }
        }

        /// <summary>
        /// Loads the user by email.
        /// </summary>
        public void LoadByEmail()
        {
            if (email != null)
            {
                using (SqlConnection conn = Config.DB.Open())
                {
                    SqlDataReader reader =
                        SqlHelper.ExecuteReader(conn, "LoadUserByEmail", email);

                    if (reader.Read())
                    {
                        username = (string) reader["Username"];
                        Load();
                    }
                    else
                    {
                        throw new NotFoundException
                            (Lang.Trans("The specified email address is not present in the database!"));
                    }
                }
            }
            else
            {
                throw new InvalidOperationException
                    (Lang.Trans("Email should be provided in order to load user info"));
            }
        }

        /// <summary>
        /// Loads the user by token.
        /// </summary>
        public static string GetUsernameByTokenUniqueId(string tokenUniqueId)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "LoadUserByTokenUniqueId", tokenUniqueId);

                if (reader.Read())
                {
                    return (string)reader["Username"];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Updates user account data
        /// </summary>
        /// <param name="user">User object</param>
        /// <param name="updatePaidStatus">if set to <c>true</c> [update paid status].</param>
        public static void Update(User user, bool updatePaidStatus)
        {
            if ((!user.latitude.HasValue || !user.longitude.HasValue)
                && Config.ThirdPartyServices.GetMissingCoordinatesFromGoogleMaps
                && Config.ThirdPartyServices.GoogleMapsAPIKey.Length > 0)
            {
                try
                {
                    double[] coordinates = GoogleMaps.GetCoordinates(user.city + "," + user.country);
                    if (coordinates != null && coordinates.Length >= 2)
                    {
                        user.latitude = coordinates[0];
                        user.longitude = coordinates[1];
                    }
                }
                catch (Exception err)
                {
                    Global.Logger.LogError("User.Update", err);
                }
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UpdateUser",
                                          user.username, user.password, user.email, user.name,
                                          user.gender, user.interestedIn, user.birthdate,
                                          user.birthdate2 == DateTime.MinValue ? null : (object) user.birthdate2,
                                          user.active, user.smsConfirmed, user.receiveEmails, user.profileVisible, user.country,
                                          user.state, user.zipCode, user.city, user.paid, user.messageVerificationsLeft,
                                          user.languageId, user.billingDetails, user.invitedBy,
                                          user.incomingMessagesRestrictions, user.deleted, user.affiliateID, user.options,
                                          user.longitude, user.latitude, updatePaidStatus, user.tokenUniqueId, user.personalizationInfo,
                                          user.credits, user.moderationScores, user.SpamSuspected, user.faceControlApproved,
                                          user.ProfileSkin, user.statusText, user.isFeaturedMember, user.mySpaceID,
                                          user.facebookID, user.eventsSettings);
            }

            string cachekey = String.Format("User_Load_{0}", user.username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cachekey] != null)
            {
                HttpContext.Current.Cache.Remove(cachekey);
            }

        }

        /// <summary>
        /// Updates user account data
        /// </summary>
        public void Update()
        {
            Update(this, false);
        }

        /// <summary>
        /// Updates the specified update paid status.
        /// </summary>
        /// <param name="updatePaidStatus">if set to <c>true</c> [update paid status].</param>
        public void Update(bool updatePaidStatus)
        {
            Update(this, updatePaidStatus);
        }

        /// <summary>
        /// Creates new user account
        /// </summary>
        /// <param name="user">User object</param>
        /// <param name="userIP">The user IP.</param>
        public static void Create(User user, string userIP)
        {
            if (Config.Users.AutoActivateUsers)
            {
                user.active = true;
            }

            if ((!user.latitude.HasValue || !user.longitude.HasValue)
                && Config.ThirdPartyServices.GetMissingCoordinatesFromGoogleMaps
                && Config.ThirdPartyServices.GoogleMapsAPIKey.Length > 0)
            {
                try
                {
                    double[] coordinates = GoogleMaps.GetCoordinates(user.city + "," + user.country);
                    if (coordinates != null && coordinates.Length >= 2)
                    {
                        user.latitude = coordinates[0];
                        user.longitude = coordinates[1];
                    }
                }
                catch (Exception err)
                {
                    Global.Logger.LogError("User.Create", err);
                }
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "CreateUser",
                                          user.username, user.password, user.email, user.name,
                                          user.gender, user.interestedIn, user.birthdate,
                                          user.birthdate2 == DateTime.MinValue ? null : (object) user.birthdate2,
                                          user.active, user.receiveEmails, user.country, user.state, user.zipCode, user.city, userIP,
                                          Config.Users.MessageVerificationsCount,
                                          user.languageId, user.billingDetails, user.invitedBy,
                                          user.incomingMessagesRestrictions,
                                          user.affiliateID, user.options, user.longitude, user.latitude,
                                          user.tokenUniqueId, user.credits, user.moderationScores, user.SpamSuspected,
                                          user.faceControlApproved, user.ProfileSkin, user.statusText,
                                          user.isFeaturedMember, user.mySpaceID, user.facebookID, user.eventsSettings);
            }

            if (!Config.Users.AutoActivateUsers && !Config.Users.SmsConfirmationRequired)
            {
                user.SendCreateActivateAccountEmail();
            }
        }

        /// <summary>
        /// Creates new user account
        /// </summary>
        public void Create(string userIP)
        {
            Create(this, userIP);
        }

        /// <summary>
        /// Mark user account as deleted
        /// </summary>
        /// <param name="username">the username of the user whose account is about to be deleted</param>
        /// <param name="reason">The reason.</param>
        public static void Delete(string username, string reason)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteUser", username, reason);
            }

            if (Config.Ratings.EnablePhotoContests)
            {
                PhotoContestEntry.DeleteByUsername(username);
            }

            if (Config.Groups.EnableGroups)
            {
                Group[] groups = Group.FetchGroupsByUsername(username);

                foreach (Group group in groups)
                {
                    if (!group.Approved)
                    {
                        Group.Delete(group.ID);
                    }
                    else
                    {
                        if (group.Owner == username)
                        {
                            GroupMember[] admins = GroupMember.Fetch(group.ID, GroupMember.eType.Admin,
                                                                     GroupMember.eSortColumn.JoinDate);
                            if (admins.Length > 1)
                            {
                                List<GroupMember> lAdmins = new List<GroupMember>();
                                foreach (GroupMember admin in admins)
                                {
                                    lAdmins.Add(admin);
                                }
                                lAdmins.Reverse();
                                admins = lAdmins.ToArray();
                                group.Owner = admins[1].Username; // the oldest administrator except the current owner
                            }
                            else
                            {
                                GroupMember[] moderators = GroupMember.Fetch(group.ID, GroupMember.eType.Moderator,
                                                                     GroupMember.eSortColumn.JoinDate);
                                if (moderators.Length > 0)
                                {
                                    List<GroupMember> lModerators = new List<GroupMember>();
                                    foreach (GroupMember moderator in moderators)
                                    {
                                        lModerators.Add(moderator);
                                    }
                                    lModerators.Reverse();
                                    moderators = lModerators.ToArray();
                                    group.Owner = moderators[0].Username; // the oldest moderator
                                }
                                else
                                {
                                    if (!GroupMember.IsMember(Config.Users.SystemUsername, group.ID))
                                    {
                                        GroupMember groupMember = new GroupMember(group.ID, Config.Users.SystemUsername);
                                        groupMember.Active = true;
                                        groupMember.Type = GroupMember.eType.Admin;
                                        groupMember.Save();
                                        group.ActiveMembers++;
                                    }

                                    group.Owner = Config.Users.SystemUsername;
                                }
                            }

                            group.ActiveMembers--;
                            group.Save();
                        }
                    }
                }
            }

            if (IsOnline(username))
            {
                var notification = new AccountDeletedNotification
                                       {
                                           Recipient = username,
                                           Text = "Your account has been deleted (" + reason + ")!"
                                       };
                RealtimeNotification.SendNotification(notification);
            }
        }

        /// <summary>
        /// Purges the specified user.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void Purge(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "PurgeUser", username);
            }
        }

        /// <summary>
        /// Checks if e-mail address is already in the database
        /// </summary>
        /// <param name="Email">The email.</param>
        /// <returns>
        /// 	<c>true</c> if email is in use; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmailUsed(string Email)
        {
            ValidateEmail(Email);

            using (SqlConnection conn = Config.DB.Open())
            {
                return
                    (bool) SqlHelper.ExecuteScalar(conn, "IsEmailUsed", Email, null);
            }
        }

        /// <summary>
        /// Determines whether the email is used by another member.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// 	<c>true</c> if the email is used by another member; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmailUsedByAnotherMember(string email)
        {
            ValidateEmail(email);

            using (SqlConnection conn = Config.DB.Open())
            {
                return
                    (bool) SqlHelper.ExecuteScalar(conn, "IsEmailUsed", email, username);
            }
        }

        /// <summary>
        /// Fetches all the user emails. The emails of non-active and deleted users
        /// will not be fetched.
        /// </summary>
        /// <returns></returns>
        public static string[] FetchUserEmails()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchEmails");

                List<string> lResults = new List<string>();

                while (reader.Read())
                {
                    lResults.Add((string) reader["Email"]);
                }

                return lResults.ToArray();
            }
        }

        /// <summary>
        /// Checks if username is already taken
        /// </summary>
        /// <param name="Username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if username is taken; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUsernameTaken(string Username)
        {
            ValidateUsername(Username);

            try
            {
                Load(Username);

                // If successfull then user exists
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether password on file is identical with the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// 	<c>true</c> if password on file is identical with the specified password; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPasswordIdentical(string password)
        {
            if (this.password == FormsAuthentication
                                     .HashPasswordForStoringInConfigFile(password, "sha1"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Fetches all answers for the current user
        /// </summary>
        /// <returns>Array of ProfileAnswer</returns>
        public ProfileAnswer[] FetchAnswers()
        {
            return ProfileAnswer.FetchByUsername(username);
        }

        /// <summary>
        /// Fetches answer for the current user
        /// </summary>
        /// <param name="QuestionID">ID of the Question</param>
        /// <returns>ProfileAnswer object</returns>
        public ProfileAnswer FetchAnswer(int QuestionID)
        {
            return ProfileAnswer.Fetch(username, QuestionID);
        }

        /// <summary>
        /// Fetches answer used for slogan for the current user
        /// </summary>
        /// <returns>ProfileAnswer object</returns>
        public ProfileAnswer FetchSlogan()
        {
            if (!ProfileQuestion.GetSloganQuestionIDByGender(Gender).HasValue)
                throw new NotFoundException("The user does not have slogan!");

            return ProfileAnswer.Fetch(username, ProfileQuestion.GetSloganQuestionIDByGender(Gender).Value);
        }

        /// <summary>
        /// Fetches answer used for skype for the current user
        /// </summary>
        /// <returns>ProfileAnswer object</returns>
        public ProfileAnswer FetchSkype()
        {
            if (!ProfileQuestion.SkypeQuestionID.HasValue)
                throw new NotFoundException("The user does not have skype!");

            return ProfileAnswer.Fetch(username, ProfileQuestion.SkypeQuestionID.Value);
        }

        /// <summary>
        /// Returns the primary photo of the user
        /// </summary>
        /// <returns>Photo object</returns>
        public Photo GetPrimaryPhoto()
        {
            return Photo.GetPrimary(username);
        }

        /// <summary>
        /// Gets the top photo.
        /// </summary>
        /// <returns></returns>
        public Photo GetTopPhoto()
        {
            return Photo.GetTop(username);
        }

        /// <summary>
        /// Saves profile view
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        public static void SaveProfileView(string fromUsername, string toUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SaveProfileView", fromUsername, toUsername);
            }
        }

        /// <summary>
        /// Fetches the profile views.
        /// </summary>
        /// <param name="viewedUsername">The viewed username.</param>
        /// <param name="fromDate">From date (use DateTime.MinValue to disable the date filter)</param>
        /// <returns>Array of usernames</returns>
        public static string[] FetchProfileViews(string viewedUsername, DateTime fromDate)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchProfileViews", viewedUsername,
                                            fromDate == DateTime.MinValue ? null : (object) fromDate);

                List<string> lResults = new List<string>();

                while (reader.Read())
                {
                    lResults.Add((string) reader["ViewerUsername"]);
                }

                if (lResults.Count > 0)
                {
                    return lResults.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Fetches the profile view date.
        /// </summary>
        /// <param name="viewerUsername">The viewer username.</param>
        /// <param name="viewedUsername">The viewed username.</param>
        /// <returns></returns>
        public static DateTime FetchProfileViewDate(string viewerUsername, string viewedUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.ExecuteScalar(conn,
                                            "FetchProfileViewDate", viewerUsername, viewedUsername);
                if (result == null || result == DBNull.Value)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return (DateTime) result;
                }
            }
        }

        /// <summary>
        /// Sends the message notification.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="message">The message.</param>
        public void SendMessageNotification(string fromUsername, string message)
        {
            if (receiveEmails)
            {
                message = HttpUtility.HtmlEncode(message);

                NameValueCollection additionalFormatter = new NameValueCollection();
                additionalFormatter.Add("SENDER", fromUsername);
                additionalFormatter.Add("RECIPIENT", username);
                additionalFormatter.Add("MESSAGE", message.Replace("\n", "<br>"));

                if (Config.Misc.SiteIsPaid && !IsPaidMember(username))
                    Classes.Email.SendTemplateEmail(typeof(EmailTemplates.MessageToNonPaidMember), email,
                                                    additionalFormatter, false, languageId);
                else
                    Classes.Email.SendTemplateEmail(typeof (EmailTemplates.MessageFromMember), email,
                                                    additionalFormatter, false, languageId);
            }
        }

        /// <summary>
        /// Sends the forgot password email.
        /// </summary>
        public void SendForgotPasswordEmail()
        {
            NameValueCollection formatter = new NameValueCollection();
            formatter.Add("RECIPIENT", username);
            formatter.Add("CONFIRM_URL", CreateActivationUrl(
                                             Config.Urls.ActivatePassword, username));
            Classes.Email.SendTemplateEmail(typeof (EmailTemplates.NewPasswordConfirmation),
                                            email, formatter, true, languageId);
        }

        /// <summary>
        /// Sends the create activate account email.
        /// </summary>
        public void SendCreateActivateAccountEmail()
        {
            NameValueCollection formatter = new NameValueCollection();
            formatter.Add("RECIPIENT", username);
            formatter.Add("CONFIRM_URL", CreateActivationUrl(
                                             Config.Urls.ActivateAccount, username));
            Classes.Email.SendTemplateEmail(typeof (EmailTemplates.RegistrationConfirmation),
                                            email, formatter, true, languageId);
        }

        public static string CreatePendingGuid(string username)
        {
            Guid guid = Guid.NewGuid();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "CreatePendingGuid", username, guid.ToString());
            }

            return guid.ToString();
        }

        public static string FetchUserByGuid(string pendingGuid)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (string)SqlHelper.ExecuteScalar(conn, "FetchUserByGuid", pendingGuid);
            }
        }

        /// <summary>
        /// Creates the activation URL.
        /// </summary>
        /// <param name="activationPage">The activation page.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static string CreateActivationUrl(string activationPage, string username)
        {
            Guid guid = Guid.NewGuid();
            string confirmationLink = "";

            confirmationLink =
                String.Format("{0}?username={1}&guid={2}",
                              activationPage, username, guid.ToString());

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "CreatePendingGuid", username, guid.ToString());
            }

            return confirmationLink;
        }

        /// <summary>
        /// Activates pending guid
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="guid">guid that identifies pending stuff</param>
        /// <returns>is activation successful</returns>
        public static bool IsValidPendingGuid(string username, string guid)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "IsValidPendingGuid", username, guid);
            }
        }

        /// <summary>
        /// Removes the pending GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public static void RemovePendingGuid(string guid)
        {
            RemovePendingGuids(guid, String.Empty);
        }

        /// <summary>
        /// Removes the pending guids.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void RemovePendingGuids(string username)
        {
            RemovePendingGuids(String.Empty, username);
        }

        /// <summary>
        /// Removes the pending guids.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="username">The username.</param>
        public static void RemovePendingGuids(string guid, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "RemovePendingGuid", guid, username);
            }
        }

        /// <summary>
        /// Adds user as favourite to the current user list
        /// </summary>
        /// <param name="username">username of the user who will be added as favourite</param>
        /// <returns></returns>
        public eAddFavouriteResult AddToFavourites(string username)
        {
            if (!IsUsernameTaken(username))
            {
                return eAddFavouriteResult.eInvalidUsername;
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("User_FetchFavouriteUsers_{0}", this.username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("User_FetchMutuallyFavouriteUsers_{0}", this.username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("User_FetchMutuallyFavouriteUsers_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                return
                    (eAddFavouriteResult)
                    SqlHelper.ExecuteScalar(conn, "AddToFavourites", this.username, username,
                                            Config.Users.MaxFavouriteUsers);
            }
        }

        /// <summary>
        /// Determines whether the specified username is user in favourite list.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified username is user in favourite list; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserInFavouriteList(string username)
        {
            return IsUserInFavouriteList(this.username, username);
        }

        /// <summary>
        /// Determines whether a user is in favourite list.
        /// </summary>
        /// <param name="listUsername">The username.</param>
        /// <param name="favouriteUsername">The favourite username.</param>
        /// <returns>
        /// 	<c>true</c> if [is user in favourite list] [the specified list username]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserInFavouriteList(string listUsername, string favouriteUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "IsUserInFavouriteList", listUsername, favouriteUsername);
            }
        }

        public eAddFriendResult AddToFriends(string username)
        {
            if (!IsUsernameTaken(username))
            {
                return eAddFriendResult.eInvalidUsername;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                return (eAddFriendResult)SqlHelper.ExecuteScalar(conn, "AddToFriends",
                                                            this.username,
                                                            username,
                                                            Config.Users.MaxFriendUsers);
            }
        }

        public bool IsUserInFriendList(string username)
        {
            return IsUserInFriendList(this.username, username);
        }

        public static bool IsUserInFriendList(string listUsername, string friendUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool)SqlHelper.ExecuteScalar(conn, "IsUserInFriendList", listUsername, friendUsername);
            }
        }

        public static bool FindFriendsConnection(string fromUsername, string toUsername, int maxHops,
            out List<string> usersChain)
        {
            int currentHop = 1;
            var checkedUsers = new List<string>();
            var pendingUsers = new List<string> {fromUsername};
            var connectionsRank = new List<List<Friend>>();
            usersChain = new List<string>();
            using (var db = new AspNetDatingDataContext())
            {
                while (currentHop++ <= maxHops)
                {
                    const int maxCount = 200;
                    var lFriends = new List<Friend>();
                    for (int i = 0; i <= pendingUsers.Count / maxCount; i++)
                    {
                        var usersToCheck = pendingUsers.Skip(i*maxCount).Take(maxCount);
                        var friends = from f in db.Friends
                                      where usersToCheck.Contains(f.u_username)
                                      select f;

                        List<Friend> friendsToCheck = friends.ToList();
                        friendsToCheck.RemoveAll(f => checkedUsers.Contains(f.f_username));
                        lFriends.AddRange(friendsToCheck);
                    }
                    connectionsRank.Add(lFriends);

                    if (connectionsRank.Last().Any(f => f.f_username == toUsername))
                    {
                        var lastUser = toUsername;
                        usersChain.Add(toUsername);
                        for (int i = connectionsRank.Count - 1; i >= 0; i--)
                        {
                            var prevUser = connectionsRank[i]
// ReSharper disable AccessToModifiedClosure
                                .First(f => f.f_username == lastUser).u_username;
// ReSharper restore AccessToModifiedClosure
                            usersChain.Insert(0, prevUser);
                            lastUser = prevUser;
                        }
                        return true;
                    }


                    checkedUsers.AddRange(pendingUsers);
                    pendingUsers.Clear();
                    pendingUsers.AddRange(connectionsRank.Last().Select(f => f.f_username));
                }
            }
            return false;
        }

        /// <summary>
        /// Fetches the favourite users.
        /// </summary>
        /// <returns></returns>
        public User[] FetchFavouriteUsers()
        {
            string[] usernames = FetchFavouriteUsers(username);
            var users = new User[usernames.Length];
            for (int i = 0; i < usernames.Length; i++)
            {
                users[i] = Load(usernames[i]);
            }
            return users;
        }

        /// <summary>
        /// Fetches the favourite users.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static string[] FetchFavouriteUsers(string username)
        {
            string cacheKey = String.Format("User_FetchFavouriteUsers_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as string[];
            }

            var lUsernames = new List<string>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchFavourites", username);

                while (reader.Read())
                {
                    lUsernames.Add((string) reader["Favourite"]);
                }
            }

            string[] usernames = lUsernames.ToArray();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, usernames, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
            }

            return usernames;
        }

        public User[] FetchFriends()
        {
            string[] usernames = FetchFriends(username);
            User[] users = new User[usernames.Length];
            for (int i = 0; i < usernames.Length; i++)
            {
                users[i] = Load(usernames[i]);
            }
            return users;
        }

        public static string[] FetchUsernamesByMySpaceID(string[] mySpaceIDs)
        {
            using (var db = new Model.AspNetDatingDataContext())
            {
                var res = from u in db.Users
                          where (from id in mySpaceIDs select id).Contains(u.u_myspaceid) &&
                          !u.u_deleted
                          select u.u_username;

                return res.ToArray();
            }
        }

        public static string[] FetchUsernamesByFacebookID(long[] facebookIDs)
        {
            using (var db = new Model.AspNetDatingDataContext())
            {
                var res = from u in db.Users
                          where (from id in facebookIDs select (long?)id).Contains(u.u_facebookid) &&
                                !u.u_deleted
                          select u.u_username;

                return res.ToArray();
            }
        }

        public static string[] FetchFriends(string username)
        {
            List<string> lUsernames = new List<string>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchFriends", username);

                while (reader.Read())
                {
                    lUsernames.Add((string)reader["Friend"]);
                }
            }

            return lUsernames.ToArray();
        }

        /// <summary>
        /// Fetches the mutually favourite users.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static string[] FetchMutuallyFavouriteUsers(string username)
        {
            string cacheKey = String.Format("User_FetchMutuallyFavouriteUsers_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as string[];
            }

            List<string> lUsernames = new List<string>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchMutuallyFavourites", username);

                while (reader.Read())
                {
                    lUsernames.Add((string)reader["Favourite"]);
                }
            }

            string[] favouriteUsernames = lUsernames.ToArray();
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, favouriteUsernames, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
            }

            return favouriteUsernames;
        }

        public static string[] FetchMutuallyFriends(string username)
        {
//            List<string> lUsernames = new List<string>();
//
//            using (SqlConnection conn = Config.DB.Open())
//            {
//                SqlDataReader reader =
//                    SqlHelper.ExecuteReader(conn, "FetchMutuallyFriends", username);
//
//                while (reader.Read())
//                {
//                    lUsernames.Add((string)reader["Friend"]);
//                }
//            }
//
//            return lUsernames.ToArray();

            using (var db = new AspNetDatingDataContext())
            {
                var res = from f in db.Friends
                          where f.u_username == username && f.f_accepted
                          select f.f_username;

                return res.ToArray();
            }
        }

        public static string[] FetchFriendsRequests(string username)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var res = from u in db.Users
                          join f in db.Friends
                          on 
                          new {u.u_username} equals new {f.u_username}
                          where f.f_username == username
                                && (from f2 in db.Friends where f2.u_username == username && f2.f_username == u.u_username select f2.u_username).Count() == 0
                          select u.u_username;

                return res.ToArray();
            }
        }

        /// <summary>
        /// Fetches the favourite time stamp.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public DateTime FetchFavouriteTimeStamp(string username)
        {
            if (!IsUserInFavouriteList(username))
            {
                throw new NotFoundException(Lang.Trans("No such user!"));
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                return (DateTime) SqlHelper.ExecuteScalar(conn, "FetchFavouriteTimeStamp", this.username, username);
            }
        }

        public DateTime FetchFriendTimeStamp(string username)
        {
            if (!IsUserInFriendList(username))
            {
                throw new NotFoundException(Lang.Trans("No such user!"));
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                return (DateTime)SqlHelper.ExecuteScalar(conn, "FetchFriendTimeStamp", this.username, username);
            }
        }

        public DateTime FetchFriendRequestTimeStamp(string username)
        {
            using (var db = new Model.AspNetDatingDataContext())
            {
                var res = from f in db.Friends
                          where f.u_username == username && f.f_username == this.username
                          select f.f_timestamp;

                return res.FirstOrDefault();
            }
        }

        /// <summary>
        /// Removes from favourites.
        /// </summary>
        /// <param name="username">The username.</param>
        public void RemoveFromFavourites(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "RemoveFromFavourites", this.username, username);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("User_FetchFavouriteUsers_{0}", this.username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("User_FetchMutuallyFavouriteUsers_{0}", this.username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("User_FetchMutuallyFavouriteUsers_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public void RemoveFromFriends(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "RemoveFromFriends", this.username, username);
            }
        }

        /// <summary>
        /// Fetches the blocked users.
        /// </summary>
        /// <returns></returns>
        public User[] FetchBlockedUsers()
        {
            Dictionary<string, DateTime> blockedUsers = FetchBlockedUsers(username);
            List<User> lBlockedUsers = new List<User>();

            foreach (KeyValuePair<string, DateTime> blockedUser in blockedUsers)
            {
                lBlockedUsers.Add(Load(blockedUser.Key));
            }

            return lBlockedUsers.ToArray();
        }

        /// <summary>
        /// Fetches the blocked users. If there are no blocked user it returns an empty dictionary.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Dictionary<string, DateTime> FetchBlockedUsers(string username)
        {
            string cacheKey = String.Format("User_FetchBlockedUsers_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Dictionary<string, DateTime>;
            }

            Dictionary<string, DateTime> dBlockedUsers = new Dictionary<string, DateTime>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchBlockedUsers", username);

                while (reader.Read())
                {
                    dBlockedUsers.Add((string)reader["BlockedUser"], (DateTime) reader["BlockedOn"]);
                }
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, dBlockedUsers, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
            }

            return dBlockedUsers;
        }

        /// <summary>
        /// Determines whether [has private photos].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has private photos]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPrivatePhotos()
        {
            string cacheKey = String.Format("User_HasPrivatePhotos_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (bool) HttpContext.Current.Cache[cacheKey];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                var hasPrivatePhotos = (bool) SqlHelper.ExecuteScalar(conn, "HasPrivatePhotos", username);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, hasPrivatePhotos, null, DateTime.Now.AddHours(1),
                                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }

                return hasPrivatePhotos;
            }
        }

        /// <summary>
        /// Determines whether [the specified username] [has access to private photos].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [the specified username] [has access to private photos] ; otherwise, <c>false</c>.
        /// </returns>
        public bool HasUserAccessToPrivatePhotos(string username)
        {
            return HasUserAccessToPrivatePhotos(this.username, username);
        }

        /// <summary>
        /// Determines whether [the specified photoowner] [has user access to private photos].
        /// </summary>
        /// <param name="photoowner">The photoowner.</param>
        /// <param name="photoviewer">The photoviewer.</param>
        /// <returns>
        /// 	<c>true</c> if [the specified photoowner] [has user access to private photos]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasUserAccessToPrivatePhotos(string photoowner, string photoviewer)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "HasUserAccessToPrivatePhotos", photoowner, photoviewer);
            }
        }

        /// <summary>
        /// Sets the access to private photos.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="grantAccess">if set to <c>true</c> [grant access].</param>
        public void SetAccessToPrivatePhotos(string username, bool grantAccess)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetAccessToPrivatePhotos", this.username, username, grantAccess);
            }

            Message msg = new Message(this.username, username);
            msg.Body = grantAccess
                           ? "I have given access to my private photos".Translate()
                           : "You no longer have access to my private photos".Translate();
            msg.Send();
        }

        /// <summary>
        /// Determines whether [has private video].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has private video]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPrivateVideo()
        {
            bool? isPrivate = VideoProfile.IsPrivate(username);
            if (isPrivate.HasValue && isPrivate.Value)
                return true;
            else return false;
        }

        /// <summary>
        /// Determines whether the user has private video upload.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the user has private video upload; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPrivateVideoUpload()
        {
            List<VideoUpload> videoUploads = VideoUpload.Load(null, username, true, true, true, null);
            return videoUploads.Count != 0;
        }

        /// <summary>
        /// Determines whether [the specified username] [has access to private video].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [the specified username] [has access to private video] ; otherwise, <c>false</c>.
        /// </returns>
        public bool HasUserAccessToPrivateVideo(string username)
        {
            return HasUserAccessToPrivateVideo(this.username, username);
        }

        /// <summary>
        /// Determines whether [the specified photoowner] [has user access to private photos].
        /// </summary>
        /// <param name="videoowner">The photoowner.</param>
        /// <param name="videoviewer">The photoviewer.</param>
        /// <returns>
        /// 	<c>true</c> if [the specified photoowner] [has user access to private photos]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasUserAccessToPrivateVideo(string videoowner, string videoviewer)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool)SqlHelper.ExecuteScalar(conn, "HasUserAccessToPrivateVideo", videoowner, videoviewer);
            }
        }

        /// <summary>
        /// Sets the access to private video.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="grantAccess">if set to <c>true</c> [grant access].</param>
        public void SetAccessToPrivateVideo(string username, bool grantAccess)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetAccessToPrivateVideo", this.username, username, grantAccess);
            }

            Message msg = new Message(this.username, username);
            msg.Body = grantAccess
                           ? "I have given access to my private video".Translate()
                           : "You no longer have access to my private video".Translate();
            msg.Send();
        }

        public bool HasPrivateAudio()
        {
            string cacheKey = String.Format("User_HasPrivateAudio_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (bool)HttpContext.Current.Cache[cacheKey];
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                var hasPrivateAudio = (bool)SqlHelper.ExecuteScalar(conn, "HasPrivateAudio", username);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, hasPrivateAudio, null, DateTime.Now.AddHours(1),
                                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }

                return hasPrivateAudio;
            }
        }

        public bool HasUserAccessToPrivateAudio(string username)
        {
            return HasUserAccessToPrivateAudio(this.username, username);
        }

        public static bool HasUserAccessToPrivateAudio(string audioowner, string audioviewer)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool)SqlHelper.ExecuteScalar(conn, "HasUserAccessToPrivateAudio", audioowner, audioviewer);
            }
        }

        public void SetAccessToPrivateAudio(string username, bool grantAccess)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetAccessToPrivateAudio", this.username, username, grantAccess);
            }

            Message msg = new Message(this.username, username);
            msg.Body = grantAccess
                           ? "I have given access to my private audio".Translate()
                           : "You no longer have access to my private audio".Translate();
            msg.Send();
        }
        
        /// <summary>
        /// Blocks the user.
        /// </summary>
        /// <param name="username">The username.</param>
        public void BlockUser(string username)
        {
            string cacheKey = String.Format("User_FetchBlockedUsers_{0}", this.username);

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "BlockUser", this.username, username);
            }
        }

        /// <summary>
        /// Unblocks the user.
        /// </summary>
        /// <param name="username">The username.</param>
        public void UnblockUser(string username)
        {
            string cacheKey = String.Format("User_FetchBlockedUsers_{0}", this.username);

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UnblockUser", this.username, username);
            }
        }

        /// <summary>
        /// Determines whether [is user blocked] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [is user blocked] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserBlocked(string username)
        {
            return IsUserBlocked(this.username, username);
        }

        /// <summary>
        /// Determines whether [is user blocked] [the specified userblocker].
        /// </summary>
        /// <param name="userblocker">The userblocker.</param>
        /// <param name="blockeduser">The blockeduser.</param>
        /// <returns>
        /// 	<c>true</c> if [is user blocked] [the specified userblocker]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserBlocked(string userblocker, string blockeduser)
        {
            // Optimization ;-) If blocked users are already cached then check there
            string cacheKey = String.Format("User_FetchBlockedUsers_{0}", userblocker);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                var blockedUsers = HttpContext.Current.Cache[cacheKey] as Dictionary<string, DateTime>;
                if (blockedUsers != null) 
                    return blockedUsers.ContainsKey(blockeduser);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "IsUserBlocked", userblocker, blockeduser);
            }
        }

        /// <summary>
        /// Sets as verified.
        /// </summary>
        /// <param name="username">The username.</param>
        public void SetAsVerified(string username)
        {
            SetAsVerified(this.username, username, false);
        }

        /// <summary>
        /// Sets as verified by admin.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void SetAsVerifiedByAdmin(string username)
        {
            SetAsVerified(null, username, true);
        }

        /// <summary>
        /// Sets as verified.
        /// </summary>
        /// <param name="verifiedby">The verifiedby.</param>
        /// <param name="verifieduser">The verifieduser.</param>
        /// <param name="byAdmin">if set to <c>true</c> [by admin].</param>
        private static void SetAsVerified(string verifiedby, string verifieduser, bool byAdmin)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetAsVerifiedUser", (object) verifiedby, verifieduser, byAdmin);
            }
        }

        /// <summary>
        /// Removes the verified status.
        /// </summary>
        /// <param name="username">The username.</param>
        public void RemoveVerifiedStatus(string username)
        {
            RemoveVerifiedStatus(this.username, username, false);
        }

        /// <summary>
        /// Removes the verified status by admin.
        /// </summary>
        /// <param name="username">The username.</param>
        public void RemoveVerifiedStatusByAdmin(string username)
        {
            RemoveVerifiedStatus(null, username, true);
        }

        /// <summary>
        /// Removes the verified status.
        /// </summary>
        /// <param name="verifiedby">The verifiedby.</param>
        /// <param name="verifieduser">The verifieduser.</param>
        /// <param name="byAdmin">if set to <c>true</c> [by admin].</param>
        private static void RemoveVerifiedStatus(string verifiedby, string verifieduser, bool byAdmin)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "RemoveVerifiedUserStatus", (object) verifiedby, verifieduser, byAdmin);
            }
        }

        /// <summary>
        /// Determines whether [is user verified] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [is user verified] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserVerified(string username)
        {
            return IsUserVerified(this.username, username, false);
        }

        /// <summary>
        /// Determines whether [is user verified] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="byAdmin">if set to <c>true</c> [by admin].</param>
        /// <returns>
        /// 	<c>true</c> if [is user verified] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserVerified(string username, bool byAdmin)
        {
            return IsUserVerified(null, username, byAdmin);
        }

        /// <summary>
        /// Determines whether [is user verified] [the specified verifiedby].
        /// </summary>
        /// <param name="verifiedby">The verifiedby.</param>
        /// <param name="verifieduser">The verifieduser.</param>
        /// <param name="byAdmin">if set to <c>true</c> [by admin].</param>
        /// <returns>
        /// 	<c>true</c> if [is user verified] [the specified verifiedby]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsUserVerified(string verifiedby, string verifieduser, bool byAdmin)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return
                    (bool)
                    SqlHelper.ExecuteScalar(conn, "IsUserVerified", (object) verifiedby, verifieduser,
                                            Config.Users.MinimumUserVotesToMarkMemberAsVerified, byAdmin);
            }
        }

        /// <summary>
        /// Gets the user verifications count.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static int GetUserVerificationsCount(string username)
        {
            return GetUserVerificationsCount(null, username);
        }

        /// <summary>
        /// Gets the user verifications count.
        /// </summary>
        /// <param name="verifiedby">The verifiedby.</param>
        /// <param name="verifieduser">The verifieduser.</param>
        /// <returns></returns>
        private static int GetUserVerificationsCount(string verifiedby, string verifieduser)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (int) SqlHelper.ExecuteScalar(conn, "FetchUserVerificationsCount", verifiedby, verifieduser);
            }
        }

        /// <summary>
        /// Sets as paid user.
        /// </summary>
        /// <param name="subscriptionID">The subscription ID.</param>
        /// <param name="isPaid">mark user as paid if set to <c>true</c>.</param>
        public static void SetAsPaidUser(int subscriptionID, bool isPaid)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "SetAsPaidUser", subscriptionID, isPaid);
            }
        }

        /// <summary>
        /// Determines whether the specified username is a paid member.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified username is a paid member; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSubscribedMember(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "IsPaidMember", username);
            }
        }

        /// <summary>
        /// Determines whether [is trial period expired] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [is trial period expired] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTrialPeriodExpired(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "IsTrialPeriodExpired", username, Config.Users.TrialPeriod);
            }
        }

        /// <summary>
        /// Gets the inactive users.
        /// </summary>
        /// <param name="forTheLastXDays">For the last X days.</param>
        /// <returns></returns>
        public static User[] GetInactiveUsers(int forTheLastXDays)
        {
            List<User> lInactiveUsers = new List<User>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchInactiveUsers", forTheLastXDays);

                while (reader.Read())
                {
                    lInactiveUsers.Add(Load((string) reader["Username"]));
                }
            }

            return lInactiveUsers.ToArray();
        }

        /// <summary>
        /// Determines whether [is non paid member] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [is non paid member] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        //[Obsolete]
        //public static bool IsNonPaidMember(string username)
        //{
        //    User user = Load(username);
        //    bool result = Config.Users.PaymentRequired &&
        //                  !IsSubscribedMember(username) &&
        //                  IsTrialPeriodExpired(username) &&
        //                  !(Config.Users.FreeForFemales && user.Gender == eGender.Female);

        //    return result;
        //}

        public static bool IsPaidMember(string username)
        {
            User user = Load(username);
            return (Config.Users.FreeForFemales && user.Gender == eGender.Female) || user.Credits > 0 || IsSubscribedMember(username);
        }

        /// <summary>
        /// Determines whether this instance [can respond to mail] the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="toUsername">To username.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can respond to mail] the specified username; otherwise, <c>false</c>.
        /// </returns>
        //public static bool CanRespondToMail(string username, string toUsername)
        //{
        //    bool usernamePaid = IsPaidMember(username);
        //    bool toUsernamePaid = IsPaidMember(toUsername);

        //    if (usernamePaid ||
        //        (Config.Users.NonPaidMembersCanRespondToPaidMembers &&
        //         toUsernamePaid && Message.MessagesExist(toUsername, username)))
        //        return true;
        //    else
        //        return false;
        //}

        /// <summary>
        /// Determines whether the specified username has profile.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="approved">if set to <c>true</c> the profile has to be approved.</param>
        /// <returns>
        /// 	<c>true</c> if the specified username has profile; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasProfile(string username, bool approved)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool) SqlHelper.ExecuteScalar(conn, "HasProfile", username, approved);
            }
        }

        /// <summary>
        /// Gets the zodiac sign.
        /// </summary>
        /// <returns></returns>
        public void GetZodiacSign(DateTime birthdate, out eZodiacSign zodiacSign)
        {
            zodiacSign = 0;
            if ((birthdate.Month == 3 && birthdate.Day >= 21) || (birthdate.Month == 4 && birthdate.Day <= 20))
            {
                zodiacSign = eZodiacSign.Aries;
            }
            else if ((birthdate.Month == 4 && birthdate.Day >= 21) || (birthdate.Month == 5 && birthdate.Day <= 21))
            {
                zodiacSign = eZodiacSign.Taurus;
            }
            else if ((birthdate.Month == 5 && birthdate.Day >= 22) || (birthdate.Month == 6 && birthdate.Day <= 21))
            {
                zodiacSign = eZodiacSign.Gemini;
            }
            else if ((birthdate.Month == 6 && birthdate.Day >= 22) || (birthdate.Month == 7 && birthdate.Day <= 23))
            {
                zodiacSign = eZodiacSign.Cancer;
            }
            else if ((birthdate.Month == 7 && birthdate.Day >= 24) || (birthdate.Month == 8 && birthdate.Day <= 23))
            {
                zodiacSign = eZodiacSign.Leo;
            }
            else if ((birthdate.Month == 8 && birthdate.Day >= 24) || (birthdate.Month == 9 && birthdate.Day <= 23))
            {
                zodiacSign = eZodiacSign.Virgo;
            }
            else if ((birthdate.Month == 9 && birthdate.Day >= 24) || (birthdate.Month == 10 && birthdate.Day <= 22))
            {
                zodiacSign = eZodiacSign.Libra;
            }
            else if ((birthdate.Month == 10 && birthdate.Day >= 23) || (birthdate.Month == 11 && birthdate.Day <= 22))
            {
                zodiacSign = eZodiacSign.Scorpio;
            }
            else if ((birthdate.Month == 11 && birthdate.Day >= 23) || (birthdate.Month == 12 && birthdate.Day <= 21))
            {
                zodiacSign = eZodiacSign.Sagittarius;
            }
            else if ((birthdate.Month == 12 && birthdate.Day >= 22) || (birthdate.Month == 1 && birthdate.Day <= 20))
            {
                zodiacSign = eZodiacSign.Capricorn;
            }
            else if ((birthdate.Month == 1 && birthdate.Day >= 21) || (birthdate.Month == 2 && birthdate.Day <= 19))
            {
                zodiacSign = eZodiacSign.Aquarius;
            }
            else if ((birthdate.Month == 2 && birthdate.Day >= 20) || (birthdate.Month == 3 && birthdate.Day <= 20))
            {
                zodiacSign = eZodiacSign.Pisces;
            }
        }

        #region Validators

        /// <summary>
        /// Validates the username.
        /// </summary>
        /// <param name="Username">The username.</param>
        public static void ValidateUsername(string Username)
        {
            if (Username == Config.Users.SystemUsername)
                return;

            if (Username == null)
            {
                throw new ArgumentNullException
                    (Lang.Trans("Username cannot be null!"));
            }

            if (Username.Length < Config.Users.UsernameMinLength)
            {
                throw new ArgumentException
                    (String.Format(Lang.Trans("Username too short! Should be at least {0} chars."),
                                   Config.Users.UsernameMinLength));
            }

            if (Username.Length > Config.Users.UsernameMaxLength)
            {
                throw new ArgumentException
                    (String.Format(Lang.Trans("Username too long! Should be at most {0} chars."),
                                   Config.Users.UsernameMaxLength));
            }

            if (!Regex.Match(Username, @"^[a-z\d_-]+$",
                             RegexOptions.IgnoreCase).Success)
            {
                throw new ArgumentException
                    (Lang.Trans("Invalid chars in username!"));
            }
        }

        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <param name="Password">The password.</param>
        public static void ValidatePassword(string Password)
        {
            if (Password == null)
            {
                throw new ArgumentNullException
                    (Lang.Trans("Password cannot be null!"));
            }

            if (Password.Length < Config.Users.PasswordMinLength)
            {
                throw new ArgumentException
                    (String.Format(Lang.Trans("Password too short! Should be at least {0} chars."),
                                   Config.Users.PasswordMinLength));
            }

            if (Password.Length > Config.Users.PasswordMaxLength)
            {
                throw new ArgumentException
                    (String.Format(Lang.Trans("Password too long! Should be at most {0} chars."),
                                   Config.Users.PasswordMaxLength));
            }
        }

        /// <summary>
        /// Validates the email. Throws ArgumentException.
        /// </summary>
        /// <param name="Email">The email.</param>
        public static void ValidateEmail(string Email)
        {
            if (Email == null)
            {
                throw new ArgumentNullException
                    (Lang.Trans("E-mail address cannot be null!"));
            }

            if (!Regex.Match(Email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}"
                                    + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\"
                                    + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Success)
            {
                throw new ArgumentException
                    (Lang.Trans("Invalid e-mail address!"));
            }
        }

        #endregion

        /// <summary>
        /// Gets the zodiac absolute image URL.
        /// </summary>
        /// <param name="zodiacSign">The zodiac sign.</param>
        /// <returns></returns>
        public static string GetZodiacAbsoluteImageUrl(eZodiacSign zodiacSign)
        {
            string imageUrl = "";

            switch (zodiacSign)
            {
                case eZodiacSign.Aquarius:
                    imageUrl = "~/Images/icon_zodiac_aquarius.png";
                    break;
                case eZodiacSign.Aries:
                    imageUrl = "~/Images/icon_zodiac_aries.png";
                    break;
                case eZodiacSign.Cancer:
                    imageUrl = "~/Images/icon_zodiac_cancer.png";
                    break;
                case eZodiacSign.Capricorn:
                    imageUrl = "~/Images/icon_zodiac_capricorn.png";
                    break;
                case eZodiacSign.Gemini:
                    imageUrl = "~/Images/icon_zodiac_gemini.png";
                    break;
                case eZodiacSign.Leo:
                    imageUrl = "~/Images/icon_zodiac_leo.png";
                    break;
                case eZodiacSign.Libra:
                    imageUrl = "~/Images/icon_zodiac_libra.png";
                    break;
                case eZodiacSign.Pisces:
                    imageUrl = "~/Images/icon_zodiac_pisces.png";
                    break;
                case eZodiacSign.Sagittarius:
                    imageUrl = "~/Images/icon_zodiac_sagittarius.png";
                    break;
                case eZodiacSign.Scorpio:
                    imageUrl = "~/Images/icon_zodiac_scorpio.png";
                    break;
                case eZodiacSign.Taurus:
                    imageUrl = "~/Images/icon_zodiac_taurus.png";
                    break;
                case eZodiacSign.Virgo:
                    imageUrl = "~/Images/icon_zodiac_virgo.png";
                    break;
            }

            return imageUrl;
        }

        /// <summary>
        /// Gets the zodiac image URL.
        /// </summary>
        /// <param name="zodiacSign">The zodiac sign.</param>
        /// <returns></returns>
        public static string GetZodiacImageUrl(eZodiacSign zodiacSign)
        {
            string imageUrl = "";

            switch (zodiacSign)
            {
                case eZodiacSign.Aquarius:
                    imageUrl = "Images/icon_zodiac_aquarius.png";
                    break;
                case eZodiacSign.Aries:
                    imageUrl = "Images/icon_zodiac_aries.png";
                    break;
                case eZodiacSign.Cancer:
                    imageUrl = "Images/icon_zodiac_cancer.png";
                    break;
                case eZodiacSign.Capricorn:
                    imageUrl = "Images/icon_zodiac_capricorn.png";
                    break;
                case eZodiacSign.Gemini:
                    imageUrl = "Images/icon_zodiac_gemini.png";
                    break;
                case eZodiacSign.Leo:
                    imageUrl = "Images/icon_zodiac_leo.png";
                    break;
                case eZodiacSign.Libra:
                    imageUrl = "Images/icon_zodiac_libra.png";
                    break;
                case eZodiacSign.Pisces:
                    imageUrl = "Images/icon_zodiac_pisces.png";
                    break;
                case eZodiacSign.Sagittarius:
                    imageUrl = "Images/icon_zodiac_sagittarius.png";
                    break;
                case eZodiacSign.Scorpio:
                    imageUrl = "Images/icon_zodiac_scorpio.png";
                    break;
                case eZodiacSign.Taurus:
                    imageUrl = "Images/icon_zodiac_taurus.png";
                    break;
                case eZodiacSign.Virgo:
                    imageUrl = "Images/icon_zodiac_virgo.png";
                    break;
            }

            return imageUrl;
        }

        /// <summary>
        /// Gets the zodiac tooltip.
        /// </summary>
        /// <param name="zodiacSign">The zodiac sign.</param>
        /// <returns></returns>
        public static string GetZodiacTooltip(eZodiacSign zodiacSign)
        {
            string tooltip = "";

            switch (zodiacSign)
            {
                case eZodiacSign.Aquarius:
                    tooltip = Lang.Trans("Aquarius");
                    break;
                case eZodiacSign.Aries:
                    tooltip = Lang.Trans("Aries");
                    break;
                case eZodiacSign.Cancer:
                    tooltip = Lang.Trans("Cancer");
                    break;
                case eZodiacSign.Capricorn:
                    tooltip = Lang.Trans("Capricorn");
                    break;
                case eZodiacSign.Gemini:
                    tooltip = Lang.Trans("Gemini");
                    break;
                case eZodiacSign.Leo:
                    tooltip = Lang.Trans("Leo");
                    break;
                case eZodiacSign.Libra:
                    tooltip = Lang.Trans("Libra");
                    break;
                case eZodiacSign.Pisces:
                    tooltip = Lang.Trans("Pisces");
                    break;
                case eZodiacSign.Sagittarius:
                    tooltip = Lang.Trans("Sagittarius");
                    break;
                case eZodiacSign.Scorpio:
                    tooltip = Lang.Trans("Scorpio");
                    break;
                case eZodiacSign.Taurus:
                    tooltip = Lang.Trans("Taurus");
                    break;
                case eZodiacSign.Virgo:
                    tooltip = Lang.Trans("Virgo");
                    break;
            }
            return tooltip;
        }

        /// <summary>
        /// Determines whether this instance is admin.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAdmin()
        {
            if (Username == Config.Users.SystemUsername)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether this sender can send message to the specified recipient.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="reason">The reason.</param>
        /// <returns>
        /// 	<c>true</c> if this sender can send message to the specified recipient; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanSendMessage(User sender, User recipient, out string reason)
        {
            bool result = true;
            reason = "";

            if (Classes.User.IsUserInFriendList(recipient.Username, sender.Username))
                return true;

            switch (recipient.IncomingMessagesRestrictions.MessagesFrom)
            {
                case Classes.IncomingMessagesRestrictions.eMessagesFrom.Men :
                    if (sender.gender != eGender.Male)
                    {
                        result = false;
                        reason = Lang.Trans("The user has restricted incoming messages from users of your gender.");
                        return result;
                    }
                    break;

                case Classes.IncomingMessagesRestrictions.eMessagesFrom.Women:
                    if (sender.gender != eGender.Female)
                    {
                        result = false;
                        reason = Lang.Trans("The user has restricted incoming messages from users of your gender.");
                        return result;
                    }
                    break;

                case Classes.IncomingMessagesRestrictions.eMessagesFrom.Favorites:
                    if (!IsUserInFavouriteList(recipient.Username, sender.Username))
                    {
                        result = false;
                        reason = Lang.Trans("The user has restricted incoming messages from users not in their favourite list.");
                        return result;
                    }
                    break;
            }

            if (sender.Age < recipient.IncomingMessagesRestrictions.AgeFrom ||sender.Age > recipient.IncomingMessagesRestrictions.AgeTo)
            {
                result = false;
                reason = Lang.Trans("The user has restricted incoming messages from users of your age.");
                return result;
            }

            if (recipient.IncomingMessagesRestrictions.PhotoRequired)
            {
                try
                {
                    sender.GetPrimaryPhoto();
                }
                catch (NotFoundException)
                {
                    result = false;
                    reason = Lang.Trans("The user has restricted incoming messages from users without photo.");
                    return result;
                }    
            }
            
            return result;
        }

        /// <summary>
        /// Determines whether the sender is an owner of some group and the recipient is a mamber of the same group.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        /// <returns>
        /// 	<c>true</c> if [is group owner] [the specified from user]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGroupOwner(User fromUser, User toUser)
        {
            bool result = false;
            Group[] groups = Group.FetchGroupsByUsername(fromUser.Username, true);

            if (groups.Length > 0)
            {
                foreach (Group group in groups)
                {
                    if (group.Owner == fromUser.username && GroupMember.IsMember(toUser.Username, group.ID))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the score.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="score">The score.</param>
        /// <param name="notes">The notes.</param>
        public static void AddScore(string username, int score, string notes)
        {
            if (score == 0) return;
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "AddUserScore", username, score, notes);
            }
        }

        /// <summary>
        /// Fetches the usernames of all users which has in their favorite list the specified user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static string[] FetchUsernamesWithFavoriteUser(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchUsernamesWithFavoriteUser", username);
                List<string> lUsernames = new List<string>();

                while (reader.Read())
                {
                    lUsernames.Add((string) reader["Username"]);
                }

                return lUsernames.ToArray();
            }
        }

        /// <summary>
        /// Determines whether the specified option is enabled.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>
        /// 	<c>true</c> if the specified option is enabled; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOptionEnabled(eUserOptions option)
        {
            return (Options & (ulong)option) == (ulong)option;
        }

        public static string[] FetchFavoritesNewPhotos(string username, DateTime fromDate)
        {
            List<string> lUsernames = new List<string>();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchFavoritesNewPhotos", username, fromDate);

                while (reader.Read())
                {
                    lUsernames.Add((string) reader["Username"]);
                }
            }

            return lUsernames.ToArray();
        }

        public static Hashtable FetchFavoritesNewBlogPosts(string username, DateTime fromDate)
        {
            Hashtable htUsers = new Hashtable();

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchFavoritesNewBlogPosts", username, fromDate);

                while (reader.Read())
                {
                    if (htUsers.ContainsKey(reader["Username"])) continue;
                    htUsers.Add(reader["Username"], (int)reader["BlogPostID"]);
                }
            }

            return htUsers;
        }

        public void ResetPersonalization()
        {
            _personalizationInfo = null;
            personalizationInfoDeserialized = null;

            Update();
        }

        /// <summary>
        /// Gets the non face control approved user.
        /// If there is no non face control approved user it returns NULL.
        /// </summary>
        /// <param name="approvedBy">The approved by.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="minPhotosRequired">The min photos required.</param>
        /// <returns></returns>
        public static User GetNonFaceControlApprovedUser(string approvedBy, eGender? gender, int minPhotosRequired)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchNonFaceControlApprovedUser", approvedBy, gender,
                                            minPhotosRequired);

                User user = new User();

                if (reader.Read())
                {
                    user.password = (string)reader["Password"];
                    user.email = (string)reader["Email"];
                    user.username = (string)reader["Username"];
                    user.name = (string)reader["Name"];
                    user.gender = (eGender)reader["Gender"];
                    if (!(reader["InterestedIn"] is Int32) ||
                        ((int)reader["InterestedIn"] == 0)
                        )
                    {
                        switch (user.gender)
                        {
                            case eGender.Male:
                                user.interestedIn = eGender.Female;
                                break;
                            case eGender.Female:
                                user.interestedIn = eGender.Male;
                                break;
                            case eGender.Couple:
                                user.interestedIn = eGender.Couple;
                                break;
                        }
                    }
                    else
                    {
                        user.interestedIn = (eGender)reader["InterestedIn"];
                    }
                    user.birthdate = (DateTime)reader["Birthdate"];
                    if (reader["Birthdate2"] is DateTime)
                    {
                        user.birthdate2 = (DateTime)reader["Birthdate2"];
                    }
                    user.active = (bool)reader["Active"];
                    user.userSince = (DateTime)reader["UserSince"];
                    user.prevLogin = (DateTime)reader["PrevLogin"];
                    user.lastLogin = (DateTime)reader["LastLogin"];
                    user.loginCount = (int)reader["LoginCount"];
                    user.lastOnline = (DateTime)reader["LastOnline"];
                    user.profileViews = (int)reader["ProfileViews"];
                    user.receiveEmails = (bool)reader["ReceiveEmails"];
                    user.profileVisible = (bool)reader["ProfileVisible"];
                    user.country = (string)reader["Country"];
                    user.state = (string)reader["State"];
                    user.ZipCode = (string)reader["ZipCode"];
                    user.city = (string)reader["City"];
                    user.deleted = (bool)reader["Deleted"];
                    user.paid = (bool)reader["Paid"];
                    user.lastSessionID = (string)reader["LastSessionID"];
                    if (reader["SignupIP"] != DBNull.Value)
                        user.signupIp = (string)reader["SignupIP"];
                    user.smsConfirmed = (bool)reader["SmsConfirmed"];
                    user.messageVerificationsLeft = (int)reader["MessageVerificationsLeft"];
                    user.languageId = (int)reader["LanguageId"];
                    if (reader["BillingDetails"] is string)
                        user.billingDetails = (string)reader["BillingDetails"];
                    if (reader["InvitedBy"] is string)
                        user.invitedBy = (string)reader["InvitedBy"];
                    user.incomingMessagesRestrictions = reader["IncomingMessagesRestrictions"] as string;
                    if (reader["DeleteReason"] is string)
                        user.deleteReason = (string)reader["DeleteReason"];
                    user.affiliateID = reader["AffiliateID"] != DBNull.Value ? (int?)reader["AffiliateID"] : null;
                    user.options = Convert.ToUInt64(reader["Options"]);
                    if (reader["Longitude"] is double)
                        user.longitude = (double)reader["Longitude"];
                    if (reader["Latitude"] is double)
                        user.latitude = (double)reader["Latitude"];
                    user.score = (int)reader["Score"];
                    user.tokenUniqueId = reader["TokenUniqueId"] as string;
                    user.personalizationInfo = reader["PersonalizationInfo"] as string;
                    user.credits = (int)reader["Credits"];
                    user.moderationScores = (int)reader["ModerationScores"];
                    user.SpamSuspected = (bool)reader["SpamSuspected"];
                    user.faceControlApproved = (bool)reader["FaceControlApproved"];
                    user.mySpaceID = reader["MySpaceID"] as string;
                    user.facebookID = reader["FacebookID"] as long?;

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void SetPhotoModerationApprovalScore(int photoID, bool approved, int scores, int penalty)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetPhotoModerationApprovalScore", photoID, approved, scores, penalty);
            }
        }

        public static void SetProfileModerationApprovalScore(string username, bool approved, int scores, int penalty)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SetProfileModerationApprovalScore", username, approved, scores, penalty);
            }
        }

        public static void CalculateMatchPercentage(string user1, string user2, out int percentage1, out int percentage2)
        {
            ProfileQuestion[] profileQuestions = ProfileQuestion.Fetch();
            var profileAnswers1 = new List<ProfileAnswer>(ProfileAnswer.FetchByUsername(user1) ?? new ProfileAnswer[0]);
            var profileAnswers2 = new List<ProfileAnswer>(ProfileAnswer.FetchByUsername(user2) ?? new ProfileAnswer[0]);
            percentage1 = CalculateMatchPercentage(profileQuestions, profileAnswers1, profileAnswers2);
            percentage2 = CalculateMatchPercentage(profileQuestions, profileAnswers2, profileAnswers1);
        }

        private static int CalculateMatchPercentage(ProfileQuestion[] profileQuestions, List<ProfileAnswer> profileAnswers1, 
            List<ProfileAnswer> profileAnswers2)
        {
            decimal points = 0;
            int count = 0;

            foreach (ProfileQuestion profileQuestion in profileQuestions)
            {
                if (profileQuestion.MatchField == null 
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.Hidden
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.MultiLine
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.SingleLine)
                {
                    continue;
                }

                count++;

                List<string> lChoices1 = new List<string>();
                List<string> lChoices2 = new List<string>();

                var answer1 = profileAnswers1.SingleOrDefault(a => a.Question.Id == profileQuestion.Id);
                var answer2 = profileAnswers2.SingleOrDefault(a => a.Question.Id == profileQuestion.MatchField);

                if (answer1 == null || answer2 == null)
                {
                    points += 0.5m;
                    continue;
                }
                
                if (profileQuestion.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceRadio
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceSelect)
                {
                    lChoices1.Add(answer1.Value);
                }
                else if (profileQuestion.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceCheck
                         || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceSelect)
                {
                    foreach (string answer in answer1.Value.Split(':'))
                    {
                        lChoices1.Add(answer);
                    }
                }

                ProfileQuestion profileQuestion2 = ProfileQuestion.Fetch(profileQuestion.MatchField.Value);

                if (profileQuestion2.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceRadio
                    || profileQuestion2.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceSelect)
                {
                    lChoices2.Add(answer2.Value);
                }
                else if (profileQuestion2.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceCheck
                         || profileQuestion2.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceSelect)
                {
                    foreach (string answer in answer2.Value.Split(':'))
                    {
                        lChoices2.Add(answer);
                    }
                }

                var choices = lChoices1.Intersect(lChoices2);

                points += choices.Count()/(decimal) lChoices1.Count;
            }

            if (count != 0) return (int)(points / count * 100);
            else return -1;
        }

        /// <summary>
        /// Gets the random photo id.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="minAge">The min age.</param>
        /// <param name="maxAge">The max age.</param>
        /// <returns></returns>
        public static int GetRandomPhotoId(string username, eGender? gender, int? minAge, int? maxAge)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, "GetRandomPhotoId",
                    username, gender,
                    maxAge == null ? null : (object)DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1) * 365.25)),
                    minAge == null ? null : (object)DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value * 365.25)),
                    Config.Ratings.RatePhotosForUsersActiveWithinXXDays));
            }
        }

        /// <summary>
        /// Checks is the recipient is online and sends the online event notification.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="text">The text.</param>
        /// <param name="thumbnailUrl">The thumbnail URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        public static void SendOnlineEventNotification(string sender, string recipient, string text, string thumbnailUrl, string redirectUrl)
        {
            if (IsOnline(recipient) || IsUsingNotifier(recipient))
            {
                var notification = new GenericEventNotification
                {
                    Recipient = recipient,
                    Sender = sender,
                    Text = text,
                    ThumbnailUrl = thumbnailUrl,
                    RedirectUrl = redirectUrl
                };
                RealtimeNotification.SendNotification(notification);
            }
        }

        public double? GetDistanceFromUser(User user, bool inKilometers)
        {
            Location from = Config.Users.GetLocation(this);
            Location to = Config.Users.GetLocation(user);

            if (from != null && to != null)
            {
                return Distance.GetDistance(from, to, inKilometers ? 'k' : 'm');
            }

            return null;
        }
    }

    /// <summary>
    /// The class that contains the current user session
    /// </summary>
    [Serializable]
    public class UserSession : User
    {
        public bool LoggedInThroughFacebook { get; set; }

        private bool isAuthorized = false;

        /// <summary>
        /// Gets a value indicating whether this instance is authorized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authorized; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthorized
        {
            get { return isAuthorized; }
        }

        private bool hasProfile;
        private bool hasProfileIsNull = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has profile.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has profile; otherwise, <c>false</c>.
        /// </value>
        public new bool HasProfile
        {
            get
            {
                if (hasProfileIsNull)
                {
                    hasProfile = User.HasProfile(base.Username, false);
                    hasProfileIsNull = false;
                }

                return hasProfile;
            }

            set
            {
                hasProfile = value;
                hasProfileIsNull = false;
            }
        }

        private bool hasApprovedProfile;
        private bool hasApprovedProfileIsNull = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has approved profile.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has approved profile; otherwise, <c>false</c>.
        /// </value>
        public bool HasApprovedProfile
        {
            get
            {
                if (hasApprovedProfileIsNull)
                {
                    hasApprovedProfile = User.HasProfile(base.Username, true);
                    hasApprovedProfileIsNull = false;
                }

                return hasApprovedProfile;
            }

            set
            {
                hasApprovedProfile = value;
                hasApprovedProfileIsNull = false;
            }
        }

        public bool HasPhotos 
        {
            get
            {
                return Photo.Fetch(Username) != null;
            }
        }

        private bool canVote = false;
        private bool canVoteIsSet = false;

        /// <summary>
        /// Gets a value indicating whether this instance can vote.
        /// </summary>
        /// <value><c>true</c> if this instance can vote; otherwise, <c>false</c>.</value>
        public bool CanVote
        {
            get
            {
                if (!canVoteIsSet)
                {
                    try
                    {
                        BasicSearch search = new BasicSearch();
                        search.deleted_isSet = false;
                        search.active_isSet = false;
                        search.hasAnswer_isSet = false;
                        search.visible_isSet = false;
                        search.IP = SignupIp;
                        canVote = IsUserVerified(Username, true) ||
                                  ((Config.Ratings.MaxAccountsPerIP == 0 || SignupIp == "0.0.0.0" ||
                                    search.GetResults().Usernames.Length <= Config.Ratings.MaxAccountsPerIP)
                                   &&
                                   UserSince <= DateTime.Now.Subtract(TimeSpan.FromDays(Config.Ratings.MinDaysToVote))
                                   && LoginCount >= Config.Ratings.MinLoginsToVote
                                   && ProfileViews >= Config.Ratings.MinViewsToVote);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError("UserSession_CanVote", err);
                    }
                    canVoteIsSet = true;
                }

                return canVote;
            }
        }

        //private bool? isNonPaidMember;
        private BillingPlanOptions billingPlanOptions;

        /// <summary>
        /// Gets or sets the billing plan options.
        /// </summary>
        /// <value>The billing plan options.</value>
        public BillingPlanOptions BillingPlanOptions
        {
            get
            {
                if (billingPlanOptions == null)
                {
                    Subscription subscription = Subscription.FetchActiveSubscription(Username);

                    if (subscription == null)
                        billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                    else
                    {
                        BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                        billingPlanOptions = plan.Options;
                    }
                }
                //if (!Config.Users.PaymentRequired)
                //{
                //    billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                //}
                //else
                //{
                //    if (!isNonPaidMember.HasValue)
                //        isNonPaidMember = !IsPaidMember(Username);

                //    if (isNonPaidMember.Value)
                //    {
                //        billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                //    }
                //    else
                //    {
                //        if (billingPlanOptions == null)
                //        {
                //            Subscription subscription = Subscription.FetchActiveSubscription(Username);
                //            //the active subscription may be null
                //            //1.when the site is not in paid mode
                //            //2.the user is marked as paid but has no subscription - this is allowed in old
                //            //versions
                //            //3."Free for Females" is on and the user is female
                //            if (subscription == null)
                //                billingPlanOptions = new BillingPlanOptions();
                //            else
                //            {
                //                BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                //                billingPlanOptions = plan.Options;
                //            }
                //        }
                //    }
                //}
                
                return billingPlanOptions;
            }
            set
            {
                //isNonPaidMember = null;
                billingPlanOptions = value;
            }
        }


        /// <summary>
        /// Default constructor.
        /// Used for creating session user object.
        /// </summary>
        /// <param name="Username">Username for the user account.</param>
        public UserSession(string Username) : base(Username)
        {
            Load();
        }

        /// <summary>
        /// Checks the provided password against the database and marks
        /// the user as authorized if match.
        /// </summary>
        /// <param name="Password">The password.</param>
        /// <param name="sessionID">The session ID.</param>
        public void Authorize(string Password, string sessionID)
        {
            Authorize(Username, Password, sessionID);

            UpdateLastOnline(true);

            Load(); // reloads user data in order to update lastLogin, prevLogin and loginCount fields

            isAuthorized = true;
        }

        /// <summary>
        /// Authorizes this instance.
        /// </summary>
        public void Authorize(string sessionID)
        {
            updateLastLogin(sessionID);
            UpdateLastOnline(true);

            isAuthorized = true;
        }

        private struct onlineUserData
        {
            public eGender Gender;
            public int Age;
            public DateTime DateEntered;
        }

        private static readonly Dictionary<string, onlineUserData> onlineUsers =
            new Dictionary<string, onlineUserData>();
        private static void addOnline(string username, eGender gender, int age)
        {
            lock (onlineUsers)
            {
                if (!onlineUsers.ContainsKey(username))
                    onlineUsers.Add(username, new onlineUserData()
                    {
                        Gender = gender,
                        Age = age,
                        DateEntered = DateTime.Now
                    });
            }
        }

        private static void removeOnline(string username)
        {
            lock (onlineUsers)
            {
                if (onlineUsers.ContainsKey(username))
                    onlineUsers.Remove(username);
            }
        }

        public static void SortUsersByOnlineStatus(List<string> usernames, bool keepOnlineUsersOnly)
        {
            for (int i = usernames.Count - 1; i >= 0; --i)
            {
                if (onlineUsers.ContainsKey(usernames[i]))
                {
                    string username = usernames[i];
                    usernames.Remove(username);
                    usernames.Add(username);
                }
            }

            if (keepOnlineUsersOnly)
                usernames.RemoveAll(u => !onlineUsers.ContainsKey(u));
        }

        public static string[] getOnline(eGender? gender, int? minAge, int? maxAge)
        {
            lock (onlineUsers)
            {
                var result = onlineUsers.Where(u => (!gender.HasValue || u.Value.Gender == gender.Value)
                                               && (!minAge.HasValue || u.Value.Age >= minAge)
                                               && (!maxAge.HasValue || u.Value.Age <= maxAge))
                                               .OrderByDescending(u => u.Value.DateEntered).Select(u => u.Key);
                return result.ToArray();
            }
        }

        public static void UpdateLastNotifierCheck(string username)
        {
            if (HttpContext.Current == null) return;
            var cacheKey = "LastNotifierCheck_" + username;
            if (HttpContext.Current.Cache[cacheKey] == null)
            {
                HttpContext.Current.Cache.Add(cacheKey, DateTime.Now, null,
                                              DateTime.Now.AddMinutes(Config.Users.NotifierCheckTime + 1),
                                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                                              null);
            }
            else
            {
                HttpContext.Current.Cache.Insert(cacheKey, DateTime.Now, null,
                                                 DateTime.Now.AddMinutes(Config.Users.NotifierCheckTime + 1),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                                                 null);

            }
        }

        /// <summary>
        /// Updates the last online.
        /// </summary>
        public void UpdateLastOnline(bool updateDB)
        {
            if (StealthMode)
                return;

            if (HttpContext.Current != null)
            {
                var cacheKey = "LastOnline_" + Username;
                if (HttpContext.Current.Cache[cacheKey] == null)
                {
                    HttpContext.Current.Cache.Add(cacheKey, DateTime.Now, null, DateTime.Now.AddMinutes(Config.Users.OnlineCheckTime + 1),
                                                  Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                                                  LastOnlineCacheExpired);

                    if (ProfileVisible)
                        addOnline(Username, Gender, Age);
                }
                else
                {
                    HttpContext.Current.Cache.Insert(cacheKey, DateTime.Now, null, DateTime.Now.AddMinutes(Config.Users.OnlineCheckTime + 1),
                                                  Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                                                  LastOnlineCacheExpired);
                    if (ProfileVisible)
                        addOnline(Username, Gender, Age);
                }
            }
            
            if (updateDB || HttpContext.Current == null)
            {
                UpdateLastOnlineInDB(Username);
            }
        }

        private static void LastOnlineCacheExpired(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Removed) return;
            var username = key.Substring(11); // Strip the "LastOnline_" from the cache key
            removeOnline(username);
            UpdateLastOnlineInDB(username);
        }

        private static void UpdateLastOnlineInDB(string username)
        {
            using (var conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UpdateLastOnline", username);
            }
        }


        public PermissionCheckResult CanBrowseGroups()
        {
            return CanDoAction(BillingPlanOptions.CanBrowseGroups);
        }

        public PermissionCheckResult CanJoinGroups()
        {
            return CanDoAction(BillingPlanOptions.CanJoinGroups);
        }

        public PermissionCheckResult CanIM()
        {
            return CanDoAction(BillingPlanOptions.CanIM);
        }

        public PermissionCheckResult CanReadEmail()
        {
            return CanDoAction(BillingPlanOptions.CanReadEmail);
        }

        public PermissionCheckResult CanViewStreamedVideo()
        {
            return CanDoAction(BillingPlanOptions.CanViewStreamedVideo);
        }

        public PermissionCheckResult CanBroadcastVideo()
        {
            return CanDoAction(BillingPlanOptions.CanBroadcastVideo);
        }

        public PermissionCheckResult CanViewPhotos()
        {
            return CanDoAction(BillingPlanOptions.CanViewPhotos);
        }

        public PermissionCheckResult CanViewUserVideos()
        {
            return CanDoAction(BillingPlanOptions.CanViewUserVideos);
        }

        public PermissionCheckResult CanViewExplicitPhotos()
        {
            return CanDoAction(BillingPlanOptions.CanViewExplicitPhotos);
        }

        public PermissionCheckResult CanCreateGroups()
        {
            return CanDoAction(BillingPlanOptions.CanCreateGroups);
        }

        public PermissionCheckResult CanSeeMessageStatus()
        {
            return CanDoAction(BillingPlanOptions.CanSeeMessageStatus);
        }

        public PermissionCheckResult CanRateProfiles()
        {
            return CanDoAction(BillingPlanOptions.CanRateProfiles);
        }

        public PermissionCheckResult CanRatePhotos()
        {
            return CanDoAction(BillingPlanOptions.CanRatePhotos);
        }

        public PermissionCheckResult CanCreateBlogs()
        {
            return CanDoAction(BillingPlanOptions.CanCreateBlogs);
        }

        public PermissionCheckResult CanUseChat()
        {
            return CanDoAction(BillingPlanOptions.UserCanUseChat);
        }

        public PermissionCheckResult CanBrowseClassifieds()
        {
            return CanDoAction(BillingPlanOptions.UserCanBrowseClassifieds);
        }

        public PermissionCheckResult CanAddComments()
        {
            return CanDoAction(BillingPlanOptions.UserCanAddComments);
        }

        public PermissionCheckResult CanSendEcards()
        {
            return CanDoAction(BillingPlanOptions.CanSendEcards);
        }

        public PermissionCheckResult CanUseSkin()
        {
            return CanDoAction(BillingPlanOptions.UserCanUseSkin);
        }

        public PermissionCheckResult CanEditSkin()
        {
            return CanDoAction(BillingPlanOptions.UserCanEditSkin);
        }

        public PermissionCheckResult CanPostAd()
        {
            return CanDoAction(BillingPlanOptions.UserCanPostAd);
        }

        private PermissionCheckResult CanDoAction(BillingPlanOption<bool> option)
        {
            if (this.IsAdmin() || option.Value)
                return PermissionCheckResult.Yes;

            if (Config.Users.FreeForFemales && this.Gender == eGender.Female &&
                (option.EnableCreditsPayment || option.UpgradableToNextPlan))
                return PermissionCheckResult.Yes;

            if (option.EnableCreditsPayment)
            {
                if (this.Credits < option.Credits)
                    return PermissionCheckResult.YesButMoreCreditsNeeded;
                else
                    return PermissionCheckResult.YesWithCredits;
            }

            if (option.UpgradableToNextPlan)
                return PermissionCheckResult.YesButPlanUpgradeNeeded;

            return PermissionCheckResult.No;
        }
    }

    public enum PermissionCheckResult
    {
        Yes,
        YesWithCredits,
        YesButMoreCreditsNeeded,
        YesButPlanUpgradeNeeded,
        No
    }

    /// <summary>
    /// Incoming messages restrictions
    /// </summary>
    [Serializable]
    public class IncomingMessagesRestrictions
    {
        /// <summary>
        /// Specifies what users can send messages
        /// </summary>
        public eMessagesFrom MessagesFrom = eMessagesFrom.All;
        /// <summary>
        /// Specifies the minimum age to send message
        /// </summary>
        public int AgeFrom = Config.Users.MinAge;
        /// <summary>
        /// Specifies the maximum age to send message
        /// </summary>
        public int AgeTo = Config.Users.MaxAge;
        /// <summary>
        /// Specifies whether photo is required to send a message
        /// </summary>
        public bool PhotoRequired = false;

        /// <summary>
        /// The types of users who can send messages
        /// </summary>
        public enum eMessagesFrom
        {
            /// <summary>
            /// Men only
            /// </summary>
            Men,
            /// <summary>
            /// Women only
            /// </summary>
            Women,
            /// <summary>
            /// Favorites only
            /// </summary>
            Favorites,
            /// <summary>
            /// Everyone
            /// </summary>
            All,
            Couples
        }
    }

    /// <summary>
    /// The user account options
    /// </summary>
    public enum eUserOptions : ulong
    {
        /// <summary>
        /// 
        /// </summary>
        DisableProfileRating = 1L,
        /// <summary>
        /// 
        /// </summary>
        DisableProfileVoting = 1L << 1,
        /// <summary>
        /// 
        /// </summary>
        DisableProfileComments = 1L << 2,
        /// <summary>
        /// 
        /// </summary>
        DisablePhotoRating = 1L << 3,
        /// <summary>
        /// 
        /// </summary>
        DisableBlogComments = 1L << 4,
        /// <summary>
        /// 
        /// </summary>
        DisableProfileViews = 1L << 5,
        /// <summary>
        /// 
        /// </summary>
        HideFriends = 1L << 6,
        /// <summary>
        /// 
        /// </summary>
        HideGroupMembership = 1L << 7,
        /// <summary>
        /// 
        /// </summary>
        DisableLevelIcon = 1L << 8,
        /// <summary>
        /// 
        /// </summary>
        DisablePhotoComments = 1L << 9,
        VisitorsCanViewProfile = 1L << 10,
        UsersCanViewProfile = 1L << 11,
        VisitorsCanViewPhotos = 1L << 12,
        UsersCanViewPhotos = 1L << 13,
        VisitorsCanViewFriends = 1L << 14,
        UsersCanViewFriends = 1L << 15,
        VisitorsCanViewVideos = 1L << 16,
        UsersCanViewVideos = 1L << 17,
        VisitorsCanViewBlog = 1L << 18,
        UsersCanViewBlog = 1L << 19,
        FriendsOfFriendsCanViewProfile = 1L << 20,
        FriendsOfFriendsCanViewPhotos = 1L << 21,
        FriendsOfFriendsCanViewFriends = 1L << 22,
        FriendsOfFriendsCanViewVideos = 1L << 23,
        FriendsOfFriendsCanViewBlog = 1L << 24,
    }
}
