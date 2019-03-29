using AppEpi.ViewModels;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Descarte : ContentPage, IConfirmacao
    {
        public Descarte()
        {
            InitializeComponent();
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (epcList.Count <= 0 || edtMotivo.Text == "")
            {
                await DisplayAlert("Descarte", "Verifique os Campos!", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Descarte", "Confirmar Descarte?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.descartItem(epcList.GetFormattedEpcList(), edtMotivo.Text);
                    var detailPage = new ResultadoTrn(result);

                    await Navigation.PushAsync(detailPage);
                }
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