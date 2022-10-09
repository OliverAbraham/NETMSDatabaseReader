# NETMS Database Reader

## OVERVIEW

NETMS Database Reader to read out dynamic data of the allofsolar NETMS 6.2 solar monitoring system.
NETMS stores the configuration and dynamic data in FDB files (Firebase format).
To be able to process mysolar systems' data, I've created this reader.

NETMS software is used to query data from the SG300MS, SD600MD, SD700MD, SD1000MQ 
and SD1200MQ Grid Tie Inverters, build by http://www.allofsolar.com. 
These inverters have build-in WiFi.
To access their data, a Wifi adapter called "data box" is needed.
NETMS connects to the data bos, but stores all data in the FDB format.


## INSTALLATION
There's no installer provided. Copy the contents of the portable, binary package or
compile and use the contents of the release folder.


## CONFIGURATION
For operation, the firebase embedded database server is needed. 
A copy is provided with this repo.
Edit the appsettings file and adjust the path to the fbembed.dll file.


### Parameters in appsettings.hjson
- FirebirdEmbeddedDB              : path and filename of the firebase embedded database
- SolarSystemDatabase             : path and filename of the NETMS FDB file
- SolarProductionDatabase         : path and filename of the NETMS FDB file
- CopyFdbFilesBeforeReading       : true to force my app to make a copy if the FDB file before reading
- ReadIntervalMinutes             : wait time between reads in minutes.


## OUTPUT
The app writes a log using nlog. You can set the parameters in nlog.config 
(filename, format, log rotation etc).
The app also writes two additional text files with the actual values: totalpower.log and detailhistory.log.



## INTERNALS
This app uses the Firebird ADO.NET Data Provider. 
The package can be found here: https://firebirdsql.org/en/net-provider/ or on www.nuget.org
I'm using the "embedded database" configuration, which requires the embedded database files.
These files can be download at www.firebase.org as a zip file.
I am using the "32-bit Embedded" version, 
("Embedded, separate download, zip kit. Custom installation required, read the Guide!")
It can be downloaded here:https://firebirdsql.org/en/firebird-2-5/

With this, I'm able to open the firebird database files directly.
I cannot send queries to a database server, because NTMS does NOT use one.
The drawback is that I have to make a copy of each database file before reading data.
That's because they're locked by NETMS.
I'd appreciate any hints to do this better! 
Please note: During operation, the FDB files will of course grow and the performance will decrease. 
Don't set the intervall too low, 5 minutes is the minimum. Smaller intervals make no sense, 
because NETMS doesn't provide data at maximum every 5 minutes.


## SOURCE CODE

https://www.github.com/OliverAbraham/NETMSDatabaseReader

## LICENSE

Licensed under Apache licence.
https://www.apache.org/licenses/LICENSE-2.0


## AUTHOR

Oliver Abraham, mail@oliver-abraham.de

