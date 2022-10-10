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
//                     HILFSKLASSE FÜR DATUM UND UHRZEIT
//
//-----------------------------------------------------------------------------

using System;

namespace Abraham.DateAndTime
{
    public class DateTimeLib
    {

        /// <summary>
        /// Gibt true zurück, wenn ein Datum innerhalb eines Zeitraums liegt (Beginn und Ende eingeschlossen)
        /// </summary>
        /// <param name="uhrzeit">Zu prüfendes Datum</param>
        /// <param name="beg">Beginn des Zeitraums (inkl. Uhrzeit)</param>
        /// <param name="end">Ende des Zeitraums (inkl. Uhrzeit)</param>
        public static bool DateIsInRange(DateTime datum, DateTime beg, DateTime end)
        {
            return DateIsInRange(datum, beg, end, true, true);
        }
        
        /// <summary>
        /// Gibt true zurück, wenn ein Datum innerhalb eines Zeitraums liegt, 
        /// wobei angegeben werden kann, ob Beginn und oder Ende eingeschlossen sind.
        /// </summary>
        /// <param name="uhrzeit">Zu prüfendes Datum</param>
        /// <param name="beg">Beginn des Zeitraums (inkl. Uhrzeit)</param>
        /// <param name="end">Ende des Zeitraums (inkl. Uhrzeit)</param>
        /// <param name="IncludeBeg">True übergeben, wenn die Beginn-Sekunde mit eingeschlossen werden soll.</param>
        /// <param name="IncludeEnd">True übergeben, wenn die Ende-Sekunde mit eingeschlossen werden soll.</param>
        public static bool DateIsInRange(DateTime datum, DateTime beg, DateTime end, bool IncludeBeg, bool IncludeEnd)
        {
            bool result1;
            if (IncludeBeg)
                result1 = (datum.CompareTo(beg) >= 0);
            else
                result1 = (datum.CompareTo(beg) > 0);

            bool result2;
            if (IncludeEnd)
                result2 = (datum.CompareTo(end) <= 0);
            else
                result2 = (datum.CompareTo(end) < 0);

            return (result1 && result2);
        }




        /// <summary>
        /// Gibt true zurück, wenn ein Datum innerhalb eines Zeitraums liegt (Beginn und Ende eingeschlossen)
        /// </summary>
        /// <param name="uhrzeit">Zu prüfende Uhrzeit</param>
        /// <param name="beg">Beginn des Zeitraums</param>
        /// <param name="end">Ende des Zeitraums</param>
        public static bool TimeIsInRange(TimeSpan uhrzeit, TimeSpan beg, TimeSpan end)
        {
            return TimeIsInRange(uhrzeit, beg, end, true, false);
        }
        
        /// <summary>
        /// Gibt true zurück, wenn eine Uhrzeit innerhalb eines Zeitraums liegt, 
        /// wobei angegeben werden kann, ob Beginn und oder Ende eingeschlossen sind.
        /// </summary>
        /// <param name="uhrzeit">Zu prüfende Uhrzeit</param>
        /// <param name="beg">Beginn des Zeitraums</param>
        /// <param name="end">Ende des Zeitraums</param>
        /// <param name="IncludeBeg">True übergeben, wenn die Beginn-Sekunde mit eingeschlossen werden soll.</param>
        /// <param name="IncludeEnd">True übergeben, wenn die Ende-Sekunde mit eingeschlossen werden soll.</param>
        public static bool TimeIsInRange(TimeSpan uhrzeit, TimeSpan beg, TimeSpan end, bool IncludeBeg, bool IncludeEnd)
        {
            bool result1;
            if (IncludeBeg)
                result1 = (uhrzeit.CompareTo(beg) >= 0);
            else
                result1 = (uhrzeit.CompareTo(beg) > 0);

            bool result2;
            if (IncludeEnd)
                result2 = (uhrzeit.CompareTo(end) <= 0);
            else
                result2 = (uhrzeit.CompareTo(end) < 0);

            return (result1 && result2);
        }


        /// <summary>
        /// Prüft eine Uhrzeit gegen einen Uhrzeitbereich, wobei man das Ende auch über Mitternacht hinaus angeben kann.
        /// Und sie unterscheidet zwischen Wochentags und Wochenende
        /// </summary>
        public static bool TimeIsInRangeExt(TimeSpan uhrzeit, DayOfWeek wochentag,
                                            TimeSpan begWT, TimeSpan endWT,
                                            TimeSpan begWE, TimeSpan endWE)
        {
            bool Wochenende = (wochentag == DayOfWeek.Saturday || wochentag == DayOfWeek.Sunday);
            if (Wochenende)
            {
                if (TimeIsInRangeExt(uhrzeit, begWE, endWE))
                    return true;
            }
            else
            {
                if (TimeIsInRangeExt(uhrzeit, begWT, endWT))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Prüft eine Uhrzeit gegen einen Zeitpunkt, wobei man das Ende auch über Mitternacht hinaus angeben kann.
        /// Und sie unterscheidet zwischen Wochentags und Wochenende
        /// </summary>
        public static bool TimeIsOnPointExt(TimeSpan time, DayOfWeek wochentag,
                                            TimeSpan weekdayPoint, TimeSpan weekendPoint)
        {
            bool Weekend = (wochentag == DayOfWeek.Saturday || wochentag == DayOfWeek.Sunday);
            
            TimeSpan point = (Weekend) ? weekendPoint : weekdayPoint;
            
            return (time.Hours   == point.Hours   && 
                    time.Minutes == point.Minutes && 
                    time.Seconds == point.Seconds);
        }

        /// <summary>
        /// Prüft eine Uhrzeit gegen einen Uhrzeitbereich, wobei man das Ende auch über Mitternacht hinaus angeben kann.
        /// </summary>
        public static bool TimeIsInRangeExt(TimeSpan uhrzeit, TimeSpan beg, TimeSpan end)
        {
            if (beg <= end && TimeIsInRange(uhrzeit, beg, end))
                return true;

            if (beg > end &&
                (
                    TimeIsInRange(uhrzeit, beg                      , new TimeSpan(23, 59, 59)) ||
                    TimeIsInRange(uhrzeit, new TimeSpan(0, 0, 0)    , end)
                ))
                return true;
            return false;
        }

        public static string FormatTimeSpan(TimeSpan ts)
        {
            string Text = "";
            if (Convert.ToInt32(ts.TotalDays) > 0)
                Text += Convert.ToInt32(ts.TotalDays) + " Tage";
            if (ts.Hours > 0)
            {
                if (Text.Length > 0)
                    Text += ", ";
                Text += ts.Hours + " Std";
            }
            if (Text.Length == 0)
            {
                if (Text.Length > 0)
                    Text += ", ";
                Text += ts.Minutes + " Min";
            }
            return Text;
        }

		public static string FormatTimespan(uint value)
		{
			string result = "";
			if (value >= 60 * 60)
			{
				uint hours = value / (60 * 60);
				result += $"{hours}h, ";
				value -= hours * (60 * 60);
			}
			if (value >= 60)
			{
				uint minutes = value / 60;
				result += $"{minutes}m, ";
				value -= minutes * 60;
			}
			if (value > 0)
			{
				uint seconds = value;
				result += $"{seconds}s";
				value -= seconds;
			}
			return result.TrimEnd(new char[] { ' ', ',' });
		}

		public static int FormatBackTimespan(string value)
		{
			int result = 0;
			var number = "";
			var separators = "dhms";
			for (int i=0; i<value.Length; i++)
			{
				if (char.IsWhiteSpace(value[i]) || char.IsPunctuation(value[i]))
				{ }
				else if (!separators.Contains( value[i].ToString() ))
					number += value[i];
				else
				{
					if (int.TryParse(number, out int convertedNumber))
					{
						switch (value[i])
						{
							case 'd': result += convertedNumber * 60 * 60 * 24; break;
							case 'h': result += convertedNumber * 60 * 60; break;
							case 'm': result += convertedNumber * 60; break;
							case 's': result += convertedNumber; break;
						}
						number = "";
					}
				}
			}
			return result;
		}
    }
}
