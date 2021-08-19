using System;

namespace GatewayConsumer {
    public static class Config {
        public static readonly Guid IdentifyProof = new("f02197dc-3c5c-44a2-bcae-47b65d589d44");
        public static readonly Guid GatewayEmpresaId = GetGuid("GATEWAY_AARIN_EMPRESA_ID") ?? Guid.Empty;
        public static readonly string GatewayAarinSenha = GetString("GATEWAY_AARIN_SENHA") ?? "";
        public static readonly string GatewayUrl = GetString("GATEWAY_URL") ?? "www.useenv.com";
        
        private static string? GetString(string name) {
            return Environment.GetEnvironmentVariable(name).NullIfEmpty();
        }
        
        private static Guid? GetGuid(string name) {

            var guidString = GetString(name);

            if (guidString == null) {
                return null;
            }

            return new Guid(guidString);
        }
        
        private static string? NullIfEmpty(this string? s) {
            return String.IsNullOrEmpty(s) ? null : s;
        }
    }
    

}