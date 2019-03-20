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

[assembly: Dependency(typeof(Bth))]
namespace RFIDComm.Droid
{
    public class Bth : IBth
    {
        private const int _pollsBeforeReconnect = 200;
        private CancellationTokenSource _ct { get; set; }
        private BluetoothSocket _bthSocket;

        public Bth()
        {
        }


        #region IBth implementation

        // Start the Reading loop 
        /// <param name="name">Name of the paired bluetooth device (also a part of the name)</param>
        public void Start(string name, int pollingTime = 100, bool readAsCharArray = true)
        {
            Task.Run(async () => 
                Loop(name, pollingTime, readAsCharArray)
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
        private async Task ScanInput(int pollingInterval, bool readAsCharArray)
        {
            var buffer = new BufferedReader(new InputStreamReader(_bthSocket.InputStream));

            int count = 0;
            while (_ct.IsCancellationRequested == false)
            {
                Thread.Sleep(pollingInterval);

                count++;
                if (count < _pollsBeforeReconnect) //precisa verificar se esta conectado (Rodolfo Cortese)
                {
                    if (buffer.Ready())
                    {
                        #region read as char array
                        string barcode = "";
                        if (readAsCharArray)
                        {
                            char[] chr = new char[100];

                            await buffer.ReadAsync(chr);
                            foreach (char c in chr)
                            {
                                if (c == '\0')
                                    break;
                                barcode += c;
                            }
                        }
                        #endregion
                        else
                        {
                            barcode = buffer.ReadLine();
                        }

                        if (barcode.Length > 0)
                        {
                            Debug.WriteLine("input: " + barcode);
                            MessagingCenter.Send<App, string>((App)Application.Current, "Barcode", barcode);
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
                if (!(count < _pollsBeforeReconnect)) // if not connected (Rodolfo Cortese)
                { // Force timed reconnection
                    //Debug.WriteLine("Unexpected connection loss. Throw exception");
                    throw new Exception();
                }
            }
            Debug.WriteLine("ScanInput loop exit");
        }


        private async Task Loop(string name, int pollingInterval, bool readAsCharArray)
        {
            _bthSocket = null;

            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(pollingInterval);

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
                                await ScanInput(pollingInterval, readAsCharArray);
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
    }
}

