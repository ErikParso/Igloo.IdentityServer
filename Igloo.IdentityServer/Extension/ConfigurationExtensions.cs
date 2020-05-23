using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Igloo.IdentityServer.Extension
{
	public static class ConfigurationExtensions
	{
		public static GoogleApi GetGoogleApi(this IConfiguration config)
			=> config.GetSection("GoogleApi").Get<GoogleApi>();

		public static IdentityServerConfig GetIdentityServerConfig(this IConfiguration config)
			=> config.GetSection("IdentityServerConfig").Get<IdentityServerConfig>();
	}

	public class GoogleApi
	{
		public string ClientId { get; set; }

		public string ClientSecret { get; set; }
	}

	public class IdentityServerConfig
	{
		public bool UseConfigurationStore { get; set; }

		public string ConfigurationStoreConnectionString { get; set; }

		public bool UseOperationalStore { get; set; }

		public string OperationalStoreConnectionString { get; set; }
	}
}
