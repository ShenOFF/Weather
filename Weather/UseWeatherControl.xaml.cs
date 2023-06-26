using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для UseWeatherControl.xaml
    /// </summary>
    public partial class UseWeatherControl : UserControl
    {
        List<WeatherDate> DateList = new List<WeatherDate>();
        public UseWeatherControl(List<WeatherDate> dateList)
        {
            InitializeComponent();
            DateList = dateList;
            lb_city.Content = MainWindow.city;
            data.Content = "На " + DateTime.Parse(dateList[0].Data).Day + " " + DateTime.Now.ToString("MMM", CultureInfo.GetCultureInfo("ru-RU"));
            foreach (var item in DateList)
            {
                parent.Items.Add(item);
            }
        }
    }
}
