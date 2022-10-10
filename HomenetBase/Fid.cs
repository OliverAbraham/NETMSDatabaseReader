namespace HomenetBase
{
    /// <summary>
    /// Funktions-IDs (FIDs)
    /// </summary>
    public enum Fid
    {
        System               = 0,  // 0x00,
        Config               = 1,  // 0x01,
        Terminal             = 16, // 0x10,
        Input                = 17, // 0x11,
        Analogsensor         = 32, // 0x20,
        Temperatursensor     = 33, // 0x21,
        Helligkeitssensor    = 34, // 0x22,
        Voltmeter            = 35, // 0x23,
        Digitalsensor        = 48, // 0x30,
        Digitalaktor         = 49, // 0x31,
        Fernschalter         = 50, // 0x32,
        Impulszähler         = 51, // 0x33,
        //Analogaktor          = 0x33,
        //Pop3                 = 0x34,
        //Audio                = 0xa0,
        //David_Isdn           = 0xd0,
        //PCTerm               = 0xe0,
        FidRangeEnd          = 254,
        HNServer             = 255,
        BusController        = 255,
        Unbekannt            = 170,
        Alle                 = 238, //0xee,
        EventAcknowledge     = 204, //0xCC
    };
}
