using Xamarin.Forms;

namespace AppEpi
{
    public partial class MAssinar : ContentPage
    {
        public MAssinar()
        {
            InitializeComponent();
            websrc.Source = "http://easyepi.com.br/homologacao/Assinatura.html?key=" + UsuarioLogado.ChaveDocumento + "&em=" + UsuarioLogado.emailAssinatura + "&nm=" + UsuarioLogado.FuncionarioAssinatura;
        }
    }
}