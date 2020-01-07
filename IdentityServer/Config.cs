﻿using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
	public class Config
	{
		public static IEnumerable<ApiResource> GetApiResources() =>
			new List<ApiResource> { new ApiResource("api1", "My API") };

		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				new Client
				{
					ClientId = "client",
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					ClientSecrets = { new Secret("secret".Sha256()) },
					AllowedScopes = { "api1" }
				}
			};
		}
	}
}
