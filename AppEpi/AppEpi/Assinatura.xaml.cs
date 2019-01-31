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
    public partial class Assinatura : ContentPage
    {
        public Assinatura()
        {
            //Browser.Source = "http://easyepi.com.br/homologacao/Assinatura.html?key=" + UsuarioLogado.ChaveDocumento + "&em=" + UsuarioLogado.emailAssinatura + "&nm=" + UsuarioLogado.FuncionarioAssinatura;
        }
    }
}