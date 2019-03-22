namespace RFIDComm.Droid.Bluetooth
{
    class Debug
    {
        public static void WriteLine(string message)
        {
            System.Diagnostics.Debug.WriteLine(string.Concat("Bluetooth: ", message));
        }
    }
}