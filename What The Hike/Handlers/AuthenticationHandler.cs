using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace What_The_Hike.Handlers
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
        : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string ApplicationName = "What The Hike";
            string ClientID = "617820017144-uub3n4vva1i6ecohfv9c5mcbq0iqm5ur.apps.googleusercontent.com";
            string ClientSecret = "nFbpxkhroDJZDO0Dc_lCC67Q";

            string[] Scopes =
            {
                "email"
            };

            UserCredential credentials = null;

            string error = string.Empty;
            try
            {
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = ClientID,
                        ClientSecret = ClientSecret
                    },
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore("Google Oath2")).Result;
            }
            catch (Exception e)
            {
                credentials = null;
                error = "Failed to get UserCredentials Initialization: " + e.ToString();
            }

            //string refreshToken = string.Empty;
            //if (credentials != null && string.IsNullOrWhiteSpace(error))
            //{
            //    refreshToken = credentials.Token.RefreshToken;
            //}

            var plusService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName,
            });

            var email = plusService.Users.GetProfile("me").Execute().EmailAddress;
            
            // check if the email from login is the same as one of our saved users's email

            var claims = new[] { new Claim(ClaimTypes.Name, email) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}