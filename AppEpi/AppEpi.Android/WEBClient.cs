﻿using System.Collections.Generic;
using Xamarin.Forms;
using AppEpi.Droid;
using System.Collections.ObjectModel;
using AppEpi.Models;
using System;
using AppEpi.ViewModels;
using System.Linq;

[assembly: Dependency(typeof(WEBClient))]
namespace AppEpi.Droid
{
    public class WEBClient : IWEBClient
    {
        public List<L_LOCALESTOQUE> retornaLocalEstoque()
        {
            try
            {
                List<L_LOCALESTOQUE> lpd = new List<L_LOCALESTOQUE>();
                WBSClient.Client cl = new WBSClient.Client();
                var result = cl.retornaLocalEstoque();
                foreach (var item in result)
                {
                    lpd.Add(new L_LOCALESTOQUE
                    {
                        CODIGO = item.CODIGO,
                        DATA_CADASTRO = item.DATA_CADASTRO,
                        DESCRICAO = item.DESCRICAO,
                        ID = item.ID,
                        NOME = item.NOME,
                        FK_CLIENTE = item.FK_CLIENTE
                    });
                }

                return lpd;
            }
            catch
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                string estoque = "";
                foreach (var item in rest)
                {
                    if(estoque == "")
                    {
                        estoque = item.EPC;
                        lpd.Add(new RESULTADOMOV
                        {
                            DataMovimentacao = item.DataMovimentacao,
                            EPC = item.EPC,
                            Resultado = item.Resultado,
                            Produto = item.Produto,
                            corAviso = isnullCor(item.CorAviso)
                        });
                    }
                    else
                    {
                        if(estoque!=item.EPC)
                        {
                            estoque = item.EPC;
                            lpd.Add(new RESULTADOMOV
                            {
                                DataMovimentacao = item.DataMovimentacao,
                                EPC = item.EPC,
                                Resultado = item.Resultado,
                                Produto = item.Produto,
                                corAviso = isnullCor(item.CorAviso)
                            });
                        }
                    }
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                string estoque = "";
                foreach (var item in rest)
                {
                    if (estoque == "")
                    {
                        estoque = item.EPC;
                        lpd.Add(new RESULTADOMOV
                        {
                            DataMovimentacao = item.DataMovimentacao,
                            EPC = item.EPC,
                            Resultado = item.Resultado,
                            Produto = item.Produto,
                            corAviso = isnullCor(item.CorAviso)
                        });
                    }
                    else
                    {
                        if (estoque != item.EPC)
                        {
                            estoque = item.EPC;
                            lpd.Add(new RESULTADOMOV
                            {
                                DataMovimentacao = item.DataMovimentacao,
                                EPC = item.EPC,
                                Resultado = item.Resultado,
                                Produto = item.Produto,
                                corAviso = isnullCor(item.CorAviso)
                            });
                        }
                    }
                }
                return lpd;
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
                ObservableCollection<DADOSEPI> lpd = new ObservableCollection<DADOSEPI>();
                foreach (var item in rest)
                {
                    if (item.Quantidade != 0)
                    {
                        lpd.Add(new DADOSEPI
                        {
                            CodProduto = item.CodigoProduto,
                            CodFornecedor = item.CodigoFornecedor,
                            EPC = item.Epc,
                            Produto = item.Produto,
                            Qtd = item.Quantidade
                        });
                    }
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();

                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso),
                        HasError = item.HasError
                    });
                }

                return lpd;
            }

            catch
            {
                return null;
            }
        }

        public List<ItemDistribuicaoViewModel> ValidaListaCrachas(string listaEpc)
        {
            var client = new WBSClient.Client();
            List<WBSClient.DistribuicaoViewModel> retults = client.ValidaListaCrachas(listaEpc).ToList();

            List<ItemDistribuicaoViewModel> distribuicaoViewModels = new List<ItemDistribuicaoViewModel>();

            foreach (var item in retults)
            {
                distribuicaoViewModels.Add(new ItemDistribuicaoViewModel
                {
                    Titulo = item.Titulo,
                    Epc = item.Epc,
                    Disponivel = item.Disponivel,
                    Icone = item.Icone,
                    Observacoes = item.Observacoes
                });
            }

            return distribuicaoViewModels;
        }

        public List<RESULTADOMOV> envioParaTeste(string listaEPCS, string codEstoque)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.envioParaTeste(listaEPCS, codEstoque);
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {

                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
            }
            catch
            {
                return null;
            }
        }


        public List<DADOSLOGIN> loginFunc(string loginUsuario, string senha)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                WBSClient.DADOSLOGIN[] rest = cl.loginFuncionario(loginUsuario, senha);

                List<DADOSLOGIN> lpd = new List<DADOSLOGIN>();

                foreach (var item in rest)
                {
                    lpd.Add(new DADOSLOGIN
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

                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
            }
            catch
            {
                return null;
            }
        }


        public List<RESULTADOMOV> consultEPIouCracha(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.consultaEPIouCracha(listaEPCS, UsuarioLogado.Cnpj);
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public List<RESULTADOMOV> retornarDEpi(string listaEPCS)
        {
            try
            {
                WBSClient.Client cl = new WBSClient.Client();
                var rest = cl.retornarDadosEpi(listaEPCS);
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
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
                List<RESULTADOMOV> lpd = new List<RESULTADOMOV>();
                foreach (var item in rest)
                {
                    lpd.Add(new RESULTADOMOV
                    {
                        DataMovimentacao = item.DataMovimentacao,
                        EPC = item.EPC,
                        Resultado = item.Resultado,
                        Produto = item.Produto,
                        corAviso = isnullCor(item.CorAviso)
                    });
                }
                return lpd;
            }
            catch
            {
                return null;
            }
        }
    }
}