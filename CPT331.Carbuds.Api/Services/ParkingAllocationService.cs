using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using CPT331.Carbuds.Api.Models.ParkingLocation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Services
{
  public interface IParkingAllocationService
  {
    Task<List<ParkingAllocation>> ListAllParkingAllocations();
    Task<ParkingAllocation> GetParkingAllocationByCar(string carUuid);
    Task<bool> DeleteParkingAllocation(ParkingAllocation record);
    Task<bool> AddEditParkingAllocation(ParkingAllocation record);
  }

  public class ParkingAllocationService : IParkingAllocationService
  {
    private IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;
    private IUtilityService _utils;

    public ParkingAllocationService(IAmazonDynamoDB dynamoDb, IConfiguration config, IUtilityService utils)
    {
      _dynamoDb = dynamoDb;
      _config = config;
      _utils = utils;
    }

    public async Task<List<ParkingAllocation>> ListAllParkingAllocations()
    {
      var allocationList = new List<ParkingAllocation>();
      ScanRequest scanReq = new ScanRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:CarParkingAllocations")
      };
      var dbResult = await _dynamoDb.ScanAsync(scanReq);

      foreach (var item in dbResult.Items)
      {
        allocationList.Add(_utils.ToObjectFromDynamoResult<ParkingAllocation>(item));
      }
      return allocationList;
    }

    public async Task<ParkingAllocation> GetParkingAllocationByCar(string carUuid)
    {
      QueryRequest query = new QueryRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:CarParkingAllocations"),
        ReturnConsumedCapacity = "TOTAL",
        KeyConditionExpression = "CarUuid = :v_CarUuid",
        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
          {
              {
                  ":v_CarUuid",
                  new AttributeValue
                  {
                      S = carUuid
                  }
              }
          }
      };

      var queryResult = await _dynamoDb.QueryAsync(query);
      return queryResult.Items.Any() ? _utils.ToObjectFromDynamoResult<ParkingAllocation>(queryResult.Items.First()) : null;
    }

    public async Task<bool> DeleteParkingAllocation(ParkingAllocation record)
    {
      var delReq = new DeleteItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:CarParkingAllocations"),
        Key = new Dictionary<string, AttributeValue>() {
            { "CarUuid", new AttributeValue { S = record.CarUuid } },
            { "LocationUuid", new AttributeValue { S = record.LocationUuid } }
        }
      };

      var response = await _dynamoDb.DeleteItemAsync(delReq);
      return true;
    }

    public async Task<bool> AddEditParkingAllocation(ParkingAllocation record)
    {
      var putReq = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:CarParkingAllocations"),
        Item = _utils.ToDynamoAttributeValueDictionary<ParkingAllocation>(record)
      };
      var response = await _dynamoDb.PutItemAsync(putReq);
      return true;
    }

  }
}
