using System;
using System.Net.Http;
using System.Threading.Tasks;
using newsparser.DAL.Models;
using NewsParser.BL.Services.Users;
using NewsParser.DAL.Models;
using NewsParser.Identity.Models;

namespace NewsParser.Auth.ExternalAuth
{
    public class ExternalAuthService: IExternalAuthService
    {
        private readonly IUserBusinessService _userBusinessService;

        public ExternalAuthService(IUserBusinessService userBusinessService)
        {
            _userBusinessService = userBusinessService;
        }

        public async Task<ExternalUser> VerifyFacebookTokenAsync(string token)
        {
            ExternalUser externalUser = new ExternalUser();
            HttpClient client = new HttpClient();

            string verifyTokenEndPoint =
                $"https://graph.facebook.com/me?access_token={token}&fields=email,first_name,last_name";
            string verifyAppEndpoint = $"https://graph.facebook.com/app?access_token={token}";

            Uri uri = new Uri(verifyTokenEndPoint);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                dynamic userObj = (Newtonsoft.Json.Linq.JObject) Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                uri = new Uri(verifyAppEndpoint);
                response = await client.GetAsync(uri);
                content = await response.Content.ReadAsStringAsync();
                dynamic appObj = (Newtonsoft.Json.Linq.JObject) Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                if (appObj["id"] == "514371415617815")
                {
                    externalUser.ExternalId = userObj["id"];
                    externalUser.Email = userObj["email"];
                    externalUser.FirstName = userObj["first_name"];
                    externalUser.LastName = userObj["last_name"];
                    externalUser.IsVerified = true;
                }

                return externalUser;
            }
            return externalUser;
        }
    }
}
