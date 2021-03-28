using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Auth
{
  public class PostLoginRequest
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
  }
}
