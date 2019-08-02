using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace Ryan.VendorManagement.WebUI.Infrastructure
{
    /// <summary>
    /// This class helps with conveting Gregorian (regular dates) 
    /// and JD Edwards version of Julian dates. This class will
    /// convert from on to the other.
    /// </summary>
    public class JdeJulianDateTool
    {
        public JdeJulianDateTool()
        {
            // empty constructor
        }

        private int _value = 0;
        /// <summary>
        /// This is the JD Edwards Julian Date
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ProcessNewJdeValue();
            }
        }

        private int _timeValue = 0;
        /// <summary>
        /// This is the JD Edwards Julian Time
        /// </summary>
        public int TimeValue
        {
            get
            {
                return _timeValue;
            }
            set
            {
                _timeValue = value;
                ProcessNewJdeTimeValue();
            }
        }

        /// <summary>
        /// Called when  Jde Value is reset.
        /// </summary>
        private void ProcessNewJdeValue()
        {
            string jdeDateString = this.Value.ToString().PadLeft(6, '0');

            // Extract Century;
            string centuryPart = jdeDateString.Substring(0, 1);
            _century = int.Parse(centuryPart);

            // Extract Year;
            string yearPart = jdeDateString.Substring(1, 2);
            _year = int.Parse(yearPart);

            // Extract Day;
            string dayPart = jdeDateString.Substring(3, 3);
            _day = int.Parse(dayPart);

            // Update DateValue
            SetDateFromJulianDate();
        }

        /// <summary>
        /// Called when Jde Time value is reset
        /// Jde Time looks like this (4 to 6 digits)
        /// HHMMSS
        /// HH = hour 1-24 (not zero filled, so if its just after midnight you would have only MMSS)
        /// MM = minute 00-59 (zero filled)
        /// SS = second 00-59 (zero filled)
        /// </summary>
        private void ProcessNewJdeTimeValue()
        {
            string jdeTimeString = this.TimeValue.ToString();

            int hourPart = 0;
            int minutePart = 0;
            int secondPart = 0;

            if (jdeTimeString.Length == 4)
            {
                // anything from 12:00:00 am to 12:59:59 am
                hourPart = 0;
                int.TryParse(jdeTimeString.Substring(0, 2), out minutePart);
                int.TryParse(jdeTimeString.Substring(2, 2), out secondPart);
            }
            else if (jdeTimeString.Length == 5)
            {
                int.TryParse(jdeTimeString.Substring(0, 1), out hourPart);
                int.TryParse(jdeTimeString.Substring(1, 2), out minutePart);
                int.TryParse(jdeTimeString.Substring(3, 2), out secondPart);
            }
            else
            {
                // everythign else
                int.TryParse(jdeTimeString.Substring(0, 2), out hourPart);
                int.TryParse(jdeTimeString.Substring(2, 2), out minutePart);
                int.TryParse(jdeTimeString.Substring(4, 2), out secondPart);

            }

            _hour = hourPart;
            _minute = minutePart;
            _second = secondPart;

            // update time part of DateValue
            SetTimeFromJulianTime();

        }

        private void SetDateFromJulianDate()
        {
            string yearPart = GetYearPart();
            int year = int.Parse(yearPart);

            _dateValue = new DateTime(year, 1, 1).AddDays(this.Day - 1);
        }

        private void SetTimeFromJulianTime()
        {
            DateTime tempDate = new DateTime(_dateValue.Year, _dateValue.Month, _dateValue.Day, Hour, Minute, Second);
            _dateValue = tempDate;
        }

        private string GetCenturyPrefix(int jdeCentury)
        {
            if (jdeCentury == 0)
            {
                return "19";
            }
            else if (jdeCentury == 1)
            {
                return "20";
            }
            else
            {
                return "21";
            }
        }

        private string GetYearPart()
        {
            StringBuilder sbYear = new StringBuilder();

            sbYear.Append(GetCenturyPrefix(this.Century));
            sbYear.Append(this.GetYearString());

            return sbYear.ToString();
        }

        private int _year = 0;
        public int Year
        {
            get
            {
                return _year;
            }
            //set
            //{

            //}
        }

        public string GetYearString()
        {
            return AddLeadingZeros(this.Year.ToString(), 2);
        }

        private string AddLeadingZeros(string originalValue, int NumberOfCharacters)
        {
            int numberOfZeros = NumberOfCharacters - originalValue.Length;
            StringBuilder sbResults = new StringBuilder();

            for (int i = 0; i < numberOfZeros; i++)
            {
                sbResults.Append("0");
            }

            sbResults.Append(originalValue);

            return sbResults.ToString();
        }

        private int _century = 0;
        public int Century
        {
            get
            {
                return _century;
            }
            //set
            //{

            //}
        }

        private int _day = 0;
        public int Day
        {
            get
            {
                return _day;
            }
            //set
            //{

            //}
        }

        public string GetDayString()
        {
            return AddLeadingZeros(this.Day.ToString(), 3);
        }

        private int _hour = 0;
        public int Hour
        {
            get
            {
                return _hour;
            }
        }

        private int _minute = 0;
        public int Minute
        {
            get
            {
                return _minute;
            }
        }

        private int _second = 0;
        public int Second
        {
            get
            {
                return _second;
            }
        }

        public string GetHourString()
        {
            string result = string.Empty;

            if (this.Hour > 0)
            {
                result = this.Hour.ToString();
            }

            return result;
        }

        public string GetMinuteString()
        {
            return AddLeadingZeros(this.Minute.ToString(), 2);
        }

        public string GetSecondString()
        {
            return AddLeadingZeros(this.Second.ToString(), 2);
        }

        private DateTime _dateValue = DateTime.Now;
        public DateTime DateValue
        {
            get
            {
                return _dateValue;
            }
            set
            {
                _dateValue = value;
                ProcessNewDateValue();
                ProcessNewTimeValue();
            }
        }

        /// <summary>
        /// Called when the DateValue field is set, it updates the Julian date.
        /// </summary>
        private void ProcessNewDateValue()
        {
            _century = GetJdeCentury();
            _year = GetJdeYear();
            _day = GetJdeDay();

            string jdeJulianDateString = string.Format("{0}{1}{2}", this.Century.ToString(), this.GetYearString(), this.GetDayString());
            _value = int.Parse(jdeJulianDateString);
        }

        /// <summary>
        /// Called when the DateValue field is set, it updates the Julian Time.
        /// </summary>
        private void ProcessNewTimeValue()
        {
            _hour = this.DateValue.Hour;
            _minute = this.DateValue.Minute;
            _second = this.DateValue.Second;

            string jdeJulianTimeString = string.Format("{0}{1}{2}", this.GetHourString(), this.GetMinuteString(), this.GetSecondString());
            _timeValue = int.Parse(jdeJulianTimeString);
        }

        private int GetJdeCentury()
        {
            if (this.DateValue.Year < 2000)
            {
                return 0;
            }
            else if (this.DateValue.Year > 1999 && this.DateValue.Year < 2100)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        private int GetJdeYear()
        {
            string yearString = this.DateValue.Year.ToString().Substring(2, 2);
            return int.Parse(yearString);
        }

        private int GetJdeDay()
        {
            return this.DateValue.DayOfYear;
        }
    }
}