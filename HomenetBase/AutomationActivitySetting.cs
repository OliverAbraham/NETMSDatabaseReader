namespace Homenet
{
	public class AutomationActivitySetting
	{
        public enum ID
        {
            Inactive      = 0,
            Active        = 1,
            ByTimer       = 2,
            OnlyAtNight   = 3,
            OnlyByDay     = 4,
        }

        public static ActivityState[] States = new ActivityState[] 
		{
			new ActivityState(ID.Inactive     , "Inactive"      ),
			new ActivityState(ID.Active       , "Active"        ),
			new ActivityState(ID.ByTimer      , "By timer"      ),
			new ActivityState(ID.OnlyAtNight  , "Only at night" ),
			new ActivityState(ID.OnlyByDay    , "Only by day"   )
		};

		public class ActivityState
		{
			public ID ID;
			public string Description;
			
            public ActivityState(ID id, string description)
            {
                ID = id;
                Description = description;
            }
		}
	}
}
