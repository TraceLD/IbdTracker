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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                builder.UseNpgsql(
                    $@"Server={cfg.Server};Port={cfg.Port};Database={cfg.DatabaseName};User Id={cfg.UserId};Password={cfg.Password};");
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
                options.AddPolicy("read:patient",
                    builder => builder.Requirements.Add(new HasPermissionRequirement("read:patient",
                        Configuration["Auth0:Domain"])));
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IbdTracker v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}