using System;
using System.Collections.ObjectModel;
using System.Data;

namespace AppEpi
{
    public interface IBluetoothController
    {        
        void Init(bool readAsCharArray = true);
        void Cancel();
        void Connect(string deviceName = "");
        void Disconnect();
        /*
         * power é um inteiro de 25-100 (Vide BRI Manual)
         * O valor final definido não é exatamente igual ao solicitado, é rounded down to the nearest multiple of 5
         * Retorna: power setting efetivamente definido (25-100)
         */
        int ReaderPower { get; set; }
        ConnectionState CurrentState { get; }
        ObservableCollection<string> PairedDevices { get; }

        event EventHandler ConnectionStateChanged;
        event EventHandler LowBatteryWarning;
        event EventHandler OverheatingWarning;
    }
}
