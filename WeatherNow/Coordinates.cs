using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherNow
{
    internal class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //Constructor for Coordinates class
        public Coordinates(double latitude, double longitude) 
        {
            Latitude = latitude;
            Longitude = longitude;
        }


    }
}
