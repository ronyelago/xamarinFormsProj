using AppEpi.Models;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class MAssinar : ContentPage
    {
        public MAssinar()
        {
            InitializeComponent();
            websrc.Source = "https://easyepi.com.br/homologacao/Assinatura.html?key=" + UsuarioLogado.ChaveDocumento + "&em=" + UsuarioLogado.emailAssinatura + "&nm=" + UsuarioLogado.FuncionarioAssinatura;
        }
    }
}