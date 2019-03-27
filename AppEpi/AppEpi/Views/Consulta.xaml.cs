using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Consulta : ContentPage
    {

        public Consulta()
        {
            InitializeComponent();
        }


        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();

            if (epcList.Count > 0)
            {
                var answer = await DisplayAlert("Consulta de Epi", "Confirmar Consulta?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var result = wbs.consultEPI(epcList.GetFormattedEpcList());
                    UsuarioLogado.Operacao = "7";
                    var detailPage = new ResultadoTrn(result);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Consulta de Epi", "Verifique os Campos!", "OK");
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}