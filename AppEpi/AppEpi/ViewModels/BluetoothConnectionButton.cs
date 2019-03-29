using AppEpi.Views;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppEpi.ViewModels
{
    public class BluetoothConnectionButton : AppEPIButton
    {
        private readonly IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        // Constructor
        public BluetoothConnectionButton() : base()
        {
            Clicked += new EventHandler(OnClicked);

            MessagingCenter.Subscribe<App, ConnectionState>(this, "BLUETOOTH_STATE", (sender, arg) =>
            {
            });
        }


        private void OnClicked(object sender, EventArgs args)
        {   
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
            string deviceName = ((Bluetooth)ParentPage).SelectedDeviceName;
            DependencyService.Get<IBluetoothController>().Start(deviceName);
        }


        private void Disconnect()
        {
            _bluetoothController.Cancel();
        }
    }
}
