using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
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
            string listEPCS = "";
            int coun = 0;

            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    coun++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (coun > 0)
            {

                var answer = await DisplayAlert("Recebimento", "Confirmar Recebimento?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {
                    var data = dtProximtoTeste.Date.Day + "-" + dtProximtoTeste.Date.Month + "-" + dtProximtoTeste.Date.Year;
                    //var result = wbs.recebimentoDoTeste(listEPCS, data, entART.Text);
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "5";
                    UsuarioLogado.DataTeste = data;
                    UsuarioLogado.ART = entART.Text;
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                    //await Navigation.PushModalAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Recebimento", "Verifique os Campos!", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";

        }
    }
}