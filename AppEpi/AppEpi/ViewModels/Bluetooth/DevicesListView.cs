using Xamarin.Forms;

namespace AppEpi.ViewModels.Bluetooth
{
    class DevicesListView : ListView
    {
        private IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        // Constructor
        public DevicesListView()
        {
            ItemsSource = _bluetoothController.PairedDevices;
            VerticalOptions = LayoutOptions.StartAndExpand;

            Refreshing += OnRefreshing;
        }


        private void OnRefreshing(object sender, System.EventArgs e)
        {
            ItemsSource = _bluetoothController.PairedDevices;
            EndRefresh();
        }
    }
}
