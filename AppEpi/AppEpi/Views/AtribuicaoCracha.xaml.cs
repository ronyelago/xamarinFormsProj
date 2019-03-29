using AppEpi.Models;
using AppEpi.ViewModels;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class AtribuicaoCracha : ContentPage, IConfirmacao
    {
        public AtribuicaoCracha()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
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