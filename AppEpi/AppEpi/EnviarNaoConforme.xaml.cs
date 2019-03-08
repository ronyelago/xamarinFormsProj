using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnviarNaoConforme : PopupPage
    {
        public EnviarNaoConforme(string EPC)
        {
            InitializeComponent();
            epc.Text = EPC;
        }

        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            if (epis.Text != "")
            {
                var result = wbs.itemNaoConforme(epc.Text, epis.Text);
                var FND = result.Find(x => x.Resultado == "OK");
                if (FND != null)
                {
                    await DisplayAlert("Não Conforme", "Realizado com Sucesso", "OK");
                    await PopupNavigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Não Conforme", FND.Resultado, "OK");
                    await PopupNavigation.PopAsync();
                }
            }
        }


        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
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