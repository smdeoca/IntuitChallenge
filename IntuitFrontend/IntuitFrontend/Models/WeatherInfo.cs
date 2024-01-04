namespace IntuitFrontend.Models
{
    public class WeatherInfo
    {
        public class currentAndForecast
        {
            public forecastWeather forecastWeather { get; set; }
            public List<siguientesDias> siguientesDias { get; set; }
            public currentWeather currentWeather { get; set; }
        }
        public class forecastWeather
        {
            public List<forecastList> list { get; set; }
        }
        public class currentWeather
        {
            public List<weather> weather { get; set; }
            public main main { get; set; }
            public string name { get; set; }
            public string iconLink { get; set; }
        }
        public class siguientesDias
        {
            public decimal minTemp { get; set; }
            public decimal maxTemp { get; set; }
            public decimal avgHumidity { get; set; }
            public DateTime fecha { get; set; }
            public string nombreDia { get; set; }
        }

        public class weather
        {
            public string main { get; set; }
            public string icon { get; set; }
        }
        public class main
        {
            public decimal temp { get; set; }
            public decimal temp_min { get; set; }
            public decimal temp_max { get; set; }
            public decimal humidity { get; set; }
        }
        public class forecastList
        {
            public main main { get; set; }
            public List<weather> weather { get; set; }
            //public rain rain { get; set; }
            public string dt_txt { get; set; }
        }
        public class rain
        {
            public float h3 { get; set; }
        }

        
    }
}
