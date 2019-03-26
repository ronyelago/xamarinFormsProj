using System;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class DevolucaoEpi : ContentPage
    {
        public DevolucaoEpi()
        {
            InitializeComponent();
        }


        async private void Button_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string localEstoque = "";

            if (epcList.Count > 0)
            {
                if (pckLocalEstoque.SelectedIndex.ToString() == "-1")
                {
                    localEstoque = "";
                }
                else
                {
                    localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                    localEstoque = localEstoque.Split('-')[0];
                }
                
                var answer = await DisplayAlert("Devolução", "Confirmar Devolução?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {

                    var result = wbs.devEPI(epcList.GetFormattedEpcList(), localEstoque);
                    var detailPage = new ResultadoTrn(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Devolução", "Verifique os Campos!", "OK");
            }
        }


        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var wbs = DependencyService.Get<IWEBClient>();
            epcList.Clear();
            try
            {
                var result = wbs.retornaLocalEstoque().Where(x => x.FK_CLIENTE == UsuarioLogado.FkCliente).ToList();

                foreach (var rs in result)
                {
                    pckLocalEstoque.Items.Add(rs.CODIGO + "-" + rs.NOME);
                }
            }
            catch
            {
            }
        }
    }
}