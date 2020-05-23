using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Igloo.IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Igloo.IdentityServer.Extension
{
	public static class StartupExtensions
	{
		public static void UpdateDatabases(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices
				.GetRequiredService<IServiceScopeFactory>()
				.CreateScope())
			{
				var config = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>().GetIdentityServerConfig();

				using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
					context.Database.Migrate();

				if (config.UseOperationalStore)
					using (var context = serviceScope.ServiceProvider.GetService<PersistedGrantDbContext>())
						context.Database.Migrate();

				if (config.UseConfigurationStore)
					using (var context = serviceScope.ServiceProvider.GetService<ConfigurationDbContext>())
						context.Database.Migrate();
			}
		}

		public static void InitializeConfigDb(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var config = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>().GetIdentityServerConfig();

				if (config.UseConfigurationStore)
				{
					var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

					if (!context.Clients.Any())
					{
						foreach (var client in Config.Clients)
						{
							context.Clients.Add(client.ToEntity());
						}
						context.SaveChanges();
					}

					if (!context.IdentityResources.Any())
					{
						foreach (var resource in Config.Ids)
						{
							context.IdentityResources.Add(resource.ToEntity());
						}
						context.SaveChanges();
					}

					if (!context.ApiResources.Any())
					{
						foreach (var resource in Config.Apis)
						{
							context.ApiResources.Add(resource.ToEntity());
						}
						context.SaveChanges();
					}
				}
			}
		}
	}
}
