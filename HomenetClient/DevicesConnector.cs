using HomenetBase;
using System.Linq;
using System.Collections.Generic;

namespace HomenetClient
{
    public class DevicesConnector
    {
        #region ------------- Fields --------------------------------------------------------------

        private HomenetConnector _Connector;
        private List<Device> _OldItems;

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public DevicesConnector(string url, string userName, string password, int timeout)
        {
            _Connector = new HomenetConnector(url, userName, password, timeout);
        }

        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public List<Device> GetAll()
        {
            var devices = _Connector.Get<List<Device>>("api/Devices");
            _OldItems = Clone(devices);
            return devices;
        }

		public List<Device> Clone(List<Device> devices)
		{
            var result = new List<Device>();
            foreach(var device in devices)
                result.Add(device.Clone());
            return result;
		}

        public Device Get(int id)
        {
            var DataObject = _Connector.Get<Device>($"api/Devices/{id}");
            return DataObject;
        }

        public void Update(int id, Device @do)
        {
            _Connector.Post($"api/Devices/{id}", @do);
        }

        public void Add(int id, Device @do)
        {
            _Connector.Put($"api/Devices/{id}", @do);
        }

        public void Delete(int id, Device @do)
        {
            _Connector.Delete($"api/Devices/{id}", @do);
        }

		public void UpdateAll(List<Device> items)
        {
            foreach (var item in items)
            {
                var oldItem = (from r in _OldItems where r.ID == item.ID select r).FirstOrDefault();
                if (oldItem == null)
                {
                    _Connector.Post($"api/devices", item); // Insert
                }
                else
                {
                    if (item.HasDifferencesTo(oldItem))
                        _Connector.Put($"api/devices/{item.ID}", item); // Update
                }
            }

            foreach (var oldItem in _OldItems)
            {
                var newRoom = (from r in items where r.ID == oldItem.ID select r).FirstOrDefault();
                if (newRoom == null)
                {
                    _Connector.Delete($"api/devices/{oldItem.ID}", oldItem);
                }
            }
        }

		#endregion
	}
}
