using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAvanaQuestionaire.Models;

namespace MyAvanaQuestionaire
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<MyAvanaQuestionaireApiClient.ApiClient>(_ => new MyAvanaQuestionaireApiClient.ApiClient(new Uri("http://localhost:5004/api/")));
            services.AddScoped<MyAvanaQuestionaireApiClient.ApiClient>(_ => new MyAvanaQuestionaireApiClient.ApiClient(new Uri("https://apistaging.myavana.com/")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = CookieSecurePolicy.None;
            });

            services.Configure<AppSettingsModel>(Configuration.GetSection("AppSettingsModel"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.MaxValue;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.LoginPath = "/Auth/Login";
                options.Cookie.Name = "AuthCookies";
                options.Cookie.Expiration = DateTime.Now.AddDays(-1).TimeOfDay;
            })
       .AddCookie("CustomerCookies", o =>
       {
           o.ExpireTimeSpan = DateTime.Now.AddDays(-1).TimeOfDay;
           o.LoginPath = new PathString("/Auth/Login");
           o.Cookie.Name = "CustomerCookies";
           o.SlidingExpiration = true;
       });
            //services.AddAuthorization(options =>
            //     {
            //         options.AddPolicy("ShouldHavePlanPurchased", policy =>
            //             policy.Requirements.Add(
            //                   new AuthorizationPolicyRequirement(true)));

            //     }); 
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = CookieSecurePolicy.None;
            });
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Views",
                    template: "{controller=Auth}/{action=Login}/{id?}");
            });
        }
    }
}
