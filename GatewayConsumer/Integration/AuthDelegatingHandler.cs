using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GatewayConsumer.Dtos;
using GatewayConsumer.Mappers;
using Microsoft.Extensions.Caching.Memory;

namespace GatewayConsumer.Integration
{
    public class AuthDelegatingHandler : DelegatingHandler
    {

        private readonly IAuthGatewayAarinIntegration _authGatewayIntegration;
        private readonly IMemoryCache _cache;


        public AuthDelegatingHandler(IMemoryCache cache, IAuthGatewayAarinIntegration authGatewayIntegration) {
            _authGatewayIntegration = authGatewayIntegration;
            _cache = cache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

            var token = await GetToken();

            request.Headers.Add("Authorization", $"Bearer {token}");
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetToken() {
            if (_cache.TryGetValue("_GatewayPixToken", out string token)) {
                return token;
            }
            
            var request = new LoginRequest {
                EmpresaId = Config.GatewayEmpresaId.ToString(),
                Senha = Config.GatewayAarinSenha,
                Escopo = new List<string>
                    {"cob.write", "cob.read", "pix.write", "pix.read", "webhook.write", "webhook.read", "account.read"}
            };
            
            var result = await _authGatewayIntegration.Login(request);
            
            if (result.Error != null || result.Content == null) {
                throw await result.Error!.ToMainIntegrationError();
            }

            _cache.Set(
                "_GatewayPixToken", result.Content.AccessToken,
                result.Content.ExpiresAt.AddMinutes(-1)
            );

            return result.Content!.AccessToken;
        }
    }
}