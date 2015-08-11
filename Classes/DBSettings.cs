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
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class DBSettings
    {
        static DBSettings()
        {
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">if set to <c>true</c> [default].</param>
        /// <returns></returns>
        public static bool Get(string Key, bool Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return Convert.ToBoolean(ret);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">The default.</param>
        /// <returns></returns>
        public static int Get(string Key, int Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return Convert.ToInt32(ret);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">The default.</param>
        /// <returns></returns>
        public static double Get(string Key, double Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return Convert.ToDouble(ret);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">The default.</param>
        /// <returns></returns>
        public static decimal Get(string Key, decimal Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return Convert.ToDecimal(ret);
        }


        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">The default.</param>
        /// <returns></returns>
        public static string Get(string Key, string Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return ret;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">The default.</param>
        /// <returns></returns>
        public static Color Get(string Key, Color Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return ret == String.Empty ? Color.Empty : Reflection.StringToColor(ret);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Default">The default.</param>
        /// <returns></returns>
        public static Hashtable Get(string Key, Hashtable Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return Reflection.StringToHashtable(ret);
        }

        public static DateTime Get(string Key, DateTime Default)
        {
            string ret = Get(Key);
            if (ret == null)
            {
                if (Key != null)
                    Set(Key, Default);

                return Default;
            }
            else return Convert.ToDateTime(ret, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        private static string Get(string Key)
        {
            if (HttpContext.Current != null && Key != null)
            {
                object value = HttpContext.Current.Cache.Get("DBSettings_" + Key);
                if (value is string)
                {
                    return (string) value;
                }
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                string value = (string) SqlHelper.ExecuteScalar(conn,
                                                                "FetchSetting", Key);

                if (HttpContext.Current != null && Key != null && value != null)
                {
                    HttpContext.Current.Cache.Insert("DBSettings_" + Key,
                                                     value, null, DateTime.Now.AddHours(1),
                                                     Cache.NoSlidingExpiration);
                }
                return value;
            }
        }


        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">if set to <c>true</c> [value].</param>
        public static void Set(string Key, bool Value)
        {
            Set(Key, Convert.ToString(Value));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        public static void Set(string Key, int Value)
        {
            Set(Key, Convert.ToString(Value));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        public static void Set(string Key, double Value)
        {
            Set(Key, Convert.ToString(Value));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        public static void Set(string Key, decimal Value)
        {
            Set(Key, Convert.ToString(Value));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        public static void Set(string Key, Hashtable Value)
        {
            Set(Key, Reflection.HashtableToString(Value));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        public static void Set(string Key, Color Value)
        {
            if (Value == Color.Empty)
                Set(Key, "");
            else
                Set(Key, Reflection.ColorToString(Value, true));
        }

        public static void Set(string Key, DateTime Value)
        {
            Set(Key, Value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        public static void Set(string Key, string Value)
        {
            if (Value == null)
            {
                return;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "UpdateSetting", Key, Value);
            }
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Remove("DBSettings_" + Key);
            }
        }
    }
}