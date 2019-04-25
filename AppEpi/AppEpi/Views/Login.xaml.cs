using AppEpi.Models;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Login : ContentPage
    {
        private int contadorErroSenha = 0;
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            Carregar_Dados_Login();
        }

        private void Carregar_Dados_Login()
        {
            if (Application.Current.Properties.ContainsKey("Matricula") &&
                Application.Current.Properties.ContainsKey("Senha"))
            {
                entMatricula.Text = Application.Current.Properties["Matricula"] as string;
                entSenha.Text = Application.Current.Properties["Senha"] as string;
            }
        }

        async private void Salvar_Dados_Login()
        {
            if (Application.Current.Properties.ContainsKey("Matricula"))
                Application.Current.Properties["Matricula"] = entMatricula.Text;
            else
                Application.Current.Properties.Add("Matricula", entMatricula.Text);

            if (Application.Current.Properties.ContainsKey("Senha"))
                Application.Current.Properties["Senha"] = entSenha.Text;
            else
                Application.Current.Properties.Add("Senha", entSenha.Text);

            await Application.Current.SavePropertiesAsync();
        }


        async private void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoginButton.Clicked -= LoginButton_Clicked;
                if (contadorErroSenha == 3)
                {
                    var detailPage = new RegistroSenha();
                    await Navigation.PushModalAsync(detailPage);
                }

                if (entMatricula.Text != "" && entSenha.Text != "")
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.loginFunc(entMatricula.Text, entSenha.Text);

                    if (result.Find(x => x.Resultado == "OK") != null)
                    {
                        Salvar_Dados_Login();

                        UsuarioLogado.DadosUsuario = result;
                        LoginButton.Clicked += LoginButton_Clicked;
                        UsuarioLogado.Cnpj = result[0].Cnpj;
                        UsuarioLogado.FkCliente = result[0].FkCliente;
                        UsuarioLogado.MatriculaDistribuicao = entMatricula.Text;
                        UsuarioLogado.SenhaConfirmada = true;
                        NavigationPage.SetHasNavigationBar(this, false);
                        var navPage = new NavigationPage(new Splash());
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