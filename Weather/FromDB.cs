using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class FromDB
    {
        public string Pressure { get; set; } //давление
        public string Temp { get; set; } //темепратура
        public string Humidity { get; set; } //влажность
        public string Data { get; set; } //дата
        public string saveDate { get; set; } //дата сохранения(поиска) по городу
        public string city { get; set; } //город
    }
}
