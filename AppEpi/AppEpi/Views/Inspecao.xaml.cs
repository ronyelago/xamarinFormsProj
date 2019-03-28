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


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            if (epcList.Count <= 0)
            {
                await DisplayAlert("Inspeção", "Verifique os Campos!", "OK");
            }
            else
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                UsuarioLogado.Latitude = position.Latitude.ToString();
                UsuarioLogado.Longitude = position.Longitude.ToString();

                var answer = await DisplayAlert("Inspeção", "Confirmar Inspeção?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "10";
                    var detailPage = new EPIparaInspecionar(result);
                    NavigationPage.SetBackButtonTitle(this, "Voltar");

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