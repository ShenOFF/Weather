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
    /// Логика взаимодействия для UseControlFromDB.xaml
    /// </summary>
    public partial class UseControlFromDB : UserControl
    {
        List<FromDB> fromDBs = new List<FromDB>();
        public UseControlFromDB(List<FromDB> fromDBs)
        {
            InitializeComponent();
            this.fromDBs = fromDBs;
            lb_city.Content = fromDBs[0].city;
            data.Content = "На " + DateTime.Parse(fromDBs[0].Data).Day + " " + DateTime.Now.ToString("MMM", CultureInfo.GetCultureInfo("ru-RU"));
            foreach (var item in fromDBs)
            {
                parent.Items.Add(item);
            }
        }
    }
}
