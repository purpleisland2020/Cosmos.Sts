using Cosmos.Sts.Settings;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Sts.Configuration
{
    public class CosmosClientStore : IClientStore
    {
        private readonly AppSettings _appSettings;

        public CosmosClientStore(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            if(clientId == "cosmos.backoffice.web") // Todo: May be prepend the company name before ex. companyname.cosmos.backoffice.web
            {
                return Task.FromResult(this.GetCosmosBackOfficeWebClient());
            }
             // Add Other Clients as needed
            return null;
        }

        private Client GetCosmosBackOfficeWebClient()
        {
            return new Client
            {
                ClientId = "cosmos.backoffice.web", // Todo: May be prepend the company name before ex. companyname.cosmos.backoffice.web
                ClientName = "Cosmos BackOffice Web Application",
                AllowedGrantTypes = GrantTypes.Hybrid,
                ClientSecrets = { new Secret("DFB3BD1A-57DA-4D24-A70A-F272C3DE0CFD".Sha256()) },
                RedirectUris = { $"{_appSettings.BaseUrls.BackOfficeWeb}signin-oidc" },
                FrontChannelLogoutUri = $"{_appSettings.BaseUrls.BackOfficeWeb}signout-oidc",
                PostLogoutRedirectUris = new List<string> { $"{_appSettings.BaseUrls.BackOfficeWeb}signout-callback-oidc" },
                AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
               //        Todo: Add Api's
                        "cosmos.usermanagement.api",
                        "cosmos.society.api",
                        "cosmos.backoffice.api"
                  },
                AllowOfflineAccess = true,
                RequireConsent = false,
                AlwaysIncludeUserClaimsInIdToken = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                // AccessTokenLifetime = 120,       // For local expiration testing
                AbsoluteRefreshTokenLifetime = 157700000  
            };
        }
    }
}
