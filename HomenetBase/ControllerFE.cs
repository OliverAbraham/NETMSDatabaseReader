//-----------------------------------------------------------------------------
//
//                               HAUSNET SERVER 2
//
//                                Oliver Abraham
//                               Abraham Beratung
//                            mail@oliver-abraham.de
//                             www.oliver-abraham.de
//
//-----------------------------------------------------------------------------
//
//       EIN CONTROLLER, DAS BAUTEIL DAS SENSOREN UND AKTOREN BEREITSTELLT
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace HomenetBase
{
	// Parameter für FID 10 Controller
	public struct KeyboardParameter
    {
        public KeyboardType Typ;
        public uint AnzahlTasten;
    };
    
    // Parameter für FID 10 Controller
    public class DisplayParameter
    {
        public DisplayType Typ;
        public uint Zeilen;
        public uint Spalten;

        public DisplayParameter()
        {
            Typ = DisplayType.Unbekannt;
            Zeilen = 0;
            Spalten = 0;
        }
    };

    public class ControllerFE : Controller
    {
        #region ------------- Types ---------------------------------------------------------------
        #endregion



        #region ------------- Properties ----------------------------------------------------------

        public bool                 Phase4Erledigt;             // Hilfsvariable für die Initialisierungssequenz (Phase 4)

        public bool                 WurdeResettet;              // Hilfsvariable für die Initialisierungssequenz (Phase 4)
        public bool                 HatAufResetReagiert;        // Hilfsvariable für die Initialisierungssequenz (Phase 4)
        public KeyboardParameter    Keyboard;
        public DisplayParameter     Display;
        public bool                 Phase5Erledigt;             // Hilfsvariable für die Initialisierungssequenz (Phase 5)
        public ITreiber             Treiber; 
        public string               FidsAsString
        {
            get 
            {   
                string x=""; 
                if (Fids != null && Fids.Count > 0)
                    foreach (byte fid in Fids) 
                        x += fid.ToString("X")+" "; 
                return x.TrimEnd(' '); 
            }
        }

        public Fid        MainFid
        {
            get
            {
                if (Fids == null || Fids.Count == 0)
                    return Fid.Unbekannt;
                else
                    return (Fid)Fids[0];
            }
        }

		public string				TypBezeichung 
        { 
            get 
            { 
                return MainFid.ToString("G"); 
            } 
        }

        public int					AktivSeit_in_Minuten
        { 
            get 
            { 
                return BerechneAlterInMinuten(AktivSeit);
            } 
        }

        public int					InaktivSeit_in_Minuten
        { 
            get 
            { 
                return BerechneAlterInMinuten(InaktivSeit);
            } 
        }

        public string               AktivInaktivStatus
        {
            get
            {
                return IsActive ? $"{AktivSeit} ({AktivSeit_in_Minuten})" : $"{InaktivSeit} ({InaktivSeit_in_Minuten})";
            }
        }

        public int					LetzteReaktion_in_Minuten
        { 
            get 
            { 
                return BerechneAlterInMinuten(LetzteReaktion);
            } 
        }

        public string               LetzteReaktionStatus
        {
            get
            {
                return $"{LetzteReaktion.ToString()}   (vor {LetzteReaktion_in_Minuten} min)";
            }
        }

        public string               Capabilities;
        public bool					IsEthernetController { get { return !string.IsNullOrWhiteSpace(IP); } }
		public string				AktuellerStatus { get; set; }

		public List<HardwarePart>	Parts;
		public byte					Port { get; set; }

        public string               TreiberStatus
        {
            get
            {
                return (Treiber != null) ? Treiber.GetType().ToString() : "";
            }
        }

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public ControllerFE() : base()
        {
			AktuellerStatus = "";
        }

        public ControllerFE(byte id, string name)
        {
            ID   = id;
            Name = name;
			AktuellerStatus = "";
        }

        public ControllerFE(byte id, string name, List<int> fids)
        {
            ID   = id;
            Name = name;
            Fids = fids;
			AktuellerStatus = "";
        }

		public ControllerFE(Controller c)
		{
            ID                        = c.ID;
            Name                      = c.Name;
            Fids                      = c.Fids;
            IsActive                     = c.IsActive;
            AktivSeit                 = c.AktivSeit;
            InaktivSeit               = c.InaktivSeit;
            LetzteReaktion            = c.LetzteReaktion;
            ZeitLetzterReset          = c.ZeitLetzterReset;
            AnzahlWieOftVerschwunden  = c.AnzahlWieOftVerschwunden;
            AnzahlDatenobjekte        = c.AnzahlDatenobjekte;
            AnzahlFehler              = c.AnzahlFehler;
            AnzahlFehlerLast          = c.AnzahlFehlerLast;
            AnzahlGesendetePakete     = c.AnzahlGesendetePakete;
            AnzahlPings               = c.AnzahlPings;
            AnzahlPingsGescheitert    = c.AnzahlPingsGescheitert;
            AnzahlResets              = c.AnzahlResets;
            AnzahlSensoren            = c.AnzahlSensoren;
            EepromGröße               = c.EepromGröße;
            IP						  = c.IP;

            Init(ID, Name);
		}
        
        protected void Init(byte id, string name)
        {
            if (Fids == null)
            	Fids                    = new List<int>();
            ID                      = id;
            Name                    = name;
            IsActive                   = true;
            AktivSeit               = DateTime.Now;
            InaktivSeit             = new DateTime(9999, 12, 31, 0, 0, 0);
            ZeitLetzterReset        = DateTime.Now;
            LetzteReaktion          = DateTime.Now;
            WurdeResettet           = false;
            Phase4Erledigt          = false;
            Phase5Erledigt          = false;
            //Treiber                 = new GenericTreiber();
			AktuellerStatus = "";
        }

        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public static Device FromController(ControllerFE ctr)
        {
            var New                      = new Device();
            New.ID                       = ctr.ID;
            New.Name                     = ctr.Name;
            New.Fids                     = ctr.Fids;
            New.Aktiv                    = ctr.IsActive;
            New.AktivSeit                = ctr.AktivSeit;
            New.InaktivSeit              = ctr.InaktivSeit;
            New.ZeitLetzterReset         = ctr.ZeitLetzterReset;
            New.LetzteReaktion           = ctr.LetzteReaktion;
            New.AnzahlWieOftVerschwunden = ctr.AnzahlWieOftVerschwunden;
            New.AnzahlDatenobjekte       = ctr.AnzahlDatenobjekte;
            New.AnzahlFehler             = ctr.AnzahlFehler;
            New.AnzahlFehlerLast         = ctr.AnzahlFehlerLast;
            New.AnzahlGesendetePakete    = ctr.AnzahlGesendetePakete;
            New.AnzahlPings              = ctr.AnzahlPings;
            New.AnzahlPingsGescheitert   = ctr.AnzahlPingsGescheitert;
            New.AnzahlResets             = ctr.AnzahlResets;
            New.AnzahlSensoren           = ctr.AnzahlSensoren;
            New.EepromGröße              = ctr.EepromGröße;
            New.Deaktiviert              = ctr.Deaktiviert;
            New.IP                       = ctr.IP;
            return New;
        }

        public void DecodeCapabilities()
        {
            if (string.IsNullOrWhiteSpace(Capabilities))
                return;

            string[] Lines = Capabilities.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Name = GetAttribute(Lines, "Name");

            string FidAsString = GetAttribute(Lines, "Fid");
			if (!string.IsNullOrWhiteSpace(FidAsString))
			{
	            int Fid;
				Fid = Convert.ToInt32(FidAsString, 16);
				Fids.Clear();
				Fids.Add(Fid);
			}

            Parts = new List<HardwarePart>();
            DecodePartLists(Lines, "Inputs", "Input");
            DecodePartLists(Lines, "Outputs", "Output");
        }

        private void DecodePartLists(string[] Lines, string attributeName1, string attributeName2)
        {
            int? Count = GetIntAttribute(Lines, attributeName1);
            if (Count != null && Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    string PartDescription = GetAttribute(Lines, $"{attributeName2}{i}");
                    string[] Values = PartDescription.Split(new char[] { ',', '\r', '\n' });
                    if (Values.GetLength(0) >= 3)
                    {
                        var Part             = new HardwarePart();
                        Part.Type            = "Input";
                        Part.Number          = i;
                        Part.ElectricalType  = Values[0];
                        Part.MeasurementUnit = Values[1];
                        Part.Description     = Values[2];
                        Parts.Add(Part);
                    }
                }
            }
        }

        private int? GetIntAttribute(string[] lines, string name)
        {
            string ValueAsString = GetAttribute(lines, name);
            int value;
            if (int.TryParse(ValueAsString, out value))
                return value;
            else
                return null;
        }

        private string GetAttribute(string[] lines, string name)
        {
            string SearchTerm = name + "=";

            foreach (var Line in lines)
            {
                if (Line.StartsWith(SearchTerm))
                {
                    return Line.Substring(SearchTerm.Length);
                }
            }
            return "";
        }

        public ControllerFE Clone(ControllerFE original)
        {
            ControllerFE Kopie = new ControllerFE(original.ID, original.Name);
            Kopie.ID                        = original.ID;
            Kopie.Name                      = original.Name;
            Kopie.Fids                      = original.Fids;
            Kopie.IsActive                     = original.IsActive;
            Kopie.AktivSeit                 = original.AktivSeit;
            Kopie.InaktivSeit               = original.InaktivSeit;
            Kopie.LetzteReaktion            = original.LetzteReaktion;
            Kopie.ZeitLetzterReset          = original.ZeitLetzterReset;
            Kopie.AnzahlWieOftVerschwunden  = original.AnzahlWieOftVerschwunden;
            Kopie.AnzahlDatenobjekte        = original.AnzahlDatenobjekte;
            Kopie.AnzahlFehler              = original.AnzahlFehler;
            Kopie.AnzahlFehlerLast          = original.AnzahlFehlerLast;
            Kopie.AnzahlGesendetePakete     = original.AnzahlGesendetePakete;
            Kopie.AnzahlPings               = original.AnzahlPings;
            Kopie.AnzahlPingsGescheitert    = original.AnzahlPingsGescheitert;
            Kopie.AnzahlResets              = original.AnzahlResets;
            Kopie.AnzahlSensoren            = original.AnzahlSensoren;
            Kopie.EepromGröße               = original.EepromGröße;
            Kopie.Phase4Erledigt            = original.Phase4Erledigt;             
            Kopie.WurdeResettet             = original.WurdeResettet;              
            Kopie.HatAufResetReagiert       = original.HatAufResetReagiert;        
            Kopie.Keyboard                  = original.Keyboard;
            Kopie.Display                   = original.Display;
            Kopie.Phase5Erledigt            = original.Phase5Erledigt;
            Kopie.Treiber                   = original.Treiber;
            Kopie.IP						= original.IP;
            return Kopie;
        }

		public Controller ToController()
		{
			var c = new	Controller();
            c.ID                        = ID;
            c.Name                      = Name;
            c.Fids                      = Fids;
            c.IsActive                     = IsActive;
            c.AktivSeit                 = AktivSeit;
            c.InaktivSeit               = InaktivSeit;
            c.LetzteReaktion            = LetzteReaktion;
            c.ZeitLetzterReset          = ZeitLetzterReset;
            c.AnzahlWieOftVerschwunden  = AnzahlWieOftVerschwunden;
            c.AnzahlDatenobjekte        = AnzahlDatenobjekte;
            c.AnzahlFehler              = AnzahlFehler;
            c.AnzahlFehlerLast          = AnzahlFehlerLast;
            c.AnzahlGesendetePakete     = AnzahlGesendetePakete;
            c.AnzahlPings               = AnzahlPings;
            c.AnzahlPingsGescheitert    = AnzahlPingsGescheitert;
            c.AnzahlResets              = AnzahlResets;
            c.AnzahlSensoren            = AnzahlSensoren;
            c.EepromGröße               = EepromGröße;
            c.IP						= IP;
			return c;
		}

		public void SetResetTimestamp()
        {
            ZeitLetzterReset = DateTime.Now;
        }

        public override string ToString()
        {
            return String.Format("ID {0} Name {1} Fids {2} Aktiv {3} Treiber {4}", ID, Name, FidsAsString, IsActive, Treiber);
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------

        private int BerechneAlterInMinuten(DateTime datum)
        {
            TimeSpan Alter;
            if (DateTime.Now > datum)
                Alter = DateTime.Now - datum;
            else
                Alter = new TimeSpan(0);
           return Convert.ToInt32(Math.Floor(Alter.TotalMinutes));
        }

        #endregion
    }
}
