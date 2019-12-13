using System.Collections.Generic;
using IdentityServer4.Test;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IdentityServer4;
using System.Security.Claims;

namespace Karenia.Visby.Account.Services
{
    public class Config
    {
        public static IConfiguration _configuration { get; set; }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("client".Sha256())
                    },
                    AllowedScopes = new[] {IdentityServer4.IdentityServerConstants.LocalApi.ScopeName},
                    // AllowedCorsOrigins=new[]{"*"}
                    AccessTokenLifetime = 3600 * 24,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { };
        }

        public static string DBConnect()
        {
            return "";
        }
    }
}