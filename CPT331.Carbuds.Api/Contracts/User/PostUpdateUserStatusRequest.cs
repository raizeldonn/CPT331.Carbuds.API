using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.User
{
  public class PostUpdateUserStatusRequest
  {
    public string UserEmail { get; set; }
    public bool AccountEnabled { get; set; }
  }
}
