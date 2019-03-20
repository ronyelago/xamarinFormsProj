﻿using System;
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
        private const int pollsBeforeReconnect = 200;

        private CancellationTokenSource _ct { get; set; }
        private BluetoothSocket bthSocket;

        public Bth()
        {
        }


        // Envia mensagem através do BT, sem manipular message
        private async Task StreamMessage(string message)
        {
            if (_ct.IsCancellationRequested == false)
            {
                try
                {
                    byte[] msgBuffer = Encoding.ASCII.GetBytes(message);

                    Stream outStream = bthSocket.OutputStream;
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
            var mReader = new InputStreamReader(bthSocket.InputStream);
            var buffer = new BufferedReader(mReader);

            int count = 0;
            while (_ct.IsCancellationRequested == false)
            {
                Thread.Sleep(pollingInterval);
                BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;

                /*
                #region check bth vars
                Debug.WriteLine("socket.isConnected= " + bthSocket.IsConnected);
                Debug.WriteLine("socket.RemoteDevice.Name= " + bthSocket.RemoteDevice.Name);
                Debug.WriteLine("adapter.SapConnState=" + adapter.GetProfileConnectionState(ProfileType.Sap));
                Debug.WriteLine("adapter.GattConnState=" + adapter.GetProfileConnectionState(ProfileType.Gatt));
                #endregion
                */

                count++;
                if (count < pollsBeforeReconnect) //precisa verificar se esta conectado (Rodolfo Cortese)
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
                if (!(count < pollsBeforeReconnect)) // if not connected (Rodolfo Cortese)
                { // Force timed reconnection
                    //Debug.WriteLine("Unexpected connection loss. Throw exception");
                    throw new Exception();
                }
            }
            Debug.WriteLine("ScanInput loop exit");
        }


        // retorna BluetoothDevice pareado buscando por nome
        private BluetoothDevice FindDevice(string name)
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;

            #region adapter debugging
            if (adapter == null)
            {
                Debug.WriteLine("No Bluetooth adapter found.");
            }
            else
            {
                Debug.WriteLine("Adapter found!");
            }

            if (!adapter.IsEnabled)
            {
                Debug.WriteLine("Bluetooth adapter is not enabled.");
            }
            else
            {
                Debug.WriteLine("Adapter enabled!");
            }

            Debug.WriteLine("Try to connect to " + name);
            #endregion

            BluetoothDevice device = null;

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
            {
                Debug.WriteLine("Named device not found.");
            }

            return device;
        }


        private async Task Loop(string name, int pollingInterval, bool readAsCharArray)
        {
            bthSocket = null;

            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(pollingInterval);

                    var device = FindDevice(name);

                    if (device != null)
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        bthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);

                        if (bthSocket != null)
                        {
                            await bthSocket.ConnectAsync();

                            if (bthSocket.IsConnected)
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
                }
            }
            Debug.WriteLine("Reading loop exit");
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

