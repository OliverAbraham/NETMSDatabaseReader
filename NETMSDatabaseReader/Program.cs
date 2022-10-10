using Abraham.ProgramSettingsManager;
using Abraham.Scheduler;
using CommandLine;
using FirebirdSql.Data.FirebirdClient;
using HomenetBase;
using HomenetClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Web;
using System.Text;

namespace NETMSDatabaseReader
{
    /// <summary>
    /// NETMS Database Reader to read out dynamic data of the NETMS 6.2 solar monitoring system.
    ///
    /// NETMS stores the configuration and dynamic data in FDB files (Firebase format).
    /// To be able to process mysolar systems' data, I've created this reader.
    /// 
    /// AUTHOR
    /// Written by Oliver Abraham, mail@oliver-abraham.de
    /// 
    /// INSTALLATION AND CONFIGURATION
    /// See the README.md
    /// 
    /// SOURCE CODE
    /// https://www.github.com/OliverAbraham/NETMSDatabaseReader
    /// 
    /// 
    /// </summary>
    public class Program
    {
        #region ------------- Fields --------------------------------------------------------------
        private static CommandLineOptions _commandLineOptions = new();
        private static ProgramSettingsManager<Configuration> _programSettingsManager = new();
        private static Configuration _config = new();
        private static NLog.Logger _logger;
        private static Scheduler _scheduler = new();
        private static DynamicData _dynamicData = new();
        private static string? _dynamicDataPowerCurrentWatts;
        private static string? _dynamicDataPowerKwhToday;
        private static string? _dynamicDataPowerKwhAll;
        #endregion



        #region ------------- Command line options ------------------------------------------------
        private class CommandLineOptions
        {
            [Option('c', "config", Default = "appsettings.hjson", Required = false, HelpText =
                """
            Configuration file (full path and filename).
            If you don't specify this option, the program will expect your configuration file 
            named 'appsettings.hjson' in your program folder.
            You can specify a different location.
            You can use Variables for special folders, like %APPDATA%.
            Please refer to the documentation of my nuget package https://github.com/OliverAbraham/Abraham.ProgramSettingsManager
            """)]
            public string ConfigurationFile { get; set; } = "";

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }

        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public static void Main(string[] args)
        {
            ParseCommandLineArguments();
            ReadConfiguration();
            ValidateConfiguration();
            InitLogger();
            PrintGreeting();
            LogConfiguration();
            HealthChecks();
            ReadDynamicData();
            
            ReadNetmsSolarSystemData();
            
            StartScheduler();
            Console.ReadKey();
            StopScheduler();
        }
        #endregion



        #region ------------- Health checks -------------------------------------------------------
        private static void HealthChecks()
        {
        }
        #endregion



        #region ------------- Configuration -------------------------------------------------------
        private static void ParseCommandLineArguments()
        {
            string[] args = Environment.GetCommandLineArgs();
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(options => { _commandLineOptions = options; })
                .WithNotParsed<CommandLineOptions>(errors => { Console.WriteLine(errors.ToString()); });
        }

        private static void ReadConfiguration()
        {
            // ATTENTION: When loading fails, you probably forgot to set the properties of appsettings.hjson to "copy if newer"!
            // ATTENTION: or you have an error in your json file
            _programSettingsManager = new ProgramSettingsManager<Configuration>()
                .UseFullPathAndFilename(_commandLineOptions.ConfigurationFile)
                .Load();
            _config = _programSettingsManager.Data;
            Console.WriteLine($"Loaded configuration file '{_programSettingsManager.ConfigFilename}'");
        }

        private static void ValidateConfiguration()
        {
            // ATTENTION: When validating fails, you missed to enter a value for a property in your json file
            _programSettingsManager.Validate();
        }

        private static void SaveConfiguration()
        {
            _programSettingsManager.Save(_programSettingsManager.Data);
        }
        #endregion



        #region ------------- Dynamic data --------------------------------------------------------
        private static void ReadDynamicData()
        {
            try
            {
                if (File.Exists("DynamicData.json"))
                {
                    var contents = File.ReadAllText("DynamicData.json");
                    _dynamicData = JsonConvert.DeserializeObject<DynamicData>(contents) ?? new();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error reading DynamicData.json: {ex}");
            }
        }

        private static void SaveDynamicData()
        {
            try
            {
                var contents = JsonConvert.SerializeObject(_dynamicData);
                File.WriteAllText("DynamicData.json", contents);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error saving DynamicData.json: {ex}");
            }
        }
        #endregion



        #region ------------- Logging -------------------------------------------------------------
        private static void InitLogger()
        {
            try
            {
                _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing our logger. {ex.ToString()}");
                throw;  // ATTENTION: When you come here, you probably forgot to set the properties of nlog.config to "copy if newer"!
            }
        }

        /// <summary>
        /// To generate text like this, use https://onlineasciitools.com/convert-text-to-ascii-art
        /// </summary>
        private static void PrintGreeting()
        {
            _logger.Debug("");
            _logger.Debug("");
            _logger.Debug("");
            _logger.Debug(@"-------------------------------------------------------------------------------------------------------------------");
            _logger.Debug(@"                                                                                                                   ");
            _logger.Debug(@" _   _ ______ _______ __  __  _____ _____        _        _                    _____                _              ");
            _logger.Debug(@"| \ | |  ____|__   __|  \/  |/ ____|  __ \      | |      | |                  |  __ \              | |             ");
            _logger.Debug(@"|  \| | |__     | |  | \  / | (___ | |  | | __ _| |_ __ _| |__   __ _ ___  ___| |__) |___  __ _  __| | ___ _ __    ");
            _logger.Debug(@"| . ` |  __|    | |  | |\/| |\___ \| |  | |/ _` | __/ _` | '_ \ / _` / __|/ _ \  _  // _ \/ _` |/ _` |/ _ \ '__|   ");
            _logger.Debug(@"| |\  | |____   | |  | |  | |____) | |__| | (_| | || (_| | |_) | (_| \__ \  __/ | \ \  __/ (_| | (_| |  __/ |      ");
            _logger.Debug(@"|_| \_|______|  |_|  |_|  |_|_____/|_____/ \__,_|\__\__,_|_.__/ \__,_|___/\___|_|  \_\___|\__,_|\__,_|\___|_|      ");
            _logger.Debug(@"                                                                                                                   ");
            _logger.Debug(@"                                                                                                                   ");
            _logger.Info($"NETMSDatabaseReader started, Version {AppVersion.Version.VERSION}                                                   ");
            _logger.Debug(@"-------------------------------------------------------------------------------------------------------------------");
        }

        private static void LogConfiguration()
        {
            _logger.Debug($"");
            _logger.Debug($"");
            _logger.Debug($"");
            _logger.Debug($"------------ 0 Configuration -------------------------------------------");
            _logger.Debug($"Loaded from file               : {_programSettingsManager.ConfigFilename}");
            _programSettingsManager.Data.LogOptions(_logger);
            _logger.Debug("");
        }

        private static void AppendToFile(string filename, string text)
        {
            File.AppendAllText(filename, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} | {text}\n");
        }
        #endregion



        #region ------------- Periodic actions ----------------------------------------------------
        private static void StartScheduler()
        {
            // set the interval to 10 seconds
            _scheduler = new Scheduler()
                .UseAction(() => PeriodicJob())
                .UseFirstStartRightNow()
                .UseIntervalMinutes(_config.ReadIntervalMinutes)
                .Start();
        }

        private static void StopScheduler()
        {
            _scheduler.Stop();
        }

        private static void PeriodicJob()
        {
            ReadProductionData();
        }
        #endregion



        #region ------------- Domain logic --------------------------------------------------------
        private static void ReadNetmsSolarSystemData()
        {
            try
            {
                _logger.Debug("");
                _logger.Debug("");
                _logger.Debug($"SOLAR SYSTEM DATA");
                _logger.Debug($"--------------------------------------------------------------------------------------------");
                var tableNames = GetTableNames(_config.SolarSystemDatabase);
                foreach(var tableName in tableNames.Where(x => x != "TEMPTABLE"))// && x != "DATAPLUSLIST"))
                {
                    _logger.Debug($"Contents of table {tableName}:");
                    if (tableName == "SYSTEMINFO")
                        ReadSystemInfo(_config.SolarSystemDatabase);
                    else if (tableName == "INVERTERLIST")
                        ReadInverterList(_config.SolarSystemDatabase);
                    else
                        ReadTableContents(_config.SolarSystemDatabase, tableName);
                    _logger.Debug("");
                }
            }
            catch (Exception ex)
            {
                _logger.Debug(ex);
                _logger.Debug($"Current Directory: {Directory.GetCurrentDirectory()}");
            }
        }

        private static void ReadProductionData()
        {
            try
            {
                _logger.Debug("");
                _logger.Debug("");
                _logger.Debug($"SOLAR PRODUCTION DATA");
                _logger.Debug($"--------------------------------------------------------------------------------------------");
                var tableNames = GetTableNames(_config.SolarProductionDatabase);
                foreach(var tableName in tableNames.Where(x => x != "TEMPTABLE"))
                {
                    _logger.Debug("");
                    _logger.Debug($"Contents of table {tableName}:");
                    if (tableName == "TOTALPOWER")
                        ReadTotalPower(_config.SolarProductionDatabase);
                    else if (tableName == "DETAILHISTORY")
                        ReadDetailHistory(_config.SolarProductionDatabase);
                    else
                        ReadTableContents(_config.SolarProductionDatabase, tableName);
                    _logger.Debug("");
                }

                var today     = DateTime.Now.Date;
                var yesterday = DateTime.Now.Date.AddDays(-1);
                var earnedUpToToday     = ReadKwhEarnedOfDate(_config.SolarProductionDatabase, today);
                var earnedUpToYesterday = ReadKwhEarnedOfDate(_config.SolarProductionDatabase, yesterday);
                var difference = earnedUpToToday - earnedUpToYesterday;
                _dynamicDataPowerKwhToday = difference.ToString();

                var earnedTotal = earnedUpToToday - _config.TotalkWhOffset;
                _dynamicDataPowerKwhAll = earnedTotal.ToString();

                SaveDynamicData();
                SendSolarDataToHomenetServer();

                Console.WriteLine("Press any key to end the program.");
            }
            catch (Exception ex)
            {
                _logger.Debug(ex);
                _logger.Debug($"Current Directory: {Directory.GetCurrentDirectory()}");
            }
        }

        private static List<string> GetTableNames(string databaseFile)
        {
            var results = new List<string>();

            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate(FbConnection connection, FbTransaction transaction)
                {
                    using (var command = new FbCommand("select * from RDB$RELATIONS where RDB$RELATION_TYPE = 0 and RDB$SYSTEM_FLAG = 0;", connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                var tableName = ((string)values[8]).Trim();
                                results.Add(tableName);
                            }
                        }
                    }
                });

            return results;
        }
        
        private static void ReadTableContents(string databaseFile, string tableName)
        {
            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate(FbConnection connection, FbTransaction transaction)
                {
                    using (var command = new FbCommand($"select * from {tableName}", connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                _logger.Debug(string.Join(" | ", values));
                            }
                        }
                    }
                });
        }
        
        private static void ReadSystemInfo(string databaseFile)
        {
            var columns = new List<string>()
            {
                "IFFIRST",
                "COMPANYNAME",
                "SYSTITLE",
                "IUPLOWCASE",
                "MODEMPORT",
                "MODEMID",
                "MAXINVERTERNUM",
                "INVERTERIDLIST",
                "CURDPOWER",
                "CURMPOWER",
                "CURYPOWER",
                "CURPOWERDATE",
                "GENDATE",
                "GENHOUR",
                "GENMINUTE",
                "GENID",
                "SHOWCLOSEDLG",
                "HIDETOTRAYICON",
                "ILANGUAGE",
                "USEPOWERPRICE",
                "SELLPOWERPRICE",
                "AUTOPATROL",
                "DAYCHARTFROM",
                "DAYCHARTTO",
                "PROTOCOL",
                "HOSTIP",
                "HOSTPORT",
                "LOCALHOSTPORT",
                "SCANINTERVAL",
                "SCANWAITTIME",
            };

            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate(FbConnection connection, FbTransaction transaction)
                {
                    using (var command = new FbCommand($"select * from SYSTEMINFO", connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var text = FormatHeaders(columns);
                                _logger.Debug(text);
                            }
                            while (reader.Read())
                            {
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                var text = FormatDataRow(columns, values);
                                _logger.Debug(text);
                            }
                        }
                    }
                });
        }
        
        private static void ReadInverterList(string databaseFile)
        {
            var columns = new List<string>()
            {
                "MIID",
                "MIMODEL",
                "MISTATUS",
                "MIPOWERTRACK",
                "MIDATE",
                "MIPOWERKWHDAY",
                "MIPOWERKWHMONTH",
                "MIPOWERKWHYEAR",
                "MIPOWERKWHALL",
                "MIVER",
                "MIDATAPLUSID",
                "MIPOWERKWH",
                "MIPOWERKWHPRE"
            };

            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate(FbConnection connection, FbTransaction transaction)
                {
                    using (var command = new FbCommand($"select * from INVERTERLIST", connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var text = FormatHeaders(columns);
                                _logger.Debug(text);
                            }
                            while (reader.Read())
                            {
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                var text = FormatDataRow(columns, values);
                                _logger.Debug(text);
                            }
                        }
                    }
                });
        }

        private static DateTime ReadTotalPower(string databaseFile)
        {
            var lastTimeRead = DateTime.MinValue;

            var columns = new List<string>()
            {
                "AUTOID",
                "MIDATE",
                "TIMESTAMP",
                "MIPOWERKWHFIVE",
                "MIPOWERKWHALL",
            };

            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate(FbConnection connection, FbTransaction transaction)
                {
                    var query = $"select * from TOTALPOWER where MIDATE > '{_dynamicData.TotalPowerLastRowTimestamp.ToString("dd.MM.yyyy HH:mm:ss")}' order by MIDATE";
                    _logger.Debug(query);
                    using (var command = new FbCommand(query, connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var text = FormatHeaders(columns);
                                _logger.Debug(text);
                            }
                            while (reader.Read())
                            {
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                var text = FormatDataRow(columns, values);
                                _logger.Debug(text);
                                AppendToFile("totalpower.log", text);

                                var midateColumnIndex = columns.IndexOf("MIDATE");
                                var midate = values[midateColumnIndex];
                                _dynamicData.TotalPowerLastRowTimestamp = Convert.ToDateTime(midate);
                            }
                        }
                    }
                });

            return lastTimeRead;
        }

        private static decimal ReadKwhEarnedOfDate(string databaseFile, DateTime date)
        {
            var from = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).ToString("dd.MM.yyyy HH:mm:ss");
            var to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59).ToString("dd.MM.yyyy HH:mm:ss");

            var query = $"select MAX(MIPOWERKWHALL) from TOTALPOWER where MIDATE between '{from}' and '{to}'";
            object value = ExecuteSkalarSql(databaseFile, query);
            return Convert.ToDecimal(value);
        }

        private static object ExecuteSkalarSql(string databaseFile, string query)
        {
            object value = null;

            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate (FbConnection connection, FbTransaction transaction)
                {
                    _logger.Debug(query);
                    using (var command = new FbCommand(query, connection, transaction))
                    {
                        value = command.ExecuteScalar();
                    }
                });

            return value;
        }

        private static void ReadDetailHistory(string databaseFile)
        {
            var columns = new List<string>()
            {
                "AUTOID", 
                "MIID", 
                "MIMODEL", 
                "MISTATUS", 
                "MIDATE", 
                "MIPOWERADC", 
                "MIPOWERVDC", 
                "MIPOWERAAC", 
                "MIPOWERVAC", 
                "MIPOWERFIVE", 
                "MIPOWERFIVESTR", 
                "MIPOWERKWHFIVE", 
                "MIPOWERKWHALL", 
                "MITEMPERATURE", 
                "MIPOWERTRACK", 
                "MITERMERATURE", 
                "MITEMPERATURE", 
            };

            ConnectToFirebirdDatabaseAndExecuteCommand(databaseFile,
                delegate(FbConnection connection, FbTransaction transaction)
                {
                    var query = $"select * from DETAILHISTORY where MIDATE > '{_dynamicData.DetailsHistoryLastRowTimestamp.ToString("dd.MM.yyyy HH:mm:ss")}' order by MIDATE";
                    _logger.Debug(query);
                    using (var command = new FbCommand(query, connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var text = FormatHeaders(columns);
                                _logger.Debug(text);
                            }
                            while (reader.Read())
                            {
                                var values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                var text = FormatDataRow(columns, values);
                                _logger.Debug(text);
                                AppendToFile("detailhistory.log", text);

                                var midateColumnIndex = columns.IndexOf("MIDATE");
                                var midate = values[midateColumnIndex];
                                _dynamicData.DetailsHistoryLastRowTimestamp = Convert.ToDateTime(midate);

                                midateColumnIndex = columns.IndexOf("MIPOWERFIVE");
                                var powerCurrentWatts = values[midateColumnIndex];
                                _dynamicDataPowerCurrentWatts = powerCurrentWatts.ToString();
                            }
                        }
                    }
                });
        }

        private delegate void FirebirdAction(FbConnection connection, FbTransaction transaction);

        private static void ConnectToFirebirdDatabaseAndExecuteCommand(string databaseFile, FirebirdAction action)
        {
            var copyOfDatabaseFile = databaseFile;

            if (_config.CopyFdbFilesBeforeReading)
            {
                copyOfDatabaseFile = databaseFile + ".copy";
                File.Copy(databaseFile, copyOfDatabaseFile, overwrite:true);
            }

            var connectionString =
                $"User=SYSDBA;Password=masterkey;Database={copyOfDatabaseFile};DataSource=localhost;Port=3050;Dialect=3;" +
                $"Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;" +
                $"ServerType=1;client library={_config.FirebirdEmbeddedDB}";

            using (var connection = new FbConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    action(connection, transaction);
                }
            }
        }
        #endregion
 
        
        
        #region ------------- Display -------------------------------------------------------------
        private static string FormatHeaders(List<string> columns, int width = 20)
        {
            var sb = new StringBuilder();
            foreach(var column in columns)
            {
                var valueStr = column.PadRight(width).Substring(0,width);
                sb.Append($"{valueStr} ");
            }
            return sb.ToString();
        }

        private static string FormatDataRow(List<string> columns, object[] values, int width = 20)
        {
            var sb = new StringBuilder();
            for (int colIndex = 0; colIndex < values.Count(); colIndex++)
            {
                var value = values[colIndex];
                var valueStr = (value is not null) ? (value?.ToString()?.Trim() ?? "") : "";
                valueStr = valueStr.PadRight(width).Substring(0,width);
                sb.Append($"{valueStr} ");
            }
            return sb.ToString();
        }
        #endregion



        #region ------------- Homeautomation Gateway ----------------------------------------------
        private static void SendSolarDataToHomenetServer()
        {
            if (string.IsNullOrWhiteSpace(_config?.HomeAutomationServerURL))
                return;
            if (_dynamicDataPowerCurrentWatts is null && 
                _dynamicDataPowerKwhToday     is null && 
                _dynamicDataPowerKwhAll       is null)
                return;

            try
            {
                _logger.Debug($"Sending solar data to home automation server: currentPower={_dynamicDataPowerCurrentWatts} todayKwh={_dynamicDataPowerKwhToday} totalKwh={_dynamicDataPowerKwhAll}");

                var connector = new DataObjectsConnector(
                    _config.HomeAutomationServerURL,
                    _config.HomeAutomationUsername,
                    _config.HomeAutomationPassword,
                    _config.HomeAutomationTimeout);
                _logger.Debug("Connect successful");

                UpdateDataObject(connector, "SOLAR_CURRENT_WATTS", _dynamicDataPowerCurrentWatts);
                UpdateDataObject(connector, "SOLAR_TODAY_KWH"    , _dynamicDataPowerKwhToday    );
                UpdateDataObject(connector, "SOLAR_TOTAL_KWH"    , _dynamicDataPowerKwhAll      );
            }
            catch (Exception ex)
            {
                _logger.Debug("Error connecting to homenet server:\n" + ex.ToString());
            }
        }

        private static void UpdateDataObject(DataObjectsConnector connector, string name, string value)
        {
            if (value is not null)
            {
                _logger.Debug($"updating {name} with value {value}");
                var success = connector.UpdateValueOnly(new DataObject() { Name = name, Value = value });
                _logger.Debug(success ? "ok" : "send error!");
            }
        }
        #endregion


    }
}