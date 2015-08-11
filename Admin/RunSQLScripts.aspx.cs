using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using AspNetDating.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace AspNetDating.Admin
{
    public partial class RunSQLScripts : AdminPageBase
    {
        private string sqlConnectionString = ConfigurationManager.ConnectionStrings["aspnetdating"].ConnectionString;

        public RunSQLScripts()
        {
            RequiresAuthorization = false;
        }

        private bool RedirectIfCredentialsAvailable()
        {
            try
            {
                if (CurrentAdminSession == null &&
                    AdminsTableExists() && AdminRecordExists())
                {
                    Response.Redirect("~/Admin/Login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                    return true;
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divInstallation.Visible = false;
                return true;
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RedirectIfCredentialsAvailable())
                return;

            if (!IsPostBack)
            {
                var files = Directory.GetFiles(Server.MapPath("~/SQL/"));
                if (files.Length == 0)
                {
                    lblError.Text = String.Format("No SQL files have been found in {0}", Server.MapPath("~/SQL/"));
                    divInstallation.Visible = false;

                }

                txtEmail.Text = "admin@aspnetdating.com";
                foreach (var file in files)
                {
                    ddSqlFiles.Items.Add(new ListItem(Path.GetFileName(file), file));
                }


                PopulateDatabaseObjects();

                divInstallation.Visible = true;
            }
        }

        private void PopulateDatabaseObjects()
        {
            var tableNames = FetchTableNames();

            lbTables.DataSource = tableNames;
            lbTables.DataBind();

            var spNames = FetchSPNames();

            lbSPs.DataSource = spNames;
            lbSPs.DataBind();
        }

        private IEnumerable<string> FetchTableNames()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                                                      "select name from sys.tables order by name");

                List<string> tableNames = new List<string>();
                while (reader.Read())
                {
                    tableNames.Add((string)reader["name"]);
                }

                return tableNames;
            }
        }

        private IEnumerable<string> FetchSPNames()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                var reader = SqlHelper.ExecuteReader(conn, CommandType.Text,
                                                      "select name from sys.procedures order by name");

                var spNames = new List<string>();
                while (reader.Read())
                {
                    spNames.Add((string)reader["name"]);
                }

                return spNames;
            }
        }

        private bool AdminsTableExists()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool)SqlHelper.ExecuteScalar(conn, CommandType.Text,
                                                      "IF EXISTS(SELECT * FROM sys.tables WHERE name = N'Admins') SELECT CAST (1 AS BIT) ELSE SELECT CAST(0 AS BIT)");
            }
        }

        private bool AdminRecordExists()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool)SqlHelper.ExecuteScalar(conn, CommandType.Text,
                                                      "IF (SELECT COUNT(1) from Admins) > 0 SELECT CAST (1 AS BIT) ELSE SELECT CAST(0 AS BIT)");
            }
        }

        private bool Run(string fileName)
        {
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                Microsoft.SqlServer.Management.Smo.Server server = new Server(new ServerConnection(conn));

                //foreach (string fileName in sqlScriptFiles)
                //{
                    try
                    {
                        FileInfo file = new FileInfo(fileName);

                        string script;

                        using (var stream = file.OpenText())
                        {
                            script = stream.ReadToEnd();
                        }

                        script = Regex.Replace(script, @"/\*(.|\n)*?\*/", String.Empty);
                        script = Regex.Replace(script, @"^--.*?$", String.Empty, RegexOptions.Multiline);
                        if (fileName.EndsWith("DefaultSettings.sql"))
                        {
                            script = script.Replace("admin@aspnetdating.com", txtEmail.Text);
                        }

                        string[] statements = Regex.Split(script, @"^GO$", RegexOptions.Multiline);

                        var context = server.ConnectionContext;

                        context.BeginTransaction();
                        foreach (var statement in statements)
                        {
                            context.ExecuteNonQuery(statement);
                        }

                        context.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = String.Format("Exception has been thrown during the execution, Exception : {0}, InnerException: {1}", 
                            ex.Message, ex.InnerException == null ? String.Empty : ex.InnerException.Message);
                        return false;
                    }
                    return true;
                //}
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (Run(ddSqlFiles.SelectedValue))
                lblError.Text = String.Format("{0} executed successfully!", ddSqlFiles.SelectedItem.Text);

            PopulateDatabaseObjects();

            RedirectIfCredentialsAvailable();
        }
    }
}