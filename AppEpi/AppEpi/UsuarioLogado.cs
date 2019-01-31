using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEpi
{
    public class UsuarioLogado
    {
        public static bool TemCadastro { get; set; }
        public static string Cep { get; internal set; }
        public static string Email { get; internal set; }
        public static string Logradouro { get; internal set; }
        public static string Nome { get; internal set; }
        public static string Numero { get; internal set; }
        public static string Telefone { get; internal set; }
        public static string Codigo { get; internal set; }
        public static string Complemento { get; internal set; }
        public static string CodigoImagem { get; internal set; }
        public static string HojePorExtenso { get; internal set; }
        public static string HorarioEntregaInicio { get; internal set; }
        public static string HorarioEntregaFim { get; internal set; }
        public static string Bairro { get; set; }
        public static string RegiaoEntrega { get; internal set; }
        public static string CPF { get; internal set; }
        public static string RetirarLoja { get; internal set; }
        public static int IDProdEscolhido { get; internal set; }
        public static int IDProdEscolhidoComplemento { get; internal set; }
        public static string PrecoProd { get; internal set; }
        public static string NomeProdEscolhido { get; internal set; }
        public static string ImagemProdEscolhido { get; internal set; }
        public static string CondicaoPagamentoEscolhido { get; internal set; }
        public static string SessaPedido { get; internal set; }
        public static int IDCategoria { get; internal set; }
        public static string LocalEstoque { get; set; }
        public static string StatusEstoque { get; set; }
        public static string Operacao { get; set; }
        public static string DataTeste { get; internal set; }
        public static string ART { get; internal set; }
        public static string Latitude { get;  set; }
        public static string Longitude { get;  set; }
        public static string MatriculaDistribuicao { get;  set; }
        public static string ChaveDocumento { get; internal set; }
        public static string FuncionarioAssinatura { get; internal set; }
        public static List<DADOSLOGIN> DadosUsuario { get; internal set; }

        public static bool SenhaConfirmada = false;
        public static bool SenhaConfirmadaEntrega = false;

        public static int QtdItensPromocao = 0;

        public static int PartesCount = 0;

        public static int Partes = 1;

        public static bool CardapioFechar = false;
        public static string CodigoPedidoEscolhido = "";
        public static bool ModalCadastro = false;

        public static bool PodeFazerPedido = true;
        public static bool IrProCarrinho = false;
        public static bool PedidoEmAndamento = false;

        public static bool PodeEnviar = false;
        internal static string emailAssinatura;

        public static string Cnpj { get; set; }
        public static int FkCliente { get; set; }

        public static string[] categoriaInspecao = new string[] { "Cinto Paraquedista", "Talabarte", "Talabarte de Posicionamento",
        "Fita de Ancoragem",
        "Trava Quedas",
        "Conectores",
        "Cordas"};
    }
}
