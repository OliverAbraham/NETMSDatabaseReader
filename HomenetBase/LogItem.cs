using System;

namespace HomenetBase
{
	public class LogItem
    {
        public DateTime  Timestamp { get; set; }
        public LogLevel  Level     { get; set; }
        public LogSource Source    { get; set; }
        public string    Message   { get; set; }

        public LogItem()
        {
        }

        public LogItem(DateTime timestamp, string message)
        {
            Timestamp = timestamp;
            Level     = LogLevel.Debug;
            Source    = LogSource.Kernel;
            Message   = message;
        }

        public LogItem(DateTime timestamp, LogLevel loglevel, LogSource source, string message)
        {
            Timestamp = timestamp;
            Level     = loglevel;
            Source    = source;
            Message   = message;
        }

        public override string ToString()
        {
            return $"{Timestamp.ToString()} - {Level.ToString("G"),-5} - {Source.ToString("G"),-13} - {Message}";
        }
    }
}
