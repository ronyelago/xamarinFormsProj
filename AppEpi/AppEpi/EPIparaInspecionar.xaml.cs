using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class EPIparaInspecionar : ContentPage
    {
        private int countConfirmacao = 0;
        private ObservableCollection<DADOSEPI> it;
        public EPIparaInspecionar(ObservableCollection<DADOSEPI> result)
        {
            InitializeComponent();
            listResultado.ItemsSource = result;
            it = result;
        }

       async private void Button_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var codigo = item.CommandParameter.ToString();
            countConfirmacao++;
            item.IsEnabled = false;
            var result = it.Where(x => x.EPC == codigo).ToList();
            string nomeProduto = result[0].Produto;
            //await DisplayAlert("ACHEI", nomeProduto, "OK");
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