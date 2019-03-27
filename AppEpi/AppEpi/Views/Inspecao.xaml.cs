using System;
using AppEpi.Models;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace AppEpi.Views
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

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            UsuarioLogado.Latitude = position.Latitude.ToString();
            UsuarioLogado.Longitude = position.Longitude.ToString();

            if (epcList.Count > 0)
            {
                var answer = await DisplayAlert("Inspeção", "Confirmar Inspeção?\nTotal de Itens:" + epcList.Count, "Sim", "Não");

                if (answer)
                {
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
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
            epcList.Clear();
        }
    }
}