using CPT331.Carbuds.Api.Contracts.Car;
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
            catch (Exception e)
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
                    Success = true,
                    Cars = await _carService.ListAllCars()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in GET to /api/cars/list: {JsonConvert.SerializeObject(e)}");
                return new GetListCarsResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        [HttpGet("getByCarId{Uuid}")]
        public async Task<GetCarResponse> GetCar(string Uuid)
        {
            try
            {
                return new GetCarResponse()
                {
                    Success = true,
                    car = await _carService.GetCar(Uuid)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in GET to /api/cars/Uuid: {JsonConvert.SerializeObject(e)}");
                return new GetCarResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }
        [HttpGet("getByParkingId{Uuid}")]
        public async Task<GetCarResponse> GetCarByParkingId(string Uuid)
        {
            try
            {
                return new GetCarResponse()
                {
                    Success = true,
                    car = await _carService.GetCarByParkingId(Uuid)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in GET to /api/cars/Uuid: {JsonConvert.SerializeObject(e)}");
                return new GetCarResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        [HttpDelete]
        public async Task<DeleteCarResponse> DeleteCar(DeleteCarRequest request)
        {
            try
            {
                return new DeleteCarResponse()
                {
                    Success = await _carService.DeleteCar(request.carUuid)
                };
            }
            catch (Exception e)
            {
                return new DeleteCarResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }

}
