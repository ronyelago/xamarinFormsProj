using AppEpi.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class RegistroSenha : ContentPage, IConfirmacao
    {
        public RegistroSenha()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (entMatricula.Text == "")
            {
                await DisplayAlert("Registrar Senha", "Verifique os Campos!", "OK");
            }
            else
            {
                var wbs = DependencyService.Get<IWEBClient>();
                var result = wbs.funcionarioCracha(entMatricula.Text);
                if (result.Count > 0)
                {
                    if (result[0].Resultado == "OK")
                    {
                        try
                        {
                            var detailPage = new CadastroSenha(result);
                            await Navigation.PushPopupAsync(detailPage);
                        }
                        catch
                        {
                            await DisplayAlert("Registrar Senha", "ERRO", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Registrar Senha", result[0].Resultado, "OK");
                    }
                }
            }
        }
    }
}