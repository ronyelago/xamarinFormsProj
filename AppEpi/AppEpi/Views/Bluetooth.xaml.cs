using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppEpi.Views
{
    public partial class Bluetooth : ContentPage
    {
        private ObservableCollection<string> _listOfDevices = new ObservableCollection<string>();

        private string _selectedDeviceName = null;
        public string SelectedDeviceName { get => deviceList.SelectedItem.ToString(); }

        public Bluetooth()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<App>(this, "RFID_LOWBATT", (sender) =>
            {
                Debug.WriteLine("LOW BATTERY EVENT");
            });
            MessagingCenter.Subscribe<App>(this, "RFID_OVERHEAT", (sender) =>
            {
                Debug.WriteLine("OVERHEAT EVENT");
            });
        }
        

        void OnSliderDragCompleted(object sender , EventArgs e)
        {
            float novaPotencia =
                DependencyService.Get<IBluetoothController>().SetReaderPower((int)potenciaSlider.Value);

            potenciaSlider.Value = novaPotencia;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            // povoa lista de dispositivos pareados
            try
            {
                _listOfDevices = DependencyService.Get<IBluetoothController>().GetPairedDevices();

                // se houver dispositivos pareados, apresenta-se uma lista com os mesmos
                if (_listOfDevices.Count > 0)
                {
                    avisoNaoHaDispositivos.IsVisible = false;
                    deviceList.IsVisible = true;

                    deviceList.ItemsSource = _listOfDevices;

                    deviceList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
                    {
                        btnConectar.IsEnabled = true;
                    };
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