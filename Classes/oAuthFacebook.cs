using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace AspNetDating.Classes
{
    public class oAuthFacebook
    {
        #region Method enum

        public enum Method
        {
            GET,
            POST
        } ;

        #endregion

        public const string AUTHORIZE = "https://graph.facebook.com/oauth/authorize";
        public const string ACCESS_TOKEN = "https://graph.facebook.com/oauth/access_token";
        private string _callBackUrl = "oob";

        private string _consumerKey = "";
        private string _consumerSecret = "";
        private string _oauthVerifier = "";
        private string _token = "";
        private string _tokenSecret = "";

        #region Properties

        public string ConsumerKey
        {
            get
            {
                if (_consumerKey.Length == 0)
                {
                    _consumerKey = Properties.Settings.Default.Facebook_API_Key;
                }
                return _consumerKey;
            }
            set { _consumerKey = value; }
        }

        public string ConsumerSecret
        {
            get
            {
                if (_consumerSecret.Length == 0)
                {
                    _consumerSecret = Properties.Settings.Default.Facebook_Secret_Key;
                }
                return _consumerSecret;
            }
            set { _consumerSecret = value; }
        }

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        public string TokenSecret
        {
            get { return _tokenSecret; }
            set { _tokenSecret = value; }
        }

        public string CallBackUrl
        {
            get { return _callBackUrl; }
            set { _callBackUrl = value; }
        }

        public string OAuthVerifier
        {
            get { return _oauthVerifier; }
            set { _oauthVerifier = value; }
        }

        public string Scope { get; set; }

        #endregion

        /// <summary>
        /// Get the link to Facebook's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLinkGet()
        {
            var linkGet = string.Format("{0}?client_id={1}&redirect_uri={2}", AUTHORIZE, ConsumerKey, HttpUtility.UrlEncode(CallBackUrl));
            if (!string.IsNullOrEmpty(Scope))
                linkGet += "&scope=" + Scope;
            return linkGet;
        }

        /// <summary>
        /// Exchange the Facebook "code" for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token or "code" is supplied by Facebook's authorization page following the callback.</param>
        public void AccessTokenGet(string authToken)
        {
            Token = authToken;
            string accessTokenUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&client_secret={3}&code={4}",
                                                  ACCESS_TOKEN, ConsumerKey, HttpUtility.UrlEncode(CallBackUrl),
                                                  ConsumerSecret, authToken);
            if (!string.IsNullOrEmpty(Scope))
                accessTokenUrl += "&scope=" + Scope;

            string response = WebRequest(Method.GET, accessTokenUrl, String.Empty);

            if (response.Length > 0)
            {
                //Store the returned access_token
                NameValueCollection qs = HttpUtility.ParseQueryString(response);

                if (qs["access_token"] != null)
                {
                    Token = qs["access_token"];
                }
            }
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            //webRequest.UserAgent = "[You user agent]";
            webRequest.Timeout = 20000;

            if (method == Method.POST)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());

                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }

                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }
}