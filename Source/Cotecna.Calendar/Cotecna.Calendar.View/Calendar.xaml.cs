using Cotecna.Calendar.View.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Cotecna.Calendar.View
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {

        #region constructors

        public Calendar()
        {
            this.DataContext = new CalendarVM();
            InitializeComponent();
        }

        #endregion

        #region methods

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CalendarVM c = (CalendarVM)this.DataContext;
            ICommand com = c.CalendarCommand;
            com.Execute(this.gCalendar);
        }

        #endregion

    }
}
