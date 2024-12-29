using System;

namespace Lesson30_WebApi
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    public interface IUser
    {
        string GetFullName();
    }

    public class User : IUser
    {
        public string GetFullName()
        {
            return "Samir Eldarov";
        }
    }
}
