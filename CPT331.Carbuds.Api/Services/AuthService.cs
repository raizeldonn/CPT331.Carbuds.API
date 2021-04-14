using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Services
{
  public interface IAuthService
  {
    Task<AuthenticationResultType> Login(string username, string password, string clientId = null);
    }

  public class AuthService: IAuthService
  {
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private IConfiguration _config;

    public AuthService(IAmazonCognitoIdentityProvider cognito, IConfiguration config)
    {
      _config = config;
      _cognito = cognito;
    }

    public async Task<AuthenticationResultType> Login(string username, string password, string clientId = null)
    {
      var authReq = new AdminInitiateAuthRequest
      {
        UserPoolId = _config.GetValue<string>("Cognito:UserPoolId"),
        ClientId = string.IsNullOrEmpty(clientId) ? _config.GetValue<string>("Cognito:AppClientId") : clientId,
        AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
        AuthParameters = new Dictionary<string, string>()
        {
          {
            "USERNAME", username
          },
          {
            "PASSWORD", password
          }
        }
      };

      try
      {
        var response = await _cognito.AdminInitiateAuthAsync(authReq);
        return response.AuthenticationResult;
      }
      catch (Exception e)
      {
        throw new Exception($"Sorry there was a problem logging you in: {e.Message}");
        //todo - specific exception handling based on the type of exception here
      }
    }
    }
}
