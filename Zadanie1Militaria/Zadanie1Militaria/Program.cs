using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Zadanie1Militaria.Services;

namespace Zadanie1Militaria
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //AUTHORIZATIONS: bearer-token-for-user
            //Przygotowałem 2metody weryfikacji - AuthorizationCodeflow i DeviceFlow

            //DeviceFlow
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string connectionString = "YOUR_CONNECTION_STRING";
            await DeviceFlow(clientId, clientSecret, connectionString);

            //AuthorizationCodeflow
            //string redirectUri = "YOUR_REDIRECT_URI";
            //await AuthorizationCodeflow(clientId, clientSecret, redirectUri, connectionString);
        }

        public static async Task DeviceFlow(string clientId, string clientSecret, string connectionString)
        {
            try
            {
                string deviceCode = await GetAccessDeviceCode(clientId, clientSecret);

                if (deviceCode == null)
                {
                    Console.WriteLine("Failed to obtain device code.");
                    return;
                }

                var tokenResponse = await PollDeviceAuthorizationStatus(clientId, clientSecret, deviceCode);

                if (tokenResponse != null)
                {
                    Console.WriteLine("Access Token: " + tokenResponse["access_token"]);
                    Console.WriteLine("Refresh Token: " + tokenResponse["refresh_token"]);
                    await GetBillingEntries(tokenResponse["access_token"]?.ToString(), connectionString);
                }
                else
                {
                    Console.WriteLine("Failed to obtain access token.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task<string> GetAccessDeviceCode(string clientId, string clientSecret)
        {
            using (HttpClient client = new HttpClient())
            {
                var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                try
                {
                    HttpResponseMessage response = await client.PostAsync($"https://allegro.pl/auth/oauth/device?client_id={clientId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var responseJson = JObject.Parse(responseString);
                        System.Diagnostics.Process.Start("cmd", $"/C start {responseJson["verification_uri_complete"]}");
                        return responseJson["device_code"]?.ToString();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
            }
        }

        private static async Task<JObject> PollDeviceAuthorizationStatus(string clientId, string clientSecret, string deviceCode)
        {
            using HttpClient client = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var url = $"https://allegro.pl/auth/oauth/token?grant_type=urn:ietf:params:oauth:grant-type:device_code&device_code={deviceCode}";

            while (true)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, null);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return JObject.Parse(responseString);
                    }

                    var errorResponse = JObject.Parse(responseString);
                    var error = errorResponse["error"]?.ToString();

                    switch (error)
                    {
                        case "authorization_pending":
                            await Task.Delay(5000);
                            break;
                        case "slow_down":
                            await Task.Delay(10000);
                            break;
                        case "access_denied":
                            Console.WriteLine("User denied access.");
                            return null;
                        case "invalid_device_code":
                            Console.WriteLine("Invalid or expired device code.");
                            return null;
                        default:
                            Console.WriteLine($"Error: {error}");
                            return null;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return null;
                }
            }
        }

        private static async Task GetBillingEntries(string accessToken, string connectionString)
        {
            try
            {
                var allegroClient = new AllegroClient(accessToken);
                var databaseService = new DatabaseService(connectionString);

                var orders = await databaseService.GetOrdersAsync();

                foreach (var order in orders)
                {
                    var billingEntries = await allegroClient.GetBillingEntriesAsync(order.OrderId);
                    await databaseService.SaveBillingEntriesAsync(billingEntries);
                }

                Console.WriteLine("Billing entries have been successfully saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task AuthorizationCodeflow(string clientId, string clientSecret, string redirectUri, string connectionString)
        {
            RequestAuthorization(clientId, redirectUri);

            // Po przekierowaniu użytkownika i uzyskaniu kodu autoryzacyjnego
            // Wprowadź kod autoryzacyjny uzyskany z przekierowania
            Console.WriteLine("Enter the authorization code:");
            string authorizationCode = Console.ReadLine();

            // Wprowadź code_verifier używany podczas pierwszego kroku
            Console.WriteLine("Enter the code verifier:");
            string codeVerifier = Console.ReadLine();

            await RequestAccessToken(clientId, clientSecret, authorizationCode, redirectUri, codeVerifier);
        }

        public static void RequestAuthorization(string clientId, string redirectUri)
        {
            string codeVerifier = PKCEHelper.GenerateCodeVerifier();
            string codeChallenge = PKCEHelper.GenerateCodeChallenge(codeVerifier);

            string authorizationEndpoint = "https://allegro.pl/auth/oauth/authorize";
            string responseType = "code";
            string codeChallengeMethod = "S256";

            string authorizationUrl = $"{authorizationEndpoint}?response_type={responseType}&client_id={clientId}&redirect_uri={redirectUri}&code_challenge={codeChallenge}&code_challenge_method={codeChallengeMethod}";

            Console.WriteLine("Open the following URL in your browser to authorize the application:");
            Console.WriteLine(authorizationUrl);

            Console.WriteLine($"Code Verifier: {codeVerifier}");
        }
        public static async Task RequestAccessToken(string clientId, string clientSecret, string code, string redirectUri, string codeVerifier)
        {
            using (HttpClient client = new HttpClient())
            {
                var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var tokenRequestUri = "https://allegro.pl/auth/oauth/token";
                var content = new StringContent($"grant_type=authorization_code&code={code}&redirect_uri={redirectUri}&code_verifier={codeVerifier}", Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpResponseMessage response = await client.PostAsync(tokenRequestUri, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JObject jsonResponse = JObject.Parse(responseString);
                    string accessToken = jsonResponse["access_token"].ToString();
                    string refreshToken = jsonResponse["refresh_token"].ToString();
                    Console.WriteLine($"Access Token: {accessToken}");
                    Console.WriteLine($"Refresh Token: {refreshToken}");
                }
                else
                {
                    Console.WriteLine("Error in token request:");
                    Console.WriteLine(responseString);
                }
            }
        }
    }
}