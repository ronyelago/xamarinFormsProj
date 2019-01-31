using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : PopupPage
    {
        private int contadorErroSenha = 0;

        public LoginPage()
        {
            InitializeComponent();
        }

        async private void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoginButton.Clicked -= LoginButton_Clicked;
                if (contadorErroSenha == 3)
                {
                    CloseAllPopup();
                    var detailPage = new RegistrarSenha();
                    await Navigation.PushModalAsync(detailPage);
                }

                if (entMatricula.Text != "" && entSenha.Text != "")
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.loginFunc(entMatricula.Text, entSenha.Text);

                    if (result.Find(x => x.Resultado == "OK") != null)
                    {
                        LoginButton.Clicked += LoginButton_Clicked;
                        UsuarioLogado.MatriculaDistribuicao = entMatricula.Text;
                        UsuarioLogado.SenhaConfirmada = true;
                        UsuarioLogado.SenhaConfirmadaEntrega = true;
                        CloseAllPopup();
                    }
                    else
                    {
                        LoginButton.Clicked += LoginButton_Clicked;
                        UsuarioLogado.SenhaConfirmada = false;
                        await DisplayAlert("Login", result[0].Resultado, "OK");
                        contadorErroSenha++;
                    }
                }
                else
                {
                    LoginButton.Clicked += LoginButton_Clicked;
                }
            }
            catch
            {
                LoginButton.Clicked += LoginButton_Clicked;
                await DisplayAlert("Login", "Verifique sua Conexão", "OK");
            }
        }

        async private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            //await DisplayAlert("1", "!", "ok");
            CloseAllPopup();
        }
        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }



        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }

        protected override Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(1);
        }

        protected override Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }
    }
}