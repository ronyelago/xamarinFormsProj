﻿using Plugin.Geolocator;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
                var answer = await DisplayAlert("Fiscalização", "Confirmar Fiscalização?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {

                    //var result = wbs.inspecaoEPIFUNC(listEPCS);
                    var result = wbs.retornarDadosEpiValidar(listEPCS);
                    UsuarioLogado.Operacao = "6";
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new Page4(result);
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