using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RFIDComm
{
    public interface IBth
    {
        void Start(string name, int sleepTime, bool readAsCharArray);
        void Cancel();
        ObservableCollection<string> PairedDevices();
    }
}
