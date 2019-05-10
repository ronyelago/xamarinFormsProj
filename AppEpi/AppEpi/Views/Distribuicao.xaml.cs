using AppEpi.Models;
using AppEpi.ViewModels;
using System.Collections.ObjectModel;
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
                    //UsuarioLogado.Operacao = UsuarioLogado.Operacoes.Distribuicao;

                    var wbs = DependencyService.Get<IWEBClient>();
                    //ObservableCollection<DADOSEPI> dadosEpiCollection = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);

                    //Page4 detailPage = new Page4(dadosEpiCollection);
                    //NavigationPage.SetBackButtonTitle(this, "Voltar");

                    await Navigation.PushAsync(new ConfirmarDistribuicao());
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