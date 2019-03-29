using AppEpi.Models;
using AppEpi.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Devolucao : ContentPage, IConfirmacao
    {
        public Devolucao()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
        {

            if (epcList.Count <= 0 || pckLocalEstoque.SelectedIndex < 0)
            {
                await DisplayAlert("Devolução", "Verifique os Campos!", "OK");
            }
            else
            {
                string localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                localEstoque = localEstoque.Split('-')[0];

                var answer = await DisplayAlert("Devolução", "Confirmar Devolução?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.devEPI(epcList.GetFormattedEpcList(), localEstoque);
                    var detailPage = new ResultadoTrn(result);

                    await Navigation.PushAsync(detailPage);
                }
            }
        }


        protected override void OnAppearing()
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