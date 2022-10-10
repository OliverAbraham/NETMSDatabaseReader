using NLog;

namespace NETMSDatabaseReader
{
    public class Configuration
    {
        public string  FirebirdEmbeddedDB        { get; set; } = "";
        public string  SolarSystemDatabase       { get; set; } = "";
        public string  SolarProductionDatabase   { get; set; } = "";
        public bool    CopyFdbFilesBeforeReading { get; set; } = false;
        public int     ReadIntervalMinutes       { get; set; } = 1;
        public decimal TotalkWhOffset            { get; set; } = 0m;
        public string  HomeAutomationServerURL   { get; set; } = ""; // Optional data to connect to my home automation system
        public string  HomeAutomationUsername    { get; set; } = ""; // Optional data to connect to my home automation system
        public string  HomeAutomationPassword    { get; set; } = ""; // Optional data to connect to my home automation system
        public int     HomeAutomationTimeout     { get; set; } = 30; // Optional data to connect to my home automation system

        public void LogOptions(ILogger logger)
        {
            //Note: To align the output in columns, set visual studio to use spaces instead of tabs!
            logger.Debug($"FirebirdEmbeddedDB             : {FirebirdEmbeddedDB       }");
            logger.Debug($"SolarSystemDatabase            : {SolarSystemDatabase      }");
            logger.Debug($"SolarProductionDatabase        : {SolarProductionDatabase  }");
            logger.Debug($"CopyFdbFilesBeforeReading      : {CopyFdbFilesBeforeReading}");
            logger.Debug($"ReadIntervalMinutes            : {ReadIntervalMinutes      }");
            logger.Debug($"TotalkWhOffset                 : {TotalkWhOffset           }");
            logger.Debug($"HomeAutomationServerURL        : {HomeAutomationServerURL  }");
            logger.Debug($"HomeAutomationUsername         : {HomeAutomationUsername   }");
            logger.Debug($"HomeAutomationPassword         : {HomeAutomationPassword   }");
            logger.Debug($"HomeAutomationTimeout          : {HomeAutomationTimeout    }");
        }
    }
}