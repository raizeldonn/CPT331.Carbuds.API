using CPT331.Carbuds.Api.Contracts.Parking;
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
    public class ParkingLocationController : ControllerBase
    {
        private IParkingLocationService _parkingLocationService;
        public ParkingLocationController(IParkingLocationService locationService)
        {
            _parkingLocationService = locationService;
        }

        [HttpPost]
        public async Task<PostAddUpdateParkingLocationResponse> AddUpdateParkingLocation(PostAddUpdateParkingLocationRequest request)
        {
            try
            {
                return new PostAddUpdateParkingLocationResponse()
                {
                    Success = await _parkingLocationService.AddUpdateParkingLocation(request.Parking)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in POST to /api/parkingLocations: {JsonConvert.SerializeObject(e)}");
                return new PostAddUpdateParkingLocationResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        [HttpGet("{locationUuid}")]
    public async Task<GetParkingLocationResponse> GetParkingLocation(string locationUuid)
    {
      try
      {
        return new GetParkingLocationResponse()
        {
          Success = true,
          Location = await _parkingLocationService.GetParkingLocation(locationUuid)
        };
      }
      catch(Exception e)
      {
        return new GetParkingLocationResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpGet("list")]
    public async Task<GetListParkingLocationsResponse> ListAllParkingLocations()
    {
      try
      {
        return new GetListParkingLocationsResponse()
        {
          Success = true,
          ParkingLocations = await _parkingLocationService.ListAllParkingLocations()
        };
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exception encountered in GET to /api/parkingLocations/list: {JsonConvert.SerializeObject(e)}");
        return new GetListParkingLocationsResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpGet("list/available")]
    public async Task<GetAvailableParkingLocationsResponse> ListAvailableParkingLocations()
    {
      try
      {
        return new GetAvailableParkingLocationsResponse()
        {
          Success = true,
          ParkingLocations = await _parkingLocationService.ListAvailableParkingLocations()
        };
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exception encountered in GET to /api/parkingLocations/list/available: {JsonConvert.SerializeObject(e)}");
        return new GetAvailableParkingLocationsResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpDelete]
    public async Task<DeleteParkingLocationResponse> DeleteParkingLocation(DeleteParkingLocationRequest request)
    {
      try
      {
        return new DeleteParkingLocationResponse()
        {
          Success = await _parkingLocationService.DeleteParkingLocation(request.LocationUuid)
        };
      }
      catch (Exception e)
      {
        return new DeleteParkingLocationResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }
  }
}
