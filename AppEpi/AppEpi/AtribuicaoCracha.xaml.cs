using System;
using Xamarin.Forms;

namespace AppEpi
{
    public partial class AtribuicaoCracha : ContentPage
    {
        public AtribuicaoCracha()
        {
            InitializeComponent();
        }


        async private void btnAtribuir_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            btnAtribuir.Clicked -= btnAtribuir_Clicked;
            if (entMatricula.Text != "" && entCracha.Text != "")
            {
                var answer = await DisplayAlert("Atribuição de Cracha", "Deseja Confirmar Atribuição?", "Sim", "Não");
                if (answer)
                {
                    var result = wbs.atribuicaoCrachar(entMatricula.Text, entCracha.Text);
                    btnAtribuir.Clicked += btnAtribuir_Clicked;
                    UsuarioLogado.Operacao = "0";
                    var detailPage = new ResultadoTrn(result);
                    await Navigation.PushAsync(detailPage);
                }
                else
                {
                    btnAtribuir.Clicked += btnAtribuir_Clicked;
                }
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            entMatricula.Text = ""; entCracha.Text = "";
        }
    }
}