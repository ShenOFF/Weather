using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<WeatherDate> listW = new List<WeatherDate>();
        public static List<FromDB> fromDB = new List<FromDB>();
        public static List<Request> requests = new List<Request>();
        public static string city;
        public static MainWindow mainWindow;
        public string con = "server=localhost;port=3301;database=OpenWeather;uid=root;pwd=;";

        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            LoadWeather();
            LoadTimer();
            search_DB.Visibility = Visibility.Hidden;
            tb_searchDB.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                parrent.Children.Clear();

                city = tb_search.Text;//указываем город для поиска

                string openweathermap = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid=748102d782b10c9cf65c0234b99476af";//api key
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(openweathermap);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string weath;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    weath = streamReader.ReadToEnd();
                }
                WeatherDate weatherResponse = JsonConvert.DeserializeObject<WeatherDate>(weath);
                JObject obj = JObject.Parse(weath);

                //подгружаем данные в класс WeatherDate
                foreach (var item in weatherResponse.list)
                {
                    listW.Add(new WeatherDate()
                    {
                        Pressure = item.main.pressure,
                        Temp = item.main.temp,
                        Humidity = item.main.humidity,
                        Data = item.dt_txt,
                    });
                }
                //создаем коллекцию для сортировки погоды по датам(чтобы на каждый день была своя погода)
                List<WeatherDate> temporarydata = new List<WeatherDate>();
                //берем дату для сравнения
                string last_day = listW[0].Data;

                //извлекаем из БД значение для подсчета запросов
                int c = 0;
                DataTable queryUsers = Connection.Select($"SELECT count FROM CountRequest");
                for (int i = 0; i < queryUsers.Rows.Count; i++)
                {
                    c = Convert.ToInt32(queryUsers.Rows[i][0]);
                }

                string resultDate = "";
                string resultCity = "";
                string resultTimer = "";
                

                for (int i = 0; i < fromDB.Count; i++)
                {
                    resultDate = fromDB[i].saveDate;
                }

                for (int i = 0; i < fromDB.Count; i++)
                {
                    resultCity = fromDB[i].city;
                }
                

                if (c < 10)
                {
                    
                    for (int i = 0; i < listW.Count; i++)
                    {

                        if (DateTime.Parse(last_day).Day == DateTime.Parse(listW[i].Data).Day)
                        {
                            temporarydata.Add(listW[i]);

                        }
                        else
                        {
                            parrent.Children.Add(new UseWeatherControl(temporarydata));

                            temporarydata.Clear();

                        }
                        last_day = listW[i].Data;

                    }
                    
                    //Добавляем все в БД 
                    for (int i = 0; i < listW.Count; i++)
                    {
                        Connection.Select($"INSERT INTO SaveWeather(Pressure,Temp,Humidity,Data,saveDate,city) values('{listW[i].Pressure}','{listW[i].Temp}','{listW[i].Humidity}','{listW[i].Data}','{DateTime.Now}','{city}')");

                    }
                    
                    Connection.Select($"UPDATE CountRequest set count={c + 1}");
                    

                    LoadWeather();
                    LoadRequest();
                    listW.Clear();

 
                }
                else
                {
                    Connection.Select($"INSERT INTO BlockTime(blockTime) values ('{DateTime.Now}')");


                    LoadTimer();
                    for (int i = 0; i < requests.Count; i++)
                    {
                        resultTimer = requests[0].dbtimer;
                    }
                    string saveDate = resultTimer; // получить дату сохранения в БД
                    DateTime nowDate = DateTime.Now; // получить дату на данный момент (для сравнения)

                    TimeSpan timeDifference = nowDate.Subtract(DateTime.Parse(saveDate));
                    double minutesDifference = timeDifference.TotalMinutes;
                    if (minutesDifference >= 30)
                    {
                        listW.Clear();
                        
                        Connection.Select($"UPDATE CountRequest set count=0");
                        Connection.Select($"TRUNCATE TABLE SaveWeather");
                        Connection.Select($"TRUNCATE TABLE BlockTime");
                        
                    }
                    else
                    {
                        search_DB.Visibility = Visibility.Visible;
                        tb_searchDB.Visibility = Visibility.Visible;
                        MessageBox.Show($"Погоду можно будет получть через 30 минут( прошло: {minutesDifference} минут), а пока будет загуржена погода из БД");
                        AddFromDB();
                    }
                    
                }






            }
            catch
            {
                //Ошибка в случае,если город указан не правильно
                MessageBox.Show("Город введен не правильно,перепроверьте правильность ввода!");
                return;
            }
        }


        public void LoadWeather()
        {
            fromDB.Clear();
            DataTable product_query = Connection.Select("SELECT * FROM SaveWeather");
            for (int i = 0; i < product_query.Rows.Count; i++)
            {
                fromDB.Add(new FromDB()
                {

                    Pressure = Convert.ToString(product_query.Rows[i][1]),
                    Temp = Convert.ToString(product_query.Rows[i][2]),
                    Humidity = Convert.ToString(product_query.Rows[i][3]),
                    Data = Convert.ToString(product_query.Rows[i][4]),
                    saveDate = Convert.ToString(product_query.Rows[i][5]),
                    city = Convert.ToString(product_query.Rows[i][6]),
                });

            }
        }


        public void LoadRequest()
        {
            requests.Clear();
            DataTable product_query = Connection.Select("SELECT * FROM CountRequest");
            for (int i = 0; i < product_query.Rows.Count; i++)
            {
                requests.Add(new Request()
                {

                    count = Convert.ToInt32(product_query.Rows[i][1]),
                });

            }
        }

        public void LoadTimer()
        {
            requests.Clear();
            DataTable product_query = Connection.Select("SELECT * FROM BlockTime");
            for (int i = 0; i < product_query.Rows.Count; i++)
            {
                requests.Add(new Request()
                {

                    dbtimer = Convert.ToString(product_query.Rows[i][1]),
                });

            }
        }

        public void AddFromDB()
        {

            List<FromDB> temporaryfromdata = new List<FromDB>();

            string last_day = fromDB[0].Data;
            
            for (int i = 0; i < fromDB.Count; i++)
            {


                
                if (DateTime.Parse(last_day).Day == DateTime.Parse(fromDB[i].Data).Day)
                {
                    temporaryfromdata.Add(fromDB[i]);

                }
                else
                {
                    parrent.Children.Add(new UseControlFromDB(temporaryfromdata));

                    temporaryfromdata.Clear();

                }
                last_day = fromDB[i].Data;

            }
        }

        private void search_DB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<FromDB> result = null;

                if (result != null)
                {
                    result = result.FindAll(x => x.city.Contains(tb_searchDB.Text));
                }
                else
                {
                    result = fromDB.FindAll(x => x.city.Contains(tb_searchDB.Text));
                }
                parrent.Children.Clear();

                List<FromDB> temporaryfromdataR = new List<FromDB>();

                string last_day = result[0].Data;
                
                for (int i = 0; i < result.Count; i++)
                {


                    
                    if (DateTime.Parse(last_day).Day == DateTime.Parse(result[i].Data).Day)
                    {
                        temporaryfromdataR.Add(result[i]);

                    }
                    else
                    {
                        parrent.Children.Add(new UseControlFromDB(temporaryfromdataR));

                        temporaryfromdataR.Clear();

                    }
                    last_day = result[i].Data;

                }
            }
            catch
            {
                MessageBox.Show("Город введен не правильно,перепроверьте правильность ввода!");
                return;
            }
        }
    }
}
