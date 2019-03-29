using AppEpi.Models;
using AppEpi.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class CadastroSenha : PopupPage, IConfirmacao
    {
        List<RESULTADOMOV> resultEnter;

        public CadastroSenha(List<RESULTADOMOV> result)
        {
            InitializeComponent();
            resultEnter = result;
            matricula.Text = resultEnter[0].EPC;
        }


        async void IConfirmacao.OnConfirmarClicked()
        {

            if (entSenha.Text != entSenhaConf.Text)
            {
                await DisplayAlert("Senha", "Senhas Diferentes!", "OK");
            }
            else
            {
                var wbs = DependencyService.Get<IWEBClient>();
                var result = wbs.cadastSenha(entSenha.Text, resultEnter[0].EPC);
                var teste = result.Find(x => x.Resultado == "OK");
                if (result.Find(x => x.Resultado == "OK") != null) // inner update error acontecendo nessa linha
                {
                    await DisplayAlert("Senha", "Senha Registrada com Sucesso!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Senha", "Funcionario=" + resultEnter[0].EPC + "\n " + result[0].Resultado, "OK");
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