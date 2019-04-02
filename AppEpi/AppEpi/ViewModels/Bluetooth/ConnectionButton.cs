using System;
using System.Data;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppEpi.ViewModels.Bluetooth
{
    public class ConnectionButton : AppEPIButton
    {
        private readonly IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        // Constructor
        public ConnectionButton() : base()
        {
            Clicked += new EventHandler(OnClicked);
        }


        private void OnClicked(object sender, EventArgs args)
        {
            Debug.WriteLine("--------------------OnClicked----------------");
            switch (_bluetoothController.CurrentState)
            {
                case ConnectionState.Closed:
                    Connect();
                    break;
                case ConnectionState.Open:
                    Disconnect();
                    break;
                case ConnectionState.Connecting:
                    Debug.WriteLine("Conexão em andamento... Click inválido.");
                    break;
                default:
                    IsVisible = false;
                    Debug.WriteLine("Solicitação inválida. " + ToString());
                    break;
            }
        }


        private void Connect()
        {
            string deviceName = ((Views.Bluetooth)ParentPage).SelectedDeviceName;
            _bluetoothController.Connect(deviceName);
        }


        private void Disconnect()
        {
            _bluetoothController.Disconnect();
        }
    }
}
