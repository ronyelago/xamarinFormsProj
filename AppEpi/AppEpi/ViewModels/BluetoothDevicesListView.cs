using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    class BluetoothDevicesListView : ListView
    {
        private IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        // Constructor
        public BluetoothDevicesListView()
        {
            ItemsSource = _bluetoothController.GetPairedDevices();
            VerticalOptions = LayoutOptions.StartAndExpand;

            Refreshing += OnRefreshing;
        }


        private void OnRefreshing(object sender, System.EventArgs e)
        {
            ItemsSource = _bluetoothController.GetPairedDevices();
            EndRefresh();
        }
    }
}
