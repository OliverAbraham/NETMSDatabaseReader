namespace HomenetBase
{
    public class SceneSettings
    {
        #region ------------- Properties ----------------------------------------------------------
        public SceneMode Mode { get; set; }

		public int Scene_DOID { get; set; }


        /// <summary>
        /// For mode "by sensor"
        /// </summary>
        public int Sensor_DOID { get; set; }
        
        /// <summary>
        /// For mode "by sensor". Es wird Tag gemeldet, wenn der Sender diesen oder einen größeren Wert meldet
        /// </summary>
        public string SensorThreshold { get; set; }


        /// <summary>
        /// For mode "by timer"
        /// </summary>
        public RuleTimer Timer { get; set; }

        public string Zeiten_serialisiert
        {
            get { return Timer.Serialize(); }
            set { Timer.Deserialize(value); }
        }


        /// <summary>
        /// For mode "Automatisch"
        /// </summary>
        public int Lattitude { get; set; }

        /// <summary>
        /// For mode "Automatisch"
        /// </summary>
        public int Longitude { get; set; }
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		public SceneSettings()
        {
            Timer = new RuleTimer();
        }
        #endregion
    }
}
