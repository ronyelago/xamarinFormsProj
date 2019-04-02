using AppEpi.Models;
using AppEpi.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class EnvioTeste : ContentPage, IConfirmacao
    {
        public EnvioTeste()
        {
            InitializeComponent();

            try
            {
                var wbs = DependencyService.Get<IWEBClient>();
                var result = wbs.retornaLocalEstoque().Where(x => x.FK_CLIENTE == UsuarioLogado.FkCliente).ToList();

                foreach (var rs in result)
                {
                    pckLocalEstoque.Items.Add(rs.CODIGO + "-" + rs.NOME);
                }
            }
            catch
            {
            }
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0 || pckLocalEstoque.SelectedIndex < 0)
            {
                await DisplayAlert("Envio Para Teste", "Verifique os Campos!", "OK");
            }
            else
            {
                string localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                localEstoque = localEstoque.Split('-')[0];

                var answer = await DisplayAlert("Envio Para Teste", "Confirmar Envio para Teste?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = UsuarioLogado.Operacoes.EnvioTeste;
                    UsuarioLogado.LocalEstoque = localEstoque;
                    var detailPage = new Page4(result);

                    await Navigation.PushAsync(detailPage);
                }
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}