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
    public partial class ManEpi : ContentPage
    {
        public ManEpi()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var wbs = DependencyService.Get<IWEBClient>();
            epis.Text = "";
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

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            string resultados = "";
            int coun = 0;
            bool confirmarMovimentacao = true;
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

            if (coun > 0)
            {
                if (pckLocalEstoque.SelectedIndex.ToString() == "-1")
                {
                    confirmarMovimentacao = false;
                    localEstoque = "";
                }
                else
                {
                    localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                    localEstoque = localEstoque.Split('-')[0];
                }


                var answer = await DisplayAlert("Manutenção", "Confirmar Manutenção?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {

                    var result = wbs.manutencaoEPIS(listEPCS, localEstoque);
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    var detailPage = new ResultadoTrn(result);
                    await Navigation.PushAsync(detailPage);
                    //await Navigation.PushModalAsync(detailPage);
                    //PushModalAsync(detailPage);
                }

            }
            else
            {
                await DisplayAlert("Manutenção", "Verifique os Campos!", "OK");
            }
        }
    }
}