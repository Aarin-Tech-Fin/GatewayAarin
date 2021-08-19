using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GatewayConsumer.Dtos
{

    public class CobrancaResponse : CriacaoCobrancaBase
    {
        [Required]
        public string Status { get; set; } = null!;

        [RegularExpression(@"[a-zA-Z0-9]{26,35}")]
        public string? TxId { get; set; }

        [Required]
        public int Revisao { get; set; }

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public LinksCobranca Links { get; set; } = null!;

        [Required]
        public Calendario Calendario { get; set; } = null!;

        [Required]
        public Guid EmpresaId { get; set; }

        [Required]
        public Guid Id { get; set; }
    }

    public class CriacaoCobrancaRequest : CriacaoCobrancaBase
    {
        [Required]
        public CalendarioBase Calendario { get; set; } = null!;
    }


    public class CriacaoCobrancaBase
    {
        public Pessoa? Devedor { get; set; }

        [Required]
        public Valor Valor { get; set; } = null!;

        [MaxLength(77)]
        public string? Chave { get; set; }

        [MaxLength(140)]
        public string? SolicitacaoPagador { get; set; }

        [MaxLength(50)]
        public List<InfoAdicional> InfoAdicionais { get; set; } = new List<InfoAdicional>();
    }

    public class CalendarioBase
    {
        public int Expiracao { get; set; }
    }

    public class Calendario : CalendarioBase
    {
        public DateTime Criacao { get; set; }
    }

    public class Valor
    {
        [Required]
        [RegularExpression(@"\d{1,10}\.\d{2}")]
        public string Original { get; set; } = null!;
    }

    public class InfoAdicional
    {
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Valor { get; set; } = null!;
    }

    public class LinksCobranca
    {

        [Required]
        public string LinkQrCode { get; set; } = null!;

        [Required]
        public string Emv { get; set; } = null!;

        public string? LinkCompartilhamento { get; set; }
    }
}