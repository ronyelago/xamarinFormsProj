namespace AppEpi.Droid.Bluetooth
{
    class Debug
    {
        public static void WriteLine(string message)
        {
            System.Diagnostics.Debug.WriteLine(string.Concat("Bluetooth: ", message));
        }

        public static void Write(string message)
        {
            System.Diagnostics.Debug.Write(string.Concat("Bluetooth: ", message));
        }
    }
}