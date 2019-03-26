using System;
using Xamarin.Forms;

namespace AppEpi.Views
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

            if (epcList.Count > 0)
            {
                if (edtMotivo.Text != "")
                {
                    var answer = await DisplayAlert("Descarte", "Confirmar Descarte?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                    if (answer)
                    {
                        var result = wbs.descartItem(epcList.GetFormattedEpcList(), edtMotivo.Text);
                        var detailPage = new ResultadoTrn(result);
                        await Navigation.PushAsync(detailPage);
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
            epcList.Clear();
            edtMotivo.Text = "";
        }
    }
}