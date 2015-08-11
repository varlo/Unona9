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
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AspNetDating.Admin;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace AspNetDating.Classes
{
    [XmlRoot("dictionary")]
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public SerializableDictionary()
        {
        }

        protected SerializableDictionary(SerializationInfo info,
                                         StreamingContext context) : base(info, context)
        {
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }


        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                var key = (TKey) keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var value = (TValue) valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }


        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof (TKey));
            var valueSerializer = new XmlSerializer(typeof (TValue));

            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        #endregion
    }

    public static class Misc
    {
        #region MessageType enum

        public enum MessageType
        {
            Error,
            Success
        }

        #endregion

        private static readonly char[] hexDigits = {
                                                       '0', '1', '2', '3', '4', '5', '6', '7',
                                                       '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
                                                   };

        public static string ToHexString(byte[] bytes)
        {
            var chars = new char[bytes.Length*2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i*2] = hexDigits[b >> 4];
                chars[i*2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }

        public static string HMACSHA1ToHex(string data, string key)
        {
            var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(key));
            return ToHexString(hmac.ComputeHash(Encoding.ASCII.GetBytes(data)));
        }

        public static string ToXml<T>(T source)
        {
            using (var ms = new MemoryStream())
            {
                var xmls = new XmlSerializer(typeof(T), "");
                xmls.Serialize(ms, source);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static T FromXml<T>(string xml)
        {
            if (xml == null)
            {
                return (T)(object)null;
            }

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var xmls = new XmlSerializer(typeof(T));
                return (T)xmls.Deserialize(ms);
            }
        }


        public static string RenderToString(this Control control)
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var hw = new HtmlTextWriter(tw);

            control.RenderControl(hw);
            return sb.ToString();
        }

        public static bool GetTempFileName(out string fileName)
        {
            fileName = null;
            string folder = HttpContext.Current.Server.MapPath("~/Temp");

            if (!checkFolderAccess(folder))
                return false;

            Guid guid = Guid.NewGuid();
            fileName = folder + @"\" + guid + ".tmp";

            return true;
        }

        private static bool checkFolderAccess(string folder)
        {
            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            Guid guid = Guid.NewGuid();
            var testFile = folder + @"\" + guid + ".temp";
            try
            {
                TextWriter tw = File.CreateText(testFile);
                tw.Close();
                File.Delete(testFile);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static Array Add(this Array array1, Array array2)
        {
            var array3 = new Array[array1.Length + array2.Length];
            array1.CopyTo(array3, 0);
            array2.CopyTo(array3, array1.Length);
            return array3;
        }

        public static string CalculateChatAuthHash(string username, string targetUsername, string timestamp)
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["AuthSecretKey"]))
                throw new Exception("AuthSecretKey must be specified in your web.config file");

            var sha1 = new SHA1Managed();
            var paramBytes = Encoding.UTF8.GetBytes(username + targetUsername + timestamp + ConfigurationManager.AppSettings["AuthSecretKey"]);
            var hashBytes = sha1.ComputeHash(paramBytes);
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hash;
        }

        public static IEnumerable<T> Select<T>(Control from, Predicate<Control> predicate) where T : class
        {
            if (from is T)
            {
                if (predicate(from))
                    yield return from as T;
            }
            foreach (Control idx in from.Controls)
            {
                foreach (var idxInner in Select<T>(idx, predicate))
                {
                    yield return idxInner;
                }
            }
        }

        public static IEnumerable<T> Select<T>(Control from) where T : class
        {
            return Select<T>(from, idx => idx is T);
        }

        public static bool IsMobileBrowser()
        {
            HttpContext context = HttpContext.Current;

            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                    new[]
                        {
                            "240x320", "blackberry", "symbian", "android",
                            "wireless", "nokia", "phone", "iphone"
                        };

                for (int i = 0; i < mobiles.Length; i++)
                {
                    if (context.Request.ServerVariables["HTTP_USER_AGENT"]
                        .IndexOf(mobiles[i], StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public static class IPToCountry
    {
        private static readonly BitVectorTrie m_trie = new BitVectorTrie();
        private static bool IpDataLoaded;
        public static int NetworkCodeCount;

        private static void Load(string filename)
        {
            StreamReader nccin;
            try
            {
                nccin = new StreamReader(filename);
            }
            catch (Exception)
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                nccin = new StreamReader(filename);
            }
            Load(nccin);
        }

        private static void Load(TextReader nccin)
        {
            try
            {
                string line;
                var seps = new[] {'|'};
                while ((line = nccin.ReadLine()) != null)
                {
                    string[] data = line.Split(seps);

                    if ((data.Length > 2) && (data[1].Length == 2) && (data[3].IndexOf('.') >= 0))
                    {
                        AddIp(data[3], data[1]);
                        NetworkCodeCount++;
                    }
                }
            }
            catch (Exception e)
            {
                Global.Logger.LogError(e);
            }
            finally
            {
                nccin.Close();
            }
        }

        private static void Load()
        {
            foreach (string filename in Directory.GetFiles(Config.Directories.Home + @"\IpData", "*.txt"))
            {
                Load(filename);
            }

            IpDataLoaded = true;
        }

        public static string GetCountry(string ip)
        {
            if (!IpDataLoaded)
                Load();

            BitVector key = IpToBitVector(ip);

            if (key != null)
                return (string) m_trie.GetBest(key);
            return null;
        }

        private static void AddIp(string ip, string country)
        {
            BitVector key = IpToBitVector(ip);
            if (key == null)
                return;

            m_trie.Add(key, String.Intern(country.ToUpper()));
        }

        private static BitVector IpToBitVector(string ip)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);

            if (ipAddress == null || ipAddress.AddressFamily != AddressFamily.InterNetwork)
                    return null;

            string[] elements = ip.Split('.');
            var bv = new BitVector();
            foreach (string e in elements)
            {
                int i = Int32.Parse(e);
                bv.AddData(i, 8);
            }
            return bv;
        }

        public static void InitializeUpdateIpDefinitionFilesTimer()
        {
            var timer = new System.Timers.Timer {AutoReset = true, Interval = TimeSpan.FromHours(1).TotalMilliseconds};
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            // Run processing the 1st time
            timer_Elapsed(null, null);
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("IPToCountry_LastUpdateIpDefinitionFiles", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromDays(7))
            {
                ThreadPool.QueueUserWorkItem(UpdateIpDefinitionFiles, null);
                DBSettings.Set("IPToCountry_LastUpdateIpDefinitionFiles", DateTime.Now);
            }
        }

        public static void UpdateIpDefinitionFiles(object data)
        {
            Global.Logger.LogStatus("UpdateIpDefinitionFiles starting...");

            try
            {
                DownloadIpDefinitionFile("delegated-apnic-latest",
                                         "ftp://ftp.apnic.net/pub/stats/apnic/delegated-apnic-latest");
                DownloadIpDefinitionFile("delegated-arin-latest",
                                         "ftp://ftp.arin.net/pub/stats/arin/delegated-arin-latest");
                DownloadIpDefinitionFile("delegated-lacnic-latest",
                                         "ftp://ftp.lacnic.net/pub/stats/lacnic/delegated-lacnic-latest");
                DownloadIpDefinitionFile("delegated-ripencc-latest",
                                         "ftp://ftp.ripe.net/ripe/stats/delegated-ripencc-latest");
            }
            catch (Exception err)
            {
                Global.Logger.LogError(err);
            }


            Global.Logger.LogStatus("UpdateIpDefinitionFiles finished...");
        }

        private static void DownloadIpDefinitionFile(string name, string url)
        {
            Global.Logger.LogStatus("UpdateIpDefinitionFiles updating " + name + "...");

            var ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpRequest.Credentials = new NetworkCredential("anonymous", "site@aspnetdating.com");

            var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            var ftpResponseStream = ftpResponse.GetResponseStream();

            var file = System.Web.Hosting.HostingEnvironment.MapPath("~/IpData/" + name + ".txt");
            if (file == null) return;
            FileStream fileStream;
            try
            {
                fileStream = File.Open(file, FileMode.Create, FileAccess.Write);
            }
            catch (Exception)
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                fileStream = File.Open(file, FileMode.Create, FileAccess.Write);
            }

            const int length = 1024;
            var buffer = new Byte[length];
            int bytesRead = ftpResponseStream.Read(buffer, 0, length);
            while (bytesRead > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
                bytesRead = ftpResponseStream.Read(buffer, 0, length);
            }

            ftpResponseStream.Close();
            fileStream.Close();
        }
    }

    public static class ApplicationPoolRecycle
    {
        /// <summary>Attempts to recycle current application pool</summary>
        /// <returns>Boolean indicating if application pool was successfully recycled</returns>
        public static bool RecycleCurrentApplicationPool()
        {
            try
            {
                // Application hosted on IIS that supports App Pools, like 6.0 and 7.0
                if (IsApplicationRunningOnAppPool())
                {
                    // Get current application pool name
                    string appPoolId = GetCurrentApplicationPoolId();

                    // Recycle current application pool
                    RecycleApplicationPool(appPoolId);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsApplicationRunningOnAppPool()
        {
            // Application is not hosted on IIS
            if (!AppDomain.CurrentDomain.FriendlyName.StartsWith("/LM/"))
                return false;

                // Application hosted on IIS that doesn't support App Pools, like 5.1
            if (!DirectoryEntry.Exists("IIS://Localhost/W3SVC/AppPools"))
                return false;

            return true;
        }

        private static string GetCurrentApplicationPoolId()
        {
            string virtualDirPath = AppDomain.CurrentDomain.FriendlyName;
            virtualDirPath = virtualDirPath.Substring(4);
            int index = virtualDirPath.Length + 1;
            index = virtualDirPath.LastIndexOf("-", index - 1, index - 1);
            index = virtualDirPath.LastIndexOf("-", index - 1, index - 1);
            virtualDirPath = "IIS://localhost/" + virtualDirPath.Remove(index);

            var virtualDirEntry = new DirectoryEntry(virtualDirPath);
            return virtualDirEntry.Properties["AppPoolId"].Value.ToString();
        }

        private static void RecycleApplicationPool(string appPoolId)
        {
            string appPoolPath = "IIS://localhost/W3SVC/AppPools/" + appPoolId;

            var appPoolEntry = new DirectoryEntry(appPoolPath);
            appPoolEntry.Invoke("Recycle");
        }
    }

    public static class Scripts
    {
        public static bool IsJQueryRegistered()
        {
            return true;
        }

        public static void RegisterJQuery(this Page page)
        {
            /*
            if (HttpContext.Current.Items["JQueryAdded"] != null) return;

#if DEBUG
            const string jqueryFile = "~/scripts/jquery.vsdoc.js";
#else
            const string jqueryFile = "~/scripts/jquery.min.js";
#endif

            if (page.Master is Site)
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference(jqueryFile));
            else
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "jQuery", page.ResolveClientUrl(jqueryFile));

            HttpContext.Current.Items["JQueryAdded"] = true;
             */ 
        }

        public static void RegisterJQueryLightbox(this Page page)
        {
            if (HttpContext.Current.Items["JQueryLightboxAdded"] != null) return;

#if DEBUG
            const string jqueryLightboxFile = "~/scripts/jquery.lightbox.js";
#else
            const string jqueryLightboxFile = "~/scripts/jquery.lightbox.min.js";
#endif

            if (page.Master is Site)
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference(jqueryLightboxFile));
            else
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "jQuery.lightbox", 
                    page.ResolveClientUrl(jqueryLightboxFile));

            HttpContext.Current.Items["JQueryLightboxAdded"] = true;
        }

        public static void RegisterJQueryEditInPlace(this Page page)
        {
            if (HttpContext.Current.Items["JQueryEditInPlace"] != null) return;

            const string jqueryEditInPlaceFile = "~/scripts/jquery.editinplace.mod.js";

            if (page.Master is Site)
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference(jqueryEditInPlaceFile));
            else
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "jQuery.editinplace",
                    page.ResolveClientUrl(jqueryEditInPlaceFile));

            HttpContext.Current.Items["JQueryEditInPlace"] = true;
        }

        public static void RegisterJQueryWatermark(this Page page)
        {
            if (HttpContext.Current.Items["JQueryWatermark"] != null) return;

            const string jqueryWatermarkFile = "~/scripts/jquery.watermarkinput.js";

            if (page.Master is Site)
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference(jqueryWatermarkFile));
            else
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "jQuery.watermark",
                    page.ResolveClientUrl(jqueryWatermarkFile));

            HttpContext.Current.Items["JQueryWatermark"] = true;
        }

        public static void RegisterJQueryCascading(this Page page)
        {
            if (HttpContext.Current.Items["JQueryCascadingAdded"] != null) return;

            const string jqueryFile = "~/scripts/jquery.cascadingDropDown.js";

            if (page.Master is Site)
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference(jqueryFile));
            else
                ScriptManager.RegisterClientScriptInclude(page, typeof (Page), "jQuery.cascadingDropDown",
                                                          page.ResolveClientUrl(jqueryFile));

            HttpContext.Current.Items["JQueryCascadingAdded"] = true;
        }

        public static void RegisterJPicker(this Page page)
        {
            if (HttpContext.Current.Items["JQueryJPickerAdded"] != null) return;

            const string jqueryFile = "~/scripts/jpicker/jpicker-1.1.6-mod.min.js";

            if (page.Master is Site)
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(
                    new ScriptReference(jqueryFile));
            else
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "jQuery.jpicker",
                                                          page.ResolveClientUrl(jqueryFile));

            HtmlLink css = new HtmlLink();
            css.Href = page.ResolveClientUrl("~/scripts/jpicker/css/jPicker-1.1.6.min.css");
            css.Attributes["rel"] = "stylesheet";
            css.Attributes["type"] = "text/css";
            css.Attributes["media"] = "all";
            page.Header.Controls.Add(css);

            HttpContext.Current.Items["JQueryJPickerAdded"] = true;
        }

        public static void RegisterCKEditor(this Page page)
        {
            if (HttpContext.Current.Items["CKEditorAdded"] != null) return;

            string jqueryAdapterFile = page.ResolveClientUrl("~/ckeditor/adapters/jquery.js");
            string ckeditorFile = page.ResolveClientUrl("~/ckeditor/ckeditor.js");

            if (page.Master is Site)
            {
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(new ScriptReference(ckeditorFile));
                ((Site)page.Master).ScriptManager.CompositeScript.Scripts.Add(new ScriptReference(jqueryAdapterFile));
                page.Header.Controls.Add(new LiteralControl("<script>var CKEDITOR_BASEPATH = 'ckeditor/';</script>"));
            }
            else
            {
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "CKEditorFile", ckeditorFile);
                ScriptManager.RegisterClientScriptInclude(page, typeof(Page), "CKEditorjQuery",
                                                          jqueryAdapterFile);
            }

            HttpContext.Current.Items["CKEditorAdded"] = true;
        }

        public static bool IsIE6(this HttpRequest request)
        {
            return request.Browser.Browser == "IE" && request.Browser.MajorVersion < 7;
        }

        public static string EscapeJqueryChars(this string str)
        {
            if (str.Contains('.'))
                str = str.Replace(".", @"\\.");

            if (str.Contains('+'))
                str = str.Replace("+", @"\\+");

            if (str.Contains(':'))
                str = str.Replace(":", @"\\:");

            return str;
        }

        public static void InitilizeHtmlEditor(Page page, PlaceHolder phEditor,
                                                ref HtmlEditor htmlEditor,
                                                ref TextBox ckeditor,
                                                string width, string height)
        {
            if (Config.Misc.UseCKEditor)
            {
                if (HttpContext.Current.Items["CKEditorForJQueryAdded"] == null)
                {
                    page.RegisterJQuery();
                    page.RegisterCKEditor();

                    HttpContext.Current.Items["CKEditorForJQueryAdded"] = true;   
                }

                ckeditor = new TextBox()
                {
                    ID = "txtCKEditor",
                    TextMode = TextBoxMode.MultiLine,
                    //Width = Unit.Parse(width),
                    Height = Unit.Parse(height),
                    CssClass = "form-control"
                };

                phEditor.Controls.Add(ckeditor);
                
                string js = null;

                if (page is AdminPageBase)
                    js = "$('#" + ckeditor.ClientID.EscapeJqueryChars() + "').ckeditor();";
                else
                    js = "$(function()"
                                + "{"
                                + "var config = {"
                                + "toolbar: [['Bold', 'Italic']]"
                                + "};"
                                + "$('#"+ ckeditor.ClientID.EscapeJqueryChars() +"').ckeditor(config);"
                                + "});";

                page.ClientScript.RegisterStartupScript(page.GetType(), "CKEditorForjQuery" + ckeditor.ClientID, js, true);
            }
            //else
            //{
            //    htmlEditor = new HtmlEditor()
            //    {
            //        ID = "htmlEditor",
            //        Width = Unit.Parse(width),
            //        Height = Unit.Parse(height),
            //        AutoFocus = true
            //    };
            //    phEditor.Controls.Add(htmlEditor);
            //}
        }
    }

    public interface IOptions
    {
        string Options { get; set; }
    }

    public static class CascadingDropDown
    {
        private static string GenerateCascadeScript(string countryClientID, string regionClientID, string cityClientID,
            string selectedRegion = "", string selectedCity = "", string regionPromptText = "", string cityPromptText = "")
        {
            return string.Format(
                "$('#{0}').CascadingDropDown('#{1}', '{7}/Services/Service.asmx/GetRegionsByCountry', " +
                " {{ postData: function() {{ return '{{ country: \"' + $('#{1}').val() + '\", selected: \"{3}\" }}' }}, promptText: '{5}' }});" +
                (cityClientID == null ? "" :
                ("$('#{2}').CascadingDropDown('#{0}', '{7}/Services/Service.asmx/GetCitiesByCountryAndRegion', " +
                " {{ postData: function() {{ return '{{ country: \"' + $('#{1}').val() + '\", region: \"' + $('#{0}').val() + '\", selected: \"{4}\" }}' }}, promptText: '{6}' }});")) +
                "$('#{1}').trigger('change');",
                regionClientID, countryClientID, cityClientID, selectedRegion, selectedCity, regionPromptText, cityPromptText, Config.Urls.Home);
        }

        public static void SetupLocationControls(Page page, HtmlSelect dropCountry, HtmlSelect dropRegion, HtmlSelect dropCity,
            bool setDefault, string selectedCountry = null, string selectedRegion = null, string selectedCity = null, string promptCountryText = "", string promptRegionText = "", string promptCityText = "")
        {
            page.RegisterJQuery();
            page.RegisterJQueryCascading();

            if (dropCountry.Items.Count == 0)
            {
                dropCountry.Items.Add(new ListItem(promptCountryText, ""));
                foreach (var country in Services.Service.GetCountries(setDefault))
                {
                    var item = new ListItem(country.Text, country.Value);

                    if (country.Selected) 
                        item.Selected = true;

                    dropCountry.Items.Add(item);

                    //if (!dropCountry.EnableViewState)
                    //{
                    //    string val = HttpContext.Current.Request[dropCountry.UniqueID];
                    //    if (val != null)
                    //        dropCountry.SelectedValue = val;
                    //}
                }

                if (selectedCountry != null)
                {
                    dropCountry.Items.DeselectAll();

                    var item = dropCountry.Items.FindByValue(selectedCountry);
                    if (item != null)
                        item.Selected = true;
                }
                else if (dropCountry.SelectedValue() != String.Empty)
                {
                    dropCountry.Items.DeselectAll();
                    var item = dropCountry.Items.FindByValue(dropCountry.SelectedValue());
                    if (item != null)
                        item.Selected = true;
                }

                if (selectedRegion == null && dropRegion.SelectedValue() != String.Empty)
                    selectedRegion = dropRegion.SelectedValue();

                if (dropCity != null)
                {
                    if (selectedCity == null && dropCity.SelectedValue() != String.Empty)
                        selectedCity = dropCity.SelectedValue();
                }
            }

            var cascadeScript = GenerateCascadeScript(dropCountry.ClientID, dropRegion.ClientID, dropCity == null ? null : dropCity.ClientID,
                selectedRegion ?? String.Empty, selectedCity ?? String.Empty, promptRegionText, promptCityText);

            //page.ClientScript.RegisterStartupScript(typeof(Page), "cascadeInit" + dropCountry.ClientID,
            //                                    cascadeScript, true);
            ScriptManager.RegisterStartupScript(page, typeof(Page), "cascadeInit" + dropCountry.ClientID,
                                                cascadeScript, true);
        }

        public static string SelectedValue(this HtmlSelect control)
        {
            return HttpContext.Current.Request[control.UniqueID] ?? String.Empty;
        }

        public static void DeselectAll(this ListItemCollection items)
        {
            var selectedItems = items.Cast<ListItem>().Where(i => i.Selected);
            foreach (var item in selectedItems)
                item.Selected = false;
        }
    }
}