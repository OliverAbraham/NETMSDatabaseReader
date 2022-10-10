using System;

namespace HomenetBase
{
	public class ConfigurationSettings
    {
        public string           ValueDescriptions           { get; set; }
        public int              Watt                        { get; set; }
        public string           Unit                        { get; set; }
        public string           ValueRange                  { get; set; }
        public string           Resolution                  { get; set; }
        public int              Threshold                   { get; set; }
        public int              Inertia                     { get; set; }
        public bool             OffTime                     { get; set; }
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

        public string Serialize()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static ConfigurationSettings Deserialize(string serializedData)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ConfigurationSettings>(serializedData);
        }
    }
}
