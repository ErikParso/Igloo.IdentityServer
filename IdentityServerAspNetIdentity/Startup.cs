using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Extension;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityServerAspNetIdentity
{
	public class Startup
	{
		public IWebHostEnvironment Environment { get; }
		public IConfiguration Configuration { get; }

		public Startup(IWebHostEnvironment environment, IConfiguration configuration)
		{
			Environment = environment;
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			// configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
			services.Configure<IISOptions>(iis =>
			{
				iis.AuthenticationDisplayName = "Windows";
				iis.AutomaticAuthentication = false;
			});

			// configures IIS in-proc settings
			services.Configure<IISServerOptions>(iis =>
			{
				iis.AuthenticationDisplayName = "Windows";
				iis.AutomaticAuthentication = false;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			var builder = services
				.AddIdentityServer(options =>
				{
					options.Events.RaiseErrorEvents = true;
					options.Events.RaiseInformationEvents = true;
					options.Events.RaiseFailureEvents = true;
					options.Events.RaiseSuccessEvents = true;
				})
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
						sql => sql.MigrationsAssembly(migrationsAssembly));
				})
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
						sql => sql.MigrationsAssembly(migrationsAssembly));
				})
				.AddAspNetIdentity<ApplicationUser>();

			// not recommended for production - you need to store your key material somewhere secure
			builder.AddDeveloperSigningCredential();

			services.AddAuthentication()
				.AddGoogle(options =>
				{
					options.ClientId = "875390894801-5r6p4gvas518q408vl2f4bklvskdcmh2.apps.googleusercontent.com";
					options.ClientSecret = "jOYGOOOTyvg7kHaNJeaz6hdH";
				});
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UpdateDatabase();
			app.InitializeConfigDb();

			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}

			app.UseStaticFiles();
			app.UseRouting();
			app.UseIdentityServer();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}