using System;
using System.Collections.Generic;
using System.Text;

namespace HomenetBase
{
    /// <summary>
    /// Attribute einer FID10-Tastatur
    /// </summary>
    public enum KeyboardType
    {
        IR              = 0x02,
        PCKBD           = 0x03,
        SimpelKeyboard  = 0x05,
        Kein            = 0x00,
        Unbekannt       = 0xFF
    };
}
