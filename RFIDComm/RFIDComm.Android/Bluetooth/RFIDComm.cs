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
                    Task.Run(async () =>
                        HandleEvent(response.Remove(0, _evtPrefix.Length))
                        );
                }
                else
                {
                    ProcessCommand(response);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION: " + e.Message);
            }
        }


        // Event Handler. Vide BRI Manual
        private static async Task HandleEvent(string eventMessage)
        {
            Debug.WriteLine("response: " + eventMessage);

            // Rodolfo Cortese: usado durante desenvolvimento do modulo apenas
            MessagingCenter.Send<App, string>((App)Application.Current, "Barcode", eventMessage);
            // remover depois!

            if (eventMessage.StartsWith(_epcPrefix)) // evento = novo EPC
            {
                BroadcastEPC(eventMessage.Remove(0, _epcPrefix.Length));
            }
            else if (eventMessage.Contains(_triggerPressEvt)) // evento = trigger pressed
            {
                BluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Fast);
            }
            else if (eventMessage.Contains(_triggerReleaseEvt)) // evento = trigger released
            {
                BluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Slow);
            }
            else
            {
                Debug.WriteLine("Handle other incoming event. Input: " + eventMessage);
            }
        }


        private static void BroadcastEPC(string epc)
        {
            Debug.WriteLine("Incoming EPC: " + epc);
            MessagingCenter.Send<App, string>((App)Application.Current, "EPC", epc);
        }


        // Command Processor. Vide BRI Manual
        private static void ProcessCommand(string command)
        {
            Debug.WriteLine("NonEvent response: " + command);
            throw new NotImplementedException();
        }
    }
}
