namespace RFIDComm.Droid.Bluetooth
{
    public class BRICommands
    {
        // output
        internal const string ReadContinuously = "READ REPORT=EVENT";
        internal const string ReadStop = "READ STOP";
        internal const string ResetFactoryDefaults = "FACDFLT";
        internal const string Ping = "PING";

        // input
        internal const string EpcPrefix = "TAG ";
        internal const string EventPrefix = "EVT:";
        internal const string LowBatteryEvent = "BATTERY LOW";
        internal const string OverheatEvent = "THERMAL OVERTEMP";
        internal const string TriggerReleaseEvent = "TRIGGER TRIGRELEASE";
        internal const string TriggerPressEvent = "TRIGGER TRIGPULL";
    }
}