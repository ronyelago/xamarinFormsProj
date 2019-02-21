using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InspVisual : ContentPage
    {
        public InspVisual()
        {
            InitializeComponent();
        }


        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            string resultados = "";
            int coun = 0;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            UsuarioLogado.Latitude = position.Latitude.ToString();
            UsuarioLogado.Longitude = position.Longitude.ToString();

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
                var answer = await DisplayAlert("Inspeção", "Confirmar Inspeção?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {

                    //var result = wbs.inspecaoEPIFUNC(listEPCS);
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "10";
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new EPIparaInspecionar(result);
                    //await Navigation.PushModalAsync(detailPage);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);

                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    //var detailPage = new NaoConforme(result);
                    //await Navigation.PushPopupAsync(detailPage);
                    //PushModalAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Inspeção", "Verifique os Campos!", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";

        }
    }
}