using System.Collections.ObjectModel;

namespace AppEpi
{
    public interface IBluetoothController
    {
        void Start(string name, bool readAsCharArray = true);
        void Cancel();
        void SendCommand(string command);
        ObservableCollection<string> GetPairedDevices();
    }
}
