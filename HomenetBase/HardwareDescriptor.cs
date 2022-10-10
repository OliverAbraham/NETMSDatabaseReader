using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomenetBase
{
	public class HardwareDescriptor
	{
		public string Type { get; set; }
		public string Range { get; set; }
		public string Connector { get; set; }

		public HardwareDescriptor(string type, string range, string connector)
		{						  
			Type 	  = type;
			Range 	  = range;
			Connector = connector;
		}
	}
}
