using CPT331.Carbuds.Api.Contracts.Booking;
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
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<PostAddUpdateBookingResponse> AddUpdateBooking(PostAddUpdateBookingRequest request)
        {
            try
            {
                return new PostAddUpdateBookingResponse()
                {
                    Success = await _bookingService.AddUpdateBooking(request.Booking)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in POST to /api/booking: {JsonConvert.SerializeObject(e)}");
                return new PostAddUpdateBookingResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        [HttpGet("list")]
        public async Task<GetListBookingsResponse> ListAllCars()
        {
            try
            {
                return new GetListBookingsResponse()
                {
                    Success = true,
                    Bookings = await _bookingService.ListAllBookings()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in GET to /api/cars/list: {JsonConvert.SerializeObject(e)}");
                return new GetListBookingsResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }

        [HttpGet("{Uuid}")]
        public async Task<GetBookingResponse> GetCar(string Uuid)
        {
            try
            {
                return new GetBookingResponse()
                {
                    Success = true,
                    Booking = await _bookingService.GetBooking(Uuid)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in GET to /api/cars/Uuid: {JsonConvert.SerializeObject(e)}");
                return new GetBookingResponse()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
