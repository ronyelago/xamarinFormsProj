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

        async private void btnEnvioTeste_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();

            if (epcList.Count > 0)
            {
                var answer = await DisplayAlert("Recebimento da Higienização", "Confirmar Recebimento?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "9";
                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Recebimento da Higienização", "Verifique os Campos!", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}