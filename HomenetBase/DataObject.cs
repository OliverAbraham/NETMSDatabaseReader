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

using Homenet;
using System;
using System.Xml.Serialization;

namespace HomenetBase
{
    [Serializable()]
    public class DataObject
    {
        #region ------------- Types and constants -------------------------------------------------

        public enum DataObjectType
        {
            Unknown        = 99,
            Digital        = 1,
            Digitalsensor  = 1,
            Analog         = 2,
            Analogsensor   = 2,
            Digitalactor   = 3,
            Analogactor    = 4,
            ImpulseCounter = 5,
            Internet       = 6,
        }

        #endregion



        #region ------------- Properties ----------------------------------------------------------

        public string           Version                     { get; set; }
        public int              DOID                        { get; set; }
        public string           Name                        { get; set; }
        public string           PrettyName                  { get; set; }
        public byte             CtrID                       { get; set; }
        public byte             ID                          { get; set; }
        public string           OldValue                    { get; set; }
        public DateTime         Timestamp                   { get; set; }
        public uint             AutoOff                     { get; set; }
        public DateTime         AutoOffTimestamp            { get; set; }
        public uint             Watt                        { get; set; }
        public uint             ActivatedTotalSeconds       { get; set; }
        public DataObjectType   Type                        { get; set; }
        public string           Unit                        { get; set; }
        public string           ValueRange                  { get; set; }
        public string           Resolution                  { get; set; }
        public uint             Room                        { get; set; }
        public uint             State                       { get; set; }
        public uint             AutomationState             { get; set; }
        public bool             HasTimer                    { get; set; }
		public string           TimerSettings               { get; set; }
		public string           AutomationTimerSettings     { get; set; }
        public string           RuleSettings                { get; set; }
        public int              Threshold                   { get; set; }
        public int              Inertia                     { get; set; }
        public bool             OffTime                     { get; set; }
        public string           Value 
        { 
            get 
            { 
                return _Value; 
            } 
            set 
            { 
                _Value = value;
                
                // Auto-Aus-Timer abschalten, wenn das Datenobjekt abgeschaltet wird
                if (_Value == "0" && AutoOff > 0)
                    AutoOff = 0;

                // Gesamte "Ein"-Zeit aufsummieren
                if (_Value == "0" && OldValue == "1")
                    ActivatedTotalSeconds += Age;
            } 
        }
        public double           ValueOriginal               { get; set; }
        /// <summary>
        /// Parameter for impulse counter, absolute offset for sensor value
        /// </summary>
        public double           Offset                      { get; set; }
        /// <summary>
        /// Parameter for impulse counter, used to calculate the count value from measured value.
        /// (for example 100 pulses a equivalent to 1 cbm gas)
        /// </summary>
        public int              Divider                     { get; set; }
        /// <summary>
        /// Parameter for impulse counter
        /// </summary>
        public int              Decimals                    { get; set; }
        public string           ValueDescriptions           { get; set; }
        #endregion



        #region ------------- Internals, not saved ------------------------------------------------
		[XmlIgnore]
		public object           RuleProcessingDynamicData   { get; set; }

        [XmlIgnore]
        public int              InertiaOld;

        [XmlIgnore]
        public object		    LowPassFilter;

        [XmlIgnore]
        public string           StateDescription { get { return FormatValue(Value); } set { } }

        /// <summary>
        /// Returns the age of the object in seconds (since last value change).
        /// </summary>
        [XmlIgnore]
        public uint             Age
        {
            get
            {
                try
                {
                    TimeSpan tsAlter = DateTime.Now - Timestamp;
                    if (tsAlter.TotalSeconds >= 0)
                        return Convert.ToUInt32(tsAlter.TotalSeconds);
                    else
                        return 0;
                }
                catch (Exception)
                {
                    return 999999999;
                }
            }
        }

        [XmlIgnore]
        public bool             StateIsActive
        {
            get
            {
                return (State == (uint)DataObjectState.ID.Active);
            }
        }
        #endregion



        #region ------------- Fields --------------------------------------------------------------

        private string          _Value;
        public DateTime         _LastChangeFromInside;
        public DateTime         _LastUpdateFromWebApi;

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public DataObject()
        {
            Type = DataObjectType.Unknown;
            _LastUpdateFromWebApi = new DateTime();
        }

        public DataObject(string name, byte id, byte ctrID, string value)
        {
            Name                  = name;
            PrettyName            = name;
            CtrID                 = ctrID;
            ID                    = id;
            _Value                = value;
            AutoOff               = 0;
            OldValue              = "0";
            Type                  = DataObjectType.Unknown;
            _LastUpdateFromWebApi = new DateTime();
        }

		#endregion



		#region ------------- Methods -------------------------------------------------------------

        public string FormatValue(string value)
        {
            if (!string.IsNullOrWhiteSpace(ValueDescriptions))
            {
                var temp = "|" + ValueDescriptions + "|";
                var valueDefinitions = temp.Split(new char[]{'|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var def in valueDefinitions)
                { 
                    var parts = def.Split('=');
                    if (parts.GetLength(0) == 2)
                    {
                        var val = parts[0];
                        var description = parts[1];
                        if (val == value)
                            return description;
                    }
                }
            }


            if (Type == DataObjectType.ImpulseCounter)
            {
                if (Decimals > 0 && Value.Length >= Decimals)
                    return Value.Insert(Value.Length-Decimals, ",");
            }


            if (Type != DataObjectType.Analogsensor && 
                Type != DataObjectType.ImpulseCounter)
            {
                if (Value == "0")
                    return "Off";
                else if (Value == "1")
                    return "On";
            }

            if (Value == null)
                return "";

            if (!string.IsNullOrWhiteSpace(Unit))
                return Value + " " + Unit;

            if (Decimals > 0 && Value.Length >= Decimals)
                return Value.Insert(Value.Length-Decimals, ",");

            if (!string.IsNullOrWhiteSpace(Unit))
                return Value + " " + Unit;

            return Value;
        }

        public override string ToString()
        {
            return $"{Name} Value={Value} PrettyName={PrettyName} Age={Age}";
        }

        public static bool IsMeasuringSensor(DataObjectType type)
        {
            return (type == DataObjectType.Analog ||
					type == DataObjectType.Analogsensor ||
					type == DataObjectType.ImpulseCounter);
        }

        public bool AnyAutomationIsActive()
        {
            return StateIsActive && 
                (AutomationState > 0 /*|| AutoOffPreset > 0 || Weihnachtsbeleuchtung */);
        }

		public bool ConsumesEnergy()
		{
			return Type != DataObjectType.ImpulseCounter &&
                   Type != DataObjectType.Internet;
		}

		public bool IsACounter()
		{
			return Type == DataObjectType.ImpulseCounter;
		}

		public bool IsSwitchable()
		{
            if (Type == DataObjectType.Internet)
                return false;
            else
				return !IsSensor() && !IsACounter();
		}

        public bool IsSensor()
        {
            return (Type == DataObjectType.Analogsensor ||
                    Type == DataObjectType.Digitalsensor);
        }

        #endregion



        #region ------------- ICloneable and other copy methods -----------------------------------
    
        public void CopyPropertiesFrom(DataObject source)
        {
            if (Name                          != source.Name                         ) { Name                    = source.Name                   ; }
            if (PrettyName                    != source.PrettyName                   ) { PrettyName              = source.PrettyName             ; }
            if (CtrID                         != source.CtrID                        ) { CtrID                   = source.CtrID                  ; }
            if (ID                            != source.ID                           ) { ID                      = source.ID                     ; }
            if (Timestamp                     != source.Timestamp                    ) { Timestamp               = source.Timestamp              ; }
            if (AutoOff                       != source.AutoOff                      ) { AutoOff                 = source.AutoOff                ; }
            if (AutoOffTimestamp              != source.AutoOffTimestamp             ) { AutoOffTimestamp        = source.AutoOffTimestamp       ; }
            if (Watt                          != source.Watt                         ) { Watt                    = source.Watt                   ; }
            if (ActivatedTotalSeconds         != source.ActivatedTotalSeconds        ) { ActivatedTotalSeconds   = source.ActivatedTotalSeconds  ; }
            if (Type                          != source.Type                         ) { Type                    = source.Type                   ; }
            if (Unit                          != source.Unit                         ) { Unit                    = source.Unit                   ; }
            if (ValueRange                    != source.ValueRange                   ) { ValueRange              = source.ValueRange             ; }
            if (Resolution                    != source.Resolution                   ) { Resolution              = source.Resolution             ; }
            if (Room                          != source.Room                         ) { Room                    = source.Room                   ; }
            if (State                         != source.State                        ) { State                   = source.State                  ; }
            if (AutomationState               != source.AutomationState              ) { AutomationState         = source.AutomationState        ; }
            if (HasTimer                      != source.HasTimer                     ) { HasTimer                = source.HasTimer               ; }
            if (TimerSettings                 != source.TimerSettings                ) { TimerSettings           = source.TimerSettings          ; }
            if (AutomationTimerSettings       != source.AutomationTimerSettings      ) { AutomationTimerSettings = source.AutomationTimerSettings; }
            if (RuleSettings                  != source.RuleSettings                 ) { RuleSettings            = source.RuleSettings           ; }
            if (Threshold                     != source.Threshold                    ) { Threshold               = source.Threshold              ; }
            if (Inertia                       != source.Inertia                      ) { Inertia                 = source.Inertia                ; }
            if (OffTime                       != source.OffTime                      ) { OffTime                 = source.OffTime                ; }
            if (Value                         != source.Value                        ) { Value                   = source.Value                  ; }
            if (ValueOriginal                 != source.ValueOriginal                ) { ValueOriginal           = source.ValueOriginal          ; }
            if (Offset                        != source.Offset                       ) { Offset                  = source.Offset                 ; }
            if (Divider                       != source.Divider                      ) { Divider                 = source.Divider                ; }
            if (Decimals                      != source.Decimals                     ) { Decimals                = source.Decimals               ; }
            if (ValueDescriptions             != source.ValueDescriptions            ) { ValueDescriptions       = source.ValueDescriptions      ; }
            _LastUpdateFromWebApi = source._LastUpdateFromWebApi;
        }
    
        public void CopyAllPropertiesForClone(DataObject source)
        {
            if (DOID                          != source.DOID                         ) { DOID                    = source.DOID                   ; }
            if (Name                          != source.Name                         ) { Name                    = source.Name                   ; }
            if (PrettyName                    != source.PrettyName                   ) { PrettyName              = source.PrettyName             ; }
            if (CtrID                         != source.CtrID                        ) { CtrID                   = source.CtrID                  ; }
            if (ID                            != source.ID                           ) { ID                      = source.ID                     ; }
            if (OldValue                      != source.OldValue                     ) { OldValue                = source.OldValue               ; }
            if (Timestamp                     != source.Timestamp                    ) { Timestamp               = source.Timestamp              ; }
            if (AutoOff                       != source.AutoOff                      ) { AutoOff                 = source.AutoOff                ; }
            if (AutoOffTimestamp              != source.AutoOffTimestamp             ) { AutoOffTimestamp        = source.AutoOffTimestamp       ; }
            if (Watt                          != source.Watt                         ) { Watt                    = source.Watt                   ; }
            if (ActivatedTotalSeconds         != source.ActivatedTotalSeconds        ) { ActivatedTotalSeconds   = source.ActivatedTotalSeconds  ; }
            if (Type                          != source.Type                         ) { Type                    = source.Type                   ; }
            if (Unit                          != source.Unit                         ) { Unit                    = source.Unit                   ; }
            if (ValueRange                    != source.ValueRange                   ) { ValueRange              = source.ValueRange             ; }
            if (Resolution                    != source.Resolution                   ) { Resolution              = source.Resolution             ; }
            if (Room                          != source.Room                         ) { Room                    = source.Room                   ; }
            if (State                         != source.State                        ) { State                   = source.State                  ; }
            if (AutomationState               != source.AutomationState              ) { AutomationState         = source.AutomationState        ; }
            if (HasTimer                      != source.HasTimer                     ) { HasTimer                = source.HasTimer               ; }
            if (TimerSettings                 != source.TimerSettings                ) { TimerSettings           = source.TimerSettings          ; }
            if (AutomationTimerSettings       != source.AutomationTimerSettings      ) { AutomationTimerSettings = source.AutomationTimerSettings; }
            if (RuleSettings                  != source.RuleSettings                 ) { RuleSettings            = source.RuleSettings           ; }
            if (Threshold                     != source.Threshold                    ) { Threshold               = source.Threshold              ; }
            if (Inertia                       != source.Inertia                      ) { Inertia                 = source.Inertia                ; }
            if (OffTime                       != source.OffTime                      ) { OffTime                 = source.OffTime                ; }
            if (Value                         != source.Value                        ) { Value                   = source.Value                  ; }
            if (ValueOriginal                 != source.ValueOriginal                ) { ValueOriginal           = source.ValueOriginal          ; }
            if (Offset                        != source.Offset                       ) { Offset                  = source.Offset                 ; }
            if (Divider                       != source.Divider                      ) { Divider                 = source.Divider                ; }
            if (Decimals                      != source.Decimals                     ) { Decimals                = source.Decimals               ; }
            if (ValueDescriptions             != source.ValueDescriptions            ) { ValueDescriptions       = source.ValueDescriptions      ; }
            _LastUpdateFromWebApi                                                                                = source._LastUpdateFromWebApi;
        }
        
		public DataObject CloneForWebRequest()
		{
            var New                     = new DataObject()       ;
            New.DOID                    = DOID                   ;
            New.Name                    = Name                   ;
            New.PrettyName              = PrettyName             ;
            New.CtrID                   = CtrID                  ;
            New.ID                      = ID                     ;
            New.Timestamp               = Timestamp              ;
            New.AutoOff                 = AutoOff                ;
            New.AutoOffTimestamp        = AutoOffTimestamp       ;
            New.Watt                    = Watt                   ;
            New.ActivatedTotalSeconds   = ActivatedTotalSeconds  ;
            New.Type                    = Type                   ;
            New.Unit                    = Unit                   ;
            New.ValueRange              = ValueRange             ;
            New.Resolution              = Resolution             ;
            New.Room                    = Room                   ;
            New.State                   = State                  ;
            New.AutomationState         = AutomationState        ;
            New.HasTimer                = HasTimer               ;
            New.TimerSettings           = TimerSettings          ;
            New.AutomationTimerSettings = AutomationTimerSettings;
            New.RuleSettings            = RuleSettings           ;
            New.Threshold               = Threshold              ;
            New.Inertia                 = Inertia                ;
            New.OffTime                 = OffTime                ;
            New.Value                   = Value                  ;
            New.ValueOriginal           = ValueOriginal          ;
            New.Offset                  = Offset                 ;
            New.Divider                 = Divider                ;
            New.Decimals                = Decimals               ;
            New.ValueDescriptions       = ValueDescriptions      ;
            return New;
		}

        public void CopyPropertiesForWebApiUpdate(DataObject source)
        {
            Name                    = source.Name                   ;
            PrettyName              = source.PrettyName             ;
            CtrID                   = source.CtrID                  ;
            ID                      = source.ID                     ;
            AutoOffTimestamp        = source.AutoOffTimestamp       ;
            Watt                    = source.Watt                   ;
            Type                    = source.Type                   ;
            Unit                    = source.Unit                   ;
            ValueRange              = source.ValueRange             ;
            Resolution              = source.Resolution             ;
            Room                    = source.Room                   ;
            State                   = source.State                  ;
            AutomationState         = source.AutomationState        ;
            HasTimer                = source.HasTimer               ;
            TimerSettings           = source.TimerSettings          ;
            AutomationTimerSettings = source.AutomationTimerSettings;
            RuleSettings            = source.RuleSettings           ;
            Threshold               = source.Threshold              ;
            Inertia                 = source.Inertia                ;
            OffTime                 = source.OffTime                ;
            ValueOriginal           = source.ValueOriginal          ;
            Offset                  = source.Offset                 ;
            Divider                 = source.Divider                ;
            Decimals                = source.Decimals               ;
            ValueDescriptions       = source.ValueDescriptions      ;
        }

        public DataObject Clone(DataObject original)
        {
            var New                     = new DataObject()                ;
            New.DOID                    = original.DOID                   ;
            New.Version                 = original.Version                ;
            New.Name                    = original.Name                   ;
            New.PrettyName              = original.PrettyName             ;
            New.CtrID                   = original.CtrID                  ;
            New.ID                      = original.ID                     ;
            New.OldValue                = original.OldValue               ;
            New.Timestamp               = original.Timestamp              ;
            New.AutoOff                 = original.AutoOff                ;
            New.AutoOffTimestamp        = original.AutoOffTimestamp       ;
            New.Watt                    = original.Watt                   ;
            New.ActivatedTotalSeconds   = original.ActivatedTotalSeconds  ;
            New.Type                    = original.Type                   ;
            New.Unit                    = original.Unit                   ;
            New.ValueRange              = original.ValueRange             ;
            New.Resolution              = original.Resolution             ;
            New.Value                   = original._Value                 ;
            New.Room                    = original.Room                   ;
            New.State                   = original.State                  ;
            New.AutomationState         = original.AutomationState        ;
            New.HasTimer                = original.HasTimer               ;
            New.Threshold               = original.Threshold              ;
            New.Inertia                 = original.Inertia                ;
            New.InertiaOld              = original.InertiaOld             ;
            New.LowPassFilter           = original.LowPassFilter          ;
            New.OffTime                 = original.OffTime                ;
            New.Value                   = original.Value                  ;
            New.StateDescription        = original.StateDescription       ;
            New.ValueOriginal           = original.ValueOriginal          ;
            New.Offset                  = original.Offset                 ;
            New.Divider                 = original.Divider                ;
            New.Decimals                = original.Decimals               ;
            New.ValueDescriptions       = original.ValueDescriptions      ;
			New.TimerSettings           = original.TimerSettings          ;
			New.AutomationTimerSettings = original.AutomationTimerSettings;
            New.RuleSettings            = original.RuleSettings           ;
            New._LastUpdateFromWebApi   = original._LastUpdateFromWebApi  ;
            return New;
        }

		#endregion
    }
}
