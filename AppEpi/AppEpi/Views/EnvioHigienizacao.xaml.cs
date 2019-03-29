using AppEpi.Models;
using AppEpi.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class EnvioHigienizacao : ContentPage, IConfirmacao
    {
        public EnvioHigienizacao()
        {
            InitializeComponent();

            var wbs = DependencyService.Get<IWEBClient>();
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


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0 || pckLocalEstoque.SelectedIndex < 0)
            {
                await DisplayAlert("Envio Para Higienização", "Verifique os Campos!", "OK");
            }
            else
            {
                string localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                localEstoque = localEstoque.Split('-')[0];

                var answer = await DisplayAlert("Envio Para Higienização", "Confirmar Envio para Higienização?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "8";
                    UsuarioLogado.LocalEstoque = localEstoque;
                    var detailPage = new Page4(result);

                    await Navigation.PushAsync(detailPage);
                }
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}