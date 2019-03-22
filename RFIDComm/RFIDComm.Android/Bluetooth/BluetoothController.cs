using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using AppEpi.Droid;
using System.Threading;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.IO;
using System.Text;
using AppEpi.Droid.Bluetooth;

[assembly: Dependency(typeof(BluetoothController))]
namespace AppEpi.Droid
{
    public class BluetoothController : IBth
    {
        private const int _fastPollingInterval = 250; // intervalo entre polls (em ms) quando trigger pressed
        private const int _slowPollingInterval = 500; // intervalo entre polls (em ms) quando trigger released
        private const int _pingIfIdleFor = 5000; // se não houver mensagem do leitor durante esse intervalo (em ms), envia-se um ping
        private const int _connectionTimeout = 10000; // se não houver mensagem do leitor durante esse intervalo (em ms), inicia-se reconexão
        private const int _connectionAwait = 1000; // tempo aguardado para confirmar conexão

        private const string _uuid = "00001101-0000-1000-8000-00805f9b34fb";

        private int _pollingInterval = _slowPollingInterval;
        private CancellationTokenSource _ct;
        private BluetoothSocket _bthSocket;
        private Bluetooth.RFIDComm _rfidComm;

        public enum PollingSpeed
        {
            Fast,
            Slow
        }

        // Constructor
        public BluetoothController()
        {
            _rfidComm = new Bluetooth.RFIDComm(this);
        }

        #region IBth implementation
        // Start the Reading loop 
        /// <param name="name"> Name of the paired bluetooth device </param>
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


        // Envia um comando ao leitor RFID conectado
        public void SendCommand(string command)
        {
            Task.Run(async () =>
                SendCommandAsync(command)
            );
        }


        public ObservableCollection<string> GetPairedDevices()
        {
            return BluetoothUtils.GetPairedDevices();
        }
        #endregion

        #region public methods
        public void SetPollingSpeed(PollingSpeed speed)
        {
            switch (speed)
            {
                case PollingSpeed.Fast:
                    _pollingInterval = 100;
                    Debug.WriteLine("Fast polling mode");
                    break;
                case PollingSpeed.Slow:
                    _pollingInterval = 250;
                    Debug.WriteLine("Slow polling mode");
                    break;
                default:
                    Debug.WriteLine("Invalid polling mode setup");
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
        // returns on connection lost or cancellation requested
        private async Task ScanInput(bool readAsCharArray)
        {
            var buffer = new BufferedReader(new InputStreamReader(_bthSocket.InputStream));

            int pingTimer = 0;
            int reconnectTimer = 0;
            while (_ct.IsCancellationRequested == false)
            {
                Thread.Sleep(_pollingInterval);

                pingTimer += _pollingInterval;
                reconnectTimer += _pollingInterval;

                if (reconnectTimer < _connectionTimeout)
                {
                    if (buffer.Ready()) // se houver o que ler
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
                            response = await buffer.ReadLineAsync();

                        if (response.Length > 0) // se a leitura foi válida
                        {
                            _rfidComm.HandleResponse(response);

                            pingTimer = 0; // timers são reiniciados
                            reconnectTimer = 0;
                        }
                        else
                            Debug.WriteLine("No data");
                    }
                    else if (pingTimer >= _pingIfIdleFor)
                    {
                        SendCommand(BRICommands.Ping);
                        pingTimer = 0;
                    }
                }
                else
                { 
                    Debug.WriteLine("Connection Timeout. Retrying...");
                    break;
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

                    await ConnectDevice(name);

                    // Escaneia continuamente até que seja solicitado cancelamento de _ct
                    // ou perdida a conexão
                    await ScanInput(readAsCharArray);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("EXCEPTION: " + e.Message);
                }
                finally
                {
                    if (_bthSocket != null)
                        _bthSocket.Close();
                }
            }
            Debug.WriteLine("Reading loop exit");
        }


        // throws descriptive exceptions
        private async Task ConnectDevice(string name)
        {
            var device = BluetoothUtils.FindDevice(name);

            if (device != null)
            {
                UUID uuid = UUID.FromString(_uuid);
                _bthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);

                if (_bthSocket != null)
                {
                    await _bthSocket.ConnectAsync();

                    if (_bthSocket.IsConnected)
                    {
                        Debug.WriteLine("Connected!");
                    }
                    else
                        throw new UnauthorizedAccessException();
                }
                else
                    throw new NullReferenceException();
            }
            else
                throw new ArgumentException();

            // Initialize RFIDReader
            await SendCommandAsync(BRICommands.ResetFactoryDefaults);
        }


        private async Task SendCommandAsync(string command)
        {
            await StreamMessage(AppendEOL(command));
        }


        private string AppendEOL(string input)
        {
            return string.Concat(input, BRICommands.Crlf);
        }
        #endregion
    }
}
