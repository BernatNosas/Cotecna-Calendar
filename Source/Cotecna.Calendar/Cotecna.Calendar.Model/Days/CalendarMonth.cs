using Cotecna.Calendar.Model.Forecast;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotecna.Calendar.Model.Days
{
    public class CalendarMonth
    {

        #region constructors

        private CalendarMonth() { }

        #endregion

        #region methods

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
                CalendarMonth.SetWeatherForecastPerDay(days, (JsonArray)result["list"]);
            }
            else
            {
             foreach(CalendarDay c in calen)
                {
                    c.Forecast = "Unavalible";
                }
            }

        }

        private static void SetWeatherForecastPerDay(List<CalendarDay> days, JsonArray result)
        {
            var filteredResults = from j in result
                          group j by DateTime.Parse(j["dt_txt"]).Day into g
                          select new { ForecastDay = g.Key, Forecasts = g.ToList() };

            foreach(var ds in filteredResults)
            {
                CalendarDay sc = days.Where(d => d.DayNumber == ds.ForecastDay).FirstOrDefault();
                var temp = (int)(ds.Forecasts[0]["main"]["temp"] - 273.15);
                sc.Forecast = ds.Forecasts[0]["weather"][0]["main"] + "(" + temp.ToString() + "ºC)";
            }
        }

        #endregion

    }
}
