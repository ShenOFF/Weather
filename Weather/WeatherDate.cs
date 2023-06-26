using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class WeatherDate
    {
        public string Pressure { get; set; } //давление
        public string Temp { get; set; } //темепратура
        public string Humidity { get; set; } //влажность
        public string Data { get; set; } //дата
        public string output { get; set; } //дата
        public string Cod { get; set; }
        public List<TemperatureInfo> list { get; set; }
        public static cit city { get; set; }
        public class cit
        {
            public string name { get; set; }
        }
        public class TemperatureInfo
        {
            public string dt { get; set; }
            public string dt_txt { get; set; }
            public string icon { get; set; }
            public temps main { get; set; }
            public class temps
            {
                public string temp { get; set; }
                public string temp_max { get; set; }
                public string temp_min { get; set; }
                public string pressure { get; set; }
                public string humidity { get; set; }

            }
            public List<weath> weather { get; set; }
            public class weath
            {
                public string id { get; set; }
                public string description { get; set; }
            }
           

        }
    }
}
