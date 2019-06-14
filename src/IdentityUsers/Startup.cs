using System;
using System.Threading.Tasks;
using IdentityUsers.Data;
using IdentityUsers.Hubs;
using IdentityUsers.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityUsers
{
    public class Startup
    {
        public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectString = string.Empty;
            var password = Environment.GetEnvironmentVariable("SQLSERVER_SA_PASSWORD");
            var hostname = Environment.GetEnvironmentVariable("SQLSERVER_HOST");

            if (password != null && hostname != null)
                connectString = $"Server={hostname};Database=master;User Id=sa;Password={password};";
            else
                connectString = Configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectString));

            services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 12;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddCookie(options => {
                    options.LoginPath = "/Account/Unauthorized/";
                    options.AccessDeniedPath = "/Account/Forbidden/";
                })
                .AddJwtBearer(options =>
                {
                    // Configure JWT Bearer Auth to expect our security key
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            LifetimeValidator = (before, expires, token, param) =>
                            {
                                return expires > DateTime.UtcNow;
                            },
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateActor = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = SecurityKey
                        };

                    // We have to hook the OnMessageReceived event in order to
                    // allow the JWT authentication handler to read the access
                    // token from the query string when a WebSocket or
                    // Server-Sent Events request comes in.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/NotificationUserHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            services.AddSignalR();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account");
                });

            // https://dotnetcoretutorials.com/2018/03/20/cannot-consume-scoped-service-from-singleton-a-lesson-in-asp-net-core-di-scopes/
            // only use addScoped
            services.AddScoped<IUserConnectionManager, UserConnectionManager>();
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
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();

            app.UseSignalR(hubs =>
            {
                hubs.MapHub<NotificationUserHub>("/NotificationUserHub");
            });
        }
    }
}
