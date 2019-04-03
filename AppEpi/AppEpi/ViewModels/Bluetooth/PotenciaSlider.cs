using System;
using System.Data;
using Xamarin.Forms;

namespace AppEpi.ViewModels.Bluetooth
{
    class PotenciaSlider : Slider
    {
        private IBluetoothController _bluetoothController = DependencyService.Get<IBluetoothController>();

        // Constructor
        public PotenciaSlider()
        {
            Maximum = 100;
            Minimum = 25;
            MinimumTrackColor = Color.Blue;
            MaximumTrackColor = Color.Gray;

            Value = _bluetoothController.ReaderPower;

            DragCompleted += OnDragCompleted;
            _bluetoothController.ConnectionStateChanged += OnConnectionStateChanged;
            OnConnectionStateChanged(this, null);
        }

        private void OnDragCompleted(object sender, EventArgs e)
        {
            _bluetoothController.ReaderPower = (int)Value;
            Value = _bluetoothController.ReaderPower;
        }

        private void OnConnectionStateChanged(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_bluetoothController.CurrentState == ConnectionState.Open)
                    IsEnabled = true;
                else
                    IsEnabled = false;
            });
        }
    }
}
