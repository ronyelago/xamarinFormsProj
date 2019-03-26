using System;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class EnvioTeste : ContentPage
    {
        public EnvioTeste()
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


        async private void btnEnvioTeste_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string localEstoque = "";

            if (pckLocalEstoque.SelectedIndex.ToString() == "-1")
            {
                localEstoque = "";
            }
            else
            {
                localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                localEstoque = localEstoque.Split('-')[0];
            }

            if (epcList.Count > 0)
            {
                var answer = await DisplayAlert("Envio Para Teste", "Confirmar Envio para Teste?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "4";
                    UsuarioLogado.LocalEstoque = localEstoque;

                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Envio Para Teste", "Verifique os Campos!", "OK");
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}