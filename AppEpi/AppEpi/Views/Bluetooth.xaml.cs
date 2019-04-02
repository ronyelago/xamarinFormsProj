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

        public string SelectedDeviceName { get => deviceList.SelectedItem.ToString(); }

        public Bluetooth()
        {
            InitializeComponent();
            
            MessagingCenter.Subscribe<App, ConnectionState>(this, "BLUETOOTH_STATE", (sender, arg) =>
            {
                UpdateLayout();
            });

            // Deixados aqui apenas para referência. É assim que se recebe esses eventos:
            MessagingCenter.Subscribe<App>(this, "RFID_LOWBATT", (sender) =>
                Debug.WriteLine("LOW BATTERY EVENT"));
            MessagingCenter.Subscribe<App>(this, "RFID_OVERHEAT", (sender) =>
                Debug.WriteLine("OVERHEAT EVENT"));
            MessagingCenter.Subscribe<App, string>(this, "RESPONSE_READERPOWER", (sender, arg) =>
                Debug.WriteLine("READER POWER = " + arg));

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

            UpdateChildrenLayout();
        }


        void OnSliderDragCompleted(object sender, EventArgs e)
        {
            float novaPotencia =
                _bluetoothController.SetReaderPower((int)potenciaSlider.Value);

            potenciaSlider.Value = novaPotencia;
        }


        private void OnItemSelected(object sender, EventArgs e)
        {
            btnConectar.IsEnabled = true;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateLayout();

            try
            {
                _listOfDevices = _bluetoothController.GetPairedDevices();

                // se houver dispositivos pareados, apresenta-se uma lista com os mesmos
                if (_listOfDevices.Count > 0)
                {
                    avisoNaoHaDispositivos.IsVisible = false;
                    deviceList.IsVisible = true;
                    deviceList.BeginRefresh();
                }
                else
                {
                    avisoNaoHaDispositivos.IsVisible = true;
                    deviceList.IsVisible = false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exceção no povoamento de dispositivos Bluetooth: " + e.Message);
            }
        }
    }
}