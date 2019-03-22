namespace RFIDComm.Droid.Bluetooth
{
    public class BRICommands
    {
        internal const string ReadStop = "READ STOP";
        internal const string ReadContinuously = "READ REPORT=EVENT";

        internal const string ResetFactoryDefaults = "FACDFLT";
        
        // input
        internal const string EventPrefix = "EVT:";
        internal const string EpcPrefix = "TAG ";
        internal const string LowBatteryEvent = "BATTERY LOW";
        internal const string OverheatEvent = "THERMAL OVERTEMP";
        internal const string TriggerReleaseEvent = "TRIGGER TRIGRELEASE";
        internal const string TriggerPressEvent = "TRIGGER TRIGPULL";
    }
}