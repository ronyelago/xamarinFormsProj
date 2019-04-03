using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using System.Threading;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.IO;
using System.Text;
using System.Data;

[assembly: Dependency(typeof(AppEpi.Droid.Bluetooth.BluetoothController))]
namespace AppEpi.Droid.Bluetooth
{
    public class BluetoothController : IBluetoothController
    {
        private const int _fastPollingInterval = 250; // intervalo entre polls (em ms) quando trigger pressed
        private const int _slowPollingInterval = 500; // intervalo entre polls (em ms) quando trigger released
        private const int _pingIfIdleFor = 5000; // se não houver mensagem do leitor durante esse intervalo (em ms), envia-se um ping
        private const int _connectionTimeout = 10000; // se não houver mensagem do leitor durante esse intervalo (em ms), inicia-se reconexão
        private const string _uuid = "00001101-0000-1000-8000-00805F9B34FB";
        private const string _configFileName = "BTH_Power";

        private bool _serverStarted = false;
        private string _targetDeviceName = null;
        private int _pollingInterval = _slowPollingInterval;
        private CancellationTokenSource _cts;
        private BluetoothSocket _bthSocket = null;
        private RFIDComm _rfidComm;

        public enum PollingSpeed
        {
            Fast,
            Slow
        }

        // Constructor
        public BluetoothController()
        {
            _rfidComm = new RFIDComm(this);

            // Inicialização da potência caso haja definição armazenada
            string power = new StorageController().LoadText(_configFileName);
            if (power != "")
            {
                if (int.TryParse(power, out int i))
                {
                    _readerPower = i;
                }
            }
        }

        #region IBth implementation

        private ConnectionState _currentState = ConnectionState.Closed;
        public ConnectionState CurrentState
        {
            get
            {
                return _currentState;
            }
            private set
            {
                // se o novo valor for diferente do anterior
                if (!value.Equals(_currentState))
                {
                    _currentState = value;
                    // envia mensagem a toda aplicação com o novo estado
                    MessagingCenter.Send((App)Application.Current, "BLUETOOTH_STATE", value);
                }
                _currentState = value;
            }
        }

        private int _readerPower = 100;
        public int ReaderPower
        {
            get
            {
                return _readerPower;
            }
            set
            {
                // O leitor RFID só aceita valores de potência multiplos de 5, de 25 a 100. Vide BRI Manual
                int power = value;

                if (power > BRICommands.MaxPower)
                    power = BRICommands.MaxPower;
                else if (power < BRICommands.MinPower)
                    power = BRICommands.MinPower;
                else // segundo o manual, só são aceitos múltiplos de 5, que seriam automaticamente rounded down
                    power -= power % BRICommands.PowerStep;

                // envia ao aparelho a nova potência
                SendCommand(BRICommands.SetReaderPower + power);

                // armazena o novo valor em arquivo, assim como na variavel privada
                new StorageController().SaveText(_configFileName, power.ToString());
                _readerPower = power;
            }
        }

        public ObservableCollection<string> PairedDevices
        {
            get
            {
                BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
                ObservableCollection<string> devices = new ObservableCollection<string>();

                foreach (var bd in adapter.BondedDevices)
                    devices.Add(bd.Name);

                return devices;
            }
        }


        // Starts the Server loop 
        /// <param name="deviceName"> Name of the paired bluetooth device </param>
        public void Init(bool readAsCharArray = true)
        {
            if (!_serverStarted)
            {
                _serverStarted = true;

                if (_currentState == ConnectionState.Closed)
                {
                    Task.Run(() =>
                        Loop(readAsCharArray)
                        );
                }
                else
                {
                    RestartServer();
                }
            }
        }


        // Cancels the Server loop
        public void Cancel()
        {
            _serverStarted = false;

            if (_cts != null)
            {
                Debug.WriteLine("Send a cancel to task!");
                _cts.Cancel();
            }
            CurrentState = ConnectionState.Closed;
        }


        public void Connect(string deviceName = "")
        {
            _targetDeviceName = deviceName;
        }


        public void Disconnect()
        {
            _targetDeviceName = null;
            RestartServer();
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


        // Envia um comando ao leitor RFID conectado
        public void SendCommand(string command)
        {
            Task.Run(() =>
                SendCommandAsync(command)
            );
        }

        #endregion

        #region private methods

        // Envia mensagem através do BT, sem manipular message
        private async Task StreamMessage(string message)
        {
            if (_cts.IsCancellationRequested == false)
            {
                try
                {
                    byte[] msgBuffer = Encoding.ASCII.GetBytes(message);
                    Stream outStream = _bthSocket.OutputStream;

                    await outStream.WriteAsync(msgBuffer, 0, msgBuffer.Length);
                    outStream.Flush();

                    Debug.Write("Sent message: " + message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("StreamMessage exception: " + e.Message);
                }
            }
        }


        // escaneia inputStream continuamente
        // throws exceptions
        // returns on connection lost or cancellation requested
        private async Task ScanInput(bool readAsCharArray)
        {
            try
            {
                var buffer = new BufferedReader(new InputStreamReader(_bthSocket.InputStream));

                int pingTimer = 0;
                int reconnectTimer = 0;
                while (_cts.IsCancellationRequested == false)
                {
                    Thread.Sleep(_pollingInterval);
                    if (_cts.IsCancellationRequested == false)
                    {
                        pingTimer += _pollingInterval;
                        reconnectTimer += _pollingInterval;

                        if (reconnectTimer < _connectionTimeout)
                        {
                            if (buffer.Ready()) // se houver o que ler
                            {
                                CurrentState = ConnectionState.Open;
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
                                //SendCommand(BRICommands.Ping);
                                // Set ReaderPower no dispositivo
                                ReaderPower = ReaderPower; // Funciona também como ping
                                pingTimer = 0;
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Connection Timeout. Retrying...");
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ScanInput Exception: " + e.Message);
            }
            Debug.WriteLine("ScanInput loop exit");
        }


        private async Task Loop(bool readAsCharArray)
        {
            try
            {
                _cts = new CancellationTokenSource();

                while (_cts.IsCancellationRequested == false)
                {
                    Thread.Sleep(_pollingInterval);

                    if (_targetDeviceName != null)
                        await ConnectDevice(_targetDeviceName);

                    if (CurrentState == ConnectionState.Open)
                    {
                        // Escaneia inputs continuamente até que caia conexão ou seja cancelado
                        await ScanInput(readAsCharArray);
                    }
                }
                Debug.WriteLine("Server loop exit");
            }
            catch (Exception e)
            {
                CurrentState = ConnectionState.Broken;
                Debug.WriteLine("Loop Exception: " + e.Message);
            }
            finally
            {
                if (_bthSocket != null)
                {
                    _bthSocket.Close();
                    _bthSocket.Dispose();
                }

                RestartServer();
            }
        }


        private async Task ConnectDevice(string name)
        {
            try
            {
                CurrentState = ConnectionState.Connecting;

                try
                {
                    // reinicia o socket se o mesmo já tiver sido criado
                    // Na verdade, até onde entendo, o socket só deveria ser fechado ao final da operação.
                    // Esse .Dispose(), no entanto, foi o que fez com que a segunda conexão funcionasse como um todo.
                    // Introduziu também alguns problemas no gerenciamento de reconexão que não tive tempo nem de analizar
                    if (_bthSocket != null)
                    {
                        _bthSocket.Close();
                        _bthSocket.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Socket disposing Exception: " + e.Message);
                }

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
                            CurrentState = ConnectionState.Open;
                            Debug.WriteLine("Connected!");

                            // Initialize RFIDReader
                            await SendCommandAsync(BRICommands.ResetFactoryDefaults);
                        }
                        else
                            return;
                    }
                    else
                        throw new NullReferenceException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ConnectDevice Exception: " + e.Message);
            }
        }


        private async Task SendCommandAsync(string command)
        {
            await StreamMessage(AppendEOL(command));
        }


        private string AppendEOL(string input)
        {
            return string.Concat(input, BRICommands.Crlf);
        }


        private void RestartServer()
        {
            Debug.WriteLine("Servidor Bluetooth reiniciado.");
            Thread.Sleep(_pollingInterval);
            Cancel();
            Thread.Sleep(2 * _pollingInterval);
            Init();
        }

        #endregion
    }
}
