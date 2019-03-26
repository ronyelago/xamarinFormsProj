using System.Collections.Generic;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class MasterPage : ContentPage
    {
        public ListView MasterListView { get { return listView; } }

        public MasterPage()
        {
            InitializeComponent();

            nameUsuario.Text = UsuarioLogado.DadosUsuario[0].Nome;

            var masterPageItems = new List<MasterPageItem>
            {
                new MasterPageItem
                {
                    Title = "Atribuir Cracha",
                    TargetType = typeof(AtribuicaoCracha)
                },

                new MasterPageItem
                {
                    Title = "Movimentação de Estoque",
                    TargetType = typeof(MovimentacaoEstoque)
                },

                new MasterPageItem
                {
                    Title = "Recebimento",
                    TargetType = typeof(Recebimento)
                },

                new MasterPageItem
                {
                    Title = "Distribuição de EPI",
                    TargetType = typeof(Distribuicao)
                },

                new MasterPageItem
                {
                    Title = "Envio para Teste",
                    TargetType = typeof(EnvioTeste)
                },

                new MasterPageItem
                {
                    Title = "Recebimento de Itens Testado",
                    TargetType = typeof(RecebimentoTeste)
                },

                new MasterPageItem
                {
                    Title = "Envio para Higienização",
                    TargetType = typeof(EnvioHigienizacao)
                },

                new MasterPageItem
                {
                    Title = "Recebimento da Higienização",
                    TargetType = typeof(RecebimentoHigienizacao)
                },


                new MasterPageItem
                {
                    Title = "Fiscalização",
                    TargetType = typeof(Fiscalizacao)
                },

                //new MasterPageItem
                //{
                //    Title = "Inspeção",
                //    TargetType = typeof(Inspecao)
                //},

                new MasterPageItem
                {
                    Title = "Manutenção de EPI",
                    TargetType = typeof(Manutencao)
                },

                new MasterPageItem
                {
                    Title = "Devolução de EPI",
                    TargetType = typeof(Devolucao)
                },

                new MasterPageItem
                {
                    Title = "Descarte de EPI",
                    TargetType = typeof(Descarte)
                },

                new MasterPageItem
                {
                    Title = "Registrar Senha",
                    TargetType = typeof(RegistrarSenha)
                },

                new MasterPageItem
                {
                    Title = "Consultar EPI",
                    TargetType = typeof(Consulta)
                },

                new MasterPageItem
                {
                    Title = "Leitor Bluetooth",
                    TargetType = typeof(Bluetooth)
                }
            };

            listView.ItemsSource = masterPageItems;
        }
    }
}