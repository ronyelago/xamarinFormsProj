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
    public partial class CadSenha : PopupPage
    {
        List<RESULTADOMOV> resultEnter;

        public CadSenha(List<RESULTADOMOV> result)
        {
            InitializeComponent();
            resultEnter = result;
            matricula.Text = resultEnter[0].EPC;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        async private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            if (entSenha.Text == entSenhaConf.Text)
            {
                
                var wbs = DependencyService.Get<IWEBClient>();
                var result = wbs.cadastSenha(entSenha.Text, resultEnter[0].EPC);
                if (result.Find(x => x.Resultado == "OK") != null)
                {
                    await DisplayAlert("Senha", "Senhas Registrada com Sucesso!", "OK");
                    await PopupNavigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Senha", "Funcionario=" + resultEnter[0].EPC + "\n " + result[0].Resultado, "OK");
                }
            }
            else
            {
                await DisplayAlert("Senha", "Senhas Diferentes!", "OK");
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