using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Parking
{
    public class GetListParkingLocationsResponse: BaseResponse
    {
        public List<CPT331.Carbuds.Api.Models.ParkingLocation.ParkingLocation> ParkingLocations { get; set; }
    }
}
