using System;
using System.Globalization;

namespace Cotecna.Calendar.Model.Days
{
    /// <summary>
    /// Class who models a day of calendar
    /// </summary>
    public class CalendarDay
    {

        #region fields

        private DateTime currentDate;
        private int weekNumber;
        private string forecast;

        #endregion

        #region constructors

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="d">day number of the instance</param>
        /// <param name="m">month number of the instance</param>
        /// <param name="y">year number of the instance</param>
        public CalendarDay(int d,int m, int y)
        {
            this.currentDate = new DateTime(y, m, d);
            this.forecast = string.Empty;
            this.GetWeekNumber();
        }

        #endregion

        #region properties

        public int DayNumber
        {
            get { return this.currentDate.Day; }
        }

        public DayOfWeek CurrentDay
        {
            get { return this.currentDate.DayOfWeek; }
        }

        public int WeekNumber
        {
            get { return this.weekNumber; }
        }

        public bool IsTodaySelected
        {
            get { return this.currentDate.Date.Equals(DateTime.Now.Date); }
        }

        public string Forecast
        {
            get { return this.forecast; }
            set { this.forecast = value; }
        }

        #endregion

        #region methods

        private void GetWeekNumber()
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            this.weekNumber = cul.Calendar.GetWeekOfYear(this.currentDate,
                 CalendarWeekRule.FirstDay,
                 DayOfWeek.Monday);
        }

        #endregion

    }
}
