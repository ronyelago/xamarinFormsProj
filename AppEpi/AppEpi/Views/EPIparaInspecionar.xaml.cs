using AppEpi.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class EPIparaInspecionar : ContentPage
    {
        private ObservableCollection<DADOSEPI> items;
        public EPIparaInspecionar(ObservableCollection<DADOSEPI> result)
        {
            InitializeComponent();
            listResultado.ItemsSource = result;
            items = result;
        }


       private async void Button_Clicked(object sender, EventArgs e)
        {
            var item = (Button)sender;
            var codigo = item.CommandParameter.ToString();
            item.IsEnabled = false;
            var result = items.Where(x => x.EPC == codigo).ToList();
            string nomeProduto = result[0].Produto;
            foreach (var i in UsuarioLogado.categoriaInspecao)
            {
                var UPP = i.ToUpper();
                if (nomeProduto.IndexOf(UPP) != -1)
                {
                    var detailPage = new InspFiscalizacao(UPP);
                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);
                }
            }
        }


        private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
        }
    }
}