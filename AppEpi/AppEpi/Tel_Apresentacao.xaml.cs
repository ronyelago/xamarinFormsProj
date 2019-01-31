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
    public partial class Tel_Apresentacao : ContentPage
    {
        public Tel_Apresentacao()
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