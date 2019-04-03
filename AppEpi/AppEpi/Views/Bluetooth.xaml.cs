using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Bluetooth : ContentPage
    {
        private ObservableCollection<string> _listOfDevices;
        private readonly IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        public string SelectedDeviceName
        {
            get
            {
                try
                {
                    return devicesList.SelectedItem.ToString();
                }
                catch
                {   // quando não se tem nada selecionado, teriamos um NullPointerException
                    DisplayAlert("Aviso", "Selecione um dispositivo na lista!", "OK");
                    return null;
                }
            }
        }

        public Bluetooth()
        {
            InitializeComponent();

            _bluetoothController.ConnectionStateChanged += OnConnectionStateChanged;
        }


        private void OnConnectionStateChanged(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UpdateLayout();

                if (_bluetoothController.CurrentState == ConnectionState.Open)
                    DisplayAlert("Bluetooth", "Conexão efetuada com sucesso!", "OK");
            });
        }


        private void UpdateLayout()
        {
            switch (_bluetoothController.CurrentState)
            {
                case ConnectionState.Open:
                    controlePotencia.IsVisible = true;
                    btnConectar.Text = "Desconectar";
                    break;
                case ConnectionState.Closed:
                    controlePotencia.IsVisible = false;
                    btnConectar.Text = "Conectar";
                    break;
                case ConnectionState.Connecting:
                    controlePotencia.IsVisible = false;
                    btnConectar.Text = "Conectando...";
                    break;
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                _listOfDevices = _bluetoothController.PairedDevices;

                // se houver dispositivos pareados, apresenta-se uma lista com os mesmos
                if (_listOfDevices.Count > 0)
                {
                    avisoNaoHaDispositivos.IsVisible = false;
                    devicesList.IsVisible = true;
                    devicesList.BeginRefresh();
                }
                else
                {
                    avisoNaoHaDispositivos.IsVisible = true;
                    devicesList.IsVisible = false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exceção no povoamento de dispositivos Bluetooth: " + e.Message);
            }

            UpdateLayout();
        }
    }
}