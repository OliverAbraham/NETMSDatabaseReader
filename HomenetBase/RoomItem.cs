using System;

namespace Homenet
{
	public class RoomItem
	{
		public int ID { get; set; }
		public string Description { get; set; }

		public RoomItem()
		{
		}

		public RoomItem(int id, string description)
		{
			ID = id;
			Description = description;
		}

		public override string ToString()
		{
			return $"{ID} {Description}";
		}

		public RoomItem Clone()
		{
			var result = new RoomItem();
			result.ID = this.ID;
			result.Description = this.Description;
			return result;
		}
	}
}
