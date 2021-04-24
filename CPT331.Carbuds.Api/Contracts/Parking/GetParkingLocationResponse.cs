using CPT331.Carbuds.Api.Models.ParkingLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Parking
{
  public class GetParkingLocationResponse: BaseResponse
  {
    public ParkingLocation Location { get; set; }
  }

}
