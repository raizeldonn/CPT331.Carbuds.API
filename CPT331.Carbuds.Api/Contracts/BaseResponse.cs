using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts
{
  public class BaseResponse
  {
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
  }
}
