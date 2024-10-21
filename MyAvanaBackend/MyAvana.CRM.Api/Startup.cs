using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.CRM.Api.Contract;
using MyAvana.CRM.Api.Services;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvanaApi.Models.ViewModels;
using NLog.Extensions.Logging;
using MyAvana.Framework.TokenService;
using MyAvanaApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using System.IO;
using MyAvanaApi.Contract;
using MyAvanaApi.Services;
using MyAvana.Models.ViewModels;
using Quartz;
using MyAvana.CRM.Api.Controllers;
namespace MyAvana.CRM.Api
{
    public class Startup
    {
        private readonly string connection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connection = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "TestKey";
            })
            .AddJwtBearer("TestKey", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.Configure<FormOptions>(Option =>
            {
                Option.MultipartBodyLengthLimit = 200000000;
            });
           

            // Add ASP.NET Core Identity
            services.AddIdentity<UserEntity, UserRoleEntity>().AddEntityFrameworkStores<AvanaContext>().AddDefaultTokenProviders();
            services.AddDbContext<AvanaContext>(option => option.UseSqlServer(connection, b => b.MigrationsAssembly("MyAvana.Auth.Api")));

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;


                // User settings
                options.User.RequireUniqueEmail = true;
            });


            //Add Lower case urls
            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddHttpClient();

            //Add Lower case urls
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddCors(
                options => options.AddPolicy("AllowCors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            //.WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                            .AllowAnyHeader();
                    })
            );

            //TODO: Swap out with a real database in production
            services.AddDbContext<AvanaContext>(opt =>
            {
                // Configure the context to use an in-memory store.
                opt.UseSqlServer(connection, b => b.MigrationsAssembly("MyAvana.CRM.Api"));

            });
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.Configure<JWTSettings>(Configuration.GetSection("TokenAuthentication"));
            services.AddTransient<MyAvana.Logger.Contract.ILogger, Logger.Services.NLogServices>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<ISocialMediaService, SocialMediaService>();
            services.AddTransient<IPromoCodeService, PromoCodeService>();
            services.AddTransient<IBaseBusiness, BaseBusiness>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IProductsService, ProductService>();
            services.AddTransient<IWebLogin, WebService>();
            services.AddTransient<IIngredientsService, IngredientsService>();
            services.AddTransient<IQuestionaire, QuestionaireService>();
            services.AddTransient<IRegimenService, RegimenService>();
            services.AddTransient<IHairProfileService, HairProfileService>();
            services.AddTransient<IStylistService, StylistService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<ICalenderService, CalenderService>();
            services.AddTransient<IToolsService, ToolsService>();
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddTransient<IIndicatorService, IndicatorService>();
            services.AddTransient<IPaymentServices, PaymentServices>();
            services.AddTransient<IStripeServices, StripeServices>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ILiveSchedules, LiveSchedulesService>();
            services.AddTransient<ISalons, SalonService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IAlexaService, AlexaService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IAws3Services, Aws3Services>();
            services.AddTransient<IHairAnalysisStatusService, HairAnalysisStatusService>();
            services.AddTransient<IMobileHelpService, MobileHelpService>();
            services.AddTransient<IHairScopeService, HairScopeService>();
            //services.AddTransient<IWebSocket, WebSocketService>();
            services.AddTransient<ClaimsPrincipal>(
               s => s.GetService<IHttpContextAccessor>().HttpContext.User);
            //services.AddTransient<WebSocketController>();
            services.AddScoped<IWebSocket, WebSocketService>();
            var fcmSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(fcmSettingsSection);

            // services.AddMvc();
            services.AddMvc()
        .AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
            // Add the required Quartz.NET services
            services.AddQuartz(q =>
            {
                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // Create a "key" for the job
                var jobKey = new JobKey("PushNotificationJob");

                // Register the job with the DI container
             //   q.AddJob<PushNotificationJob>(opts => opts.WithIdentity(jobKey));

                // Create a trigger for the job
              //  q.AddTrigger(opts => opts
               //     .ForJob(jobKey) // link to the HelloWorldJob
                //    .WithIdentity("HelloWorldJob-trigger")
                 //   .WithCronSchedule("0 0/1 * * * ?")); // run every 1 minute
            });

            // Add the Quartz.NET hosted service

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);

            //services.AddScoped<WebSocketController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {//Enable CORS policy "AllowCors"  
            app.UseCors("AllowCors");
            loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };
            app.UseWebSockets(webSocketOptions);
                  
            app.Map("/ws", builder =>
            {
                builder.UseMiddleware<WebSocketController>();
            });         
            app.UseAuthentication();
            app.UseMvc();
            
          
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
                     


            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"questionnaireFile")),
            //    RequestPath = new PathString("/questionnaireFile")
            //});
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"HairProfileAnalysis")),
            //    RequestPath = new PathString("/HairProfileAnalysis")
            //});


        }
    }
}
