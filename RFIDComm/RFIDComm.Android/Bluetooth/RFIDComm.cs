using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RFIDComm.Droid.Bluetooth
{
    class RFIDComm
    {
        private BluetoothController _bluetoothController = null;

        private const int _epcLength = 24;

        private Queue<string> commandQueue = new Queue<string>();
        private Queue<string> eventQueue = new Queue<string>();
        private Task eventHandlingTask;

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
                foreach (string line in response.Split(BRICommands.Crlf, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.Contains(BRICommands.EventPrefix))
                    {
                        eventQueue.Enqueue(line); // guarda os eventos em uma queue
                    }
                    else
                        ProcessCommand(line);
                }

                if (eventQueue.Count > 0) // se houver eventos em espera
                {
                    if (eventHandlingTask == null || eventHandlingTask.IsCompleted) // que não estejam sendo tratados
                    {
                        eventHandlingTask = Task.Run(async () =>
                           HandleEventQueue()
                            );
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("RESPONSE HANDLING EXCEPTION: " + e.Message);
            }
        }


        // Event Handler. Vide BRI Manual
        private async Task HandleEventQueue()
        {
            while (eventQueue.Count > 0)
            {
                string message = eventQueue.Dequeue();

                // Rodolfo Cortese: usado durante desenvolvimento do modulo apenas
                MessagingCenter.Send<App, string>((App)Application.Current, "Barcode", message);
                // remover depois!

                try
                {
                    // validação e preparação da mensagem para tratamento
                    if (message.Contains(BRICommands.EventPrefix))
                    {
                        message = message.Split(BRICommands.EventPrefix, StringSplitOptions.RemoveEmptyEntries)[1];
                    }
                    else
                    {
                        Debug.WriteLine("Invalid event: " + message);
                        break;
                    }


                    // evento = novo EPC
                    if (message.StartsWith(BRICommands.EpcPrefix))
                    {
                        string epc = message
                            .Remove(0, BRICommands.EpcPrefix.Length); // retira prefixo

                        if (epc.Length > _epcLength) // throws exception otherwise
                            epc = epc.Remove(_epcLength); // retira qualquer coisa que possa ter vindo extra por engano

                        if (epc.Length == _epcLength)
                            BroadcastEPC(epc);
                        else
                            Debug.WriteLine("Invalid EPC: " + epc);
                    }
                    // evento = trigger pressed
                    else if (message.Contains(BRICommands.TriggerPressEvent))
                    {
                        _bluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Fast);
                        _bluetoothController.SendCommand(BRICommands.ReadContinuously);
                    }
                    // evento = trigger released
                    else if (message.Contains(BRICommands.TriggerReleaseEvent))
                    {
                        _bluetoothController.SetPollingSpeed(BluetoothController.PollingSpeed.Slow);
                        _bluetoothController.SendCommand(BRICommands.ReadStop);
                    }
                    // evento = low battery warning
                    else if (message.Contains(BRICommands.LowBatteryEvent))
                    {
                        throw new NotImplementedException();
                    }
                    // evento = overheating
                    else if (message.Contains(BRICommands.OverheatEvent))
                    {
                        throw new NotImplementedException();
                    }
                    else
                        Debug.WriteLine("Handle other incoming event. Input: " + message);

                }
                catch (Exception e)
                {
                    Debug.WriteLine("EVENT HANDLING EXCEPTION: " + e.Message);
                }
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
            if (command != BRICommands.Ok)
            {
                commandQueue.Enqueue(command);
                return;
            }

            if (commandQueue.Count > 0)
            {
                Debug.WriteLine("Command:\n");
                foreach (string line in commandQueue)
                    Debug.WriteLine(line);
            }
            else
                Debug.WriteLine("RFID says Ok!\n");

            // os comandos são armazenados na ordem correta, mas ainda não são tratados
            commandQueue.Clear();
        }
    }
}
