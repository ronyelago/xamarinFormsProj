using AppEpi.Models;
using AppEpi.ViewModels;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class RecebimentoTeste : ContentPage, IConfirmacao
    {
        public RecebimentoTeste()
        {
            InitializeComponent();
            dtProximtoTeste.MinimumDate = DateTime.Now;
        }


        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0)
            {
                await DisplayAlert("Recebimento", "Verifique os Campos!", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Recebimento", "Confirmar Recebimento?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var data = dtProximtoTeste.Date.Day + "-" + dtProximtoTeste.Date.Month + "-" + dtProximtoTeste.Date.Year;
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = UsuarioLogado.Operacoes.RecebimentoTeste;
                    UsuarioLogado.DataTeste = data;
                    UsuarioLogado.ART = entART.Text;
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