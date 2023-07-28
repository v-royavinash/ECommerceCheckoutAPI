using ECommerceCheckout.Domain;
using ECommerceCheckout.Domain.Models;
using ECommerceCheckout.Domain.Services;
using ECommerceCheckoutAPI.Authentication;
using Microsoft.AspNetCore.Authentication;
using ECommerceCheckout.Utilities.Commons;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

namespace ECommerceCheckoutAPI
{
    /// <summary>
    /// Startup class to configure the ASP.NET Core Web API application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration instance.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method to configure services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            services.AddControllers();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddSingleton<IEnumerable<Watch>>(WatchCatalog.Watches);

            string apiKey = Configuration.GetValue<string>("ApiKey");
            services.AddAuthentication(Constants.ApiKeyAuthenticationScheme)
                     .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                         Constants.ApiKeyAuthenticationScheme,
                         options => { options.ApiKey = apiKey; });

            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ECommerce Checkout API",
                    Version = "v1",
                    Description = "API for handling checkout actions.",
                    Contact = new OpenApiContact
                    {
                        Name = "Avinash Roy",
                        Email = "nashavi12@gmail.com",
                        Url = new System.Uri("https://github.com/v-royavinash/ECommerceCheckoutAPI")
                    }
                });

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "ApiKey",
                    Description = "API key needed to access the endpoints.",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            },
                        },
                        new List<string>()
                    }
                });
            });

        }

        /// <summary>
        /// The configure method.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce Checkout API v1");
            });
        }
    }
}
