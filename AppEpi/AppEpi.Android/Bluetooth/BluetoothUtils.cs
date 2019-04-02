using Android.Bluetooth;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppEpi.Droid.Bluetooth
{
    public class BluetoothUtils
    {
        private static int _deviceCyclingIndex = 0;

        // retorna BluetoothDevice pareado buscando por nome
        public static BluetoothDevice FindDevice(string name)
        {
            Debug.WriteLine("Try to connect to " + name);

            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            #region adapter debug
            if (adapter == null)
                Debug.WriteLine("No adapter found.");
            else
                Debug.WriteLine("Adapter found!");

            if (!adapter.IsEnabled)
                Debug.WriteLine("Bluetooth adapter is not enabled.");
            else
                Debug.WriteLine("Adapter enabled!");
            #endregion

            BluetoothDevice device = null;

            // se o nome for vazio, cicla-se o retorno por cada dispositivo encontrado
            if (name == "")
            {
                if (_deviceCyclingIndex >= adapter.BondedDevices.Count)
                    _deviceCyclingIndex = 0;

                device = adapter.BondedDevices.OfType<BluetoothDevice>().ElementAtOrDefault(_deviceCyclingIndex);
                _deviceCyclingIndex++;
                return device;
            }

            foreach (var bd in adapter.BondedDevices)
            {
                Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
                if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                {
                    Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
                    device = bd;
                    break;
                }
            }
            if (device == null)
                Debug.WriteLine("Named device not found.");

            return device;
        }
    }
}
