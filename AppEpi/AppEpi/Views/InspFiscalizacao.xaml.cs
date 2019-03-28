using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class InspFiscalizacao : ContentPage
    {
        public InspFiscalizacao(string titulo)
        {
            InitializeComponent();
            Title = titulo;
        }


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("", "Inspeção realizado com sucesso!", "OK");
        }
    }
}