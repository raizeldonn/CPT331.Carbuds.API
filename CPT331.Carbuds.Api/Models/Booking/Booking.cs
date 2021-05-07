using System;

namespace CPT331.Carbuds.Api.Models.Booking
{
  public class Booking
  {
    public string Uuid { get; set; }
    public string ClientEmail { get; set; }
    public string CarUuid { get; set; }
    public string PickUpLocation { get; set; }
    public string DropOffLocation { get; set; }
    public string StartDateTime { get; set; }
    public string EndDateTime { get; set; }
    public string Status { get; set; }
    public int Cost { get; set; }
  }
}