﻿using AppEpi.Models;
using AppEpi.ViewModels;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class MovimentacaoEstoque : ContentPage, IConfirmacao
    {
        public MovimentacaoEstoque()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();

            try
            {
                var wbs = DependencyService.Get<IWEBClient>();
                var result = wbs.retornaLocalEstoque().Where(x => x.FK_CLIENTE == UsuarioLogado.FkCliente).ToList();

                foreach (var rs in result)
                    pckLocalEstoque.Items.Add(rs.CODIGO + "-" + rs.NOME);
            }
            catch
            {
            }
        }


        async void IConfirmacao.OnConfirmarClicked()
        {

            if (epcList.Count <= 0 ||
                pckLocalEstoque.SelectedIndex < 0 ||
                pckEntradaSaida.SelectedIndex < 0)
            {
                await DisplayAlert("Movimentação de Estoque", "Verifique os Campos!", "OK");
            }
            else
            {
                string localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
                string entradaSaida = pckEntradaSaida.Items[pckEntradaSaida.SelectedIndex];

                var answer = await DisplayAlert("Movimentação de Estoque", "Confirmar Transação?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                if (answer)
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = UsuarioLogado.Operacoes.MovimentacaoEstoque;
                    UsuarioLogado.LocalEstoque = localEstoque.Split('-')[0];
                    UsuarioLogado.StatusEstoque = AbreviarStatus(entradaSaida);
                    var detailPage = new Page4(result);

                    await Navigation.PushAsync(detailPage);
                }
            }
        }


        private string AbreviarStatus(string entradaSaida)
        {
            switch (entradaSaida)
            {
                case "Entrada":
                    return "E";
                case "Saida":
                    return "S";
            }
            return "S";
        }
    }
}