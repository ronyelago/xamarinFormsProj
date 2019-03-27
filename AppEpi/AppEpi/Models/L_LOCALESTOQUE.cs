using System;

namespace AppEpi.Models
{
    public class L_LOCALESTOQUE
    {
        public int ID { get; set; }
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public string DESCRICAO { get; set; }
        public DateTime? DATA_CADASTRO { get; set; }
        public int? FK_CLIENTE { get; set; }
    }
}
