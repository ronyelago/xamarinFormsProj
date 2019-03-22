namespace AppEpi.Droid.Bluetooth
{
    public class BRICommands
    {
        #region output
        internal const string ReadContinuously = "READ REPORT=EVENT";
        internal const string ReadStop = "READ STOP";
        internal const string ResetFactoryDefaults = "FACDFLT";
        internal const string Ping = "PING";

        // Vide documentação "Manual BRI", em FIELDSTRENGTH
        internal const string ReaderPower = "ATTRIBUTE FIELDSTRENGTH=";
        internal enum Power
        {
            Full = 100,
            Half = 85,
            Quarter = 70,
            Eigth = 55,
            Sixteenth = 40,
            Min = 25
        }
        #endregion

        #region input
        internal const string EpcPrefix = "TAG H";
        //internal const string EventPrefix = "EVT:";
        internal const string EventPrefix = ":"; // usado num Contains()... Nada mais parece usar ':'.
        internal const string LowBatteryEvent = "BATTERY LOW";
        internal const string OverheatEvent = "THERMAL OVERTEMP";
        //internal const string TriggerReleaseEvent = "TRIGGER TRIGRELEASE";
        //internal const string TriggerPressEvent = "TRIGGER TRIGPULL";
        internal const string TriggerReleaseEvent = "TRIGR"; // usado num Contains()
        internal const string TriggerPressEvent = "TRIGP"; // usado num Contains()
        internal const string Ok = "OK>";
        #endregion
        
        #region general
        internal const string Crlf = "\r\n";
        #endregion
    }
}