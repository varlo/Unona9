﻿using System;
using System.Web;
using System.Web.Services;
using System.Xml;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GroupPhotoData : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            XmlWriter xml = XmlWriter.Create(context.Response.Output);
            xml.WriteStartDocument();
            xml.WriteStartElement("playlist", "http://xspf.org/ns/0/");
            xml.WriteStartElement("trackList");

            GroupPhoto[] groupPhotos = GroupPhoto.FetchByGroupID(Convert.ToInt32(context.Request.Params["gid"]), GroupPhoto.eSortColumn.DateUploaded);
            if (groupPhotos != null && groupPhotos.Length > 0)
            {
                foreach (GroupPhoto groupPhoto in groupPhotos)
                {
                    xml.WriteStartElement("track");
                    xml.WriteElementString("title", groupPhoto.Name);
                    xml.WriteElementString("creator", groupPhoto.Username);
                    xml.WriteElementString("location", GroupImage.CreateImageUrl(groupPhoto.ID, 450, 450, true));
                    xml.WriteRaw("<meta rel=\"type\">jpg</meta>");
                    xml.WriteEndElement();
                }
            }

            xml.WriteEndElement();
            xml.WriteEndElement();
            xml.WriteEndDocument();
            xml.Close();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion
    }
}