using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using CPT331.Carbuds.Api.Contracts.User;
using CPT331.Carbuds.Api.Models.User;
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
    Task<UserProfile> GetUserInfo(string userId);
    }

  public class UserService: IUserService
  {
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private IConfiguration _config;
    private IAmazonDynamoDB _dynamoDb;
    private IUtilityService _utils;

    public UserService(IAmazonCognitoIdentityProvider cognito, IConfiguration config, IAmazonDynamoDB dynamoDb, IUtilityService utils)
    {
      _cognito = cognito;
      _config = config;
      _dynamoDb = dynamoDb;
      _utils = utils;
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
      UserProfile newUserProfile = new UserProfile()
      {
        Email = request.Email,
        Name = request.Name,
        PaymentCardNumber = request.CardNumber,
        PaymentCardCvv = request.CardCvv,
        PaymentCardExpiry = request.CardExpiry,
        LicenseCountry = request.LicenseCountry,
        LicenseNumber = request.LicenseNumber
      };
      var profileCreated = await AddUpdateUserProfile(newUserProfile);

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

    public async Task<bool> AddUpdateUserProfile(UserProfile profile)
    {
      var putReq = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:UserProfiles"),
        Item = _utils.ToDynamoAttributeValueDictionary<UserProfile>(profile)
      };
      var response = await _dynamoDb.PutItemAsync(putReq);
      return true;
    }
        public async Task<UserProfile> GetUserInfo(string userEmail)
        {
            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
            { "Email", new AttributeValue { S = userEmail } },
            };
            GetItemRequest itemReq = new GetItemRequest()
            {
                TableName = _config.GetValue<string>("DynamoDb:Tablenames:UserProfiles"),
                Key = key
            };
            var dbResult = await _dynamoDb.GetItemAsync(itemReq);
            var user = _utils.ToObjectFromDynamoResult<UserProfile>(dbResult.Item);

            return user;
        }
    }
}
