using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Car
{
  public class PostAddUpdateCarRequest
  {
    public CPT331.Carbuds.Api.Models.Car.Car Car { get; set; }
  }
}
