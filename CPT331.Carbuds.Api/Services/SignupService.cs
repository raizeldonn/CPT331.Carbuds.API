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
    Task<SignUpResponse> Signup(string Password, string Name, string Username, string email, string clientId);
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

    public async Task<SignUpResponse> Signup(string password, string username, string name, string email, string clientId = null)
    {
        var SignupRequest = new SignUpRequest
        {
            ClientId = string.IsNullOrEmpty(clientId) ? _config.GetValue<string>("Cognito:AppClientId") : clientId,
            //ClientId = "1btr18nik21i9fn341cou475tg",
            Password = password,
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
            Value = email
        };
            SignupRequest.UserAttributes.Add(nameAttribute);
            SignupRequest.UserAttributes.Add(emailAttribute);

            try
        {
            var response = await _cognito.SignUpAsync(SignupRequest);
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
