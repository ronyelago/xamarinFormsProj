using System.Collections.ObjectModel;

namespace AppEpi
{
    public interface IBluetoothController
    {
        void Start(string deviceName, bool readAsCharArray = true);
        void Cancel();
        /*
         * power é um inteiro de 25-100 (Vide BRI Manual)
         * O valor final definido não é exatamente igual ao solicitado, é rounded down to the nearest multiple of 5
         * Retorna: power setting efetivamente definido (25-100)
         */
        int SetReaderPower(int power);
        ObservableCollection<string> GetPairedDevices();
    }
}
