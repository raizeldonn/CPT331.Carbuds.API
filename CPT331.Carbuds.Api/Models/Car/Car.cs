using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Models.Car
{
  public class Car
  {
    public string Uuid { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string ImageId { get; set; }
    public string Transmission { get; set; }
    public int Kilometers { get; set; }
    public string Body { get; set; }
    public string Location { get; set; }
    public int Doors { get; set; }
    public int Seats { get; set; }
    public decimal PriceHour { get; set; }
    public decimal PriceDay { get; set; }
    public bool IsActive { get; set; }
  }
}