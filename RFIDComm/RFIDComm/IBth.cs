using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RFIDComm
{
    public interface IBth
    {
        void Start(string name, int pollingTime, bool readAsCharArray = true);
        void Cancel();
        void SendCommand(string command);
        ObservableCollection<string> PairedDevices();
    }
}
