using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ManagerImage.Data;
using Radzen;
using ManagerImage.Pages;
using ManagerImage;
using BlazorAuthenticationSample.Client.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorAuthenticationSample.Client.Features.Identity;
using ManagerImage.Services;
using Blazored.Toast;

namespace ManagerImage
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        partial void OnConfigureServices(IServiceCollection services);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ManagerImageConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", c => c.RequireRole("Admin"));
                options.AddPolicy("RequireReport", c => c.RequireRole("Report"));
            });

            services.AddControllers();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();




            services.AddHttpContextAccessor();
            services.AddScoped<HttpClient>(serviceProvider =>
            {

              var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();

              return new HttpClient
              {
                BaseAddress = new Uri(uriHelper.BaseUri)
              };
            });

            services.AddHttpClient();

            services.AddScoped<ManagerImageService>();
            services.AddScoped<ELImageService>();
            services.AddScoped<AWSService>();

            services.AddDbContext<ManagerImage.Data.ManagerImageContext>(options =>
            {
              options.UseSqlServer(Configuration.GetConnectionString("ManagerImageConnection"),
                sqlOptions => sqlOptions.UseRowNumberForPaging());
            });

            services.AddDbContext<ManagerImage.Data.Silfab_caContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Silfab_caConnection"),
                  sqlOptions => sqlOptions.UseRowNumberForPaging());
            });

            services.AddDbContext<ManagerImage.Data.eMaintContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("eMaint_Connection"),
                  sqlOptions => sqlOptions.UseRowNumberForPaging());
            });


            services.AddRazorPages();
            services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            });
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddBlazoredToast();
            OnConfigureServices(services);
        }

        partial void OnConfigure(IApplicationBuilder app, IWebHostEnvironment env);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use((ctx, next) =>
                {
                    return next();
                });
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            OnConfigure(app, env);



          
        }
    }

}
