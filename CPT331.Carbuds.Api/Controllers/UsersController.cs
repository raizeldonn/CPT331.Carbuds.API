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
                User = await _userService.GetUserInfo(email),
                UserActive = await _userService.GetCognitoUserActivatedStatus(email)
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

    [HttpGet("list")]
    public async Task<GetListUsersResponse> ListUsers()
    {
      try
      {
        return new GetListUsersResponse()
        {
          Success = true,
          Users = await _userService.ListUsers()
        };
      }
      catch(Exception e)
      {
        return new GetListUsersResponse()
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

    [HttpPost("usersignup")]
    public async Task<PostCreateCognitoUserResponse> UserSignUp(PostCreateCognitoUserRequest request)
    {
      try
      {
        return new PostCreateCognitoUserResponse()
        {
          Success = await _userService.SelfServeSignUpuser(request)
        };
      }
      catch (Exception e)
      {
        return new PostCreateCognitoUserResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpPost("verify")]
    public async Task<PostVerifyUserResponse> VerifyUser(PostVerifyUserRequest request)
    {
      try
      {
        return new PostVerifyUserResponse()
        {
          Success = await _userService.VerifyCognitoUser(request)
        };
      }
      catch(Exception e)
      {
        return new PostVerifyUserResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }
  }
}
