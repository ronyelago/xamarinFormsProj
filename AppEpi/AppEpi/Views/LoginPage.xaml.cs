﻿using System;
using AppEpi.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class LoginPage : PopupPage
    {
        private int contadorErroSenha = 0;

        public LoginPage()
        {
            InitializeComponent();
        }


        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoginButton.Clicked -= LoginButton_Clicked;
                if (contadorErroSenha == 3)
                {
                    CloseAllPopup();
                    var detailPage = new RegistroSenha();
                    await Navigation.PushModalAsync(detailPage);
                }

                if (entMatricula.Text != "" && entSenha.Text != "")
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.loginFunc(entMatricula.Text, entSenha.Text);

                    if (result.Find(x => x.Resultado == "OK") != null)
                    {
                        LoginButton.Clicked += LoginButton_Clicked;
                        UsuarioLogado.MatriculaDistribuicao = entMatricula.Text;
                        UsuarioLogado.SenhaConfirmada = true;
                        UsuarioLogado.Cnpj = result[0].Cnpj;
                        UsuarioLogado.FkCliente = result[0].FkCliente;
                        CloseAllPopup();
                    }
                    else
                    {
                        LoginButton.Clicked += LoginButton_Clicked;
                        UsuarioLogado.SenhaConfirmada = false;
                        await DisplayAlert("Login", result[0].Resultado, "OK");
                        contadorErroSenha++;
                    }
                }
                else
                {
                    LoginButton.Clicked += LoginButton_Clicked;
                }
            }
            catch
            {
                LoginButton.Clicked += LoginButton_Clicked;
                await DisplayAlert("Login", "Verifique sua Conexão", "OK");
            }
        }


        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }


        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
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