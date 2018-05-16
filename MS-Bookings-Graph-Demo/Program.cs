using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace MS_Bookings_Graph_Demo
{
    class Program
    {
        #region getters&strings

        // The Azure AD instance where you domain is hosted
        public static string AADInstance {
            get { return "https://login.microsoftonline.com"; }
        }

        // The Office 365 domain (e.g. contoso.microsoft.com)
        public static string Domain {
            get { return "todo"; }
        }

        // The authority for authentication; combining the AADInstance
        // and the domain.
        public static string Authority {
            get { return string.Format("{0}/{1}/", AADInstance, Domain); }
        }

        // The client Id of your native Azure AD application
        public static string ClientId {
            get { return "todo"; }
        }

        // The redirect URI specified in the Azure AD application todo
        public static Uri RedirectUri {
            get { return new Uri("todo"); }
        }

        // The resource identifier for the Microsoft Graph
        public static string GraphResource {
            get { return "https://graph.microsoft.com/"; }
        }

        // The Microsoft Graph version, can be "v1.0" or "beta"
        public static string GraphVersion {
            get { return "beta"; }
        }

        /// <summary>
        /// The default AAD instance to use when authenticating.
        /// </summary>
        private const string DefaultAadInstance = "https://login.microsoftonline.com/common/";

        #endregion


        static void Main(string[] args)
        {
            try
            {
                // Get an access token and configure the HttpClient
                var accessToken = GetAccessToken();
                var httpClient = GetHttpClient(accessToken);
                var url = "https://graph.microsoft.com/beta/bookingBusinesses";
                var result = GetResult(httpClient, url).Result;
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }

        //gets the result of the query
        static async Task<string> GetResult(HttpClient client, string url)
        {
            using (var r = await client.GetAsync(new Uri(url)))
            {
                string result = await r.Content.ReadAsStringAsync();
                return result;
            }
        }

        // Get an access token for the Microsoft Graph using ADAL
        public static string GetAccessToken()
        {
            // Create the authentication context (ADAL)
            var authenticationContext = new AuthenticationContext(Authority);

            // Get the access token
            var authenticationResult = authenticationContext.AcquireToken(GraphResource,
                ClientId, RedirectUri, PromptBehavior.RefreshSession);
            var accessToken = authenticationResult.AccessToken;
            return accessToken;
        }

        // Prepare an HttpClient with the an authorization header (access token)
        public static HttpClient GetHttpClient(string accessToken)
        {
            // Create the HTTP client with the access token
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer",
                accessToken);
            return httpClient;
        }
    }
}
