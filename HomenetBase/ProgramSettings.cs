using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomenetBase
{
    public class ProgramSettings
    {
		#region ------------- Properties ----------------------------------------------------------

		public List<SettingsItem> Items { get; set; }

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public ProgramSettings()
        {
            Items = new List<SettingsItem>();
        }

        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public int GetInt(string name)
        {
            var Item = (from s in Items where s.Name == name select s).FirstOrDefault();
            if (Item == null)
                throw new System.Exception("not found");

            int.TryParse(Item.Value, out int value);
            return value;
        }

        public string Get(string name)
        {
            var Item = (from s in Items where s.Name == name select s).FirstOrDefault();
            if (Item == null)
                throw new System.Exception("not found");
            return Item.Value;
        }

		public ProgramSettings Clone(ProgramSettings settings)
		{
            var New = new ProgramSettings();
            foreach(var item in settings.Items)
                New.Items.Add(item.Clone());
            return New;
		}

        public override string ToString()
        {
            return $"{Items.Count} items";
        }

        #endregion
    }
}
