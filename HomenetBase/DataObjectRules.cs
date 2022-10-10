//-----------------------------------------------------------------------------
//
//                               HAUSNET SERVER 2
//
//                                Oliver Abraham
//                               Abraham Beratung
//                            mail@oliver-abraham.de
//                             www.oliver-abraham.de
//
//
//  Der Teil "Serial Ports" basiert auf dem Quellcode von Timothy J. Krell.
//  
//  Die Architektur basiert auf den Quellcodes HNServer, HNTreiber und 
//  HNGateway von Andreas Schultze (Elk Datensysteme) und Oliver Abraham.
//
//  Neuerstellung in 10/2009
//-----------------------------------------------------------------------------
//
//                  DAS DATENOBJEKT, DIE ZENTRALE ENTITÄT DES SYSTEMS
//
//-----------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HomenetBase
{
	public class DataObjectRules
    {
        #region ------------- Properties ----------------------------------------------------------

        public List<Rule> Rules { get; set; }
        public DateTime LastUpdate { get; set; }

        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public DataObjectRules()
        {
            Rules = new List<Rule>();
        }

        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public string Serialize()
        {
            string json = JsonSerializer.Serialize(Rules);
            return json;
        }

        public void Deserialize(string json)
        {
            if (json == null)
                Rules = new List<Rule>();
            else
                Rules = JsonSerializer.Deserialize<List<Rule>>(json);
        }

        public override string ToString()
        {
            return $"{Rules.Count} rules";
        }

        #endregion
	}
}
