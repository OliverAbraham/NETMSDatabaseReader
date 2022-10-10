using HomenetBase;
using System.Collections.Generic;

namespace HomenetClient
{
    public class SettingsConnector
    {
        #region ------------- Fields --------------------------------------------------------------
        private HomenetConnector _Connector;
        private ProgramSettings _OldSettings;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public SettingsConnector(string url, string userName, string password, int timeout)
        {
            _Connector = new HomenetConnector(url, userName, password, timeout);
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        public ProgramSettings ReadAll()
        {
            var settings = new ProgramSettings();
            settings.Items = _Connector.Get<List<SettingsItem>>("api/settings");
            _OldSettings = settings.Clone(settings);
            return settings;
        }

        public void UpdateAll(ProgramSettings settings)
        {
            for (int i=0; i < settings.Items.Count; i++)
            {
                if (_OldSettings.Items[i].Value != settings.Items[i].Value)
                {
                    int id = settings.Items[i].ID;
                    SettingsItem newValue = settings.Items[i];
                    _Connector.Put($"api/settings/{id}", newValue);
                }
            }
        }
        #endregion
    }
}
