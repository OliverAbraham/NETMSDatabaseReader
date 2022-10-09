using NLog;

namespace NETMSDatabaseReader
{
    public class Configuration
    {
        public string FirebirdEmbeddedDB        { get; set; } = "";
        public string SolarSystemDatabase       { get; set; } = "";
        public string SolarProductionDatabase   { get; set; } = "";
        public bool   CopyFdbFilesBeforeReading { get; set; } = false;
        public int    ReadIntervalMinutes       { get; set; } = 1;

        public void LogOptions(ILogger logger)
        {
            //Note: To align the output in columns, set visual studio to use spaces instead of tabs!
            logger.Debug($"FirebirdEmbeddedDB             : {FirebirdEmbeddedDB       }");
            logger.Debug($"SolarSystemDatabase            : {SolarSystemDatabase      }");
            logger.Debug($"SolarProductionDatabase        : {SolarProductionDatabase  }");
            logger.Debug($"CopyFdbFilesBeforeReading      : {CopyFdbFilesBeforeReading}");
            logger.Debug($"ReadIntervalMinutes            : {ReadIntervalMinutes      }");
        }
    }
}