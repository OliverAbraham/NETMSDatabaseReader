using HomenetBase;
using System.Collections.Generic;

namespace HomenetClient
{
    public class ServerLogConnector
    {
        #region ------------- Fields --------------------------------------------------------------
        private HomenetConnector _Connector;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public ServerLogConnector(string url, string userName, string password, int timeout)
        {
            _Connector = new HomenetConnector(url, userName, password, timeout);
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public List<LogItem> GetAllLogItems()
        {
            return _Connector.Get<List<LogItem>>($"api/LogItems");
        }

        #endregion
    }
}
