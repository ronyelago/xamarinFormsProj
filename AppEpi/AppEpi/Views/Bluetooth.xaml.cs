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

        public Bluetooth()
        {
            InitializeComponent();

            // povoa lista de dispositivos pareados
            try
            {
                _listOfDevices = DependencyService.Get<IBluetoothController>().GetPairedDevices();
                deviceList.ItemsSource = _listOfDevices;

                deviceList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
                {
                    _selectedDeviceName = deviceList.SelectedItem.ToString();
                    btnConectar.IsEnabled = true;
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exceção no povoamento de dispositivos Bluetooth: " + e.Message);    
            }
        }


        private void btnConectar_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IBluetoothController>().Start(_selectedDeviceName);
        }


        void OnSliderDragCompleted(object sender , EventArgs e)
        {
            float novaPotencia =
                DependencyService.Get<IBluetoothController>().SetReaderPower((int)potenciaSlider.Value);

            potenciaSlider.Value = novaPotencia;

            Debug.WriteLine("NEW VALUE = " + potenciaSlider.Value);
        } 
    }
}