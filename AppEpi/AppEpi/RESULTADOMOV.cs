using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEpi
{
    public partial class RESULTADOMOV
    {
        public DateTime DataMovimentacao { get; set; }
        public string EPC { get; set; }

        public string Produto { get; set; }
        public string Resultado { get; set; }

        public string corAviso { get; set; }
    }
}
