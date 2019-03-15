using Plugin.Geolocator;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class Inspecao : ContentPage
    {
        public Inspecao()
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
                var answer = await DisplayAlert("Fiscalização", "Confirmar Fiscalização?\nTotal de Itens:" + count, "Sim", "Não");
                if (answer)
                {
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "6";
                    var detailPage = new Page4(result);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Fiscalização", "Verifique os Campos!", "OK");
            }
        }


        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";
        }
    }
}