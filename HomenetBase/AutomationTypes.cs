namespace Homenet
{
	public class AutomationTypes
	{
        public enum ID
        {
            Bewegungsmelder_mit_Retriggerung = 0,
            Türkontakt_ohne_Retriggerung     = 1
        }

		public class AutomationType
		{
			public ID ID;
			public string Description;
			
            public AutomationType(ID id, string description)
            {
                ID = id;
                Description = description;
            }
		}


        public static AutomationType[] Types = new AutomationType[] 
        {
            new AutomationType(ID.Bewegungsmelder_mit_Retriggerung, "Motion sensor (with retrigger)"),
            new AutomationType(ID.Türkontakt_ohne_Retriggerung    , "Door contact (without retrigger)")
        };
	}
}
