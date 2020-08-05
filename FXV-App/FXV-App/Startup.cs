using System.Text;
using System.Threading.Tasks;
using FXV.CustomAuth;
using FXV.Data;
using FXV.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;

namespace FXV
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("fxv.co.nz");
                options.ExcludedHosts.Add("www.fxv.co.nz");
            });

            services.Configure<IISServerOptions>(
                options =>
                {
                    options.AutomaticAuthentication = true;
                }); // IIS service options

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = true;
            }); // IIS options


            services.AddDbContext<ApplicationDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
                );


            services.AddIdentity<AppUser, AppRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                }
                ).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<CustomUserManager<AppUser>>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddCustomJwtBearer(
                options =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["access_token"];
                            return Task.CompletedTask;
                        }
                    };
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters =
                    new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "localhost",
                        ValidAudience = "localhost",
                        IssuerSigningKey = new
                        SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:privatekey"]))
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("All", policy =>
                                  policy.RequireClaim("Role", "Athlete", "Organization", "Admin", "Manager"));
                options.AddPolicy("All_NoAthlete", policy =>
                                  policy.RequireClaim("Role", "Organization", "Admin", "Manager"));
                options.AddPolicy("Manager", policy =>
                                  policy.RequireClaim("Role", "Organization", "Manager"));
                options.AddPolicy("AdminOrganization", policy =>
                                  policy.RequireClaim("Role", "Admin", "Organization"));
                options.AddPolicy("Athlete", policy =>
                                  policy.RequireClaim("Role", "Athlete"));
                options.AddPolicy("Organization", policy =>
                                  policy.RequireClaim("Role", "Organization"));
                options.AddPolicy("Admin", policy =>
                                  policy.RequireClaim("Role", "Admin"));
                options.AddPolicy("Permission_All", policy =>
                                  policy.RequireClaim("Permission", "1"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction())
            {
                app.UseStatusCodePagesWithReExecute("/Errors/Index", "?statusCode={0}");
            }

            app.UseHsts();


            app.UseHttpsRedirection();

            app.UseRewriter(new RewriteOptions()
                .AddRedirectToWww()
                .AddRedirectToHttps()
            );

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseFileServer();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=account}/{action=index}/{var?}");
            });

        }
    }
}
