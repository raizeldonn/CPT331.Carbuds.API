﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Car
{
  public class GetListCarsResponse: BaseResponse
  {
    public List<CPT331.Carbuds.Api.Models.Car.Car> Cars { get; set; }
  }
} 
