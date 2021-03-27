using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Auth
{
  public class PostLoginResponse: BaseResponse
  {
    public string IdToken { get; set; }
    public long TokenExpiresIn { get; set; }
  }
}
