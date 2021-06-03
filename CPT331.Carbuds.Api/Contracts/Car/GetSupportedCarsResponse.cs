using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Car
{
  public class GetSupportedCarsResponse: BaseResponse
  {
    public List<CPT331.Carbuds.Api.Models.Car.CarModels> SupportedCars { get; set; }
  }
} 
