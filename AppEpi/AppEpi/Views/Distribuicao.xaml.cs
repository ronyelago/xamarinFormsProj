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
                    var listaCrachas = wbs.ValidaListaCrachas(epcList.GetFormattedEpcList());

                    DistribuicaoViewModel distribuicaoViewModel = new DistribuicaoViewModel();
                    distribuicaoViewModel.ListaCrachas = new ObservableCollection<ItemDistribuicaoViewModel>(listaCrachas);

                    //Page4 detailPage = new Page4(dadosEpiCollection);
                    NavigationPage.SetBackButtonTitle(this, "Voltar");

                    await Navigation.PushAsync(new ConfirmarDistribuicao(distribuicaoViewModel));
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