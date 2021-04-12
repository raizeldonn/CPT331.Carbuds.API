using CPT331.Carbuds.Api.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.User
{
  public class GetUserResponse: BaseResponse
  {
    public UserProfile User { get; set; }
  }
}
