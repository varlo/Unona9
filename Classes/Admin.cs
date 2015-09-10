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
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Xml.Serialization;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// Represents the administrator. Provides methods to create, retrieve, update and delete admin objects.
    /// </summary>
    [Serializable]
    public class Admin
    {
        /// <summary>
        /// Specifies the access mode.
        /// </summary>
        [Flags]
        public enum eAccess
        {
            /// <summary>
            /// No access.
            /// </summary>
            None = 0,

            /// <summary>
            /// Can read.
            /// </summary>
            Read = 1,

            /// <summary>
            /// Can write.
            /// </summary>
            Write = 2,

            /// <summary>
            /// Can read and write.
            /// </summary>
            ReadWrite = 3
        }

        public class RawAdminPrivileges
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RawAdminPrivileges"/> class.
            /// </summary>
            public RawAdminPrivileges()
            {
                lSections = new List<string>(20);
                lReadPermissions = new List<bool>(20);
                lWritePermissions = new List<bool>(20);
            }

            public List<string> lSections;
            public List<bool> lReadPermissions;
            public List<bool> lWritePermissions;
        }

        /// <summary>
        /// Represents privileges of the administrator. Every admin account can have different privileges.
        /// For example some admin accounts can browse messages of the users but others can't.
        /// </summary>
        [Serializable]
        public class AdminPrivileges
        {
            //add new sections here
            [Reflection.DescriptionAttribute("Manage Users")]
            public eAccess userAccounts = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Messages")]
            public eAccess browseMessages = eAccess.None;

            [Reflection.DescriptionAttribute("Browse CreditHistory")]
            public eAccess browseCreditHistory = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Texts")]
            public eAccess editTexts = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Content Pages")]
            public eAccess editContentPages = eAccess.None;

            [Reflection.DescriptionAttribute("Edit e-cards")]
            public eAccess editEcards = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Photos")]
            public eAccess browsePhotos = eAccess.None;

            [Reflection.DescriptionAttribute("Photos Approval")]
            public eAccess photoApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Salute Photos Approval")]
            public eAccess salutePhotoApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Videos Approval")]
            public eAccess videoApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Audio Approval")]
            public eAccess audioApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Answers Approval")]
            public eAccess answerApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Blog Post Approval")]
            public eAccess blogPostApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Send Announcement")]
            public eAccess sendAnnouncement = eAccess.None;

            [Reflection.DescriptionAttribute("Spam Check")]
            public eAccess spamCheck = eAccess.None;

            [Reflection.DescriptionAttribute("Abuse Reports")]
            public eAccess abuseReports = eAccess.None;

            [Reflection.DescriptionAttribute("User Levels")]
            public eAccess userLevels = eAccess.None;

            [Reflection.DescriptionAttribute("Topics & Questions")]
            public eAccess topicsQuestions = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Meta Tags")]
            public eAccess editMetaTags = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Languages")]
            public eAccess editLanguages = eAccess.None;

            [Reflection.DescriptionAttribute("Manage WebPart Components")]
            public eAccess manageWebParts = eAccess.None;

            [Reflection.DescriptionAttribute("News")]
            public eAccess news = eAccess.None;

            [Reflection.DescriptionAttribute("Polls")]
            public eAccess polls = eAccess.None;

            [Reflection.DescriptionAttribute("Manage Bad Words")]
            public eAccess manageBadWords = eAccess.None;            

            [Reflection.DescriptionAttribute("Templates")]
            public eAccess templates = eAccess.None;

            [Reflection.DescriptionAttribute("Settings")]
            public eAccess generalSettings = eAccess.None;

            [Reflection.DescriptionAttribute("Manage Admins")]
            public eAccess manageAdmins = eAccess.None;

            [Reflection.DescriptionAttribute("Billing Settings")]
            public eAccess billingSettings = eAccess.None;

            [Reflection.DescriptionAttribute("Groups Approval")]
            public eAccess groupApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Groups")]
            public eAccess browseGroups = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Groups")]
            public eAccess editGroups = eAccess.None;

            [Reflection.DescriptionAttribute("Manage Group Categories")]
            public eAccess manageGroupCategories = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Affiliates")]
            public eAccess browseAffiliates = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Affiliates")]
            public eAccess editAffiliates = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Affiliate Banners")]
            public eAccess browseAffiliatesBanners = eAccess.None;
            
            [Reflection.DescriptionAttribute("Browse Affiliate Payments")]
            public eAccess browseAffiliatesPayments = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Affiliates Payment History")]
            public eAccess browseAffiliatesPaymentHistory = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Affiliate Commissions")]
            public eAccess browseAffiliateCommissions = eAccess.None;

            [Reflection.DescriptionAttribute("Manage Contests")]
            public eAccess manageContests= eAccess.None;

            [Reflection.DescriptionAttribute("Browse Video Uploads")]
            public eAccess browseVideoUploads = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Credits Packages")]
            public eAccess browseCreditsPackages = eAccess.None;

            [Reflection.DescriptionAttribute("Browse Spam Suspects")]
            public eAccess browseSpamSuspects = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Classified Categories")]
            public eAccess editAdsCategories = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Classifieds")]
            public eAccess editAds = eAccess.None;

            [Reflection.DescriptionAttribute("Classifieds Approval")]
            public eAccess adsApproval = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Google Analytics")]
            public eAccess editGoogleAnalytics = eAccess.None;

            [Reflection.DescriptionAttribute("Edit Banners")]
            public eAccess editBanners = eAccess.None;

            [Reflection.DescriptionAttribute("Upload Logo")]
            public eAccess uploadLogo = eAccess.None;

            [Reflection.DescriptionAttribute("Change Theme")]
            public eAccess changeTheme = eAccess.None;

            //Everyone will have access to gadgets except if in readonly mode
            public eAccess generateGadgets = eAccess.ReadWrite;

            public eAccess browseNewUsersStats = eAccess.ReadWrite;

            public eAccess browseOnlineUsersStats = eAccess.ReadWrite;
        }

        #region Properties

        private string username;

        /// <summary>
        /// The username for the user account.
        /// </summary>
        /// <exception cref="ArgumentException">Throwed if username is invalid</exception>
        public string Username
        {
            get { return username; }
            set
            {
                try
                {
                    ValidateUsername(value);
                }
                catch
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
        /// <exception cref="ArgumentException">Throwed if the password is invalid</exception>
        public string Password
        {
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

        protected string lastSessionID = String.Empty;

        /// <summary>
        /// Gets the last session ID.
        /// The property is read-only.
        /// </summary>
        /// <value>The last session ID.</value>
        public string LastSessionID
        {
            get { return lastSessionID; }
        }

        private AdminPrivileges privileges;

        /// <summary>
        /// Gets or sets the privileges of the admin.
        /// </summary>
        /// <value>The privileges.</value>
        public AdminPrivileges Privileges
        {
            get
            {
                if (privileges == null)
                    privileges = new AdminPrivileges();
                return privileges;
            }
            set { privileges = value; }
        }

        private DateTime lastlogin = DateTime.MinValue;

        /// <summary>
        /// Gets the last login.
        /// The property is read-only.
        /// </summary>
        /// <value>The last login.</value>
        public DateTime LastLogin
        {
            get { return lastlogin; }
        }

        #endregion

        public Admin()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Default constructor.
        /// Used for creating of new or virtual user accounts.
        /// </summary>
        /// <param name="Username">Username for the user account.</param>
        public Admin(string Username)
        {
            this.Username = Username;
        }

        /// <summary>
        /// Validates user credentials against the DB
        /// </summary>
        /// <param name="Username">Username for the user account.</param>
        /// <param name="Password">Password for the user account.</param>
        /// <param name="SessionID">SessionID for the user account.</param>
        /// <exception cref="ArgumentException">Username or Password is invalid.</exception>
        /// <exception cref="NotFoundException">Username was not found.</exception>
        /// <exception cref="AccessDeniedException">Password is invalid.</exception>
        public static void Authorize(string Username, string Password, string SessionID)
        {
            Admin admin = Authorize(Username, Password);
            admin.updateLastLogin(SessionID);
        }

        public static Admin Authorize(string Username, string Password)
        {
            ValidateUsername(Username);
            ValidatePassword(Password);

            Admin admin = Load(Username);

            if (!admin.IsPasswordIdentical(Password))
                throw new AccessDeniedException
                    (Lang.Trans("The provided password is invalid!"));

            return admin;
        }

        /// <summary>
        /// Updates the last login by specified sessionID to the current date and time.
        /// </summary>
        /// <param name="sessionID">The session ID.</param>
        protected void updateLastLogin(string sessionID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UpdateAdminLastLogin", Username, sessionID);
            }
        }

        /// <summary>
        /// Stores the specified admin in the DB.
        /// </summary>
        /// <param name="admin">The admin.</param>
        public static void Create(Admin admin)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "CreateAdmin",
                                          admin.username, admin.password, Misc.ToXml(admin.Privileges));
            }
        }

        /// <summary>
        /// Loads admin account data from DB by specified username.
        /// </summary>
        /// <param name="Username">Username identifying the admin</param>
        /// <returns>User object</returns>
        /// <exception cref="NotFoundException">Username was not found.</exception>
        public static Admin Load(string Username)
        {
            Admin admin = new Admin(Username);
            admin.Load();
            return admin;
        }

        /// <summary>
        /// Loads admin account data from DB.
        /// </summary>
        /// <exception cref="NotFoundException">The specified account does not exist.</exception>
        public void Load()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "LoadAdmin", username);

                if (reader.Read())
                {
                    username = (string)reader["Username"];
                    password = (string)reader["Password"];
                    lastSessionID = (string)reader["LastSessionID"];
                    lastlogin = (DateTime)reader["LastLogin"];
                    try
                    {
                        if (username == Config.Users.SystemUsername)
                        {
                            privileges = GetFullAccess();
                        }
                        else
                            privileges = Misc.FromXml<AdminPrivileges>((string)reader["Privileges"]);
                    }
                    catch
                    {
                        //no privileges found ... default will be used instead
                    }
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The specified account does not exist!"));
                }
            }
        }

        /// <summary>
        /// Determines whether the password is identical with the specified one.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// 	<c>true</c> if the passwords are identical; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPasswordIdentical(string password)
        {
            if (this.password == FormsAuthentication
                                     .HashPasswordForStoringInConfigFile(password, "sha1"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Updates admin account data
        /// </summary>
        /// <param name="admin">User object</param>
        public static void Update(Admin admin)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UpdateAdmin",
                                          admin.username, admin.password, Misc.ToXml(admin.Privileges));
            }
        }

        /// <summary>
        /// Updates user account data.
        /// </summary>
        public void Update()
        {
            Update(this);
        }

        /// <summary>
        /// Fetches admin by specified username from DB.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Admin object or null if the admin with the specified username doesn't exists.</returns>
        public static Admin Fetch(string username)
        {
            Admin[] admins = FetchAdmins(username);

            if (admins.Length > 0)
                return admins[0];
            else
                return null;
        }

        /// <summary>
        /// Fetches all admins from DB.
        /// </summary>
        /// <returns>All admins. If there are no admins in DB it returns an empty array.</returns>
        public static Admin[] Fetch()
        {
            return FetchAdmins(null);
        }

        /// <summary>
        /// Fetches admin by specified username from DB or fetches all admins if the argument is null.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>An array with admins.</returns>
        /// <example>The following code example fetches all admins from DB.
        /// <code>
        /// Admin[] allAdmins = Admin.FetchAdmins(null);
        /// </code>
        /// </example>
        private static Admin[] FetchAdmins(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader;
                if (username == null || username == "")
                    reader = SqlHelper.ExecuteReader(conn, "FetchAdmins");
                else
                    reader = SqlHelper.ExecuteReader(conn, "FetchAdmins", username);

                List<Admin> lAdmins = new List<Admin>();

                while (reader.Read())
                {
                    Admin admin = new Admin();

                    admin.username = (string)reader["Username"];
                    admin.password = (string)reader["Password"];
                    admin.lastlogin = (DateTime)reader["LastLogin"];
                    try
                    {
                        if (admin.Username == Config.Users.SystemUsername)
                        {
                            admin.Privileges = GetFullAccess();
                        }
                        else
                            admin.Privileges = Misc.FromXml<AdminPrivileges>((string)reader["Privileges"]);
                    }
                    catch
                    {
                        //no privileges found ... default will be used instead
                    }

                    lAdmins.Add(admin);
                }

                return lAdmins.ToArray();
            }
        }

        /// <summary>
        /// Deletes the admin by specified username from DB.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void Delete(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteAdmin", username);
            }
        }

        /// <summary>
        /// Determines whether the specified username is taken.
        /// </summary>
        /// <param name="Username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified username is taken; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUsernameTaken(string Username)
        {
            ValidateUsername(Username);

            try
            {
                Load(Username);

                // If successfull then admin exists
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the raw data from privileges.
        /// </summary>
        /// <param name="privileges">The privileges.</param>
        /// <returns></returns>
        public static RawAdminPrivileges GetRawDataFromPrivileges(AdminPrivileges privileges)
        {
            RawAdminPrivileges rawdata = new RawAdminPrivileges();

            FieldInfo[] fis = privileges.GetType().GetFields();
            foreach (FieldInfo fi in fis)
            {
                if (fi.FieldType == typeof(eAccess))
                {
                    Reflection.DescriptionAttribute att =
                        Attribute.GetCustomAttribute(fi, typeof(Reflection.DescriptionAttribute)) as
                        Reflection.DescriptionAttribute;
                    eAccess access = (eAccess)fi.GetValue(privileges);

                    if (att != null)
                    {
                        rawdata.lSections.Add(att.Description);
                        rawdata.lReadPermissions.Add((access & eAccess.Read) == eAccess.Read);
                        rawdata.lWritePermissions.Add((access & eAccess.Write) == eAccess.Write);
                    }
                    else continue;
                }
            }

            return rawdata;
        }

        /// <summary>
        /// Gets the privileges from raw data.
        /// </summary>
        /// <param name="rawdata">The rawdata.</param>
        /// <returns></returns>
        public static AdminPrivileges GetPrivilegesFromRawData(RawAdminPrivileges rawdata)
        {
            AdminPrivileges privileges = new AdminPrivileges();

            for (int i = 0; i < rawdata.lSections.Count; ++i)
            {
                foreach (FieldInfo fi in typeof(AdminPrivileges).GetFields())
                {
                    Reflection.DescriptionAttribute att =
                        Attribute.GetCustomAttribute(fi, typeof(Reflection.DescriptionAttribute)) as
                        Reflection.DescriptionAttribute;

                    if (att != null && att.Description == rawdata.lSections[i])
                    {
                        eAccess readAccess = rawdata.lReadPermissions[i] ? eAccess.Read : eAccess.None;
                        eAccess writeAccess = rawdata.lWritePermissions[i] ? eAccess.Write : eAccess.None;

                        fi.SetValue(privileges, readAccess | writeAccess);
                        break;
                    }
                }
            }

            return privileges;
        }

        /// <summary>
        /// Gets the full access.
        /// </summary>
        /// <returns>AdminPrivileges with full access.</returns>
        private static AdminPrivileges GetFullAccess()
        {
            AdminPrivileges privileges = new AdminPrivileges();

            foreach (FieldInfo fi in typeof(AdminPrivileges).GetFields())
            {
                if (fi.FieldType == typeof(eAccess))
                    fi.SetValue(privileges, eAccess.Read | eAccess.Write);
            }

            return privileges;
        }

        ///// <summary>
        ///// Serializes the specified AdminPrivileges to XML.
        ///// </summary>
        ///// <param name="privileges">The privileges.</param>
        ///// <returns>String which contains admin privileges.</returns>
        //private static string ToXml(AdminPrivileges privileges)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    XmlSerializer xmls = new XmlSerializer(typeof(AdminPrivileges), "");
        //    xmls.Serialize(ms, privileges);

        //    byte[] buffer = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(buffer, 0, (int)ms.Length);

        //    return new string(Encoding.UTF8.GetChars(buffer));
        //}

        ///// <summary>
        ///// Deserializes the specified AdminPrivileges from xml.
        ///// </summary>
        ///// <param name="xml">The XML.</param>
        ///// <returns>AdminPrivileges object which contains admin privileges.</returns>
        //private static AdminPrivileges FromXml(string xml)
        //{
        //    if (xml == null)
        //    {
        //        return null;
        //    }

        //    XmlSerializer xmls = new XmlSerializer(typeof(AdminPrivileges));
        //    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        //    return (AdminPrivileges)xmls.Deserialize(ms);
        //}

        #region Validators

        /// <summary>
        /// Validates the specified username.
        /// </summary>
        /// <param name="Username">The username.</param>
        /// <exception cref="ArgumentNullException">
        /// Username is null
        /// -or-
        /// is too short. The username length is defined in admin section.
        /// -or-
        /// is too long. The username length is defined in admin section.
        /// -or-
        /// contains invalid characters.
        /// </exception>
        public static void ValidateUsername(string Username)
        {
            if (Username == null)
                throw new ArgumentNullException
                    (Lang.Trans("Username cannot be null!"));

            if (Username.Length < Config.Users.UsernameMinLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Username too short! Should be at least {0} chars."),
                                   Config.Users.UsernameMinLength));

            if (Username.Length > Config.Users.UsernameMaxLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Username too long! Should be at most {0} chars."),
                                   Config.Users.UsernameMaxLength));

            if (!Regex.Match(Username, @"^[\w_-]+$").Success)
                throw new ArgumentException
                    (Lang.Trans("Invalid chars in username!"));
        }

        /// <summary>
        /// Validates the specified password.
        /// </summary>
        /// <param name="Password">The password.</param>
        /// <exception cref="ArgumentNullException">
        /// Password is null
        /// -or-
        /// is too short. The password length is defined in admin section.
        /// -or-
        /// is too long. The password length is defined in admin section.
        /// </exception>
        public static void ValidatePassword(string Password)
        {
            if (Password == null)
                throw new ArgumentNullException
                    (Lang.Trans("Password cannot be null!"));

            if (Password.Length < Config.Users.PasswordMinLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Password too short! Should be at least {0} chars."),
                                   Config.Users.PasswordMinLength));

            if (Password.Length > Config.Users.PasswordMaxLength)
                throw new ArgumentException
                    (String.Format(Lang.Trans("Password too long! Should be at most {0} chars."),
                                   Config.Users.PasswordMaxLength));
        }

        #endregion
    }

    /// <summary>
    /// Represents administrator session. Provides method for authorization.
    /// </summary>
    [Serializable]
    public class AdminSession : Admin
    {
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

        /// <summary>
        /// Default constructor.
        /// Used for creating session admin object.
        /// </summary>
        /// <param name="Username">Username for the admin account.</param>
        public AdminSession(string Username)
            : base(Username)
        {
            Load();
        }

        /// <summary>
        /// Checks the provided password against the database and marks
        /// the admin as authorized if match.
        /// </summary>
        /// <param name="Password">The password.</param>
        public new void Authorize(string Password, string SessionID)
        {
            Authorize(Username, Password, SessionID);
            lastSessionID = SessionID;
            isAuthorized = true;
        }
    }
}