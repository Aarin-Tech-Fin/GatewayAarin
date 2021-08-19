using System;
using System.Threading.Tasks;
using GatewayConsumer.Dtos;
using Refit;

namespace GatewayConsumer.Integration {
    public interface IGatewayAarinIntegration {
        [Post("/cob")]
        Task<ApiResponse<CobrancaResponse>> CriarCobrancaDinamica(CriacaoCobrancaRequest body);
        
        [Put("/pix/{pixId}/devolucao/{id}")]
        Task<ApiResponse<DevolucaoResponse>> CriarDevolucao(Guid pixId, string id, CriarDevolucaoRequest body);
    }
}