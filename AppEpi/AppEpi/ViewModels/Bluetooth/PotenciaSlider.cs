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

            DragCompleted += OnDragCompleted;
        }

        private void OnDragCompleted(object sender, EventArgs e)
        {
            float novaPotencia =
                _bluetoothController.SetReaderPower((int)Value);

            Value = novaPotencia;
        }
    }
}
