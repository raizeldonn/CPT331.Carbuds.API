using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Models.ParkingLocation
{
    public class ParkingLocation
    {
        public string Location { get; set; }
        public string FriendlyName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Postcode { get; set; }
        public string State { get; set; }
    }
}
