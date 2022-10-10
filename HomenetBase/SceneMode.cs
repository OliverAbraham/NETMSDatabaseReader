namespace HomenetBase
{
    public enum SceneMode
    {
        /// <summary>
        /// Not yet initialized
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Switched ba a timer
        /// </summary>
        ByTimer = 1,

        /// <summary>
        /// An external sensor is being asked about the brightness
        /// </summary>
        BySensor = 2,

        /// <summary>
        /// Calculated by a sun position fomula
        /// </summary>
        BySunPosition = 3
    }
}
