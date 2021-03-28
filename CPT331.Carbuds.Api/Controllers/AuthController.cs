using CPT331.Carbuds.Api.Contracts.Auth;
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
  public class AuthController : ControllerBase
  {

    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("login")]
    public async Task<PostLoginResponse> Login(PostLoginRequest request)
    {

      try
      {
        var authResult = await _authService.Login(request.Username, request.Password, request.ClientId);
        return new PostLoginResponse()
        {
          Success = true,
          IdToken = authResult.IdToken,
          TokenExpiresIn = authResult.ExpiresIn
        };
      }
      catch (Exception e)
      {
        return new PostLoginResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }

    }


  }
}
