using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GatewayConsumer.Dtos {
    
    public class ErrorResponse {
        public List<ErrorItem> ErrorList { get; set; } = new();
    }
    // ReSharper enaable All

    public class ErrorItem {
        public string Message { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Parameter { get; set; }
    }


    public class Pessoa
    {
        [Required]
        public string Nome { get; set; } = null!;

        [RegularExpression(@"\d{11}")]
        public string? Cpf { get; set; }

        [RegularExpression(@"\d{14}")]
        public string? Cnpj { get; set; }
    }
}