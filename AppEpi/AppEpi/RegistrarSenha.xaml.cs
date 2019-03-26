using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;

namespace AppEpi
{
    public partial class RegistrarSenha : ContentPage
    {
        public RegistrarSenha()
        {
            InitializeComponent();
        }


        async private void Button_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();

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
            }
            else
            {
                await DisplayAlert("Senha", "Verifique os Campos!", "OK");
            }
        }
    }
}