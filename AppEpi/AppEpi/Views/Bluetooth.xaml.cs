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

            try
            {
                _listOfDevices = DependencyService.Get<IBluetoothController>().GetPairedDevices();
                deviceList.ItemsSource = _listOfDevices;

                deviceList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
                {
                    _selectedDeviceName = deviceList.SelectedItem.ToString();
                    btnConectar.IsVisible = true;
                };

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception no povoamento de dispositivos Bluetooth: " + e.Message);    
            }
        }


        async private void btnConectar_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IBluetoothController>().Start(_selectedDeviceName);
        }
    }
}