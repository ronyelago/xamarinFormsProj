using System;
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

            Value = Maximum;

            DragCompleted += OnDragCompleted;
        }

        private void OnDragCompleted(object sender, EventArgs e)
        {
            _bluetoothController.ReaderPower = (int)Value;
            Value = _bluetoothController.ReaderPower;
        }
    }
}
