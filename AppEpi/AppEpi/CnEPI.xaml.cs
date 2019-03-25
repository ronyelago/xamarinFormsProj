using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace AppEpi
{
    public partial class CnEPI : ContentPage
    {
        private ObservableCollection<string> _epcList = new ObservableCollection<string>();

        public CnEPI()
        {
            InitializeComponent();

            epcList.ItemsSource = _epcList;

            MessagingCenter.Subscribe<App, string>(this, "EPC", (sender, arg) =>
            {
                _epcList.Add(arg);
            });
        }


        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string WbsFormattedEpcList = "";
            int count = 0;

            

            //string[] lines = epis.Text.Split('\n');
            string[] epcList = new string[_epcList.Count];
            _epcList.CopyTo(epcList, 0);

            foreach (string epc in epcList)
            {
                if (epc != "")
                {
                    count++;
                    WbsFormattedEpcList += "|" + epc;
                }
            }

            if (count > 0)
            {
                var answer = await DisplayAlert("Consulta de Epi", "Confirmar Consulta?\nTotal de Itens:" + count, "Sim", "Não");
                if (answer)
                {
                    var result = wbs.consultEPI(WbsFormattedEpcList);
                    UsuarioLogado.Operacao = "7";
                    var detailPage = new ResultadoTrn(result);

                    NavigationPage.SetBackButtonTitle(this, "Voltar");
                    await Navigation.PushAsync(detailPage);
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
            _epcList.Clear();
        }
    }
}