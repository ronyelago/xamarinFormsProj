using AppEpi.Models;
using AppEpi.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Page4 : ContentPage, IConfirmacao
    {
        private int countConfirmacao = 0;
        private ObservableCollection<DADOSEPI> items;

        public Page4(ObservableCollection<DADOSEPI> result)
        {
            InitializeComponent();

            items = result;
            listResultado.ItemsSource = items;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), TimerElapsed);
        }


        private bool TimerElapsed()
        {
            if (UsuarioLogado.Operacao == UsuarioLogado.Operacoes.Distribuicao)
            {
                btnSenha.IsVisible = false;
                btnConfirmar.IsVisible = true;
            }
            else
            {
                btnSenha.IsVisible = false;
                btnConfirmar.IsVisible = true;
            }
            return true;
        }


        private void Button_Clicked(object sender, EventArgs args)
        {
            var codigo = ((Button)sender).CommandParameter.ToString();
            countConfirmacao++;
            ((Button)sender).IsEnabled = false;
        }


        private async void Button_ClickedRemove(object sender, EventArgs args)
        {
            var codigo = ((Button)sender).CommandParameter.ToString();
            var rst = items.Where(x => x.EPC == codigo).ToList();

            items.Remove(rst.FirstOrDefault());

            if (items.Count <= 0)
            {
                await Navigation.PopAsync();
            }
        }


        async void IConfirmacao.OnConfirmarClicked()
        {
            if (countConfirmacao != items.Count)
            {
                await DisplayAlert("", "Confirme Todos os Itens!", "OK");
            }
            else
            {
                int count = 0;
                string listEPCS = "";

                foreach (var item in items)
                {
                    if (item.EPC != "")
                    {
                        count++;
                        listEPCS = listEPCS + "|" + item.EPC;
                    }
                }

                if (count <= 0)
                {
                    await DisplayAlert("", "Verifique os Campos!", "OK");
                }
                else
                {
                    var wbs = DependencyService.Get<IWEBClient>();
                    List<RESULTADOMOV> result = new List<RESULTADOMOV>();

                    switch (UsuarioLogado.Operacao)
                    {
                        case UsuarioLogado.Operacoes.Recebimento:
                            result = wbs.recebimentoEstoquesCnpj(listEPCS, UsuarioLogado.Cnpj);
                            break;
                        case UsuarioLogado.Operacoes.MovimentacaoEstoque:
                            result = wbs.movimentacaoEstoque(listEPCS, UsuarioLogado.LocalEstoque, UsuarioLogado.StatusEstoque);
                            break;
                        case UsuarioLogado.Operacoes.Distribuicao:
                            result = wbs.distribuicaoEPIS(listEPCS, UsuarioLogado.MatriculaDistribuicao);
                            break;
                        case UsuarioLogado.Operacoes.EnvioTeste:
                            result = wbs.envioParaTeste(listEPCS, UsuarioLogado.LocalEstoque);
                            break;
                        case UsuarioLogado.Operacoes.RecebimentoTeste:
                            result = wbs.recebimentoDoTeste(listEPCS, UsuarioLogado.DataTeste, UsuarioLogado.ART);
                            break;
                        case UsuarioLogado.Operacoes.Fiscalizacao:
                            result = wbs.inspecaoEPIFUNC(listEPCS, UsuarioLogado.Latitude, UsuarioLogado.Longitude);
                            break;
                        case UsuarioLogado.Operacoes.EnvioHigienizacao:
                            result = wbs.envioParaHigienizacao(listEPCS, UsuarioLogado.LocalEstoque);
                            break;
                        case UsuarioLogado.Operacoes.RecebimentoHigienizacao:
                            result = wbs.recebimentoDaHigienizacao(listEPCS);
                            break;
                        //case 10:
                            //INSPEÇÃO VISUAL
                            //var detailPage = new InspFiscalizacao();
                            //NavigationPage.SetBackButtonTitle(this, "Voltar");
                            //await Navigation.PushAsync(detailPage);
                            //break;
                    }

                    if (UsuarioLogado.Operacao != UsuarioLogado.Operacoes.Fiscalizacao)
                    {
                        var detailPage = new ResultadoTrn(result);

                        NavigationPage.SetBackButtonTitle(this, "Voltar");
                        await Navigation.PushAsync(detailPage);
                    }
                    else
                    {
                        var detailPage = new NaoConforme(result);

                        NavigationPage.SetBackButtonTitle(this, "Voltar");
                        await Navigation.PushAsync(detailPage);
                    }
                }
            }
        }


        private async void btnSenha_Clicked(object sender, EventArgs e)
        {
            string listEPCS = "";
            int count = 0;
            foreach (var item in items)
            {
                if (item.EPC != "")
                {
                    count++;
                    listEPCS = listEPCS + "|" + item.EPC;
                }
            }

            var ret = ValidarMatriculaSenha(listEPCS);
            if (ret)
            {
                var detailPage = new LoginPage();
                await Navigation.PushPopupAsync(detailPage);
            }
            else
            {
                var detailPage = new RegistroSenha();
                await Navigation.PushModalAsync(detailPage);
            }
        }


        private bool ValidarMatriculaSenha(string listEPCS)
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