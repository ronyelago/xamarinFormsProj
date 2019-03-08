using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page4 : ContentPage
    {
        private int countConfirmacao = 0;
        private ObservableCollection<DADOSEPI> it;

        public Page4(ObservableCollection<DADOSEPI> result)
        {
            InitializeComponent();

            listResultado.ItemsSource = result;
            it = result;



            //foreach (var i in result)
            //{
            //    var tc = new TextCell();
            //    tc.TextColor = Color.Black;
            //    tc.Text = "EPC:" + i.EPC + "\n" + i.Resultado;
            //    tc.Detail = i.DataMovimentacao.ToLongDateString();
            //    tc.DetailColor = Color.FromHex("#313131");
            //    tbPedido.Add(tc);
            //}


        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), TimerElapsed);

            if (UsuarioLogado.Operacao == "3")
            {
                //if (!UsuarioLogado.SenhaConfirmadaEntrega)
                //{
                //    btnSenha.IsVisible = true;
                //    btnConfirmar.IsVisible = false;
                //}
                //else
                //{
                    btnSenha.IsVisible = false;
                    btnConfirmar.IsVisible = true;
                //}
            }
            else
            {
                btnSenha.IsVisible = false;
                btnConfirmar.IsVisible = true;
            }


        }

        private bool TimerElapsed()
        {
            if (UsuarioLogado.Operacao == "3")
            {
                //if (!UsuarioLogado.SenhaConfirmadaEntrega)
                //{
                //    btnSenha.IsVisible = true;
                //    btnConfirmar.IsVisible = false;
                //}
                //else
                //{
                    btnSenha.IsVisible = false;
                    btnConfirmar.IsVisible = true;
                //}
            }
            else
            {
                btnSenha.IsVisible = false;
                btnConfirmar.IsVisible = true;
            }

            return true;
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var codigo = item.CommandParameter.ToString();
            countConfirmacao++;
            item.IsEnabled = false;
        }

        async private void Button_ClickedRemove(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var codigo = item.CommandParameter.ToString();
            var rst = it.Where(x => x.EPC == codigo).ToList();
            it.Remove(rst[0]);
            listResultado.ItemsSource = it;
            if (it.Count == 0)
            {
                await Navigation.PopModalAsync();
            }
        }

        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            var wbs = DependencyService.Get<IWEBClient>();
            string listEPCS = "";
            int coun = 0;
            List<RESULTADOMOV> result = new List<RESULTADOMOV>();



            if (countConfirmacao == it.Count)
            {

                foreach (var item in it)
                {
                    if (item.EPC != "")
                    {
                        coun++;
                        listEPCS = listEPCS + "|" + item.EPC;
                    }
                }

                if (coun > 0)
                {
                    //var answer = await DisplayAlert("Recebimento", "Confirmar Recebimento?\nTotal de Itens:" + coun, "Sim", "Não");
                    //if (answer)
                    //{

                    switch (UsuarioLogado.Operacao)
                    {
                        case "1":
                            result = wbs.recebimentoEstoquesCnpj(listEPCS, UsuarioLogado.Cnpj);
                            break;
                        case "2":
                            result = wbs.movimentacaoEstoque(listEPCS, UsuarioLogado.LocalEstoque, UsuarioLogado.StatusEstoque);
                            break;
                        case "3":
                            result = wbs.distribuicaoEPIS(listEPCS, UsuarioLogado.MatriculaDistribuicao);
                            break;
                        case "4":
                            result = wbs.envioParaTeste(listEPCS, UsuarioLogado.LocalEstoque);
                            break;
                        case "5":
                            result = wbs.recebimentoDoTeste(listEPCS, UsuarioLogado.DataTeste, UsuarioLogado.ART);
                            break;
                        case "6":
                            result = wbs.inspecaoEPIFUNC(listEPCS, UsuarioLogado.Latitude, UsuarioLogado.Longitude);
                            break;
                        case "8":
                            //HIGIENIZACAO
                            result = wbs.envioParaHigienizacao(listEPCS, UsuarioLogado.LocalEstoque);
                            break;
                        case "9":
                            //HIGIENIZACAO
                            result = wbs.recebimentoDaHigienizacao(listEPCS);
                            break;
                        case "10":
                            //INSPEÇÃO VISUAL
                            //var detailPage = new InspFiscalizacao();
                            //NavigationPage.SetBackButtonTitle(this, "Voltar");
                            //await Navigation.PushAsync(detailPage);
                            break;
                    }

                    //var result = wbs.retornarDadosEpiValidar(listEPCS);
                    //await DisplayAlert("Recebimento", result.Count.ToString(), "OK");
                    if (UsuarioLogado.Operacao != "6")
                    {
                        var detailPage = new ResultadoTrn(result);
                        //await Navigation.PushModalAsync(detailPage);

                        NavigationPage.SetBackButtonTitle(this, "Voltar");
                        await Navigation.PushAsync(detailPage);


                    }
                    else
                    {
                        var detailPage = new NaoConforme(result);
                        //await Navigation.PushPopupAsync(detailPage);

                        NavigationPage.SetBackButtonTitle(this, "Voltar");
                        await Navigation.PushAsync(detailPage);
                    }
                    //}
                }
                else
                {
                    await DisplayAlert("", "Verifique os Campos!", "OK");
                }
            }
            else
            {
                await DisplayAlert("", "Confirme Todos os Itens!", "OK");
            }
        }

        private async void btnSenha_Clicked(object sender, EventArgs e)
        {
            string listEPCS = "";
            int coun = 0;
            foreach (var item in it)
            {
                if (item.EPC != "")
                {
                    coun++;
                    listEPCS = listEPCS + "|" + item.EPC;
                }
            }
            var ret = validarMatriculaSenha(listEPCS);
            if (ret)
            {
                var detailPage = new LoginPage();
                await Navigation.PushPopupAsync(detailPage);
            }
            else
            {
                var detailPage = new RegistrarSenha();
                await Navigation.PushModalAsync(detailPage);
            }
        }

        private bool validarMatriculaSenha(string listEPCS)
        {
            try
            {
                var wbs = DependencyService.Get<IWEBClient>();
                var ret = wbs.temSenha(listEPCS);
                return ret;
            }
            catch
            {
                return false;
            }

        }
    }
}