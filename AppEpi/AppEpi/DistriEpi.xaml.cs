using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class DistriEpi : ContentPage
    {
        public DistriEpi()
        {
            InitializeComponent();
        }


        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int count = 0;
            btnConfirmar.Clicked -= btnConfirmar_Clicked;
            string[] lines = crachaepis.Text.Split('\n');
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
                var answer = await DisplayAlert("Entrega de EPI", "Confirmar Entrega de EPI?", "Sim", "Não");
                if (answer)
                {
                    UsuarioLogado.Operacao = "3";
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    var detailPage = new Page4(result);
                    btnConfirmar.Clicked += btnConfirmar_Clicked;
                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);
                }
                else
                {
                    btnConfirmar.Clicked += btnConfirmar_Clicked;
                }
            }
            else
            {
                btnConfirmar.Clicked += btnConfirmar_Clicked;
                await DisplayAlert("Entrega de EPI", "Verifique os Campos!", "OK");
            }
        }


        async protected override void OnAppearing()
        {
            base.OnAppearing();
            crachaepis.Text = "";
        }


        async private void crachaepis_Unfocused(object sender, FocusEventArgs e)
        {
            //await DisplayAlert("Entrega de EPI", $"Total de Itens{epcs}", "Confirmar", "Cancelar");
        }


        async private void crachaepis_TextChanged(object sender, TextChangedEventArgs e)
        {

            //if (crachaepis.Text != "")
            //{
            //    string[] lines = crachaepis.Text.Split('\n');
            //    int coun = 0;

            //    //crachaepis.Text = "";
            //    foreach (string line in lines)
            //    {
            //        if (line != "")
            //        {
            //            if (line != "Unlicensed SerialDeviceManager")
            //            {
            //                coun++;
            //                epcs += "\n" + line;
            //            }
            //        }
            //    }
            //}

            //var result = await DisplayAlert("Entrega de EPI", $"Total de Itens{coun}", "Confirmar", "Cancelar");
        }
    }
}