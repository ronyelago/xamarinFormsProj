using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using System.Diagnostics;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RFIDComm.Droid.Bluetooth
{
    class RFIDComm
    {
        private const string EvtPrefix = "EVT:";

        public static void HandleResponse(string response)
        {
            if (response.StartsWith(EvtPrefix))
            {
                response = response.Remove(0, EvtPrefix.Length);
                Debug.WriteLine("response = " + response);
            }
        }
    }
}