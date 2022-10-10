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
	public class Controller
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

        public byte                 ID;
        public string               Name;
        public List<int>            Fids;
		public bool                 IsActive;
        public DateTime             AktivSeit;
        public DateTime             InaktivSeit;
        public DateTime             ZeitLetzterReset;
        public DateTime             LetzteReaktion;
        public uint                 AnzahlWieOftVerschwunden;
        public uint                 AnzahlDatenobjekte;
        public uint                 AnzahlFehler;
        public uint                 AnzahlFehlerLast;
        public uint                 AnzahlGesendetePakete;
        public uint                 AnzahlPings;
        public uint                 AnzahlPingsGescheitert;
        public uint                 AnzahlResets;
        public uint                 AnzahlSensoren;
        public uint                 EepromGröße;
        public bool                 Deaktiviert;
        public string               IP;

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public Controller()
        {
			Fids = new List<int>();
        }

        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public override string ToString()
        {
            return String.Format("ID {0} Name {1} Aktiv {2}", ID, Name, IsActive);
        }

        #endregion
    }
}
