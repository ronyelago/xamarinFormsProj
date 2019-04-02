using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppEpi.Droid.Bluetooth
{
    class RFIDComm
    {
        private const int _epcLength = 24;

        private BluetoothController _bluetoothController = null;
        private Queue<string> _commandQueue = new Queue<string>();
        private Queue<string> _eventQueue = new Queue<string>();
        private Task _eventHandlingTask;

        // Constructor
        public RFIDComm(BluetoothController bluetoothController)
        {
            _bluetoothController = bluetoothController;
        }


        public void HandleResponse(string response)
        {
            try
            {
                foreach (string line in response.Split(BRICommands.Crlf, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.Contains(BRICommands.EventPrefix))
                        _eventQueue.Enqueue(line); // guarda os eventos em uma queue
                    else
                        ProcessCommand(line);
                }

                if (_eventQueue.Count > 0) // se houver eventos em espera
                {
                    // que não estejam sendo tratados
                    if (_eventHandlingTask == null)
                    {
                        _eventHandlingTask = Task.Run(() =>
                           HandleEventQueue()
                            );
                    }
                    else if (_eventHandlingTask.IsCompleted || _eventHandlingTask.IsCanceled)
                    {
                        _eventHandlingTask = Task.Run(() =>
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
        private void HandleEventQueue()
        {
            while (_eventQueue.Count > 0)
            {
                string message = _eventQueue.Dequeue();

                try
                {
                    // validação e preparação da mensagem para tratamento
                    if (message.Contains(BRICommands.EventPrefix))
                    {
                        // message = parte da string que vem depois de EventPrefix
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

                        if (epc.Length == _epcLength) // validação do tamanho do resultado
                            BroadcastEPC(epc);
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
                        MessagingCenter.Send((App)Application.Current, "RFID_LOWBATT");
                    }
                    // evento = overheating
                    else if (message.Contains(BRICommands.OverheatEvent))
                    {
                        MessagingCenter.Send((App)Application.Current, "RFID_OVERHEAT");
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
            MessagingCenter.Send((App)Application.Current, "EPC", epc);
        }


        // Command Processor. Vide BRI Manual
        private void ProcessCommand(string command)
        {
            if (command != BRICommands.Ok)
            {
                _commandQueue.Enqueue(command);
                return;
            }

            if (_commandQueue.Count > 0)
            {
                Debug.WriteLine("Command:\n");
                foreach (string line in _commandQueue)
                {
                    // resposta: Reader Power request
                    if (line.Contains(BRICommands.ReaderPowerResponse))
                    {
                        var power = line.Split(BRICommands.ReaderPowerResponse)[1];
                        _bluetoothController.ReaderPower = int.Parse(power);
                    }

                    Debug.WriteLine(line);
                }
            }
            else
                Debug.WriteLine("RFID says Ok!\n");

            // os comandos são armazenados na ordem correta, mas ainda não são tratados
            _commandQueue.Clear();
        }
    }
}
