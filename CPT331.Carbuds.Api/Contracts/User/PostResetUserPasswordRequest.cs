using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.User
{
  public class PostResetUserPasswordRequest
  {
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }
  }
}
