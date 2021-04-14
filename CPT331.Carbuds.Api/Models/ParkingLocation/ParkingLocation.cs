using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Models.ParkingLocation
{
    public class ParkingLocation
    {
        public string Uuid { get; set; }
        public string FriendlyName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
