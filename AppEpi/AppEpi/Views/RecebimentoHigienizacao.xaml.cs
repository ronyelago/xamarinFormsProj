using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class RecebimentoHigienizacao : ContentPage
    {
        public RecebimentoHigienizacao()
        {
            InitializeComponent();
        }

        private async void btnEnvioTeste_Clicked(object sender, EventArgs e)
        {
            if (epcList.Count <= 0)
            {
                await DisplayAlert("Recebimento da Higienização", "Verifique os Campos!", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Recebimento da Higienização", "Confirmar Recebimento?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "9";
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