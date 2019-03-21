using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using System.Diagnostics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RFIDComm.Droid.Bluetooth
{
    class RFIDComm
    {
        private const string _evtPrefix = "EVT: ";
        private const string _epcPrefix = "EPC: ";
        private const string _triggerPressEvt = "TRIGGER PRESS";
        private const string _triggerReleaseEvt = "TRIGGER RELEASE";

        public static void HandleResponse(string response)
        {
            try
            {
                if (response.StartsWith(_evtPrefix))
                {
                    string evtResponse = response.Remove(0, _evtPrefix.Length);

                    Task.Run(async () =>
                        HandleEvent(evtResponse)
                        );
                }
                else
                {
                    Debug.WriteLine("NonEvent response: " + response);
                    throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION: " + e.Message);
            }
        }


        private static async Task HandleEvent(string evtResponse)
        {
            Debug.WriteLine("response: " + evtResponse);

            // Rodolfo Cortese: usado durante desenvolvimento do modulo apenas
            MessagingCenter.Send<App, string>((App)Application.Current, "Barcode", evtResponse);
            // remover depois!
            
            if (evtResponse.StartsWith(_epcPrefix)) // evento = novo EPC
            {
                string epc = evtResponse.Remove(0, _epcPrefix.Length);
                Debug.WriteLine("Incoming EPC: " + evtResponse);
                MessagingCenter.Send<App, string>((App)Application.Current, "EPC", epc);
            }
            else
            {
                if (evtResponse.Contains(_triggerPressEvt)) // evento = trigger pressed
                {
                    BluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Fast);
                }
                else if (evtResponse.Contains(_triggerReleaseEvt)) // evento = trigger released
                {
                    BluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Slow);
                }
                else
                {
                    Debug.WriteLine("Handle other incoming event. Input: " + evtResponse);
                }
            }
        }
    }
}
