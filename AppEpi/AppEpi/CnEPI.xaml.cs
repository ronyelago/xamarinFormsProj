using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class CnEPI : ContentPage
    {
        public CnEPI()
        {
            InitializeComponent();
        }


        async private void btnConfirmar_Clicked(object sender, EventArgs e)
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
                var answer = await DisplayAlert("Consulta de Epi", "Confirmar Consulta?\nTotal de Itens:" + count, "Sim", "Não");
                if (answer)
                {

                    //var result = wbs.inspecaoEPIFUNC(listEPCS);
                    var result = wbs.consultEPI(listEPCS);
                    UsuarioLogado.Operacao = "7";
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new ResultadoTrn(result);
                    //await Navigation.PushModalAsync(detailPage);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    //await Navigation.PushModalAsync(detailPage);
                    await Navigation.PushAsync(detailPage);



                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    //var detailPage = new NaoConforme(result);
                    //await Navigation.PushPopupAsync(detailPage);
                    //PushModalAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Consulta de Epi", "Verifique os Campos!", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";

        }
    }
}