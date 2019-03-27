using System.Collections.ObjectModel;

namespace AppEpi
{
    public interface IBluetoothController
    {
        void Start(string deviceName, bool readAsCharArray = true);
        void Cancel();
        ObservableCollection<string> GetPairedDevices();
    }
}
