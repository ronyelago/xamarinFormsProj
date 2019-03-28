using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class RecebimentoTeste : ContentPage
    {
        public RecebimentoTeste()
        {
            InitializeComponent();
            dtProximtoTeste.MinimumDate = DateTime.Now;
        }


        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
        }


        async private void btnEnvioTeste_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();

            if (epcList.Count > 0)
            {
                var answer = await DisplayAlert("Recebimento", "Confirmar Recebimento?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var data = dtProximtoTeste.Date.Day + "-" + dtProximtoTeste.Date.Month + "-" + dtProximtoTeste.Date.Year;
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "5";
                    UsuarioLogado.DataTeste = data;
                    UsuarioLogado.ART = entART.Text;
                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Recebimento", "Verifique os Campos!", "OK");
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}