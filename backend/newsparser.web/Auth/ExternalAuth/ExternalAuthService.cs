using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using newsparser.DAL.Models;
using NewsParser.Web.Identity;

namespace NewsParser.Web.Auth.ExternalAuth
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly IConfiguration _config;

        public ExternalAuthService(IConfiguration config)
        {
            _config = config;
        }

        public Task<ExternalUser> VerifyAccessTokenAsync(string token, ExternalAuthProvider provider)
        {
            switch (provider)
            {
                case ExternalAuthProvider.Facebook:
                    return VerifyFacebookTokenAsync(token);

                case ExternalAuthProvider.Google:
                    return VerifyGoogleTokenAsync(token);

                default:
                    throw new AuthException("External auth provider is not supported");
            }
        }

        private async Task<ExternalUser> VerifyFacebookTokenAsync(string token)
        {
            ExternalUser externalUser = new ExternalUser();
            HttpClient client = new HttpClient();

            string verifyTokenEndPoint =
                $"https://graph.facebook.com/me?access_token={token}&fields=email,first_name,last_name";
            
            Uri uri = new Uri(verifyTokenEndPoint);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode && IsFacebookAppVerified(token).Result)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic user = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                externalUser.ExternalId = user["id"];
                externalUser.AuthProvider = ExternalAuthProvider.Facebook;
                externalUser.Email = user["email"];
                externalUser.FirstName = user["first_name"];
                externalUser.LastName = user["last_name"];
                externalUser.IsVerified = true;
            }

            return externalUser;
        }

        private async Task<bool> IsFacebookAppVerified(string token)
        {
            string verifyAppEndpoint = $"https://graph.facebook.com/app?access_token={token}";
            HttpClient client = new HttpClient();

            var uri = new Uri(verifyAppEndpoint);
            var response = await client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            dynamic appObj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            return appObj["id"] == _config["Authentication:Facebook:AppId"];
        }

        private async Task<ExternalUser> VerifyGoogleTokenAsync(string token)
        {
            ExternalUser externalUser = new ExternalUser();
            HttpClient client = new HttpClient();

            string verifyTokenEndPoint =
                $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}";

            Uri uri = new Uri(verifyTokenEndPoint);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                dynamic userObj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                if (userObj["aud"] == _config["Authentication:Google:ClientID"])
                {
                    externalUser.ExternalId = userObj["sub"];
                    externalUser.AuthProvider = ExternalAuthProvider.Google;
                    externalUser.Email = userObj["email"];
                    externalUser.FirstName = userObj["given_name"];
                    externalUser.LastName = userObj["family_name"];
                    externalUser.IsVerified = true;
                }
            }
            return externalUser;
        }
    }
}
