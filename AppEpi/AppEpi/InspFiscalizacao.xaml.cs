using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class InspFiscalizacao : ContentPage
    {
        public InspFiscalizacao(string titulo)
        {
            InitializeComponent();
            Title = titulo;
        }

        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("", "Inspeção realizado com sucesso!", "OK");
        }
    }
}