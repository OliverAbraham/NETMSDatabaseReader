using System;

namespace HomenetBase
{
    public class SettingsItem
    {
        #region ------------- Properties ----------------------------------------------------------
        public int    ID    { get; set; }
        public string Name  { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public SettingsItem()
        {
        }

        public SettingsItem(int iD, string name, string value)
        {
            ID    = iD;
            Name  = name;
            Value = value;
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        public override string ToString()
        {
            return $"{ID}     {Name}     ({Value})";
        }

        public SettingsItem Clone()
        {
            var New = new SettingsItem();
            New.ID    = ID;
            New.Name  = Name;
            New.Value = Value;
            return New;
        }
        #endregion
    }
}
