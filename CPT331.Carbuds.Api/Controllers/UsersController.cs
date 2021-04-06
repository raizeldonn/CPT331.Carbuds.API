using CPT331.Carbuds.Api.Contracts.User;
using CPT331.Carbuds.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;
    
    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost("signup")]
    public async Task<PostCreateCognitoUserResponse> SignUp(PostCreateCognitoUserRequest request)
    {
      try
      {
        return new PostCreateCognitoUserResponse()
        {
          Success = await _userService.CreateCognitoUser(request)
        };
      }
      catch(Exception e)
      {
        return new PostCreateCognitoUserResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }
  }
}
