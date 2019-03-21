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
using static RFIDComm.Droid.BluetoothController;

namespace RFIDComm.Droid.Bluetooth
{
    class RFIDComm
    {
        private const string _evtPrefix = "EVT:";
        private const string _epcPrefix = "TAG ";
        private const string _triggerPressEvt = "TRIGGER TRIGPULL";
        private const string _triggerReleaseEvt = "TRIGGER TRIGRELEASE";
        private const string _lowBattEvt = "BATTERY LOW";
        private const string _overheatEvt = "THERMAL OVERTEMP";

        private BluetoothController _bluetoothController = null;

        public RFIDComm(BluetoothController bluetoothController)
        {
            _bluetoothController = bluetoothController;
        }

        public void HandleResponse(string response)
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
        private async Task HandleEvent(string eventMessage)
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
                _bluetoothController.SetPollingSpeed(PollingSpeed.Fast);
                _bluetoothController.SetReadingMode(ReadingMode.Continuous);
            }
            else if (eventMessage.Contains(_triggerReleaseEvt)) // evento = trigger released
            {
                _bluetoothController.SetPollingSpeed(PollingSpeed.Slow);
                _bluetoothController.SetReadingMode(ReadingMode.Disabled);
            }
            else if (eventMessage.Contains(_lowBattEvt)) // evento = low battery warning
            {
                throw new NotImplementedException();
            }
            else if (eventMessage.Contains(_overheatEvt)) // evento = overheating
            {
                throw new NotImplementedException();
            }
            else
            {
                Debug.WriteLine("Handle other incoming event. Input: " + eventMessage);
            }
        }


        private void BroadcastEPC(string epc)
        {
            Debug.WriteLine("Incoming EPC: " + epc);
            MessagingCenter.Send((App)Application.Current, "EPC", epc);
        }


        // Command Processor. Vide BRI Manual
        private void ProcessCommand(string command)
        {
            Debug.WriteLine("NonEvent response: " + command);
            throw new NotImplementedException();
        }
    }
}
