using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RFIDComm
{
    public interface IBth
    {
        void Start(string name, int pollingTime, bool readAsCharArray);
        void Cancel();
        void SendMessage(string message);
        ObservableCollection<string> PairedDevices();
    }
}
