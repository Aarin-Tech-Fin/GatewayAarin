using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GatewayConsumer.Dtos;
using GatewayConsumer.Integration;
using GatewayConsumer.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewayConsumer.Controllers {

    [ApiController]
    [Route("pix")]
    public class PIxController : ControllerBase {
        private readonly IGatewayAarinIntegration _gatewayAarinIntegration;

        public PIxController(IGatewayAarinIntegration gatewayAarinIntegration) {
            _gatewayAarinIntegration = gatewayAarinIntegration;
        }

        [HttpPost("notificacao")]
        public async Task ReceberNotificacaoPix([FromHeader(Name = "identity-proof")] Guid? identifyProof,
            [FromBody] PixDto request) {
            await Task.Yield();

            if (identifyProof != Config.IdentifyProof) {
                throw new BadHttpRequestException("Identidade incorreta");
            }

            Console.WriteLine($"Implementa aqui sua lógica de conciliação do Pix de E2eid: {request.E2EId}");
        }
        
        [HttpPut("{pixId}/devolucao/{id}")]
        public async Task<DevolucaoResponse> CriarDevolucao([FromRoute, Required] Guid pixId,
            [FromRoute, Required, RegularExpression(@"[a-zA-Z0-9]{1,35}")] string id,
            [FromBody, Required] CriarDevolucaoRequest request) {
            var result = await _gatewayAarinIntegration.CriarDevolucao(pixId,id,request);
            
            if (result.Error != null || result.Content == null) {
                throw await result.Error!.ToMainIntegrationError();
            }

            return result.Content;
        }
    }
}