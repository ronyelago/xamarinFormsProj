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
            int count = 0;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            UsuarioLogado.Latitude = position.Latitude.ToString();
            UsuarioLogado.Longitude = position.Longitude.ToString();

            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    count++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (count > 0)
            {
                var answer = await DisplayAlert("Inspeção", "Confirmar Inspeção?\nTotal de Itens:" + count, "Sim", "Não");

                if (answer)
                {
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "10";
                    var detailPage = new EPIparaInspecionar(result);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);
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