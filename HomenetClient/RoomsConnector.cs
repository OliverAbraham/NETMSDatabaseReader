using Homenet;
using System.Collections.Generic;
using System.Linq;

namespace HomenetClient
{
    public class RoomsConnector
    {
        #region ------------- Fields --------------------------------------------------------------
        private HomenetConnector _Connector;
        private List<RoomItem> _OldRooms;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public RoomsConnector(string url, string userName, string password, int timeout)
        {
            _Connector = new HomenetConnector(url, userName, password, timeout);
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        public List<RoomItem> ReadAll()
        {
            var rooms = _Connector.Get<List<RoomItem>>("api/rooms");
            _OldRooms = Clone(rooms);
            return rooms;
        }

		public List<RoomItem> Clone(List<RoomItem> rooms)
		{
            var result = new List<RoomItem>();
            foreach(var room in rooms)
                result.Add(room.Clone());
            return result;
		}

        public void UpdateAll(List<RoomItem> rooms)
        {
            foreach (var newRoom in rooms)
            {
                var oldRoom = (from r in _OldRooms where r.ID == newRoom.ID select r).FirstOrDefault();
                if (oldRoom == null)
                {
                    _Connector.Post($"api/rooms", newRoom); // Insert
                }
                else
                {
                    if (oldRoom.Description != newRoom.Description)
                        _Connector.Put($"api/rooms/{newRoom.ID}", newRoom); // Update
                }
            }

            foreach (var oldRoom in _OldRooms)
            {
                var newRoom = (from r in rooms where r.ID == oldRoom.ID select r).FirstOrDefault();
                if (newRoom == null)
                {
                    _Connector.Delete($"api/rooms/{oldRoom.ID}", oldRoom);
                }
            }
        }
        #endregion
    }
}
