using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AspNetDating.Classes;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class EcardContent : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ecardTypeID = 0;

            try
            {
                ecardTypeID = Convert.ToInt32(context.Request.Params["ect"]);
            }
            catch (Exception)
            {
                return;
            }

            EcardType ecardType = EcardType.Fetch(ecardTypeID);

            if (ecardType != null && ecardType.Content != null)
            {
                string contentType = String.Empty;

                if (ecardType.Type == EcardType.eType.Image) contentType = "image/jpeg";
                else contentType = "application/x-shockwave-flash";

                context.Response.Clear();
                context.Response.ContentType = contentType;
                context.Response.BinaryWrite(ecardType.Content);
                //context.Response.Flush();
                //context.Response.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
