using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.User
{
  public class PostVerifyUserRequest
  {
    public string Email { get; set; }
    public string VerificationCode { get; set; }
  }
}
