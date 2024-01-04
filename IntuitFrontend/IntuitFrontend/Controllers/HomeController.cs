using IntuitFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace IntuitFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();   
        }

        public IActionResult SearchCity(int cityId) 
        {
            WeatherInfo.currentAndForecast info = new WeatherInfo.currentAndForecast();
            List<WeatherInfo.siguientesDias> listaDias = new List<WeatherInfo.siguientesDias>();
            var culture = new System.Globalization.CultureInfo("es-ES");

            //Tengo que hacer dos llamadas a la API. Una para el día actual y otra para los siguientes 5.
            //Esto es porque la funcionalidad de los datos diarios de la API es pago.
            string weatherUrl = "https://api.openweathermap.org/data/2.5/weather?id=";
            string forecastUrl = "https://api.openweathermap.org/data/2.5/forecast?id=";

            var currentResult = BuscarEnAPI(cityId, weatherUrl);
            var forecastResult = BuscarEnAPI(cityId, forecastUrl);
            
            var currentInfo = JsonConvert.DeserializeObject<WeatherInfo.currentWeather>(currentResult);
            var forecastInfo = JsonConvert.DeserializeObject<WeatherInfo.forecastWeather>(forecastResult);
                
            //Los datos diarios de la API que uso no son gratis. Así que tengo que hacer lo siguiente para separar por días:

            for(int i = 1; i < 6; i++)
            {
                WeatherInfo.siguientesDias siguientesDias = new WeatherInfo.siguientesDias();
                //actualDay devuelve una lista con los datos de los horarios de un día especifico.
                var actualDay = forecastInfo.list.Where(f => DateTime.Parse(f.dt_txt).Date == DateTime.Now.Date.AddDays(i));

                //Maxima/Minima temperatura de ese día. Promedio de humedad.
                var maxTemp = actualDay.Max(f => f.main.temp_max);
                var minTemp = actualDay.Min(f => f.main.temp_min);
                var averageHumidity = actualDay.Average(f => f.main.humidity);

                //Conversión para descartar ceros.
                siguientesDias.minTemp = decimal.Round(minTemp, 2, MidpointRounding.AwayFromZero);
                siguientesDias.maxTemp = decimal.Round(maxTemp, 2, MidpointRounding.AwayFromZero);
                siguientesDias.avgHumidity = decimal.Round(averageHumidity, 2, MidpointRounding.AwayFromZero);

                //Guardo la fecha del día actual. Puedo agarrar cualquier elemento de la lista del día actual, por eso
                //hago un FirstOrDefault(). Finalmente guardo el nombre del día en el campo nombreDia.
                
                siguientesDias.fecha = DateTime.Parse(actualDay.FirstOrDefault().dt_txt);
                siguientesDias.nombreDia = culture.DateTimeFormat.GetDayName(siguientesDias.fecha.DayOfWeek).ToString().ToUpper();
                listaDias.Add(siguientesDias);
            }

            //Concateno el link con el código de icono que consumí en la API para mostrarlo en la vista.
            var iconLink = "https://openweathermap.org/img/wn/" + currentInfo.weather[0].icon + "@2x.png";
            currentInfo.iconLink = iconLink;


            
            info.currentWeather = currentInfo;
            info.forecastWeather = forecastInfo;
            info.siguientesDias = listaDias;
            return View("Index", info);
            
        }

        public string BuscarEnAPI(int cityId, string url)
        {
            var apiKey = _config.GetValue<string>("ApiKey");

            
            string weatherUrl = url + cityId + "&appid=" + apiKey + "&units=metric";

            using (var client = new HttpClient())
            {
                var result = client.GetAsync(weatherUrl).Result.Content.ReadAsStringAsync().Result;
                return result;
            }
        }
    }
}
