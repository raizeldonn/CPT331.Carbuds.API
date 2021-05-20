using System;

namespace CPT331.Carbuds.Api.Models.Booking
{
  public class Booking
  {
    public string Uuid { get; set; }
    public string CarUuid { get; set; }
    public string ParkingUuid { get; set; }
    public string UserEmail { get; set; }
    public string StartDate { get; set; }
    public string StartTime { get; set; }
    public string EndDate { get; set; }
    public string EndTime { get; set; }
    public string Status { get; set; }
    public int Cost { get; set; }
    }
}