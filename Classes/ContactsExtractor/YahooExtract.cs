using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AspNetDating.Classes.ContactsExtractor;

namespace AspNetDating.Classes.ContactsExtractor
{
    public class YahooExtract : IMailContactExtract
    {
        private const string _addressBookUrl =
            "http://address.yahoo.com/yab/us/Yahoo_ab.csv?loc=us&.rand=1671497644&A=H&Yahoo_ab.csv";

        private const string _authUrl = "https://login.yahoo.com/config/login?";
        private const string _loginPage = "https://login.yahoo.com/config/login";

        private const string _userAgent =
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.3) Gecko/20070309 Firefox/2.0.0.3";

        #region IMailContactExtract Members

        public bool Extract(NetworkCredential credential, out MailContactList list)
        {
            bool result = false;

            list = new MailContactList();

            try
            {
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.UserAgent] = _userAgent;
                webClient.Encoding = Encoding.UTF8;

                byte[] firstResponse = webClient.DownloadData(_loginPage);
                string firstRes = Encoding.UTF8.GetString(firstResponse);


                var postToLogin = new NameValueCollection();
                var regex = new Regex("type=\"hidden\" name=\"(.*?)\" value=\"(.*?)\"", RegexOptions.IgnoreCase);
                Match match = regex.Match(firstRes);
                while (match.Success)
                {
                    if (match.Groups[0].Value.Length > 0)
                    {
                        postToLogin.Add(match.Groups[1].Value, match.Groups[2].Value);
                    }
                    match = regex.Match(firstRes, match.Index + match.Length);
                }


                postToLogin.Add(".save", "Sign In");
                postToLogin.Add(".persistent", "y");

                string login = credential.UserName.Split('@')[0];
                postToLogin.Add("login", login);
                postToLogin.Add("passwd", credential.Password);

                webClient.Headers[HttpRequestHeader.UserAgent] = _userAgent;
                webClient.Headers[HttpRequestHeader.Referer] = _loginPage;
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers[HttpRequestHeader.Cookie] = webClient.ResponseHeaders[HttpResponseHeader.SetCookie];

                webClient.UploadValues(_authUrl, postToLogin);
                string cookie = webClient.ResponseHeaders[HttpResponseHeader.SetCookie];

                if (string.IsNullOrEmpty(cookie))
                {
                    return false;
                }

                string newCookie = string.Empty;
                string[] tmp1 = cookie.Split(',');
                foreach (string var in tmp1)
                {
                    string[] tmp2 = var.Split(';');
                    newCookie = String.IsNullOrEmpty(newCookie) ? tmp2[0] : newCookie + ";" + tmp2[0];
                }

                // set login cookie
                webClient.Headers[HttpRequestHeader.Cookie] = newCookie;
                byte[] thirdResponse = webClient.DownloadData(_addressBookUrl);
                string thirdRes = Encoding.UTF8.GetString(thirdResponse);

                string crumb = string.Empty;
                var regexCrumb = new Regex("type=\"hidden\" name=\"\\.crumb\" id=\"crumb1\" value=\"(.*?)\"",
                                           RegexOptions.IgnoreCase);
                match = regexCrumb.Match(thirdRes);
                if (match.Success && match.Groups[0].Value.Length > 0)
                {
                    crumb = match.Groups[1].Value;
                }


                var postDataAB = new NameValueCollection();
                postDataAB.Add(".crumb", crumb);
                postDataAB.Add("vcp", "import_export");
                postDataAB.Add("submit[action_export_yahoo]", "Export Now");

                webClient.Headers[HttpRequestHeader.UserAgent] = _userAgent;
                webClient.Headers[HttpRequestHeader.Referer] = _addressBookUrl;

                byte[] FourResponse = webClient.UploadValues(_addressBookUrl, postDataAB);
                string csvData = Encoding.UTF8.GetString(FourResponse);

                string[] lines = csvData.Split('\n');
                foreach (string line in lines)
                {
                    string[] items = line.Split(',');
                    if (items.Length < 5)
                    {
                        continue;
                    }
                    string email = items[4];
                    string name = items[3];
                    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
                    {
                        email = email.Trim('\"');
                        name = name.Trim('\"');
                        if (!email.Equals("Email") && !name.Equals("Nickname"))
                        {
                            var mailContact = new MailContact();
                            mailContact.Name = name;
                            mailContact.Email = email;
                            list.Add(mailContact);
                        }
                    }
                }

                result = true;
            }
            catch
            {
            }
            return result;
        }

        #endregion
    }
}