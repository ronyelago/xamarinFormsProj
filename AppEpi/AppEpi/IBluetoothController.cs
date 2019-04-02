using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace AppEpi
{
    public interface IBluetoothController
    {
        ConnectionState CurrentState { get; }
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

        ObservableCollection<string> GetPairedDevices();
    }
}
