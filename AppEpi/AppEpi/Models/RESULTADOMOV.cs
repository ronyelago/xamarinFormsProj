using System;

namespace AppEpi.Models
{
    public partial class RESULTADOMOV
    {
        public DateTime DataMovimentacao { get; set; }
        public string EPC { get; set; }
        public bool HasError { get; set; }
        public string Produto { get; set; }
        public string Resultado { get; set; }
        public string corAviso { get; set; }
    }
}
