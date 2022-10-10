using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HomenetBase
{
    public class Device : INotifyPropertyChanged
    {
        #region ------------- Types ---------------------------------------------------------------
        public class HardwarePart
        {
            public string Type;
            public int    Number;
            public string ElectricalType;
            public string MeasurementUnit;
            public string Description;
        }
        #endregion



        #region ------------- Properties ----------------------------------------------------------

        public byte                 ID                       { get; set; }
        public string               Name                     { get; set; }
        public List<int>            Fids                     { get; set; }
		public bool                 Aktiv                    { get; set; }
        public DateTime             AktivSeit                { get; set; }
        public DateTime             InaktivSeit              { get; set; }
        public DateTime             ZeitLetzterReset         { get; set; }
        public DateTime             LetzteReaktion           { get; set; }
        public uint                 AnzahlWieOftVerschwunden { get; set; }
        public uint                 AnzahlDatenobjekte       { get; set; }
        public uint                 AnzahlFehler             { get; set; }
        public uint                 AnzahlFehlerLast         { get; set; }
        public uint                 AnzahlGesendetePakete    { get; set; }
        public uint                 AnzahlPings              { get; set; }
        public uint                 AnzahlPingsGescheitert   { get; set; }
        public uint                 AnzahlResets             { get; set; }
        public uint                 AnzahlSensoren           { get; set; }
        public uint                 EepromGröße              { get; set; }
        public bool                 Deaktiviert              { get; set; }
        public string               IP                       { get; set; }
        public HNEventType            EventType                { get; set; }

        public string FidsAsText 
        { 
            get 
            { 
                string Result = "";
                foreach (var fid in Fids)
                    Result += fid.ToString("X2") + " ";
                return Result;
            } 
        }

        public string BackgroundColor { get; set; }
		#endregion



		#region ------------- Fields --------------------------------------------------------------
		#endregion



		#region ------------- Init ----------------------------------------------------------------

		public Device()
        {
			Fids = new List<int>();
            BackgroundColor = "White";
        }

		public Device(int id, string name)
        {
			Fids = new List<int>();
            BackgroundColor = "White";
            ID = (byte)id;
            Name = name;
        }

        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public static Device FromController(Controller c)
        {
            var d = new Device();
            d.ID                       = c.ID                      ;
            d.Name                     = c.Name                    ;
            d.Fids                     = c.Fids                    ;
		    d.Aktiv                    = c.IsActive                   ;
            d.AktivSeit                = c.AktivSeit               ;
            d.InaktivSeit              = c.InaktivSeit             ;
            d.ZeitLetzterReset         = c.ZeitLetzterReset        ;
            d.LetzteReaktion           = c.LetzteReaktion          ;
            d.AnzahlWieOftVerschwunden = c.AnzahlWieOftVerschwunden;
            d.AnzahlDatenobjekte       = c.AnzahlDatenobjekte      ;
            d.AnzahlFehler             = c.AnzahlFehler            ;
            d.AnzahlFehlerLast         = c.AnzahlFehlerLast        ;
            d.AnzahlGesendetePakete    = c.AnzahlGesendetePakete   ;
            d.AnzahlPings              = c.AnzahlPings             ;
            d.AnzahlPingsGescheitert   = c.AnzahlPingsGescheitert  ;
            d.AnzahlResets             = c.AnzahlResets            ;
            d.AnzahlSensoren           = c.AnzahlSensoren          ;
            d.EepromGröße              = c.EepromGröße             ;
            d.Deaktiviert              = c.Deaktiviert             ;
            d.IP                       = c.IP                      ;
            return d;
        }

        public Device Clone()
        {
            var d = new Device();
            d.ID                       = ID                      ;
            d.Name                     = Name                    ;
            d.Fids                     = Fids                    ;
		    d.Aktiv                    = Aktiv                   ;
            d.AktivSeit                = AktivSeit               ;
            d.InaktivSeit              = InaktivSeit             ;
            d.ZeitLetzterReset         = ZeitLetzterReset        ;
            d.LetzteReaktion           = LetzteReaktion          ;
            d.AnzahlWieOftVerschwunden = AnzahlWieOftVerschwunden;
            d.AnzahlDatenobjekte       = AnzahlDatenobjekte      ;
            d.AnzahlFehler             = AnzahlFehler            ;
            d.AnzahlFehlerLast         = AnzahlFehlerLast        ;
            d.AnzahlGesendetePakete    = AnzahlGesendetePakete   ;
            d.AnzahlPings              = AnzahlPings             ;
            d.AnzahlPingsGescheitert   = AnzahlPingsGescheitert  ;
            d.AnzahlResets             = AnzahlResets            ;
            d.AnzahlSensoren           = AnzahlSensoren          ;
            d.EepromGröße              = EepromGröße             ;
            d.Deaktiviert              = Deaktiviert             ;
            d.IP                       = IP                      ;
            return d;
        }

        public void CopyPropertiesFrom(Device d)
        {
            Name                     = d.Name                    ;
            Fids                     = d.Fids                    ;
		    Aktiv                    = d.Aktiv                   ;
            AktivSeit                = d.AktivSeit               ;
            InaktivSeit              = d.InaktivSeit             ;
            ZeitLetzterReset         = d.ZeitLetzterReset        ;
            LetzteReaktion           = d.LetzteReaktion          ;
            AnzahlWieOftVerschwunden = d.AnzahlWieOftVerschwunden;
            AnzahlDatenobjekte       = d.AnzahlDatenobjekte      ;
            AnzahlFehler             = d.AnzahlFehler            ;
            AnzahlFehlerLast         = d.AnzahlFehlerLast        ;
            AnzahlGesendetePakete    = d.AnzahlGesendetePakete   ;
            AnzahlPings              = d.AnzahlPings             ;
            AnzahlPingsGescheitert   = d.AnzahlPingsGescheitert  ;
            AnzahlResets             = d.AnzahlResets            ;
            AnzahlSensoren           = d.AnzahlSensoren          ;
            EepromGröße              = d.EepromGröße             ;
            Deaktiviert              = d.Deaktiviert             ;
            IP                       = d.IP                      ;
        }

        public bool HasDifferencesTo(Device oldItem)
        {
            return
            oldItem.ID                       != ID                       ||
            oldItem.Name                     != Name                     ||
            oldItem.Fids                     != Fids                     ||
		    oldItem.Aktiv                    != Aktiv                    ||
            //oldItem.AktivSeit                != AktivSeit                ||
            //oldItem.InaktivSeit              != InaktivSeit              ||
            //oldItem.ZeitLetzterReset         != ZeitLetzterReset         ||
            //oldItem.LetzteReaktion           != LetzteReaktion           ||
            //oldItem.AnzahlWieOftVerschwunden != AnzahlWieOftVerschwunden ||
            //oldItem.AnzahlDatenobjekte       != AnzahlDatenobjekte       ||
            //oldItem.AnzahlFehler             != AnzahlFehler             ||
            //oldItem.AnzahlFehlerLast         != AnzahlFehlerLast         ||
            //oldItem.AnzahlGesendetePakete    != AnzahlGesendetePakete    ||
            //oldItem.AnzahlPings              != AnzahlPings              ||
            //oldItem.AnzahlPingsGescheitert   != AnzahlPingsGescheitert   ||
            //oldItem.AnzahlResets             != AnzahlResets             ||
            //oldItem.AnzahlSensoren           != AnzahlSensoren           ||
            //oldItem.EepromGröße              != EepromGröße              ||
            oldItem.Deaktiviert              != Deaktiviert              ||
            oldItem.IP                       != IP                       ;
        }

        public override string ToString()
        {
            return String.Format("ID {0} Name {1} Aktiv {2}", ID, Name, Aktiv);
        }

    #endregion



        #region ------------- Implementation ------------------------------------------------------
        #endregion



        #region ------------- INotifyPropertyChanged ----------------------------------------------

        [NonSerialized]
        private PropertyChangedEventHandler _PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _PropertyChanged += value;
            }
            remove
            {
                _PropertyChanged -= value;
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler Handler = _PropertyChanged; // avoid race condition
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
