using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class DescrtItem : ContentPage
    {
        public DescrtItem()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int coun = 0;

            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    coun++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (coun > 0)
            {
                if (edtMotivo.Text != "")
                {


                    var answer = await DisplayAlert("Descarte", "Confirmar Descarte?\nTotal de Itens:" + coun, "Sim", "Não");
                    if (answer)
                    {

                        var result = wbs.descartItem(listEPCS, edtMotivo.Text);
                        //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                        var detailPage = new ResultadoTrn(result);
                        await Navigation.PushAsync(detailPage);
                        //await Navigation.PushModalAsync(detailPage);
                        //PushModalAsync(detailPage);
                    }
                }
                else
                {
                    await DisplayAlert("Descarte", "Verifique os Campos!", "OK");
                }

            }
            else
            {
                await DisplayAlert("Descarte", "Verifique os Campos!", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";
            edtMotivo.Text = "";
        }
    }
}