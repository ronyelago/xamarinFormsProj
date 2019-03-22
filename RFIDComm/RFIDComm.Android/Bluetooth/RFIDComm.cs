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
            Debug.WriteLine("NEW RESPONSE---------------------\n" + response);
            try
            {
                string[] splitResponse = response.Split(BRICommands.Crlf, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in splitResponse)
                {
                    if (line.StartsWith(BRICommands.EventPrefix))
                    {
                        Task.Run(async () =>
                            HandleEvent(line.Remove(0, BRICommands.EventPrefix.Length))
                            );
                    }
                    else
                    {
                        ProcessCommand(line);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("RESPONSE HANDLING EXCEPTION: " + e.Message);
            }
        }


        // Event Handler. Vide BRI Manual
        private async Task HandleEvent(string eventMessage)
        {
            // Rodolfo Cortese: usado durante desenvolvimento do modulo apenas
            MessagingCenter.Send<App, string>((App)Application.Current, "Barcode", eventMessage);
            // remover depois!

            try
            {
                if (eventMessage.StartsWith(BRICommands.EpcPrefix)) // evento = novo EPC
                {
                    string epc = eventMessage
                        .Remove(0, BRICommands.EpcPrefix.Length); // retira prefixo

                    if (epc.Length > _epcLength) // throws exception otherwise
                        epc = epc.Remove(_epcLength); // retira qualquer coisa que possa ter vindo extra por engano

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
            catch (Exception e)
            {
                Debug.WriteLine("EVENT HANDLING EXCEPTION: " + e.Message);
            }
        }


        private void BroadcastEPC(string epc)
        {
            Debug.WriteLine("---EPC: " + epc);
            MessagingCenter.Send((App)Application.Current, "EPC", epc);
        }


        // Command Processor. Vide BRI Manual
        private void ProcessCommand(string command)
        {
            Debug.WriteLine("COMMAND: " + command);
            throw new NotImplementedException();
        }
    }
}
