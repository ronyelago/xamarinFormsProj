﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnvioTeste : ContentPage
    {
        public EnvioTeste()
        {
            InitializeComponent();

            var wbs = DependencyService.Get<IWEBClient>();
            try
            {
                var result = wbs.retornaLocalEstoque().Where(x => x.FK_CLIENTE == UsuarioLogado.FkCliente).ToList();

                foreach (var rs in result)
                {
                    pckLocalEstoque.Items.Add(rs.CODIGO + "-" + rs.NOME);
                }
            }
            catch
            {

            }
        }

        async private void btnEnvioTeste_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int coun = 0;
            string localEstoque = "";
            string[] lines = epis.Text.Split('\n');


            foreach (string line in lines)
            {

                if (line != "")
                {
                    coun++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (pckLocalEstoque.SelectedIndex.ToString() == "-1")
            {
                localEstoque = "";
            }
            else
            {
                localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                localEstoque = localEstoque.Split('-')[0];
            }


            if (coun > 0)
            {
                var answer = await DisplayAlert("Envio Para Teste", "Confirmar Envio para Teste?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {

                    //var result = wbs.envioParaTeste(listEPCS, localEstoque);
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "4";
                    UsuarioLogado.LocalEstoque = localEstoque;

                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                    //await Navigation.PushModalAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Envio Para Teste", "Verifique os Campos!", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";
        }
    }
}