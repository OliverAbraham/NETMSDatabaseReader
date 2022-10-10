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

namespace HomenetBase
{
	public class Rule
    {
        #region ------------- Enums ---------------------------------------------------------------

        public enum Type
        {
            AutoOff,
            SwitchDataObject,
            SendEmail,
            SendPushbulletCommand,
            StartProgram,
			Unknown,
			SceneSwitcher,
			Timer,
            InternetResetter
        }

        public class RuleTypeCombobox
        {
            public Rule.Type ID { get; set; }
            public string Description { get; set; }
        }

        public enum Operator
        {
            Equal,
        }

        public enum AutoOffMode
        {
            MotionSensorWithRetrigger   = 0,
            DoorContactWithoutRetrigger = 1,
            Unknown                     = 2
        }
        #endregion

        #region ------------- Properties ----------------------------------------------------------

        public Type             RuleType                        { get; set; }
        public string           Parameters_serialized           { get; set; }

        #region AutoOff parameters
        public uint             AutoOffPreset                   { get; set; }
        public AutoOffMode      AutoAusModus                    { get; set; }
		#endregion

        #region SwitchDataObject parameters
		public string           ConditionValue                  { get; set; }
        public Operator         ConditionOperator               { get; set; }
        public int              DoidToBeSwitched                { get; set; }
        public string           SwitchToValue                   { get; set; }
        public uint             DelayBefore                     { get; set; }
		#endregion

        #region SendEmail parameters
        public string           Subject                         { get; set; }
        public string           Body                            { get; set; }
        public string           ReceiverEmailAddress            { get; set; }
		#endregion

        #region SendPushbulletCommand parameters
        public string           PbApiKey                        { get; set; }
        public string           PbDevice                        { get; set; }
        public string           PbTitle                         { get; set; }
        public string           PbBody                          { get; set; }
		#endregion

        #region StartProgram parameters
        public string           ProgramName                     { get; set; }
        public string           ProgramArguments                { get; set; }
        public string           StartDirectory                  { get; set; }
		#endregion

        #region Reset internet router parameters
        public uint             CheckIntervalSeconds            { get; set; }
        public uint             RouterOffTimeSeconds            { get; set; }
        public uint             WaitUntilNextResetNMinutes      { get; set; }
        public string           FritzboxUsername                { get; set; }
        public string           FritzboxPassword                { get; set; }
        #endregion
        #endregion
    }
}
