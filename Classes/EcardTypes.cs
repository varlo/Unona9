using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class EcardType
    {
        #region fields

        private int? id = null;
        private string name = String.Empty;
        private byte[] content;
        private eType type;
        private bool active = false;
        private eSortColumn sortColumn;

        public enum eType
        {
            Image,
            Flash
        }

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Name
        }

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;
                }
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public byte[] Content
        {
            get
            {
                if (content == null)
                {
                    content = LoadContent(ID);
                }
                return content;
            }
            set { content = value; }
        }

        public eType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        public int? CreditsRequired { get; set; }

        #endregion

        #region Methods

        public static EcardType[] Fetch()
        {
            return Fetch(null, null, null, null, eSortColumn.None);
        }

        public static EcardType[] Fetch(bool active)
        {
            return Fetch(null, null, null, active, eSortColumn.Name);
        }

        public static EcardType Fetch(int id)
        {
            EcardType[] ecards = Fetch(id, null, null, null, eSortColumn.None);

            if (ecards.Length > 0)
            {
                return ecards[0];
            }

            return null;
        }

        private static EcardType[] Fetch(int? id, string name, eType? type, bool? active, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchEcardTypes",
                                                               id,
                                                               name,
                                                               type,
                                                               active,
                                                               sortColumn);

                List<EcardType> lEcardType = new List<EcardType>();

                while (reader.Read())
                {
                    EcardType ecardType = new EcardType();

                    ecardType.id = (int)reader["ID"];
                    ecardType.name = (string) reader["Name"];
                    ecardType.type = (eType) reader["Type"];
                    ecardType.active = (bool) reader["Active"];
                    ecardType.CreditsRequired = reader["CreditsRequired"] as int?;

                    lEcardType.Add(ecardType);
                }

                return lEcardType.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveEcardType",
                                                        id,
                                                        name,
                                                        Content,
                                                        type,
                                                        active, CreditsRequired);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteEcardType", id);
            }
        }

        public static byte[] LoadContent(int ecardTypeID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchEcardTypeContent", ecardTypeID);

                if (reader.Read())
                {
                    byte[] buffer = (byte[])reader["Content"];

                    return buffer;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void SaveContent(int ecardTypeID, byte[] content)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "SaveEcardTypeContent", ecardTypeID, content);
            }
        }

        #endregion
    }
}
