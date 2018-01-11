﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Test;

namespace TekConf.Identity
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", new[] {"role"}),
                new IdentityResource("experience", new[] {"yearsofexperience"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            var api = new ApiResource("tekconfApi", "TekConf API");
            api.Scopes.Add(new Scope("tekconfApiPostAttendee"));

            return new List<ApiResource>
            {
                api
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(string webUrl)
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ExternalApiClient",

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"tekconfApi", "tekconfApiPostAttendee"},
                    AllowedGrantTypes = GrantTypes.ClientCredentials
                },

                new Client
                {
                    ClientId = "tekconfweb",
                    ClientName = "TekConf MVC Client",

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = {$"{webUrl}/signin-oidc"},
                    PostLogoutRedirectUris = {webUrl},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "tekconfApi",
                        "roles",
                        "experience"
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                new Client
                {
                    ClientId = "com.arteksoftware.tekconf",
                    ClientName = "TekConf Mobile App",

                    RedirectUris = {"tekconfmobile://callback"},
                    PostLogoutRedirectUris = {"tekconfmobile://callback"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "tekconfApi",
                        "roles",
                        "experience"
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d10841fe-d702-434b-9050-745eea366b87",
                    Username = "Roland",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Roland"),
                        new Claim("website", "http://rolandguijt.com"),
                        new Claim("role", "speaker"),
                        new Claim("yearsofexperience", 5.ToString())
                    }
                }
            };
        }
    }
}