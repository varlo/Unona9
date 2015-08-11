﻿using System.Web;
using System.Web.Services;
using System.Xml;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UserPhotoData : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            XmlWriter xml = XmlWriter.Create(context.Response.Output);
            xml.WriteStartDocument();
            xml.WriteStartElement("playlist", "http://xspf.org/ns/0/");
            xml.WriteStartElement("trackList");

            Photo[] photos = Photo.Fetch(context.Request.Params["uid"]);
            if (photos != null && photos.Length > 0)
            {
                foreach (Photo photo in photos)
                {
                    if (!photo.Approved || photo.PrivatePhoto || photo.ExplicitPhoto) continue;
                    xml.WriteStartElement("track");
                    xml.WriteElementString("title", photo.Name);
                    xml.WriteElementString("creator", photo.Username);
                    xml.WriteElementString("location", ImageHandler.CreateImageUrl(photo.Id,
                                                                                   450, 450, false, true, false));
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