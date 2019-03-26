using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Splash : ContentPage
    {
        public Splash()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (UsuarioLogado.DadosUsuario!= null && UsuarioLogado.DadosUsuario.Count > 0)
            {
                lblEmpresa.Text = UsuarioLogado.DadosUsuario[0].Empresa;
                lblNome.Text = UsuarioLogado.DadosUsuario[0].Nome;
            }

            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                fechaSplash();
                return false;
            });
        }


        async private void fechaSplash()
        {
            var navPage = new NavigationPage(new MainPage());
            Application.Current.MainPage = navPage;
        }
    }
}