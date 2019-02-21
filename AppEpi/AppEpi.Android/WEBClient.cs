using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using AppEpi.Droid;
using System.Collections.ObjectModel;

[assembly: Dependency(typeof(WEBClient))]
namespace AppEpi.Droid
{
    public class WEBClient : IWEBClient
    {
        public List<L_LOCALESTOQUE> retornaLocalEstoque()
        {
            try
            {

                List<L_LOCALESTOQUE> Lpd = new List<L_LOCALESTOQUE>();
                WBSClient.Client cl = new WBSClient.Client();
                var result = cl.retornaLocalEstoque();
                foreach (var item in result)
                {

                    Lpd.Add(new L_LOCALESTOQUE
                    {
                        CODIGO = item.CODIGO,
                        DATA_CADASTRO = item.DATA_CADASTRO,
                        DESCRICAO = item.DESCRICAO,
                        ID = item.ID,
                        NOME = item.NOME,
                        FK_CLIENTE = item.FK_CLIENTE
                    });
                }

                return Lpd;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public List<RESULTADOMOV> movimentacaoEstoque(string listaEPCS, string estoque, string entSaida)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.movimentacaoEstoque(listaEPCS, estoque, entSaida);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> recebimentoEstoques(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.recebimentoEstoque(listaEPCS);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                string estoque = "";
                foreach (var item in rest)
                {
                    if(estoque == "")
                    {
                        estoque = item.EPC;
                        Lpd.Add(new RESULTADOMOV
                        {
                            DataMovimentacao = item.DataMovimentacao,
                            EPC = item.EPC,
                            Resultado = item.Resultado,
                            Produto = item.Produto,
                            corAviso = isnullCor(item.corAviso)
                        });
                    }
                    else
                    {
                        if(estoque!=item.EPC)
                        {
                            estoque = item.EPC;
                            Lpd.Add(new RESULTADOMOV
                            {
                                DataMovimentacao = item.DataMovimentacao,
                                EPC = item.EPC,
                                Resultado = item.Resultado,
                                Produto = item.Produto,
                                corAviso = isnullCor(item.corAviso)
                            });
                        }
                    }

                   
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> recebimentoEstoquesCnpj(string listaEPCS, string cnpj)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.recebimentoEstoqueCnpj(listaEPCS, cnpj);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                string estoque = "";
                foreach (var item in rest)
                {
                    if (estoque == "")
                    {
                        estoque = item.EPC;
                        Lpd.Add(new RESULTADOMOV
                        {
                            DataMovimentacao = item.DataMovimentacao,
                            EPC = item.EPC,
                            Resultado = item.Resultado,
                            Produto = item.Produto,
                            corAviso = isnullCor(item.corAviso)
                        });
                    }
                    else
                    {
                        if (estoque != item.EPC)
                        {
                            estoque = item.EPC;
                            Lpd.Add(new RESULTADOMOV
                            {
                                DataMovimentacao = item.DataMovimentacao,
                                EPC = item.EPC,
                                Resultado = item.Resultado,
                                Produto = item.Produto,
                                corAviso = isnullCor(item.corAviso)
                            });
                        }
                    }


                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> atribuicaoCrachar(string matricula, string cracha)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.atribuicaoCracha(matricula, cracha);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> distribuicaoEPIS(string listaEPCS, string matricula)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.distribuicaoEPI(listaEPCS, matricula);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> envioParaTeste(string listaEPCS, string codEstoque)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.envioParaTeste(listaEPCS, codEstoque);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> recebimentoDoTeste(string listaEPCS, string dataTeste, string art)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.recebimentoTeste(listaEPCS, dataTeste, art);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> inspecaoEPIFUNC(string listaEPCS, string latitude, string longitude)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.inspecaoEPIFuncionario(listaEPCS, latitude, longitude);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> itemNaoConforme(string epc, string motivo)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.itemNaoConforme(epc, motivo);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> manutencaoEPIS(string epc, string estoque)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.manutencaoEPI(epc, estoque);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> funcionarioCracha(string cracha)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.funcionarioCracha(cracha);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> cadastSenha(string senha, string matricula)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.cadastrarSenha(senha, matricula);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<DADOSLOGIN> loginFunc(string matricula, string senha)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.loginFuncionario(matricula, senha);
                List<DADOSLOGIN> Lpd = new List<DADOSLOGIN>();
                foreach (var item in rest)
                {

                    Lpd.Add(new DADOSLOGIN
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso),
                        Empresa = item.Empresa,
                        Nome = item.Nome,
                        FkCliente = item.FkCliente,
                        Cnpj = item.Cnpj
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> devEPI(string epc, string estoque)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.devolucaoEPI(epc, estoque);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> descartItem(string epc, string estoque)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.descartarItem(epc, estoque);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> consultEPI(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.consultaEPI(listaEPCS, UsuarioLogado.Cnpj);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> retornarDEpi(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.retornarDadosEpi(listaEPCS);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<DADOSEPI> retornarDadosEpiValidar(string listaEPCS, string cnpj, int fkCliente)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.retornarDadosEpiValidar(listaEPCS, cnpj, fkCliente);
                ObservableCollection<DADOSEPI> Lpd = new ObservableCollection<DADOSEPI>();
                foreach (var item in rest)
                {
                    if (item.Qtd != 0)
                    {
                        Lpd.Add(new DADOSEPI
                        {
                            CodProduto = item.CodProduto,
                            CodFornecedor = item.CodFornecedor,
                            EPC = item.EPC,
                            Produto = item.Produto,
                            Qtd = item.Qtd
                        });
                    }
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        private string isnullCor(string corAviso)
        {
            if (corAviso != null)
            {
                return corAviso;
            }
            else
            {
                return "#ffffff";
            }
        }

        public bool temSenha(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.FuncionarioTemSenha(listaEPCS);
                return rest;

            }
            catch
            {
                return false;
            }
        }

        public List<RESULTADOMOV> envioParaHigienizacao(string listaEPCS, string codEstoque)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.envioParaHigienizacao(listaEPCS, codEstoque);
                
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }

        public List<RESULTADOMOV> recebimentoDaHigienizacao(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.recebimentoHigienizacao(listaEPCS);
                List<RESULTADOMOV> Lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    Lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.corAviso)
                    });
                }
                return Lpd;
            }
            catch
            {
                return null;
            }
        }
    }
}