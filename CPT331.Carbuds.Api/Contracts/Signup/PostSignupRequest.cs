using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.Signup
{
  public class PostSignupRequest
  {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
  }
}
