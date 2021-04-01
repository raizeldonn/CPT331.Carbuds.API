using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Signup
{
  public class PostSignupResponse: BaseResponse
  {
    public bool isSuccess { get; set; }
  }
}
