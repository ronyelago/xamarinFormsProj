using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEpi
{
    public class L_LOCALESTOQUE
    {
        public int ID { get; set; }
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<int> FK_CLIENTE { get; set; }
    }
}
