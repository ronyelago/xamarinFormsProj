using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class EnviarNaoConforme : PopupPage
    {
        public EnviarNaoConforme(string EPC)
        {
            InitializeComponent();
            epc.Text = EPC;
        }


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            if (epis.Text != "")
            {
                var result = wbs.itemNaoConforme(epc.Text, epis.Text);
                var FND = result.Find(x => x.Resultado == "OK");
                if (FND != null)
                {
                    await DisplayAlert("Não Conforme", "Realizado com Sucesso", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Não Conforme", FND.Resultado, "OK");
                    await Navigation.PopAsync();
                }
            }
        }


        private void OnClose(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearingAnimationEnd()
        {
            return;// Content.FadeTo(1);
        }

        protected override void OnDisappearingAnimationBegin()
        {
            return;// Content.FadeTo(1);
        }
    }
}