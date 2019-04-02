using System.Collections.Generic;

namespace AppEpi.Models
{
    public class UsuarioLogado
    {
        public static string Nome { get; internal set; }
        public static string LocalEstoque { get; set; }
        public static string StatusEstoque { get; set; }
        public static string DataTeste { get; internal set; }
        public static string ART { get; internal set; }
        public static string Latitude { get; set; }
        public static string Longitude { get; set; }
        public static string MatriculaDistribuicao { get; set; }
        public static string ChaveDocumento { get; internal set; }
        public static string FuncionarioAssinatura { get; internal set; }
        public static List<DADOSLOGIN> DadosUsuario { get; internal set; }
        public static Operacoes Operacao { get; set; }

        public static bool SenhaConfirmada = false;

        internal static string emailAssinatura;

        public static string Cnpj { get; set; }
        public static int FkCliente { get; set; }

        public enum Operacoes
        {
            AtribuicaoCracha = 0,
            Recebimento = 1,
            MovimentacaoEstoque = 2,
            Distribuicao = 3,
            EnvioTeste = 4,
            RecebimentoTeste = 5,
            Fiscalizacao = 6,
            Consulta = 7,
            EnvioHigienizacao = 8,
            RecebimentoHigienizacao = 9
        };
    }
}
