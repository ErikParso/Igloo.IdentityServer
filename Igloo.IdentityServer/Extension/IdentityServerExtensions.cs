using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Igloo.IdentityServer.Extension
{
	public static class IdentityServerExtensions
	{
		private static string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

		public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IdentityServerConfig config) =>
			services
				.AddIdentityServer(options =>
				{
					options.Events.RaiseErrorEvents = true;
					options.Events.RaiseInformationEvents = true;
					options.Events.RaiseFailureEvents = true;
					options.Events.RaiseSuccessEvents = true;
				})
				.AddConfiguration(config)
				.AddPersistedGrants(config);

		private static IIdentityServerBuilder AddConfiguration(this IIdentityServerBuilder builder, IdentityServerConfig config) =>
			config.UseConfigurationStore
				? builder
					.AddConfigurationStore(options =>
					{
						options.ConfigureDbContext = b => b.UseSqlServer(config.ConfigurationStoreConnectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));
					})
				: builder
					.AddInMemoryClients(Config.Clients)
					.AddInMemoryApiResources(Config.Apis)
					.AddInMemoryIdentityResources(Config.Ids);

		private static IIdentityServerBuilder AddPersistedGrants(this IIdentityServerBuilder builder, IdentityServerConfig config) =>
			config.UseOperationalStore
				? builder
					.AddOperationalStore(options =>
					{
						options.ConfigureDbContext = b => b.UseSqlServer(config.OperationalStoreConnectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));
					})
				: builder
					.AddInMemoryPersistedGrants();
	}
}
