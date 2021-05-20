
namespace CPT331.Carbuds.Api.Contracts.Booking
{
  public class GetBookingResponse : BaseResponse
  {
    public CPT331.Carbuds.Api.Models.Booking.Booking Booking { get; set; }
  }
}
