using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace GatewayConsumer.Integration {
    public class LoginResponse {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string RefresExpiresAthToken { get; set; } = null!;
    }

    public class LoginRequest {
        public string EmpresaId { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public List<string> Escopo = new List<string>();
    }

    public interface IAuthGatewayAarinIntegration {
        [Post("/oauth/token")]
        Task<ApiResponse<LoginResponse>> Login(LoginRequest body);
    }
}