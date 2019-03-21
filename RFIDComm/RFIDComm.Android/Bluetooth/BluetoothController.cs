using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using RFIDComm.Droid;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.IO;
using System.Text;

[assembly: Dependency(typeof(BluetoothController))]
namespace RFIDComm.Droid
{
    public class BluetoothController : IBth
    {
        private const int _fastPolling = 100;
        private const int _slowPolling = 250;
        private const int _reconnectTime = 30000;

        private static int _pollingInterval = _slowPolling;
        private CancellationTokenSource _ct;
        private BluetoothSocket _bthSocket;

        public enum PollingSpeed
        {
            Fast,
            Slow
        }

        public BluetoothController()
        {
        }

        #region IBth implementation
        // Start the Reading loop 
        /// <param name="name">Name of the paired bluetooth device (also a part of the name)</param>
        public void Start(string name, bool readAsCharArray = true)
        {
            Task.Run(async () =>
                Loop(name, readAsCharArray)
                );
        }


        // Cancel the Reading loop
        public void Cancel()
        {
            if (_ct != null)
            {
                Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }


        // Envia um comando ao leitor RFID conectado (assincrono)
        public void SendCommand(string command)
        {
            Task.Run(async () =>
                StreamMessage(string.Concat(command, "\r\n"))
            );
        }


        public ObservableCollection<string> GetPairedDevices()
        {
            return BluetoothUtils.GetPairedDevices();
        }
        #endregion

        #region public methods
        public static void SetPollingSpeed(PollingSpeed speed)
        {
            switch (speed)
            {
                case PollingSpeed.Fast:
                    _pollingInterval = 100;
                    Debug.WriteLine("Bluetooth: Fast polling mode");
                    break;
                case PollingSpeed.Slow:
                    _pollingInterval = 250;
                    Debug.WriteLine("Bluetooth: Slow polling mode");
                    break;
                default:
                    Debug.WriteLine("Bluetooth: Invalid polling mode setup");
                    break;
            }
        }
        #endregion

        #region private methods
        // Envia mensagem através do BT, sem manipular message
        private async Task StreamMessage(string message)
        {
            if (_ct.IsCancellationRequested == false)
            {
                try
                {
                    byte[] msgBuffer = Encoding.ASCII.GetBytes(message);

                    Stream outStream = _bthSocket.OutputStream;
                    await outStream.WriteAsync(msgBuffer, 0, msgBuffer.Length);

                    outStream.Flush();
                    Debug.WriteLine("Sent message: \"" + message + "\"");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("EXCEPTION: " + e.Message);
                }
            }
        }


        // escaneia inputStream continuamente
        // throws exceptions
        // exits on disconnection
        private async Task ScanInput(bool readAsCharArray)
        {
            var buffer = new BufferedReader(new InputStreamReader(_bthSocket.InputStream));

            int reconnectTimer = 0;
            while (_ct.IsCancellationRequested == false)
            {
                Thread.Sleep(_pollingInterval);

                reconnectTimer += _pollingInterval;
                if (reconnectTimer < _reconnectTime) //precisa verificar se esta conectado (Rodolfo Cortese)
                {
                    if (buffer.Ready())
                    {
                        string response = "";

                        if (readAsCharArray)
                        #region read as char array
                        {
                            char[] chr = new char[100];

                            await buffer.ReadAsync(chr);
                            foreach (char c in chr)
                            {
                                if (c == '\0')
                                    break;
                                response += c;
                            }
                        }
                        #endregion
                        else
                        {
                            response = buffer.ReadLine();
                        }

                        if (response.Length > 0)
                        {
                            Bluetooth.RFIDComm.HandleResponse(response);
                        }
                        else
                        {
                            Debug.WriteLine("No data");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No data to read");
                    }
                }
                if (!(reconnectTimer < _reconnectTime)) // if not connected (Rodolfo Cortese)
                { // Force timed reconnection
                    //Debug.WriteLine("Unexpected connection loss. Throw exception");
                    throw new Exception();
                }
            }
            Debug.WriteLine("ScanInput loop exit");
        }


        private async Task Loop(string name, bool readAsCharArray)
        {
            _bthSocket = null;

            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(_pollingInterval);

                    var device = BluetoothUtils.FindDevice(name);

                    if (device != null)
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        _bthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);

                        if (_bthSocket != null)
                        {
                            await _bthSocket.ConnectAsync();

                            if (_bthSocket.IsConnected)
                            {
                                Debug.WriteLine("Connected!");

                                // Escaneia continuamente até que seja solicitado cancelamento de _ct
                                // ou perdida a conexão
                                await ScanInput(readAsCharArray);
                            }
                            else
                            {
                                Debug.WriteLine("bthSocket = null");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("EXCEPTION: " + e.Message);
                }

                finally
                {
                    if (_bthSocket != null)
                    {
                        _bthSocket.Close();
                    }
                }
            }
            Debug.WriteLine("Reading loop exit");
        }
        #endregion
    }
}
