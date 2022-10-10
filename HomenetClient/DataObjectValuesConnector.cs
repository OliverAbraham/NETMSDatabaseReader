using HomenetBase;
using System;
using System.Collections.Generic;

namespace HomenetClient
{
    public class DataObjectValuesConnector
    {
        #region ------------- Fields --------------------------------------------------------------
        private HomenetConnector _Connector;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public DataObjectValuesConnector(string url, string userName, string password, int timeout)
        {
            _Connector = new HomenetConnector(url, userName, password, timeout);
        }
        #endregion



        #region ------------- Query Methods -------------------------------------------------------
        public List<DataObjectValue> GetAll(List<int> doids, DateTime dateFrom, DateTime dateTo)
        {
            System.Diagnostics.Debug.WriteLine("DataObjectValuesConnector - GetAll - start");
            var parameters = $"ids={string.Join(',', doids)}&dateFrom={dateFrom.ToString("yyyy-MM-dd hh:mm:ss")}&dateTo={dateTo.ToString("yyyy-MM-dd hh:mm:ss")}";
            var query = $"api/DataObjectValues/{parameters}";
            var values = _Connector.Get<List<DataObjectValue>>(query);
            System.Diagnostics.Debug.WriteLine($"DataObjectValuesConnector - GetAll - end. {values.Count} rows");
            return values;
        }
        #endregion
    }
}
