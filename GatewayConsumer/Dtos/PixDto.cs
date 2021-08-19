using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatewayConsumer.Dtos {

    public class PixDto {
        public Guid Id { get; set; }

        [RegularExpression(@"[a-zA-Z0-9]{32}")]
        public string? E2EId { get; set; }
        
        [Required]
        public Guid CobrancaId { get; set; }
        
        [Required]
        [RegularExpression(@"\d{1,10}\.\d{2}")]
        public string Valor { get; set; } = null!;
        
        [Required]
        public DateTime Horario { get; set; }
        
        [MaxLength(140)]
        public string InfoPagador { get; set; } = null!;
        
        public Pessoa? Pagador { get; set; }
        
        [Required]
        public List<DevolucaoResponse> Devolucoes { get; set; } = null!;
    }
    
    public class CriarDevolucaoRequest {
        [RegularExpression(@"\d{1,10}\.\d{2}")]
        [Required]
        public string Valor { get; set; } = null!;
    }
    
    public class DevolucaoResponse : CriarDevolucaoRequest {
        
        [RegularExpression(@"[a-zA-Z0-9]{32}")]
        public string? RtrId { get; set; }
        
        [Required]
        [RegularExpression(@"[a-zA-Z0-9]{1,35}")]
        public string Id { get; set; } = null!;
        
        public Guid PixId { get; set; }
        
        [Required]
        public StatusDevolucao Status { get; set; }
        
        [Required]
        public HorarioDevolucao Horario { get; set; } = null!;
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusDevolucao {
        [EnumMember(Value = "EM_PROCESSAMENTO")]
        EmProcessamento,
        [EnumMember(Value = "DEVOLVIDO")]
        Devolvido,
        [EnumMember(Value = "NAO_REALIZADO")]
        NaoRealizado
    }
    
    public class HorarioDevolucao {
        /// <summary>Horário no qual a devolução foi solicitada no PSP (No TimeZone UTC).</summary>
        /// <example>2020-07-28T20:14:00.364Z</example>
        [Required]
        public DateTime Solicitacao { get; set; }

        /// <summary>Horário no qual a devolução foi liquidada no PSP (No TimeZone UTC).</summary>
        /// <example>2020-07-31T20:14:00.364Z</example>
        public DateTime? Liquidacao { get; set; }
    }
}