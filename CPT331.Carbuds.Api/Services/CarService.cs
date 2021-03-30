using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPT331.Carbuds.Api.Models.Car;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;

namespace CPT331.Carbuds.Api.Services
{
  public interface ICarService
  {
    Task<List<Car>> ListAllCars();
    Task<bool> AddUpdateCar(Car record);
  }

  public class CarService: ICarService
  {
    private IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;
    private IUtilityService _utils;

    public CarService(IAmazonDynamoDB dynamoDb, IConfiguration config, IUtilityService utils)
    {
      _dynamoDb = dynamoDb;
      _config = config;
      _utils = utils;
    }

    public async Task<List<Car>> ListAllCars()
    {
      var carList = new List<Car>();
      ScanRequest scanReq = new ScanRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:Cars")
      };

      var dbResult = await _dynamoDb.ScanAsync(scanReq);
      foreach (var item in dbResult.Items)
      {
        carList.Add(_utils.ToObjectFromDynamoResult<Car>(item));
      }

      return carList;
      
    }

    public async Task<bool> AddUpdateCar(Car record)
    {
      var putReq = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:Cars"),
        Item = _utils.ToDynamoAttributeValueDictionary<Car>(record)
      };
      var response = await _dynamoDb.PutItemAsync(putReq);
      return true;
    }

    //QueryRequest query = new QueryRequest()
    //{
    //  TableName = _config.GetValue<string>("DynamoDb:TableNames:Cars"),
    //  ReturnConsumedCapacity = "TOTAL",
    //  KeyConditionExpression = "CustomerId = :v_CustomerId",
    //  ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
    //            {
    //                {
    //                    ":v_CustomerId",
    //                    new AttributeValue
    //                    {
    //                        S = customerId
    //                    }
    //                }
    //            }
    //};

  }
}
