// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IndentityServerEcossistema
{
    public static class Config
    {
        //### Configuracao para funcionar requisicoes vindas de APIs
        public static IEnumerable<ApiResource> ApiResources =>
         new List<ApiResource>
         {
                new ApiResource("doughnutapi")
                {
                    Scopes = { "doughnutapi", "email" }
                }
         };


        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "User role(s)", new List<string> { "role" }),
            };


        /// <summary>
        /// Escopos suportados: Utilizei para que Cliente ReactWeb funcione.
        /// É necessario que os escopos que os cliente configurem estejam nesta lista
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),

                //###
                new ApiScope("doughnutapi", "Doughnut API"),


                new ApiScope("console-cliente"),
                
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2" }
                },

                
                // React client
                new Client
                {
                    ClientId = "wewantdoughnuts",
                    ClientName = "We Want Doughnuts",
                    ClientUri = "http://localhost:3000",

                    AllowedGrantTypes = GrantTypes.Code,

                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:3000/signin-oidc",
                    },

                    PostLogoutRedirectUris = { "http://localhost:3000/signout-oidc" },
                    AllowedCorsOrigins = { "http://localhost:3000" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "doughnutapi",
                        "roles"
                    },

                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                },

                 // Console application cliente
                new Client
                {
                    ClientId = "console-cliente",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("console-cliente".Sha256()) },

                    AllowedScopes = { "doughnutapi" }
                    
                    //todo: configurar escopo para o console application
                    //AllowedScopes = { "console-cliente" }
                },

            };
    }
}