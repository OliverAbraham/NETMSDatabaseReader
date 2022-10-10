namespace Homenet
{
	public class DataObjectClasses
	{
        public enum ID
        {
            Unbekannt     = 99,
            Digital       = 1,
            Digitalsensor = 1,
            Analog        = 2,
            Analogsensor  = 2,
            Digitalaktor  = 3,
            Analogaktor   = 4,
            Impulszähler  = 5,
            Internet      = 6,
        }

        public class DataObjectClass
        {
            public ID ID;
            public string Description;

            public DataObjectClass(ID id, string description)
            {
                ID = id;
                Description = description;
            }
        }

        public static DataObjectClass[] Classes = new DataObjectClass[] 
        {
            new DataObjectClass(ID.Unbekannt    , "Unknown"        ),
            new DataObjectClass(ID.Digitalsensor, "Digital sensor" ),
            new DataObjectClass(ID.Analogsensor , "Analog sensor"  ),
            new DataObjectClass(ID.Digitalaktor , "Digital actor"  ),
            new DataObjectClass(ID.Analogaktor  , "Analog actor"   ),
            new DataObjectClass(ID.Impulszähler , "Impulse counter" ),
            new DataObjectClass(ID.Internet     , "Internet"       )
        };


        public int IndexZuTyp(ID id)
        {
            int Index = 0;
            foreach (var item in Classes)
            {
                if (item.ID == id)
                    return Index;
                Index++;
            }
            return -1;
        }
	}
}
