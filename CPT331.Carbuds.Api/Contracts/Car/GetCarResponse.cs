using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPT331.Carbuds.Api.Models.Car;

namespace CPT331.Carbuds.Api.Contracts.Car
{
  public class GetCarResponse: BaseResponse
  {
    public CPT331.Carbuds.Api.Models.Car.Car car { get; set; }
  }
}
