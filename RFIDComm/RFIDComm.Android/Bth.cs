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

        private CancellationTokenSource _ct { get; set; }

        const int RequestResolveError = 1000;
        private BluetoothSocket bthSocket;

        public Bth()
        {
        }

        #region IBth implementation

        /// <summary>
        /// Start the "reading" loop 
        /// </summary>
        /// <param name="name">Name of the paired bluetooth device (also a part of the name)</param>
        public void Start(string name, int sleepTime = 200, bool readAsCharArray = false)
        {
            Task.Run(async () => Loop(name, sleepTime, readAsCharArray));
        }


        private async Task Loop(string name, int sleepTime, bool readAsCharArray)
        {
            BluetoothDevice device = null;
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            bthSocket = null;

            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(sleepTime);

                    adapter = BluetoothAdapter.DefaultAdapter;

                    if (adapter == null)
                        Debug.WriteLine("No Bluetooth adapter found.");
                    else
                        Debug.WriteLine("Adapter found!!");

                    if (!adapter.IsEnabled)
                        Debug.WriteLine("Bluetooth adapter is not enabled.");
                    else
                        Debug.WriteLine("Adapter enabled!");

                    Debug.WriteLine("Try to connect to " + name);

                    foreach (var bd in adapter.BondedDevices)
                    {
                        Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
                        if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                        {
                            Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
                            device = bd;
                            break;
                        }
                    }

                    if (device == null)
                        Debug.WriteLine("Named device not found.");
                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4 
                        {
                            bthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        }
                        else
                        {
                            bthSocket = device.CreateRfcommSocketToServiceRecord(uuid);
                        }

                        if (bthSocket != null)
                        {
                            await bthSocket.ConnectAsync();

                            if (bthSocket.IsConnected)
                            {
                                Debug.WriteLine("Connected!");

                                var mReader = new InputStreamReader(bthSocket.InputStream);
                                var buffer = new BufferedReader(mReader);
                                while (_ct.IsCancellationRequested == false)
                                {
                                    if (buffer.Ready())
                                    {
                                        char[] chr = new char[100];
                                        string barcode = "";
                                        if (readAsCharArray)
                                        {

                                            await buffer.ReadAsync(chr);
                                            foreach (char c in chr)
                                            {
                                                if (c == '\0')
                                                    break;
                                                barcode += c;
                                            }
                                        }
                                        else
                                        {
                                            barcode = await buffer.ReadLineAsync();
                                        }

                                        if (barcode.Length > 0)
                                        {
                                            Debug.WriteLine("Letto: " + barcode);
                                            MessagingCenter.Send<App, string>((App)Application.Current, "Barcode", barcode);
                                        }
                                        else
                                            Debug.WriteLine("No data");
                                    }
                                    else
                                    {
                                        Debug.WriteLine("No data to read");
                                    }

                                    // A little stop to the uneverending thread...
                                    Thread.Sleep(sleepTime);
                                    if (!bthSocket.IsConnected)
                                    {
                                        Debug.WriteLine("BthSocket.IsConnected = false, Throw exception");
                                        throw new Exception();
                                    }
                                }
                                Debug.WriteLine("Exit the inner loop");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("bthSocket = null");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EXCEPTION: " + ex.Message);
                }

                finally
                {
                    if (bthSocket != null)
                    {
                        bthSocket.Close();
                    }
                    device = null;
                }
            }
            Debug.WriteLine("Exit the external loop");
        }


        /// <summary>
        /// Cancel the Reading loop
        /// </summary>
        /// <returns><c>true</c> if this instance cancel ; otherwise, <c>false</c>.</returns>
        public void Cancel()
        {
            if (_ct != null)
            {
                Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }

        // Rodolfo: NÃO PARECE ESTAR FUNCIONANDO AINDA
        // Requires further developing
        public void SendMessage(string message)
        {
            if (_ct != null)
            {
                try
                {
                    throw new NotSupportedException();

                    Stream outStream = bthSocket.OutputStream;
                    byte[] msgBuffer = Encoding.ASCII.GetBytes(message);

                    //Task.Run(async () => outStream.WriteAsync(msgBuffer, 0, msgBuffer.Length));
                    outStream.Write(msgBuffer, 0, msgBuffer.Length);
                    outStream.Flush();

                    Debug.WriteLine("Sent message: \"" + message + "\"");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EXCEPTION: " + ex.Message);
                }
            }
        }


        public ObservableCollection<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
            {
                devices.Add(bd.Name);
            }
            return devices;
        }
        #endregion
    }
}

