using Cotecna.Calendar.Model.Days;
using Cotecna.Calendar.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cotecna.Calendar.View.Commands
{
    public class GetCalendarCommand : ICommand
    {
        
        #region fields

        private CalendarVM viewModel;
        List<CalendarDay> currentMonth;

        #endregion

        #region constructors

        public GetCalendarCommand(CalendarVM c)
        {
            this.viewModel = c;
        }

        #endregion

        #region methods

        private void ClearCurrentCalendar(Grid g)
        {
            List<UIElement> deleteTexts = new List<UIElement>();
            for(int i = 0; i <= 6; i++)
            {
                for(int j = 1; j<=6; j++)
                {
                   IEnumerable<UIElement> elements = g.Children.Cast<UIElement>().
                        Where(e => Grid.GetRow(e) == j &&
                                   Grid.GetColumn(e) == i && 
                                   e.GetType() == typeof(TextBlock));
                   if (elements.Any())
                    {
                        foreach (UIElement el in elements)
                        {
                            deleteTexts.Add(el);
                        }
                    }                         
                }
            }
            if (deleteTexts.Any())
            {
                foreach(UIElement de in deleteTexts)
                {
                    g.Children.Remove(de);
                }
            }
        }

        private async void CreateCurrentCalendar(Grid g)
        {
            this.currentMonth = await CalendarMonth.GetCalendarMonth(this.viewModel.Year, (int)this.viewModel.Month);
            List<int> weekNumbers = this.GetWeekNumbers();
            int currentRowNumber = 1;
            foreach (int i in weekNumbers)
            {
                IEnumerable<CalendarDay> dayWeek = this.GetDaysByWeekNumber(i);
                foreach(CalendarDay d in dayWeek)
                {
                    TextBlock td = this.GetDayTextBlock(d, currentRowNumber);
                    g.Children.Add(td);
                    TextBlock tf= this.GetForecastTextBlock(d, currentRowNumber);
                    g.Children.Add(tf);
                }
                currentRowNumber++;
            }
        }

        private List<int> GetWeekNumbers()
        {
            return this.currentMonth.Select(c => c.WeekNumber).Distinct().ToList();
        }

        private IEnumerable<CalendarDay> GetDaysByWeekNumber(int weekNumber)
        {
            return this.currentMonth.Where(c => c.WeekNumber == weekNumber).Select(c => c);
        }

        private TextBlock GetDayTextBlock(CalendarDay cale,int rowNumber)
        {
            TextBlock tbDay = new TextBlock()
            {
                Text = cale.DayNumber.ToString(),
                FontSize = 25.0,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(left: 0, top: 0, right: 0, bottom: 0),
            };
            if (cale.IsTodaySelected)
            {
                tbDay.Foreground = new SolidColorBrush(Colors.Red);
            }
            tbDay.SetValue(Grid.ColumnProperty, this.GetColumnNumber(cale));
            tbDay.SetValue(Grid.RowProperty, rowNumber);
            tbDay.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            return tbDay;
        }

        private TextBlock GetForecastTextBlock(CalendarDay cale, int rowNumber)
        {
            TextBlock tbFore = new TextBlock()
            {
                Text = cale.Forecast,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(left: 0, top: 0, right: 0, bottom: 20),
                Foreground = new SolidColorBrush(Colors.Gray)
            };
            tbFore.SetValue(Grid.ColumnProperty, this.GetColumnNumber(cale));
            tbFore.SetValue(Grid.RowProperty, rowNumber);
            tbFore.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            return tbFore;
        }

        private int GetColumnNumber(CalendarDay cale)
        {
            if(cale.CurrentDay == DayOfWeek.Sunday)
            {
                return 6;
            }
            else
            {
                return (int)cale.CurrentDay - 1 ;
            }
        }

        #endregion

        #region ICommand

         public event EventHandler CanExecuteChanged;

         bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

         void ICommand.Execute(object parameter)
        {
            Grid g = (Grid)parameter;
            this.ClearCurrentCalendar(g);       
            this.CreateCurrentCalendar(g);
      
        }

        #endregion
    }
}
