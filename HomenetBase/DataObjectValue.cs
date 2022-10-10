using System;

namespace HomenetBase
{
	public class DataObjectValue
	{
		public int              ID                          { get; set; }
		public int              DataObjectID                { get; set; }
        public string           Value                       { get; set; }
        public DateTime         Timestamp                   { get; set; }

        public DataObjectValue()
        {
        }

        public DataObjectValue(int id, int dataObjectID, string value, DateTime timestamp)
        {
            ID           = id;
            DataObjectID = dataObjectID;
            Value        = value;
            Timestamp    = timestamp;
        }
	}
}
