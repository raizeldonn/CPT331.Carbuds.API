using System;

namespace CPT331.Carbuds.Api.Models.Booking
{
  public class Booking
  {
    public string Uuid { get; set; }
    public string CarUuid { get; set; }
    public string ParkingUuid { get; set; }
    public string UserEmail { get; set; }
    public long StartDateTimeUtc { get; set; }
    public long EndDateTimeUtc { get; set; }
    public string Status { get; set; }
    public long Cost { get; set; }
  }
}