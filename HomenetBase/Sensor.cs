namespace HomenetBase
{
	public class Sensor
	{
		public string Fid                      { get; set; }
		public string Sid                      { get; set; }
		public string Typ                      { get; set; }
		public string Einheit_und_Wertebereich { get; set; }
		public string Bezeichnung              { get; set; }
        public string Wert                     { get; set; }

		public static Sensor FromHardwareDescriptor(HardwareDescriptor hwd, int fid, string sid)
		{
			return new Sensor()
			{
				Fid                      = fid.ToString(),
				Sid                      = sid,
				Typ                      = hwd.Type,
				Einheit_und_Wertebereich = hwd.Range,
				Bezeichnung              = hwd.Connector
			};
		}
    }
}
