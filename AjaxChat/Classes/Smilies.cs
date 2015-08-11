#if !AJAXCHAT_INTEGRATION
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace AjaxChat.Classes
{
    public class Smilies
    {
        private static Hashtable htSmilies = new Hashtable();
        private static ArrayList alSmiliesKeysSorted = new ArrayList();
        private static bool smiliesLoaded = false;
        private static bool smiliesLoadingInProgress = false;

        public static Hashtable HtSmileys
        {
            get
            {
                if (!smiliesLoaded) LoadSmilies();
                return htSmilies;
            }
        }

        public static ArrayList AlSmiliesKeysSorted
        {
            get
            {
                if (!smiliesLoaded) LoadSmilies();
                return alSmiliesKeysSorted;
            }
        }

        static Smilies()
        {
        }

        private static void LoadSmilies()
        {
            if (!(HttpContext.Current.ApplicationInstance is IHttpApplicationSupportSmilies))
                return;

            string smiliesDir = ((IHttpApplicationSupportSmilies) 
                HttpContext.Current.ApplicationInstance).GetSmiliesDirectory();

            if (!File.Exists(smiliesDir + @"\smilies.pak")) return;

            try
            {
                smiliesLoadingInProgress = true;

                lock (htSmilies)
                {
                    TextReader textReader;
                    try
                    {
                        textReader = File.OpenText(smiliesDir + @"\smilies.pak");
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    htSmilies.Clear();

                    string textLine;
                    while ((textLine = textReader.ReadLine()) != null)
                    {
                        string[] splitLine = Regex.Split(textLine, Regex.Escape("=+:"));
                        Smiley smiley = new Smiley();
                        smiley.Key = splitLine[2];
                        smiley.Description = splitLine[1];
                        smiley.Image = splitLine[0];
                        htSmilies.Add(smiley.Key, smiley);
                    }
                }

                lock (alSmiliesKeysSorted)
                {
                    alSmiliesKeysSorted.AddRange(htSmilies.Keys);
                    alSmiliesKeysSorted.Sort(new LengthComparer());
                    alSmiliesKeysSorted.Reverse();
                }

                smiliesLoaded = true;
            }
            finally
            {
                smiliesLoadingInProgress = false;
            }
        }

        public static void Process(ref string text)
        {
            HttpApplication app;
            if (HttpContext.Current != null)
                app = HttpContext.Current.ApplicationInstance;
            else if (ChatEngine.ApplicationInstance != null)
                app = ChatEngine.ApplicationInstance;
            else
                return;
            if (!(app is IHttpApplicationSupportSmilies))
                return;

            if (smiliesLoadingInProgress) return;
            if (!smiliesLoaded) LoadSmilies();

            string smiliesUrl = ((IHttpApplicationSupportSmilies)
                app).GetSmiliesUrl();
            
            foreach (string key in alSmiliesKeysSorted)
            {
                Smiley smiley = (Smiley) htSmilies[key];
                text = Regex.Replace(text, Regex.Escape(key), String.Format("<img src=\"{0}\" alt=\"{1}\" />",
                                                                            smiliesUrl + "/" +
                                                                            smiley.Image,
                                                                            smiley.Description), RegexOptions.IgnoreCase);
            }
        }
    }

    public class LengthComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            if (x is string && y is string)
            {
                if (((string) x).Length == ((string) y).Length)
                {
                    return 0;
                }
                if (((string) x).Length > ((string) y).Length)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }


    public struct Smiley
    {
        public string Key;
        public string Description;
        public string Image;
    }
}
#endif