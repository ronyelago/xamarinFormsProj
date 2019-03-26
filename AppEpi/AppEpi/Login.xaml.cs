using System;
using Xamarin.Forms;

namespace AppEpi
{
    public partial class Login : ContentPage
    {
        private int contadorErroSenha = 0;
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }


        async private void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoginButton.Clicked -= LoginButton_Clicked;
                if (contadorErroSenha == 3)
                {
                    var detailPage = new RegistrarSenha();
                    await Navigation.PushModalAsync(detailPage);
                }

                if (entMatricula.Text != "" && entSenha.Text != "")
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.loginFunc(entMatricula.Text, entSenha.Text);

                    if (result.Find(x => x.Resultado == "OK") != null)
                    {
                        UsuarioLogado.DadosUsuario = result;
                        LoginButton.Clicked += LoginButton_Clicked;
                        UsuarioLogado.Cnpj = result[0].Cnpj;
                        UsuarioLogado.FkCliente = result[0].FkCliente;
                        UsuarioLogado.MatriculaDistribuicao = entMatricula.Text;
                        UsuarioLogado.SenhaConfirmada = true;
                        NavigationPage.SetHasNavigationBar(this, false);
                        var navPage = new NavigationPage(new Tel_Apresentacao());
                        Application.Current.MainPage = navPage;
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
    }
}