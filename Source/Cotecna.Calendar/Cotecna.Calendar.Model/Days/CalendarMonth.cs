using Cotecna.Calendar.Model.Forecast;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Cotecna.Calendar.Model.Days
{
    /// <summary>
    /// Functionalities to get one calendar month
    /// </summary>
    public class CalendarMonth
    {
        #region constants

        private const double KELVIN_TO_DEGREE_CONVERSION = 273.15;
        private const string FORECAST_DAY_JSON_KEY = "dt_txt";
        private const string FORECAST_LIST_VALUES_JSON_KEY = "list";
        private const string FORECAST_MAIN_RESULTS_JSON_KEY = "main";
        private const string FORECAST_TEMPERATURE_JSON_KEY = "temp";
        private const string FORECAST_WEATHER_JSON_KEY = "weather";
        private const string WEATHER_FORECAST_UNAVALIBLE_MESSAGE = "Unavalible";

        #endregion

        #region constructors

        private CalendarMonth() { }

        #endregion

        #region methods

        /// <summary>
        /// Gets the month through the parameters of year and month number
        /// </summary>
        /// <param name="year">Year number of the calendar</param>
        /// <param name="month">Month number of the calendar</param>
        /// <returns>The calendar in days list format of type <see cref="CalendarDay"/></returns>
        public static async Task<List<CalendarDay>> GetCalendarMonth (int year, int month)
        {
            int d = DateTime.DaysInMonth(year, month);
            List<CalendarDay> days = new List<CalendarDay>();
            for (int i = 1; i <= d; i++)
            {
                days.Add(new CalendarDay(i, month, year));
            }
            if (DateTime.Now.Month == month && DateTime.Now.Year == year)
            {
                await CalendarMonth.SetWeatherForecast(days);
            }
            return days;
        }

        private static async Task SetWeatherForecast(List<CalendarDay> days)
        {
            IEnumerable<CalendarDay> calen = days.Where(d => 
                                                        d.DayNumber > DateTime.Now.Day &&
                                                        d.DayNumber < DateTime.Now.AddDays(6).Day).ToList();
            JsonValue result = await WeatherForecast.GetWeatherForecast();
            if (result != null)
            {
                CalendarMonth.SetWeatherForecastPerDay(days, (JsonArray)result[FORECAST_LIST_VALUES_JSON_KEY]);
            }
            else
            {
             foreach(CalendarDay c in calen)
                {
                    c.Forecast = WEATHER_FORECAST_UNAVALIBLE_MESSAGE;
                }
            }

        }

        private static void SetWeatherForecastPerDay(List<CalendarDay> days, JsonArray result)
        {
            var filteredResults = from j in result
                          group j by DateTime.Parse(j[FORECAST_DAY_JSON_KEY]).Day into g
                          select new { ForecastDay = g.Key, Forecasts = g.ToList() };

            foreach(var ds in filteredResults)
            {
                CalendarDay sc = days.Where(d => d.DayNumber == ds.ForecastDay).FirstOrDefault();
                var temp = (int)(ds.Forecasts[0][FORECAST_MAIN_RESULTS_JSON_KEY][FORECAST_TEMPERATURE_JSON_KEY] - KELVIN_TO_DEGREE_CONVERSION);
                sc.Forecast = $"{ds.Forecasts[0][FORECAST_WEATHER_JSON_KEY][0][FORECAST_MAIN_RESULTS_JSON_KEY]} ({temp.ToString()} ºC)";
            }
        }

        #endregion

    }
}
