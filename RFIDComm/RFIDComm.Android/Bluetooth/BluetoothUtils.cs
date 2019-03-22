using Android.Bluetooth;
using System.Collections.ObjectModel;
using RFIDComm.Droid.Bluetooth;

namespace RFIDComm.Droid
{
    public class BluetoothUtils
    {
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


        public static ObservableCollection<string> GetPairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }
    }
}
