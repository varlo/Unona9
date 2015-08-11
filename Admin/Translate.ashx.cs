using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetDating.Classes;
using System.Web.SessionState;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for Translate
    /// </summary>
    public class Translate : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var key = context.Request["keyValue"];
            var updateValue = context.Request["update_value"];
            var originalHtml = context.Request["original_html"];
            var elementID = context.Request["element_id"];
            var languageId = Int32.Parse(context.Request["languageId"]);
            var adminPanel = Boolean.Parse(context.Request["adminPanel"]);

            if (!HasWriteAccess)
            {
                context.Response.Write(HttpUtility.HtmlEncode(originalHtml));
                return;
            }

            if (originalHtml != updateValue)
            {
                Translation.SaveTranslation(languageId, key, updateValue, adminPanel);
            }


            context.Response.Write(HttpUtility.HtmlEncode(updateValue));
        }

        private bool HasWriteAccess
        {
            get
            {
                if (Config.AdminSettings.ReadOnly)
                    return false;

                bool hasWriteAccess = true;

                var adminSession = HttpContext.Current.Session["AdminSession"] as AdminSession;

                if (adminSession == null)
                    return false;

                if (adminSession.Username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled)
                {
                    hasWriteAccess = (adminSession.Privileges.editTexts & Classes.Admin.eAccess.Write) != 0;
                }

                return hasWriteAccess;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}