using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RFIDComm.Droid.Bluetooth
{
    class RFIDComm
    {
        private BluetoothController _bluetoothController = null;

        private const int _epcLength = 24;

        // Constructor
        public RFIDComm(BluetoothController bluetoothController)
        {
            _bluetoothController = bluetoothController;
        }


        public void HandleResponse(string response)
        {
            try
            {
                if (response.StartsWith(BRICommands.EventPrefix))
                {
                    Task.Run(async () =>
                        HandleEvent(response.Remove(0, BRICommands.EventPrefix.Length))
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

            if (eventMessage.StartsWith(BRICommands.EpcPrefix)) // evento = novo EPC
            {
                string epc = eventMessage
                    .Remove(0, BRICommands.EpcPrefix.Length) // retira prefixo
                    .Remove(_epcLength);    // retira <CRLF> + qualquer coisa que tenha vindo junto

                if (epc.Length == _epcLength)
                {
                    BroadcastEPC(epc);
                }
                else
                {
                    Debug.WriteLine("Invalid EPC: " + epc);
                }
            }
            else if (eventMessage.Contains(BRICommands.TriggerPressEvent)) // evento = trigger pressed
            {
                _bluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Fast);
                _bluetoothController.SendCommand(BRICommands.ReadContinuously);
            }
            else if (eventMessage.Contains(BRICommands.TriggerReleaseEvent)) // evento = trigger released
            {
                _bluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Slow);
                _bluetoothController.SendCommand(BRICommands.ReadStop);
            }
            else if (eventMessage.Contains(BRICommands.LowBatteryEvent)) // evento = low battery warning
            {
                throw new NotImplementedException();
            }
            else if (eventMessage.Contains(BRICommands.OverheatEvent)) // evento = overheating
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
