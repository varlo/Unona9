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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// IP logger
    /// </summary>
    public static class IPLogger
    {
        /// <summary>
        /// Action type
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// User logged in successfully
            /// </summary>
            Login = 1,
            /// <summary>
            /// Profile deleted by user
            /// </summary>
            DeleteProfile = 2,
            /// <summary>
            /// Administrator logged in successfully
            /// </summary>
            AdminLoginSuccess = 3,
            /// <summary>
            /// Administrator failed to log in
            /// </summary>
            AdminLoginFailed = 4,
            /// <summary>
            /// User edited by administrator
            /// </summary>
            AdminEditUser = 5,
            /// <summary>
            /// User deleted by administrator
            /// </summary>
            AdminDeleteUser = 6,
            /// <summary>
            /// User confirmed their account via SMS
            /// </summary>
            SmsConfirmed = 7,
            /// <summary>
            /// Affiliate login succeeded
            /// </summary>
            AffiliateLoginSuccess = 8,
            /// <summary>
            /// Affiliate login failed
            /// </summary>
            AffiliateLoginFailed = 9,
            /// <summary>
            /// Login via CardSpace
            /// </summary>
            LoginCardSpace = 10
        }

        /// <summary>
        /// Logs the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="ip">The ip.</param>
        /// <param name="action">The action.</param>
        public static void Log(string username, string ip, ActionType action)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SaveIPLog", username, ip, (int) action);
            }
        }
    }
}