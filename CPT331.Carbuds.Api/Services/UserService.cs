using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using CPT331.Carbuds.Api.Contracts.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Services
{
  public interface IUserService
  {
    Task<bool> CreateCognitoUser(PostCreateCognitoUserRequest request);
  }

  public class UserService: IUserService
  {
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private IConfiguration _config;

    public UserService(IAmazonCognitoIdentityProvider cognito, IConfiguration config)
    {
      _cognito = cognito;
      _config = config;
    }

    public async Task<bool> CreateCognitoUser(PostCreateCognitoUserRequest request)
    {

      AdminCreateUserRequest createUserReq = new AdminCreateUserRequest()
      {
        UserPoolId = _config.GetValue<string>("Cognito:UserPoolId"),
        Username = request.Email,
        TemporaryPassword = request.Password,
        UserAttributes = new List<AttributeType>() {
            new AttributeType()
            {
              Name = "email_verified",
              Value = "True"
            },
            new AttributeType()
            {
                Name = "email",
                Value = request.Email
            }
        },
        DesiredDeliveryMediums = new List<string>() { "EMAIL" },
        MessageAction = "SUPPRESS"
      };

      var userCreated = await _cognito.AdminCreateUserAsync(createUserReq);

      AdminAddUserToGroupRequest addToGroupReq = new AdminAddUserToGroupRequest()
      {
        GroupName = "carbuds-users",
        Username = request.Email,
        UserPoolId = _config.GetValue<string>("Cognito:UserPoolId")
      };
      var addComplete = await _cognito.AdminAddUserToGroupAsync(addToGroupReq);

      var pwSet = await SetCognitoUserPassword(request.Email, request.Password);      

      //todo- create a blank user profile here maybe?

      return true;
    }


    public async Task<bool> SetCognitoUserPassword(string userEmail, string newPassword)
    {
      AdminSetUserPasswordRequest confirmPw = new AdminSetUserPasswordRequest()
      {
        Username = userEmail,
        UserPoolId = _config.GetValue<string>("Cognito:UserPoolId"),
        Password = newPassword,
        Permanent = true
      };

      var setPw = await _cognito.AdminSetUserPasswordAsync(confirmPw);
      return true;
    }
  }
}
