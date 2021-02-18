using System.Collections.Generic;
using System.Security.Claims;
using FluentValidation.AspNetCore;
using IbdTracker.Core;
using IbdTracker.Core.Config;
using IbdTracker.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace IbdTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // register db config;
            services.Configure<DbConfig>(Configuration.GetSection("Database"));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<DbConfig>>().Value);

            // add db context;
            services.AddDbContext<IbdSymptomTrackerContext>((provider, builder) =>
            {
                var cfg = provider.GetRequiredService<DbConfig>();
                builder.UseNpgsql(cfg.ConnectionString);
            });

            services.AddMediatR(typeof(Startup));

            // configure auth;
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Domain"];
                    options.Audience = Configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
            services.AddAuthorization(options =>
            {
                foreach (var permission in ApiPermissions.PermissionsList)
                {
                    options.AddPolicy(permission,
                        builder => builder.Requirements.Add(
                            new HasPermissionRequirement(permission, Configuration["Auth0:Domain"])));
                }
            });
            services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();

            services.AddControllers()
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>();
                    configuration.ImplicitlyValidateChildProperties = true;
                });

            // configure swagger/openapi;
            // TODO: add swagger/openapi decorators to controllers;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "IbdTracker", Version = "v1"});
            });

            if (Environment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:8080")
                                .AllowCredentials()
                                .AllowAnyHeader();
                        });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IbdTracker v1"));
                app.UseCors();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}