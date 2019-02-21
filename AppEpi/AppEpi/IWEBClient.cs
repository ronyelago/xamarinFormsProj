using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEpi
{
    public interface IWEBClient
    {
        List<L_LOCALESTOQUE> retornaLocalEstoque();
        List<RESULTADOMOV> movimentacaoEstoque(string listaEPCS, string estoque, string entSaida);
        List<RESULTADOMOV> recebimentoEstoques(string listaEPCS);

        List<RESULTADOMOV> recebimentoEstoquesCnpj(string listaEPCS, string cnpj);
        List<RESULTADOMOV> atribuicaoCrachar(string matricula, string cracha);
        List<RESULTADOMOV> distribuicaoEPIS(string listaEPCS, string matricula);
        List<RESULTADOMOV> envioParaTeste(string listaEPCS, string codEstoque);

        List<RESULTADOMOV> recebimentoDoTeste(string listaEPCS, string dataTeste, string art);

        List<RESULTADOMOV> inspecaoEPIFUNC(string listaEPCS, string latitude, string longitude);
        List<RESULTADOMOV> itemNaoConforme(string epc, string motivo);

        List<RESULTADOMOV> manutencaoEPIS(string epc, string estoque);

        List<RESULTADOMOV> funcionarioCracha(string cracha);

        List<RESULTADOMOV> cadastSenha(string senha, string matricula);

        List<DADOSLOGIN> loginFunc(string matricula, string senha);

        List<RESULTADOMOV> devEPI(string epc, string estoque);

        List<RESULTADOMOV> descartItem(string epc, string estoque);

        List<RESULTADOMOV> consultEPI(string listaEPCS);

        List<RESULTADOMOV> retornarDEpi(string listaEPCS);

        ObservableCollection<DADOSEPI> retornarDadosEpiValidar(string listaEPCS, string cnpj, int fkCliente);

        bool temSenha(string listaEPCS);

        List<RESULTADOMOV> envioParaHigienizacao(string listaEPCS, string codEstoque);
        List<RESULTADOMOV> recebimentoDaHigienizacao(string listaEPCS);
    }
}
