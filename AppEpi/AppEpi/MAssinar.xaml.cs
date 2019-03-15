using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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