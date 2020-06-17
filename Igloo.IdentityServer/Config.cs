// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Igloo.IdentityServer
{
	public static class Config
	{
		public static IEnumerable<IdentityResource> Ids =>
			new IdentityResource[]
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new IdentityResources.Email(),
				new IdentityResources.Phone(),
				new IdentityResources.Address(),
			};

		public static IEnumerable<ApiResource> Apis =>
			new ApiResource[]
			{
				new ApiResource("api1", "My API #1"),
				new ApiResource("iglooSmartHomeApi", "Igloo Smart Home", new List<string> {
					IdentityServerConstants.StandardScopes.Email
				})
			};

		public static IEnumerable<Client> Clients =>
			new List<Client>
			{
				new Client
				{
					ClientId = "client",
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedScopes = { "api1" }
				},
				new Client
				{
					ClientId = "mvc",
					ClientSecrets = { new Secret("secret".Sha256()) },
					AllowedGrantTypes = GrantTypes.Code,
					RequireConsent = false,
					RequirePkce = true,
					RedirectUris = { "http://localhost:5002/signin-oidc" },
					PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Phone,
						IdentityServerConstants.StandardScopes.Address,
						"api1",
					},
					AllowOfflineAccess = true
				},
				new Client
				{
					ClientId = "js",
					ClientName = "JavaScript Client",
					AllowedGrantTypes = GrantTypes.Code,
					RequirePkce = true,
					RequireClientSecret = false,
					RedirectUris = { "http://localhost:5003/callback.html" },
					PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
					AllowedCorsOrigins = { "http://localhost:5003" },

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"api1"
					}
				},
				new Client
				{
					ClientId = "iglooSmartHomeClient",
					ClientName = "Igloo SmartHome Client",
					AllowedGrantTypes = GrantTypes.Code,
					RequirePkce = true,
					RequireClientSecret = false,
					RedirectUris = { "http://localhost:4200/smarthome" },
					PostLogoutRedirectUris = { "http://localhost:4200/smarthome" },
					AllowedCorsOrigins = { "http://localhost:4200" },

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.Email,
						"iglooSmartHomeApi"
					}
				}
			};
	}
}