using AppEpi.Models;
using AppEpi.ViewModels;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class RecebimentoHigienizacao : ContentPage, IConfirmacao
    {
        public RecebimentoHigienizacao()
        {
            InitializeComponent();
        }

        async void IConfirmacao.OnConfirmarClicked()
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