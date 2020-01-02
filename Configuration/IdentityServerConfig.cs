using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Cosmos.Sts.Configuration
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[]
                {
                  new ApiResource
                   {
                       Name="cosmos.usermanagement.api",
                       DisplayName="User Management Api",
                       Scopes = new List<Scope>()
                       {
                           new Scope("cosmos.usermanagement.api")
                       }
                   },
                   new ApiResource
                   {
                       Name="cosmos.society.api",
                       DisplayName="Society Api",
                       Scopes = new List<Scope>()
                       {
                           new Scope("cosmos.society.api")
                       }
                   },
                   new ApiResource
                   {
                       Name="cosmos.backoffice.api",
                       DisplayName="BackOffice Api",
                       Scopes = new List<Scope>()
                       {
                           new Scope("cosmos.backoffice.api")
                       }
                   },
                };
        }

        public static X509Certificate2 GetSigningCertificate(string certName, string certPassword)
        {
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), "Certificates", certName);
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Signing Certificate is missing!");
            }
            var cert = new X509Certificate2(fileName, certPassword);
            return cert;
        }
    }
}
