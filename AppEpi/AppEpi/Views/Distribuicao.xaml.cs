using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Distribuicao : ContentPage
    {
        public Distribuicao()
        {
            InitializeComponent();
        }


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            btnConfirmar.Clicked -= btnConfirmar_Clicked;

            if (epcList.Count > 0)
            {
                var answer = await DisplayAlert("Entrega de EPI", "Confirmar Entrega de EPI?", "Sim", "Não");
                if (answer)
                {
                    UsuarioLogado.Operacao = "3";
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
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


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}