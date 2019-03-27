using AppEpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class ResultadoTrn : ContentPage
    {
        private List<RESULTADOMOV> _chave;
        private List<RESULTADOMOV> _it;
        private List<RESULTADOMOV> _temChave;


        public ResultadoTrn(List<RESULTADOMOV> result)
        {
            InitializeComponent();
            
            _temChave = result.Where(x => x.Produto == "chave").ToList();
            List<RESULTADOMOV> l = new List<RESULTADOMOV>();

            foreach(var i in result)
            {
                if (i.Produto != "chave")
                {
                    l.Add(new RESULTADOMOV
                    {
                        corAviso = i.corAviso,
                        DataMovimentacao = i.DataMovimentacao,
                        EPC = i.EPC,
                        Produto = i.Produto,
                        Resultado = i.Resultado
                    });
                }
            }

            listResultado.ItemsSource = l;
            _it = result;
        }


        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var result = _it.Where(x => x.corAviso == "#ff7f7f").ToList();
            bool erro = false;
            if (result != null)
            {
                if (result.Count > 0)
                {
                    erro = true;
                    await DisplayAlert("Resultado", "Existe itens em Inconformidades\nQtd Itens:" + result.Count, "OK");
                }
            }

            _chave = _temChave;

            if(UsuarioLogado.Operacao == "3")
            {
                if (!erro)
                {
                    btnAssinar.IsVisible = true;
                }
                else
                {
                    btnAssinar.IsVisible = false;
                }
            }
            else
            {
                btnAssinar.IsVisible = false;
            }
        }


        async private void btnAssinar_Clicked(object sender, EventArgs e)
        {
            string key = _chave[0].EPC;
            UsuarioLogado.ChaveDocumento = key;
            UsuarioLogado.emailAssinatura = _chave[0].Resultado.Split('|')[1];
            UsuarioLogado.FuncionarioAssinatura = "Funcionario";
            var detailPage = new MAssinar();
            NavigationPage.SetBackButtonTitle(this, "Voltar");
            await Navigation.PushAsync(detailPage);
        }
    }
}