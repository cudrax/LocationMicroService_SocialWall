using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationMicroService_SocialWall.Models
{
    public class Location
    {
        public int Id { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public bool Active { get; set; }
    }
}