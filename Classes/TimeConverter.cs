using System;
using System.Text;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        public string convertTime(string aTime)
        {
            TimeSpan time = ParseTimeString(aTime);
            string clockLampsString = BuildBerlinClockString(time);
            return clockLampsString;
        }

        private TimeSpan ParseTimeString(string timeString)
        {
            var invariantCultureInfo = System.Globalization.CultureInfo.InvariantCulture;

            if (string.IsNullOrWhiteSpace(timeString))
            {
                throw new ArgumentException("Time string parameter is null or empty", "timeString");
            }

            string[] timeParts = timeString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (timeParts.Length != 3)
            {
                throw new ArgumentException("Time string format is not valid", "timeString");
            }

            int hours, minutes, seconds;
            if (!int.TryParse(timeParts[0], System.Globalization.NumberStyles.Integer, invariantCultureInfo.NumberFormat, out hours)
                || !int.TryParse(timeParts[1], System.Globalization.NumberStyles.Integer, invariantCultureInfo.NumberFormat, out minutes)
                || !int.TryParse(timeParts[2], System.Globalization.NumberStyles.Integer, invariantCultureInfo.NumberFormat, out seconds)
            )
            {
                throw new ArgumentException("Time string format is not valid", "timeString");
            }

            if (hours < 0 || hours > 24)
            {
                throw new ArgumentOutOfRangeException("Hours");
            }
            if (minutes < 0 || hours > 59)
            {
                throw new ArgumentOutOfRangeException("Minutes");
            }
            if (seconds < 0 || seconds > 59)
            {
                throw new ArgumentOutOfRangeException("Seconds");
            }

            return new TimeSpan(hours, minutes, seconds);
        }

        private string BuildBerlinClockString(TimeSpan time)
        {
            int hours = (int)time.TotalHours;
            int minutes = time.Minutes;
            int seconds = time.Seconds;

            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}\r\n", TwoSecondsLamp(seconds));
            result.AppendFormat("{0}\r\n", FiveHoursLamps(hours));
            result.AppendFormat("{0}\r\n", OneHourLamps(hours));
            result.AppendFormat("{0}\r\n", FiveMinutesLamps(minutes));
            result.AppendFormat("{0}", OneMinuteLamps(minutes));
            return result.ToString();
        }

        private string TwoSecondsLamp(int seconds)
        {
            return seconds % 2 == 0 ? "Y" : "O";
        }

        private string FiveHoursLamps(int hours)
        {
            StringBuilder result = new StringBuilder();

            int lampsOn = hours / 5;
            int lampsOff = 4 - lampsOn;
            for (int l = 0; l < lampsOn; l++)
            {
                result.Append("R");
            }
            for (int l = 0; l < lampsOff; l++)
            {
                result.Append("O");
            }
            return result.ToString();
        }

        private string OneHourLamps(int hours)
        {
            StringBuilder result = new StringBuilder();

            int lampsOn = hours % 5;
            int lampsOff = 4 - lampsOn;
            for (int l = 0; l < lampsOn; l++)
            {
                result.Append("R");
            }
            for (int l = 0; l < lampsOff; l++)
            {
                result.Append("O");
            }
            return result.ToString();
        }

        private string FiveMinutesLamps(int minutes)
        {
            StringBuilder result = new StringBuilder();

            int lampsOn = minutes / 5;
            int lampsOff = 11 - lampsOn;
            for (int l = 1; l <= lampsOn; l++)
            {
                result.Append(l % 3 == 0 ? "R" : "Y");
            }
            for (int l = 1; l <= lampsOff; l++)
            {
                result.Append("O");
            }
            return result.ToString();
        }

        private string OneMinuteLamps(int minutes)
        {
            StringBuilder result = new StringBuilder();

            int lampsOn = minutes % 5;
            int lampsOff = 4 - lampsOn;
            for (int l = 1; l <= lampsOn; l++)
            {
                result.Append("Y");
            }
            for (int l = 1; l <= lampsOff; l++)
            {
                result.Append("O");
            }
            return result.ToString();
        }
    }
}
