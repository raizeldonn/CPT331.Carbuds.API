using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Booking
{
  public class GetListBookingsResponse: BaseResponse
  {
    public List<CPT331.Carbuds.Api.Models.Booking.Booking> Bookings { get; set; }
  }
} 
