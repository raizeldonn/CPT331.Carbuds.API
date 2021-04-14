using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Contracts.User
{
  public class PostCreateCognitoUserRequest
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string CardNumber { get; set; }
    public string CardExpiry { get; set; }
    public string CardCvv { get; set; }
  }
}