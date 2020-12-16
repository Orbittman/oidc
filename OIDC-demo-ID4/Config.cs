// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
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
                new IdentityResource(
                    "height",
                    "The users height",
                    new List<string>{"height"}),
                new IdentityResource(
                    "subscription",
                    "The users subscription",
                    new List<string>{"subscription"})
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope(
                    "demoapi", 
                    "OIDC demo API",
                    new List<string> {"role", "height", "subscription"} )
            };

        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource(
                    "demoapi",
                    "OIDC demo API")
                {
                    Scopes = new List<string>{"demoapi"},
                    ApiSecrets = new List<Secret>{ new Secret("secret".Sha256())}
                }
            };        

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    AccessTokenType = AccessTokenType.Reference,
                    IdentityTokenLifetime  = 60 * 5, // default
                    AuthorizationCodeLifetime = 60 * 5, // default
                    AccessTokenLifetime = 60 * 2,
                    AllowOfflineAccess = true, // Refresh tokens allowed
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = TimeSpan.FromDays(1).Seconds,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    // SlidingRefreshTokenLifetime = TimeSpan.FromDays(1).Seconds,
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
                        "demoapi",
                        "height",
                        "subscription"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
    }
}