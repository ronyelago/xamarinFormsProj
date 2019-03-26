using System;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi
{
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();
        }


        async protected override void OnAppearing()
        {
            base.OnAppearing();
            var wbs = DependencyService.Get<IWEBClient>();
            try
            {
                epis.Text = "";
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


        async void EditorCompleted(object sender, EventArgs e)
        {
            // sender is cast to an Editor to enable reading the `Text` property of the view.
            var text = ((Editor)sender).Text; 
        }


        async void EditorTextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            bool confirmarMovimentacao = true;
            string localEstoque = "";
            string entradaSaida = "";
            string listEPCS = "";
            int coun = 0;
            if (epis.Text == "")
            {
                confirmarMovimentacao = false;
            }
            
            if (pckLocalEstoque.SelectedIndex.ToString() == "-1")
            {
                confirmarMovimentacao = false;
            }
            else
            {
                localEstoque = pckLocalEstoque.Items[pckLocalEstoque.SelectedIndex];
            }
            
            if (pckEntradaSaida.SelectedIndex.ToString() == "-1")
            {
                confirmarMovimentacao = false;
            }
            else
            {
                entradaSaida = pckEntradaSaida.Items[pckEntradaSaida.SelectedIndex];
            }

            string[] lines = epis.Text.Split('\n');
            foreach (string line in lines)
            {
                if (line != "")
                {
                    coun++;
                    listEPCS = listEPCS + "|" + line;
                }
            }

            if (confirmarMovimentacao)
            {
                var answer = await DisplayAlert("Movimentação de Estoque", "Confirmar Transação?\nTotal de Itens:" + coun, "Sim", "Não");
                if (answer)
                {
                    var result = wbs.retornarDadosEpiValidar(listEPCS, UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                    UsuarioLogado.Operacao = "2";
                    UsuarioLogado.LocalEstoque = localEstoque.Split('-')[0];
                    UsuarioLogado.StatusEstoque = abreviarStatus(entradaSaida);
                    var detailPage = new Page4(result);
                    await Navigation.PushAsync(detailPage);
                }
            }
            else
            {
                await DisplayAlert("Easy Epi", "Verifique os Campos!", "OK");
            }
        }


        private string abreviarStatus(string entradaSaida)
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