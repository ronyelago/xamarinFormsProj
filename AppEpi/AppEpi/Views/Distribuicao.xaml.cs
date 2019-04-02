using AppEpi.Models;
using AppEpi.ViewModels;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Distribuicao : ContentPage, IConfirmacao
    {
        public Distribuicao()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0)
            {
                await DisplayAlert("Entrega de EPI", "Verifique os Campos!", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Entrega de EPI", "Confirmar Entrega de EPI?", "Sim", "Não");
                if (answer)
                {
                    UsuarioLogado.Operacao = UsuarioLogado.Operacoes.Distribuicao;
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    var detailPage = new Page4(result);
                    NavigationPage.SetBackButtonTitle(this, "Voltar");

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