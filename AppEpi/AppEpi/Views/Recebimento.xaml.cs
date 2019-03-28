using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Recebimento : ContentPage
    {
        public Recebimento()
        {
            InitializeComponent();
        }


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
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
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "1";
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