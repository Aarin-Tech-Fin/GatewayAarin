using System;
using System.Threading.Tasks;
using GatewayConsumer.Dtos;
using GatewayConsumer.Integration;
using GatewayConsumer.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace GatewayConsumer.Controllers {
    
    [ApiController]
    [Route("cob")]
    public class CobrancaController : ControllerBase {
        private readonly IGatewayAarinIntegration _gatewayAarinIntegration;

        public CobrancaController(IGatewayAarinIntegration gatewayAarinIntegration) {
            _gatewayAarinIntegration = gatewayAarinIntegration;
        }

        [HttpPost]
        public async Task<CobrancaResponse> CriarCobranca([FromBody] CriacaoCobrancaRequest request) {
            var result = await _gatewayAarinIntegration.CriarCobrancaDinamica(request);
            
            if (result.Error != null || result.Content == null) {
                throw await result.Error!.ToMainIntegrationError();
            }

            return result.Content;
        }
    }
}