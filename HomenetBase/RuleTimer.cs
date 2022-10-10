using System;

namespace HomenetBase
{
	public class RuleTimer
    {
        #region ------------- Properties ----------------------------------------------------------
		public static string[] Days = { "Mo-", "Tu-", "We-", "Th-", "Fr-", "Sa-", "Su-" };

        public bool IsActive { get; set; }
        #endregion



        #region ------------- Fields --------------------------------------------------------------
        private string[] Data;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public RuleTimer()
        {
            Data = new string[7];
            Data[0] = "111111100000000000000111"; 
			Data[1] = "111111100000000000000111"; 
			Data[2] = "111111100000000000000111"; 
			Data[3] = "111111100000000000000111"; 
			Data[4] = "111111100000000000000111"; 
			Data[5] = "111111100000000000000111"; 
			Data[6] = "111111100000000000000111"; 
            IsActive = false;
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------

        public void SetCompleteDay(int day, string hours)
		{
            if (day < 0 || day > 6) 
                throw new ArgumentException("day out of range");
            if (hours.Length != 24) 
                throw new ArgumentException("hours out of range");

			Data[day] = hours;
		}

		public string GetCompleteDay(int day)
		{
            if (day < 0 || day > 6) 
                throw new ArgumentException("day out of range");
			return Data[day];
		}

        public string Serialize()
        {
            string v = "{" + "{" + IsActive.ToString() + "},";

			for(int day = 0; day < 7; day++)
                v += "{" + Days[day] + Data[day] + "},";
            
            v = v.TrimEnd(',') + "}";
			return v;
        }

        public void Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                for(int day=0; day<7; day++)
                    Data[day] = "111111111111111111111111";
                IsActive = false;
                return;
            }

            string[] Parts = value.Split(new char[] { '{', '}', ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            int i=0;
            if (Parts[i] == "True" || Parts[i] == "False")
            {
                IsActive = (Parts[i++] == "True");
            }

            string Period = Parts[i++];
            if (Period.StartsWith("Mo-Fr") || Period.StartsWith("Mo-So"))
                Deserialize_old_format(Parts, ref i, Period);
            else
                Deserialize_new_format(Parts, i);
        }

        private void Deserialize_new_format(string[] parts, int i)
        {
            i--;
            int day = 0;
            for( ; i < 7+1; i++)
                Data[day++] = parts[i].Substring(3, 24);
        }

        private void Deserialize_old_format(string[] parts, ref int i, string period)
        {
            TimeSpan WT_Von = ConvertStringToTimeSpan(parts[i++]);
            TimeSpan WT_Bis = ConvertStringToTimeSpan(parts[i++]);
            TimeSpan WE_Von;
            TimeSpan WE_Bis;

            if (parts.GetLength(0) <= 4)
            {
                WE_Von = WT_Von;
                WE_Bis = WT_Bis;
            }
            else
            {
                period = parts[i++];
                WE_Von = ConvertStringToTimeSpan(parts[i++]);
                WE_Bis = ConvertStringToTimeSpan(parts[i++]);
            }

            int day;
            for(day=0; day < 7; day++)
                Data[day] = "";

            for(day=0; day < 5; day++)
            {
                for(int hour = 0; hour <= 23; hour++)
                    Data[day] += (WT_Von.Hours <= hour && hour <= WT_Bis.Hours) ? "1" : "0";
            }

            for( ; day < 7; day++)
            {
                for(int hour = 0; hour <= 23; hour++)
                    Data[day] += (WE_Von.Hours <= hour && hour <= WE_Bis.Hours) ? "1" : "0";
            }
        }

        public bool TimeIsInRange(DateTime now)
		{
            int day = GetCurrentDay(now);
            return Data[day][now.TimeOfDay.Hours] == '1';
		}

		public bool TimeIsAtStartPoint(DateTime now)
		{
            int day = GetCurrentDay(now);
            bool currentlyOn = GetCurrentHour(day, now.Hour);
            bool previousHourOn = GetPreviousHour(day, now.Hour);
            bool nowAtFirstSecondOfTheHour = now.Minute == 0 && now.Second == 0;

			return nowAtFirstSecondOfTheHour && currentlyOn && !previousHourOn;
		}

		public bool TimeIsAtEndPoint(DateTime now)
		{
            int day = GetCurrentDay(now);
            bool currentlyOn = GetCurrentHour(day, now.Hour);
            bool nextHourOn = GetNextHour(day, now.Hour);
            bool nowAtLastSecondOfTheHour = now.Minute == 59 && now.Second == 59;

			return nowAtLastSecondOfTheHour && currentlyOn && !nextHourOn;
		}

        public override string ToString()
        {
            return FormatTimerData();
        }

        public bool HasTimer(string serializedValues)
        {
            return serializedValues != null && serializedValues.StartsWith("{{True},");
        }
        #endregion



        #region ------------- Implementation ------------------------------------------------------

        private int GetCurrentDay(DateTime now)
        {
            return (now.DayOfWeek > 0) ? (int)now.DayOfWeek-1 : 6;
        }

        private bool GetCurrentHour(int day, int hour)
        {
            return Data[day][hour] == '1';
        }

        private bool GetPreviousHour(int day, int hour)
        {
            if (hour > 0)
            {
                return Data[day][hour-1] == '1';
            }
            else
            {
                int previousDay = (day > 0) ? day-1 : 6;
                return Data[previousDay][23] == '1';
            }
        }

        private bool GetNextHour(int day, int hour)
        {
            if (hour < 23)
            {
                return Data[day][hour+1] == '1';
            }
            else
            {
                int nextDay = (day < 6) ? day+1 : 0;
                return Data[nextDay][0] == '1';
            }
        }

        private TimeSpan ConvertStringToTimeSpan(string time)
        {
            return TimeSpan.Parse(time);
        }

        private string FormatTimerData()
        {
            return "???????"; 
            //string.Format("Weekdays {0} - {1}, weekend {2} - {3}",
            //                    TimeToString(WT_Von), TimeToString(WT_Bis),
            //                    TimeToString(WE_Von), TimeToString(WE_Bis));
        }
		#endregion
	}
}
