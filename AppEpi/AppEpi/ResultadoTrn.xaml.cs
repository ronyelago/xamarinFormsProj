using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    public partial class ResultadoTrn : ContentPage
    {
        private List<RESULTADOMOV> chave;
        private List<RESULTADOMOV> it;
        private List<RESULTADOMOV> temChave;

        public ResultadoTrn(List<RESULTADOMOV> result)
        {
            InitializeComponent();

            
            temChave = result.Where(x => x.Produto == "chave").ToList();
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
            it = result;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var result = it.Where(x => x.corAviso == "#ff7f7f").ToList();
            bool erro = false;
            if (result != null)
            {
                if (result.Count > 0)
                {
                    erro = true;
                    await DisplayAlert("Resultado", "Existe itens em Inconformidades\nQtd Itens:" + result.Count, "OK");

                }
            }

            chave = temChave;

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
            string key = chave[0].EPC;
            UsuarioLogado.ChaveDocumento = key;
            UsuarioLogado.emailAssinatura = chave[0].Resultado.Split('|')[1];
            UsuarioLogado.FuncionarioAssinatura = "Funcionario";
            //App.Current.MainPage = new NavigationPage(new MAssinar());
            var detailPage = new MAssinar();
            NavigationPage.SetBackButtonTitle(this, "Voltar");
            await Navigation.PushAsync(detailPage);
            //Navigation.PushAsync(detailPage);
        }
    }
}