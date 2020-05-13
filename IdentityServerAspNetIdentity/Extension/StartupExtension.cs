using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerAspNetIdentity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace IdentityServerAspNetIdentity.Extension
{
	public static class StartupExtension
	{
		/// <summary>
		/// Uses migrations to update application, configuration and oprational database.
		/// </summary>
		/// <param name="app"></param>
		public static void UpdateDatabase(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices
				.GetRequiredService<IServiceScopeFactory>()
				.CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
				{
					context.Database.Migrate();
				}

				using (var context = serviceScope.ServiceProvider.GetService<PersistedGrantDbContext>())
				{
					context.Database.Migrate();
				}

				using (var context = serviceScope.ServiceProvider.GetService<ConfigurationDbContext>())
				{
					context.Database.Migrate();
				}
			}
		}

		/// <summary>
		/// Uses <see cref="Config"/> as seed for configurational database.
		/// </summary>
		/// <param name="app"></param>
		public static void InitializeConfigDb(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

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
