﻿using System.Web;
using System.Web.Services;

namespace AspNetDating
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ChartData : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            string guid = context.Request.Params["guid"];
            string chartData = context.Cache.Get(guid) as string;
            if (string.IsNullOrEmpty(chartData)) return;
            context.Response.Write(chartData);
            context.Cache.Remove(guid);
        }

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion
    }
}