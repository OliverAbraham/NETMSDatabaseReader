namespace Homenet
{
    public class AutomationActions
	{
        public enum ID
        {
            Ein           = 0,
            Aus           = 1,
            Email         = 2,
            DosCommand    = 3,
            PushbulletAPI = 4
        }

        public class AutomationAction
        {
            public ID ID;
            public string Description;

            public AutomationAction(ID id, string description)
            {
                ID = id;
                Description = description;
            }
        }

        public static AutomationAction[] Actions = new AutomationAction[] 
		{
            new AutomationAction(ID.Ein          , "DataObject On"),
			new AutomationAction(ID.Aus          , "DataObject Off"),
            new AutomationAction(ID.Email        , "Send EMail"),
            new AutomationAction(ID.DosCommand   , "Execute DOS command"),
            new AutomationAction(ID.PushbulletAPI, "Send Pushbullet API command")
		};
	}
}
