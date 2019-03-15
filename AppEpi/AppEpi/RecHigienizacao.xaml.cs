using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class RecHigienizacao : ContentPage
    {
        public RecHigienizacao()
        {
            InitializeComponent();
        }

        async private void btnEnvioTeste_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int count = 0;

            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    count++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (count > 0)
            {

                var answer = await DisplayAlert("Recebimento da Higienização", "Confirmar Recebimento?\nTotal de Itens:" + count, "Sim", "Não");
                if (answer)
                {
                    
                    //var result = wbs.recebimentoDoTeste(listEPCS, data, entART.Text);
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "9";
                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Recebimento da Higienização", "Verifique os Campos!", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";

        }
    }
}