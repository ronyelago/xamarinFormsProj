﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEpi
{
    public class DADOSLOGIN
    {
        public string Cnpj { get; set; }

        public string corAviso { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string Empresa { get; set; }
        public string EPC { get; set; }
        public int FkCliente { get; set; }
        public string Nome { get; set; }
        public string Produto { get; set; }
        public string Resultado { get; set; }
    }
}