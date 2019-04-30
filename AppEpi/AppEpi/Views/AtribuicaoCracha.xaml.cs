using AppEpi.Models;
using AppEpi.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
                await DisplayAlert("Erro", "Verifique os Campos.", "OK");
            }

            else
            {
                IWEBClient wbs = DependencyService.Get<IWEBClient>();
                List<RESULTADOMOV> result = wbs.atribuicaoCrachar(entMatricula.Text, epcList.GetFormattedEpcList());
                UsuarioLogado.Operacao = (int)UsuarioLogado.Operacoes.AtribuicaoCracha;

                if (result.Any(r => r.HasError == true))
                {
                    await DisplayAlert("Erro", $"{result[0].Resultado}", "Fechar");
                }

                else
                {
                    bool answer = await DisplayAlert("Atribuição de Crachá", "Confirmar Atribuição?", "Sim", "Não");

                    if (answer)
                    {
                        var detailPage = new ResultadoTrn(result);

                        await Navigation.PushAsync(detailPage);
                    }
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