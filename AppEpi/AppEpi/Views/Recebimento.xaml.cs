using AppEpi.Models;
using AppEpi.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Recebimento : ContentPage, IConfirmacao
    {
        public Recebimento()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0)
            {
                await DisplayAlert("Recebimento", "Verifique os Campos!", "OK");
            }

            else
            {
                bool answer = await DisplayAlert("Recebimento", "Confirmar Recebimento?\nTotal de Itens:" + epcList.Count, "Sim", "Não");

                if (answer)
                {
                    IWEBClient wbs = DependencyService.Get<IWEBClient>();
                    ObservableCollection<DADOSEPI> result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);

                    // Define qual recurso será consumido
                    UsuarioLogado.Operacao = UsuarioLogado.Operacoes.Recebimento;

                    Page4 detailPage = new Page4(result);
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