using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GatewayConsumer.Dtos;
using Refit;

namespace GatewayConsumer.Mappers {
    public static class GatewayExceptionMapper {
        public static async Task<List<IntegrationException>> ToIntegrationError(
            this ApiException ex) {

            var errorList = new List<IntegrationException>();
            var defaultError = new IntegrationException("Erro de conexão com o gateway Pix", "erro_interno", 500);

            try {
                var errorItems = await ex.GetContentAsAsync<ErrorItem[]>();

                foreach (var errorItem in errorItems!) {
                    if (ex.StatusCode == HttpStatusCode.BadRequest)
                        errorList.Add(new IntegrationException(errorItem.Message, "argumento_invalido", 400));
                    else if (ex.StatusCode == HttpStatusCode.NotFound)
                        errorList.Add(new IntegrationException(errorItem.Message, "nao_encontrado", 404));
                    else if (ex.StatusCode == HttpStatusCode.Conflict) errorList.Add(new IntegrationException(errorItem.Message, "conflito", 409));
                }
                return errorList;
            } catch {
                errorList.Add(defaultError);
                return errorList;
            }
        }

        /// <summary> Get the first error in the integration with the gateway. </summary>
        public static async Task<IntegrationException> ToMainIntegrationError(this ApiException ex) {
            return (await ex.ToIntegrationError()).First();
        }
    }

    public class IntegrationException : BaseException {
        public IntegrationException(string message, string code, int statusCode) : base(message, code, statusCode, null) { }
    }

    public class BaseException : Exception {
        protected BaseException(string message, string code, int statusCode, string? parameter) : base(message) {
            Code = code;
            StatusCode = statusCode;
            Parameter = parameter;
        }

        public string Code { get; }
        public string? Parameter { get; }
        public int StatusCode { get; }
    }


}