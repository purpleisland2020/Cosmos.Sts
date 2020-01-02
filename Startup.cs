using Cosmos.Sts.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Cosmos.Sts.Models;
using Cosmos.Sts.Settings;
using Microsoft.AspNetCore.Identity;
using Cosmos.Sts.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.HttpOverrides;
using IdentityServer4.Quickstart.UI;

namespace Cosmos.Sts
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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<CosmosContext>(
                options =>
                {
                    options.UseSqlServer(
                        Configuration.GetConnectionString("CosmosContextConnection"));
                });
            services.AddIdentity<CosmosUser, CosmosRole>()
                    .AddEntityFrameworkStores<CosmosContext>()
                    .AddDefaultTokenProviders();
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            var defaultSigning = Configuration["SigningCertificate:Default"];
            if (defaultSigning == null || defaultSigning == "true")
            {
                services.AddIdentityServer(options =>
                {
                    options.PublicOrigin = Configuration["BaseUrls:Sts"]; //Info: For  load balancing scenarios
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                        .AddDeveloperSigningCredential() // Info: During Dev --> use dev cert
                        .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                        .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                        .AddAspNetIdentity<CosmosUser>()
                        .AddProfileService<CosmosProfileService>()
                        .AddClientStore<CosmosClientStore>();
            }
            else
            {
                services.AddIdentityServer(options => options.PublicOrigin = Configuration["BaseUrls:Sts"])  // For  load balancing scenarios
                    .AddSigningCredential(IdentityServerConfig.GetSigningCertificate(Configuration["SigningCertificate:Name"], Configuration["SigningCertificate:Password"]))
                    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                    .AddAspNetIdentity<CosmosUser>()
                    .AddProfileService<CosmosProfileService>()
                    .AddClientStore<CosmosClientStore>();
            }
            services.AddScoped<IProfileService, CosmosProfileService>();
            services.AddSingleton<IClientStore, CosmosClientStore>();

            services.AddCors();
            //Info: The below are load balancing scenarios
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.RequireHeaderSymmetry = false;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Info: Check identity server documentation - as there are changes for .net core 3.1
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            AccountOptions.ShowLogoutPrompt = false;
            AccountOptions.AutomaticRedirectAfterSignOut = true;
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            // The below does not exist in .net core 3.0
            // https://stackoverflow.com/questions/58266344/net-core-3-mvc-using-usemvcwithdefaultroute-to-configure-mvc-is-not-suppo
            //app.UseMvcWithDefaultRoute();

        }
    }
}
