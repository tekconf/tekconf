using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using tekconf.api.Repositories;

namespace tekconf.api
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
            var identityUrl = configuration["identityUrl"];
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "confArchApi";
                });

            services.AddSingleton<AttendeeRepo>();
            services.AddSingleton<ConferenceRepo>();
            services.AddSingleton<ProposalRepo>();

            services.AddAuthorization(o => o.AddPolicy("PostAttendee",
                p => p.RequireClaim("scope", "confArchApiPostAttendee")));

            if (!_hostingEnvironment.IsDevelopment())
            {
                services.Configure<MvcOptions>(
                    o => o.Filters.Add(new RequireHttpsAttribute())
                );
            }



            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!_hostingEnvironment.IsDevelopment())
            {
                app.UseHsts(h => h.MaxAge(days: 365));
            }
            app.UseCsp(options => options.DefaultSources(s => s.Self()));
            app.UseXfo(o => o.Deny());
            app.UseCors(o => o.AllowAnyOrigin());
            app.UseMvcWithDefaultRoute();
        }
    }
}
