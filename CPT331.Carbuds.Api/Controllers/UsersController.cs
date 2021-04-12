using CPT331.Carbuds.Api.Contracts.User;
using CPT331.Carbuds.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

    [HttpGet("{email}")]
    public async Task<GetUserResponse> GetUserInfo(string email)
    {

        try
        {
            return new GetUserResponse()
            {
                Success = true,
                User = await _userService.GetUserInfo(email)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception encountered in GET to /api/user/userInfo: {JsonConvert.SerializeObject(e)}");
            return new GetUserResponse()
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
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
