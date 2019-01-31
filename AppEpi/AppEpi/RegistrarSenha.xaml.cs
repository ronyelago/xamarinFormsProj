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
    public partial class RegistrarSenha : ContentPage
    {
        public RegistrarSenha()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            string resultados = "";
            int coun = 0;
            bool confirmarMovimentacao = true;
            string localEstoque = "";

            string[] lines = entMatricula.Text.Split('\n');


            if (entMatricula.Text != "")
            {

                var result = wbs.funcionarioCracha(entMatricula.Text);
                if (result.Count > 0)
                {
                    if (result[0].Resultado == "OK")
                    {
                        var detailPage = new CadSenha(result);
                        await Navigation.PushPopupAsync(detailPage);
                    }
                    else
                    {
                        await DisplayAlert("Senha", result[0].Resultado, "OK");
                    }
                }
                //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                //var detailPage = new ResultadoTrn(result);
                //await Navigation.PushModalAsync(detailPage);
            }
            else
            {
                await DisplayAlert("Senha", "Verifique os Campos!", "OK");
            }
        }
    }
}