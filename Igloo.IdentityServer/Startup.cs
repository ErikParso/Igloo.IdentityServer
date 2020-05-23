using Igloo.IdentityServer.Data;
using Igloo.IdentityServer.Extension;
using Igloo.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Igloo.IdentityServer
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

			services
				.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services
				.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services
				.AddIdentityServer(Configuration.GetIdentityServerConfig())
				.AddAspNetIdentity<ApplicationUser>()
				.AddDeveloperSigningCredential();

			services
				.AddAuthentication()
				.AddGoogle(options =>
				{
					options.ClientId = Configuration.GetGoogleApi().ClientId;
					options.ClientSecret = Configuration.GetGoogleApi().ClientSecret;
				});
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UpdateDatabases();
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