using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class AtribuicaoCracha : ContentPage
    {
        public AtribuicaoCracha()
        {
            InitializeComponent();
        }


        async void btnAtribuir_Clicked(object sender, EventArgs e)
        {

            if (epcList.Count <= 0 || entMatricula.Text == "")
            {
                await DisplayAlert("Atribuição", "Verifique os Campos!", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Atribuição de Cracha", "Deseja Confirmar Atribuição?", "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.atribuicaoCrachar(entMatricula.Text, epcList.GetFormattedEpcList());
                    UsuarioLogado.Operacao = "0";
                    var detailPage = new ResultadoTrn(result);

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