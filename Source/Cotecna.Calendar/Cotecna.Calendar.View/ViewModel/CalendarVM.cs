using Cotecna.Calendar.Model.Enums;
using Cotecna.Calendar.View.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cotecna.Calendar.View.ViewModel
{
    /// <summary>
    /// Main View Model
    /// </summary>
    public class CalendarVM
    {

        #region constants

        private const int INITIAL_YEAR = 1970;

        #endregion

        #region fields

        private eMonths month;
        private int year;
        private ObservableCollection<int> avalibleYears;
        private GetCalendarCommand calendarCommand;

        #endregion

        #region constructors

        /// <summary>
        /// Class default constructor
        /// </summary>
        public CalendarVM()
        {
            this.Month = (eMonths)DateTime.Now.Month;      
            this.SetAvalibleYears();
            this.year = DateTime.Now.Year;      
        }

        #endregion

        #region properties
        public IEnumerable<eMonths> Months => Enum.GetValues(typeof(eMonths)).Cast<eMonths>();

        public ObservableCollection<int> Years => this.avalibleYears;

        public eMonths Month {
            get { return this.month; }
            set { this.month = value; }
        }

        public int Year
        {
            get { return this.year; }
            set { this.year = value; }
        }

        public GetCalendarCommand CalendarCommand
        {
            get
            {
                if(this.calendarCommand == null)
                {
                    this.calendarCommand = new GetCalendarCommand(this);
                }
                return this.calendarCommand;
            }
        }

        #endregion

        #region methods

        private void SetAvalibleYears()
        {
            avalibleYears = new ObservableCollection<int>();
            for (int i = INITIAL_YEAR; i <= DateTime.Now.Year; i++)
            {
                avalibleYears.Add(i);
            }
        }

        #endregion
    }
}
