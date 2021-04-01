using CPT331.Carbuds.Api.Contracts.Signup;
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
    public class SignupController : ControllerBase
    {
        private ISignupService _signupService;

        public SignupController(ISignupService signupService)
        {
            _signupService = signupService;
        }

        [HttpPost("signup")]
        public async Task<PostSignupResponse> CreateUser(PostSignupRequest request)
        {
            try
            {
                var result = await _signupService.Signup(request.Email, request.Username, request.Password, request.Name, null);
                return new PostSignupResponse()
                {
                    isSuccess = true
                };

            }
            catch (Exception e)
            {
                return new PostSignupResponse()
                {
                    isSuccess = false
                };
            }

        }
    }
}
