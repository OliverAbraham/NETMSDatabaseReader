using HomenetBase;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomenetClient
{
    public class DataObjectsConnector
    {
        #region ------------- Constants and types ------------------------------------------------
        public class OnDataObjectChangeEventArgs
        {
            public DataObject Item { get; set; }
        }
        #endregion



        #region ------------- Properties ----------------------------------------------------------
        public string LastError { get; private set; }
        #endregion



        #region ------------- Fields --------------------------------------------------------------
        private HomenetConnector _Connector;
        private List<DataObject> _AllDataObjects;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public DataObjectsConnector(string url, string userName, string password, int timeout)
        {
            _Connector = new HomenetConnector(url, userName, password, timeout);
        }
        #endregion



        #region ------------- Query Methods -------------------------------------------------------
        public List<DataObject> GetAll()
        {
            var DataObjects = _Connector.Get<List<DataObject>>("api/DataObjects");
            return DataObjects;
        }

        public int TryGet_DOID_by_name(List<DataObject> allDataObjects, string name)
        {
            return (from d in allDataObjects where d.Name == name select d.DOID).FirstOrDefault();
        }

        public int Get_DOID_by_name(List<DataObject> allDataObjects, string name)
        {
            try
            {
                return (from d in allDataObjects where d.Name == name select d.DOID).FirstOrDefault();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Exists(string dataObjectName)
        {
            int DOID = DetermineDOID(dataObjectName);
            return DOID > 0;
        }

        public DataObject TryGet(string dataObjectName)
        {
            int DOID = DetermineDOID(dataObjectName);
            return TryGet(DOID);
        }

        public DataObject TryGet(int doid)
        {
            return _Connector.Get<DataObject>($"api/DataObjects/{doid}");
        }

        public DataObject Get(int doid)
        {
            try
            {
                return _Connector.Get<DataObject>($"api/DataObjects/{doid}");
            }
            catch (Exception)
            {
                return new DataObject();
            }
        }
        #endregion



        #region ------------- Insert Update Delete ------------------------------------------------
        public void Update(int doid, DataObject Do)
        {
			var dataobject = JsonConvert.SerializeObject(Do);
            var response = _Connector.Put($"api/DataObjects/{doid}", dataobject);
			if (!response.IsSuccessful)
				throw new Exception($"Communication error: Status code {response.StatusCode} from server");
        }

		public bool UpdateValueOnly(DataObject Do)
		{
            if (Do.DOID == 0)
                Do.DOID = DetermineDOID(Do.Name);

			var dataobject = JsonConvert.SerializeObject(Do);
			RestResponse response = _Connector.Put($"/api/DataObjects/value/{Do.DOID}", dataobject);
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LastError = "";
                return true;
            }
            else
            {
                LastError = response.StatusDescription;
                return false;
            }
		}

        public void Add(DataObject Do)
        {
			var dataobject = JsonConvert.SerializeObject(Do);
            var response = _Connector.Post($"api/DataObjects", dataobject);
        }

        public void Delete(int doid, DataObject @do)
        {
            _Connector.Delete($"api/DataObjects/{doid}", @do);
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------

        private int DetermineDOID(string name)
        {
            if (_AllDataObjects == null || _AllDataObjects.Count == 0)
                _AllDataObjects = GetAll();

            if (_AllDataObjects == null || _AllDataObjects.Count == 0)
                return 0;

            return TryGet_DOID_by_name(_AllDataObjects, name);
        }

        #endregion
    }
}
