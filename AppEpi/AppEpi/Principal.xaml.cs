using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEpi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Principal : ContentPage
    {
        public string MyIp { get; private set; }

        public Principal()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                //loginUsuario();
                return false;
            });
        }

        async private void loginUsuario()
        {
            //foreach (IPAddress adress in Dns.GetHostAddresses(Dns.GetHostName()))
            //{
            //    MyIp = "IP Adress: " + adress.ToString();


            //}

            //await DisplayAlert("OK", MyIp, "ok");
            var detailPage = new LoginPage();
            await Navigation.PushPopupAsync(detailPage);
        }

        async private void splashApresentacao()
        {
            //foreach (IPAddress adress in Dns.GetHostAddresses(Dns.GetHostName()))
            //{
            //    MyIp = "IP Adress: " + adress.ToString();


            //}

            //await DisplayAlert("OK", MyIp, "ok");
            var detailPage = new LoginPage();
            await Navigation.PushPopupAsync(detailPage);
        }

        private void verificarCadastro()
        {
            try
            {
                var loadArquivo = DependencyService.Get<ISaveAndLoad>().LoadText("temp.txt");
                if (loadArquivo != null)
                {
                    UsuarioLogado.TemCadastro = true;
                    UsuarioLogado.Codigo = separarDados(loadArquivo.Split(':')[1]);
                    UsuarioLogado.Nome = separarDados(loadArquivo.Split(':')[2]);
                    UsuarioLogado.Numero = separarDados(loadArquivo.Split(':')[3]);
                    UsuarioLogado.Telefone = separarDados(loadArquivo.Split(':')[4]);
                    UsuarioLogado.Cep = separarDados(loadArquivo.Split(':')[5]);
                    UsuarioLogado.Email = separarDados(loadArquivo.Split(':')[6]);
                    UsuarioLogado.Logradouro = separarDados(loadArquivo.Split(':')[7]);
                    try
                    {
                        UsuarioLogado.Bairro = separarDados(loadArquivo.Split(':')[8]);
                    }
                    catch
                    {
                        UsuarioLogado.Bairro = "";
                    }


                    //var wbs = DependencyService.Get<IWEBClient>();
                    //var result = wbs.retornaFuncionamento();
                    //var jsonFuncionamento = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    //DependencyService.Get<ISaveAndLoad>().SaveText("funcionamento.json", jsonFuncionamento);



                    //var resultBairro = wbs.retornaBairroAtendimento();
                    //var jsonBairroAtend = Newtonsoft.Json.JsonConvert.SerializeObject(resultBairro);
                    //DependencyService.Get<ISaveAndLoad>().SaveText("bairrosAtendimento.json", jsonBairroAtend);

                    //var retEndEntrApp = DependencyService.Get<ISaveAndLoad>().LoadText("enderecoEntrega.json");
                    //List<ENDERECO_ENTREGA_APP> endEntrega = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ENDERECO_ENTREGA_APP>>(retEndEntrApp);
                    //if (endEntrega.Count == 0)
                    //{
                    //    ENDERECO_ENTREGA_APP eea = new ENDERECO_ENTREGA_APP();
                    //    eea.CEP = UsuarioLogado.Cep;
                    //    eea.CODIGO = UsuarioLogado.Codigo;
                    //    eea.COMPLEMENTO = UsuarioLogado.Complemento;
                    //    eea.LOGRADOURO = UsuarioLogado.Logradouro;
                    //    eea.NUMERO = UsuarioLogado.Numero;
                    //    eea.TIPO = "P";
                    //    eea.Bairro = UsuarioLogado.Bairro;
                    //    eea.gravarEnderecoEntrega();

                    //}

                    //var hoje = wbs.hojePorExtenso();
                    //UsuarioLogado.HojePorExtenso = hoje;
                }
                else
                {
                    UsuarioLogado.TemCadastro = false;
                }

            }
            catch
            {
                UsuarioLogado.TemCadastro = false;
            }



        }

        private string separarDados(string v)
        {
            try
            {
                return v.Split('|')[0];
            }
            catch
            {
                return "";
            }
        }

        async void btnPrincipalFacaPedido(object sender, EventArgs e)
        {
            //var newPage = new Menu();
            //await Navigation.PushAsync(newPage);
        }
        
    }
}