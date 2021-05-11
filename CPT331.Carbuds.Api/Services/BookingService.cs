using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using CPT331.Carbuds.Api.Models.Booking;

namespace CPT331.Carbuds.Api.Services
{
  public interface IBookingService
  {
    Task<List<Booking>> ListAllBookings();
    Task<bool> AddUpdateBooking(Booking record);
    Task<Booking> GetBooking(string Uuid);
    Task<List<Booking>> ListClientsBookings(string userEmail);
  }

  public class BookingService : IBookingService
  {
    private IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;
    private IUtilityService _utils;

    public BookingService(IAmazonDynamoDB dynamoDb, IConfiguration config, IUtilityService utils)
    {
      _dynamoDb = dynamoDb;
      _config = config;
      _utils = utils;
    }

    public async Task<List<Booking>> ListAllBookings()
    {
      var bookingList = new List<Booking>();
      ScanRequest scanReq = new ScanRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:Bookings")
      };

      var dbResult = await _dynamoDb.ScanAsync(scanReq);
      foreach (var item in dbResult.Items)
      {
        bookingList.Add(_utils.ToObjectFromDynamoResult<Booking>(item));
      }

      return bookingList;
    }

    public async Task<List<Booking>> ListClientsBookings(string userEmail)
    {
      var bookingList = new List<Booking>();

      QueryRequest query = new QueryRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:TableNames:Bookings"),
        ReturnConsumedCapacity = "TOTAL",
        IndexName = "UserEmailIndex",
        KeyConditionExpression = "UserEmail = :v_UserEmail",
        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            {
                ":v_UserEmail",
                new AttributeValue
                {
                    S = userEmail
                }
            }
        }
      };

      var dbResult = await _dynamoDb.QueryAsync(query);
      foreach (var item in dbResult.Items)
      {
        bookingList.Add(_utils.ToObjectFromDynamoResult<Booking>(item));
      }

      return bookingList;
    }

    public async Task<bool> AddUpdateBooking(Booking record)
    {
      var putReq = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:Bookings"),
        Item = _utils.ToDynamoAttributeValueDictionary<Booking>(record)
      };
      var response = await _dynamoDb.PutItemAsync(putReq);

      return true;
    }

    public async Task<Booking> GetBooking(string Uuid)
    {
      Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
        {
        { "Uuid", new AttributeValue { S = Uuid } },
        { "UserEmail", new AttributeValue { S = "testuser@carbuds.io" } }
        };
      GetItemRequest itemReq = new GetItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:Bookings"),
        Key = key
      };
      var dbResult = await _dynamoDb.GetItemAsync(itemReq);
      var booking = _utils.ToObjectFromDynamoResult<Booking>(dbResult.Item);
      return booking;
    }
  }
}
