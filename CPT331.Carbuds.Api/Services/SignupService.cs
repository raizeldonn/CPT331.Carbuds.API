using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Services
{
  public interface ISignupService
  {
    Task<AdminCreateUserResponse> CreateUser(string Password, string Name, string Username, string email);
  }

  public class SignupService: ISignupService
  {
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private IConfiguration _config;

    public SignupService(IAmazonCognitoIdentityProvider cognito, IConfiguration config)
    {
      _config = config;
      _cognito = cognito;
    }

    public async Task<AdminCreateUserResponse> CreateUser(string password, string username, string name, string email)
    {
        var CreateUserRequest = new AdminCreateUserRequest
        {
            UserPoolId = _config.GetValue<string>("Cognito:UserPoolId"),
            TemporaryPassword = password,
            Username = username,
        };
        var nameAttribute = new AttributeType
        {
            Name = "Name",
            Value = name
        };
        var emailAttribute = new AttributeType
        {
            Name = "email",
            Value = name
        };
            CreateUserRequest.UserAttributes.Add(nameAttribute);
            CreateUserRequest.UserAttributes.Add(emailAttribute);

            try
        {
            var response = await _cognito.AdminCreateUserAsync(CreateUserRequest);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception($"Sorry there was a problem during registration: {e.Message}");
            //todo - specific exception handling based on the type of exception here
        }
    }
    }
}
