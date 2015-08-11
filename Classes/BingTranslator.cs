using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace AspNetDating.Classes
{
    public static class BingTranslator
    {
        public static DateTime? AccessTokenReceived { get; set; }
        public static AdmAccessToken AccessToken { get; set; }

        public static object accessTokenLock = new object();

        private static AdmAccessToken GetAccessToken()
        {
            lock (accessTokenLock)
            {
                if (!AccessTokenReceived.HasValue || DateTime.Now - AccessTokenReceived.Value > TimeSpan.FromMinutes(9.0))
                {
                    //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
                    //Refer obtaining AccessToken (http://msdn.microsoft.com/en-us/library/hh454950.aspx) 
                    AdmAuthentication admAuth = new AdmAuthentication(Config.ThirdPartyServices.BingClientID, Config.ThirdPartyServices.BingClientSecret);
                    try
                    {
                        AccessToken = admAuth.GetAccessToken();
                        AccessTokenReceived = DateTime.Now;
                    }
                    catch (WebException e)
                    {
                        Global.Logger.LogError(e);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        Global.Logger.LogError(ex);
                        return null;
                    }
                }

                return AccessToken;
            }
        }

        public static string GetHeaderValue()
        {
            var token = GetAccessToken();

            return token != null ? "Bearer " + token.access_token : null;
        }
    }


    [DataContract]
    public class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string cientSecret;
        private string request;

        public AdmAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.cientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
        }

        public AdmAccessToken GetAccessToken()
        {
            return HttpPost(DatamarketAccessUri, this.request);
        }

        private AdmAccessToken HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}