using AppEpi.Models;
using AppEpi.ViewModels;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Consulta : ContentPage, IConfirmacao
    {

        public Consulta()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0)
            {
                await DisplayAlert("Consulta de Epi", "Verifique os Campos!", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Consulta de Epi", "Confirmar Consulta?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.consultEPIouCracha(epcList.GetFormattedEpcList());
                    UsuarioLogado.Operacao = UsuarioLogado.Operacoes.Consulta;
                    var detailPage = new ResultadoTrn(result);
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