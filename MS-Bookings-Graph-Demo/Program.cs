using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace MS_Bookings_Graph_Demo
{
    class Program
    {
        private const string ClientApplicationAppId = "todo";
        private const string ResourceId = "todo";
        //private readonly Uri _clientApplicationRedirectUri = new Uri("todo");
        private static readonly UserCredential Credentials = new UserCredential("username","password");

        /// <summary>
        /// The default AAD instance to use when authenticating.
        /// </summary>
        private const string DefaultAadInstance = "https://login.microsoftonline.com/common/";

        static void Main(string[] args)
        {
            try
            {
                // ADAL: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-authentication-libraries
                var authenticationContext = new AuthenticationContext(DefaultAadInstance, TokenCache.DefaultShared);
                var authenticationResult =
                    authenticationContext.AcquireTokenAsync(ResourceId, ClientApplicationAppId, Credentials);

                Console.WriteLine("Token: " + authenticationResult.Result.AccessToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
