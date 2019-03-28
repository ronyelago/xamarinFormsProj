using AppEpi.Models;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Fiscalizacao : ContentPage
    {
        public Fiscalizacao()
        {
            InitializeComponent();

            Task.Run(() =>
                RequestPermission(Permission.Location)
                );
        }


        // Solicita a permissão desejada e retorna o estado da mesma após interação do usuário
        private async Task<PermissionStatus> RequestPermission(Permission permission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(permission))
                        status = results[permission];
                }
                return status;
            }
            catch (Exception e)
            {
                Debug.WriteLine("RequestPermission Exception:" + e.Message);
                return PermissionStatus.Unknown;
            }
        }


        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (epcList.Count <= 0)
                {
                    await DisplayAlert("Fiscalização", "Verifique os Campos!", "OK");
                }
                else
                {
                    var location = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
                    UsuarioLogado.Latitude = location.Latitude.ToString();
                    UsuarioLogado.Longitude = location.Longitude.ToString();

                    var answer = await DisplayAlert("Fiscalização", "Confirmar Fiscalização?\nTotal de Itens:" + epcList.Count, "Sim", "Não");
                    if (answer)
                    {
                        var wbs = DependencyService.Get<IWEBClient>();
                        var result = wbs.retornarDadosEpiValidar(epcList.GetFormattedEpcList(), UsuarioLogado.Cnpj, UsuarioLogado.FkCliente);
                        UsuarioLogado.Operacao = "6";
                        var detailPage = new Page4(result);
                        NavigationPage.SetBackButtonTitle(this, "Voltar");

                        await Navigation.PushAsync(detailPage);

                    }
                }
            }
            catch (Plugin.Geolocator.Abstractions.GeolocationException ex)
            {
                Debug.WriteLine("Unauthorized Exception:" + ex.Message);
                await DisplayAlert("Erro: Sem permissão", "Não é possível prosseguir sem permissão de acesso a localização!", "OK");
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            epcList.Clear();
        }
    }
}