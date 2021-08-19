using System;
using GatewayConsumer.Integration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Newtonsoft.Json;

namespace GatewayConsumer {
    public class Startup {

        public void ConfigureServices(IServiceCollection services) {
            DotNetEnv.Env.Load();
            services.AddControllers();
            services.AddMemoryCache();
            
            JsonSerializerSettings jsonSettings = new() {
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"
            };

            var settings = new RefitSettings {
                ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSettings),
            };

            var gatewayClientBuilder = services.AddRefitClient<IGatewayAarinIntegration>(settings);
            var authGatewayClientBuilder = services.AddRefitClient<IAuthGatewayAarinIntegration>(settings);
            
            services.AddScoped<AuthDelegatingHandler>();

            authGatewayClientBuilder.ConfigureHttpClient(client => {
                client.BaseAddress = new Uri(Config.GatewayUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            gatewayClientBuilder.ConfigureHttpClient(client => {
                client.BaseAddress = new Uri(Config.GatewayUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            gatewayClientBuilder.AddHttpMessageHandler<AuthDelegatingHandler>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}