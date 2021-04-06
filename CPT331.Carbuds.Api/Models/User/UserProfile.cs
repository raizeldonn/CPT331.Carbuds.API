using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Models.User
{
  public class UserProfile
  {
    public string Email { get; set; }
    public string Name { get; set; }
    public string PaymentCardNumber { get; set; }
    public string PaymentCardExpiry { get; set; }
    public string PaymentCardCvv { get; set; }
  }
}
