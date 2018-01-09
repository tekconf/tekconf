using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using tekconf.web.Authorization;
using tekconf.web.Api;

namespace tekconf.web
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(_hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            var configuration = builder.Build();
            var apiUrl = configuration["apiUrl"];
            var identityUrl = configuration["identityUrl"];

            if (!_hostingEnvironment.IsDevelopment())
            {
                services.Configure<MvcOptions>(
                    o => o.Filters.Add(new RequireHttpsAttribute())
                );
            }
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", b =>
            {
                b.SignInScheme = "Cookies";
                b.Authority = identityUrl;
                b.RequireHttpsMetadata = false;

                b.ClientId = "confarchweb";
                b.ClientSecret = "secret";

                b.ResponseType = "code id_token";
                b.Scope.Add("confArchApi");
                b.Scope.Add("roles");
                b.Scope.Add("experience");
                b.SaveTokens = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OrganizerAccessPolicy", policy => policy.RequireRole("organizer"));

                options.AddPolicy("SpeakerAccessPolicy",
                    policy => policy.RequireAssertion(context => context.User.IsInRole("speaker")));

                options.AddPolicy("YearsOfExperiencePolicy",
                    policy => policy.AddRequirements(new YearsOfExperienceRequirement(6)));

                options.AddPolicy("ProposalEditPolicy",
                    policy => policy.AddRequirements(new ProposalRequirement(false)));

            });

            services.AddSingleton<IAuthorizationHandler, YearsOfExperienceAuthorizationHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ConferenceApiService>();
            services.AddTransient<ProposalApiService>();
            services.AddTransient<AttendeeApiService>();
            services.AddSingleton(x => new HttpClient
            {
                BaseAddress = new Uri(apiUrl)
            });
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            if (!_hostingEnvironment.IsDevelopment())
            {
                app.UseHsts(h => h.MaxAge(days: 365));
            }
            app.UseCsp(options => options.DefaultSources(s => s.Self()));
            app.UseXfo(o => o.Deny());
            app.UseCors(o => o.AllowAnyOrigin());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
