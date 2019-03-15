﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class RecebimentoItens : ContentPage
    {
        public RecebimentoItens()
        {
            InitializeComponent();
        }


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int count = 0;

            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    count++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (count > 0)
            {
                var answer = await DisplayAlert("Recebimento", "Confirmar Recebimento?\nTotal de Itens:" + count, "Sim", "Não");
                if (answer)
                {

                    //var result = wbs.recebimentoEstoques(listEPCS);
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "1";
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Recebimento", "Verifique os Campos!", "OK");
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            epis.Text = "";
        }
    }
}