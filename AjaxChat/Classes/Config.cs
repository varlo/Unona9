#if !AJAXCHAT_INTEGRATION
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace AjaxChat.Classes
{
    public static class DB
    {
        private static string connectionString;

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public static string ConnectionString
        {
            get
            {
                if (connectionString == null)
                {
                    if (HttpContext.Current != null 
                        && HttpContext.Current.ApplicationInstance is IHttpApplicationConnectionStringProvider)
                    {
                        connectionString = ((IHttpApplicationConnectionStringProvider)
                                            HttpContext.Current.ApplicationInstance).GetConnectionString();
                    }
                    if (ConfigurationManager.ConnectionStrings["AjaxChat"] != null)
                    {
                        connectionString = ConfigurationManager.
                            ConnectionStrings["AjaxChat"].ConnectionString;
                    }
                }
                return connectionString;
            }
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection Open()
        {
            SqlConnection conSql = new SqlConnection(ConnectionString);
            conSql.Open();
            return conSql;
        }

        public static T ParseDBNull<T>(object value)
        {
            if (value == null || value == DBNull.Value) return (T)(object)null;
            return (T) value;
        }

        public static T ConvertDBNull<T>(object value)
        {
            if (value == DBNull.Value) return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static T ConvertDBNull<T>(object value, T defaultValue)
        {
            if (value == DBNull.Value) return defaultValue;
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
#endif