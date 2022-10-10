namespace Homenet
{
	public class DataObjectState
	{
        public enum ID
        {
            Active   = 0,
            Inactive = 1
        }

        public static State[] States = new State[] 
		{
			new State(ID.Active  , "Active"  ),
			new State(ID.Inactive, "Inactive") 
		};

		public class State
		{
			public ID ID;
			public string Description;

            public State(ID id, string description)
            {
                ID = id;
                Description = description;
            }
		}
	}
}
