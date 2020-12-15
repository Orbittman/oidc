// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace OIDC_demo_ID4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles", "Your role(s)", new List<string>{"role"}),
                new IdentityResource("age", "The users age", new List<string>{"age"}),
                new IdentityResource("subscription", "The users subscription", new List<string>{"subscription"})
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope(
                    "demoapi", 
                    "OIDC demo API",
                    new List<string> {"role"} )
            };


        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource(
                    "demoapi", 
                    "OIDC demo API",
                    new List<string> {"age", "subscription"})
                {
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    RequirePkce = true,
                    RequireConsent = false,
                    ClientName = "OIDC demo client",
                    ClientId="oidc-demo-client",
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris=new List<string>{ "https://localhost:44389/signin-oidc" },
                    PostLogoutRedirectUris = new List<string> { "https://localhost:44389/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "age",
                        "subscription",
                        "demoapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
    }
}