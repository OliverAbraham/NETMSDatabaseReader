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


## SOURCE CODE

https://www.github.com/OliverAbraham/NETMSDatabaseReader

## LICENSE

Licensed under Apache licence.
https://www.apache.org/licenses/LICENSE-2.0


## AUTHOR

Oliver Abraham, mail@oliver-abraham.de

