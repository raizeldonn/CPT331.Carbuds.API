﻿using CPT331.Carbuds.Api.Contracts.Car;
using CPT331.Carbuds.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CarsController : ControllerBase
  {
    private ICarService _carService;
    public CarsController(ICarService carService)
    {
      _carService = carService;
    }

    [HttpPost]
    public async Task<PostAddUpdateCarResponse> AddUpdateCar(PostAddUpdateCarRequest request)
    {
      try
      {
        return new PostAddUpdateCarResponse()
        {
          Success = await _carService.AddUpdateCar(request.Car)
        };
      }
      catch(Exception e)
      {
        Console.WriteLine($"Exception encountered in POST to /api/cars: {JsonConvert.SerializeObject(e)}");
        return new PostAddUpdateCarResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpGet("list")]
    public async Task<GetListCarsResponse> ListAllCars()
    {
      try
      {
        return new GetListCarsResponse()
        {
          Cars = await _carService.ListAllCars()
        };
      }
      catch(Exception e)
      {
        Console.WriteLine($"Exception encountered in GET to /api/cars/list: {JsonConvert.SerializeObject(e)}");
        return new GetListCarsResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }
  }
}
