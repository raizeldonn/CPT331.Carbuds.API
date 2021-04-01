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
        public async Task<PostCreateUserResponse> CreateUser(PostCreateUserRequest request)
        {
            try
            {
                var result = await _signupService.CreateUser(request.Email, request.Username, request.Password, request.Name);
                return new PostCreateUserResponse()
                {
                    isSuccess = true
                };

            }
            catch (Exception e)
            {
                return new PostCreateUserResponse()
                {
                    isSuccess = false
                };
            }

        }
    }
}
