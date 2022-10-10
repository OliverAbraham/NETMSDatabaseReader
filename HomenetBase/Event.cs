using System;

namespace HomenetBase
{
    public enum HNEventType
    {
        DataObjectChangedFromOutside,
        DataObjectChangedFromInside,
        DeviceNew,
        DeviceRemoved,
        DeviceChanged,
        SceneChanged,
        InitializationHasEnded,
        Debug,
		LogValueChangeToSeparateFile,
		RuleCommand_SendEmail,
		RuleCommand_ExecuteDosCommand,
		RuleCommand_SendPushbulletCommand,
        DriverDebug,
        DriverError,
		DataObjectRepositoryError,
		DataObjectRepositoryDebug,
		DataObjectRepositoryInfo,
		DataObjectFinallyChanged,
        RuleprocessorDebug,
		RuleprocessorInfo,
		RuleprocessorWarning,
		RuleprocessorError,
	}

    public delegate void EventHandler_Type (Event e);
        

    public class Event
    {
        #region ------------- Properties ----------------------------------------------------------

        public HNEventType  Type;
        public string       Name;
        public string       Value;
        public int          Trigger;
        public DataObject   Do;
        public DateTime     Timestamp;
        public string       Source;

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public Event(HNEventType type, DataObject dobj, string value, string source = "default2", int trigger = 0)
        {
            Type    = type;
            Do      = dobj;
			Value   = value;
            Source  = source;
            Trigger = trigger;
        }

        public Event(HNEventType type, string name, string value, string source = "Kernel")
        {
            Type   = type;
            Name   = name;
            Value  = value;
            Source = source;
        }

        #endregion
    }
}
