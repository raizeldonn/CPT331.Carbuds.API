using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Parking
{
    public class PostAddUpdateParkingLocationRequest
    {
        public CPT331.Carbuds.Api.Models.ParkingLocation.ParkingLocation Parking { get; set; }
    }
}
