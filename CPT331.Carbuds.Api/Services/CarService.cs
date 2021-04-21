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
    Task<bool> DeleteCar(string carUuid);
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

    public async Task<bool> DeleteCar(string carUuid)
    {
      var delReq = new DeleteItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:Cars"),
        Key = new Dictionary<string, AttributeValue>() {
            { "Uuid", new AttributeValue { S = carUuid } },
        }
      };

      var response = await _dynamoDb.DeleteItemAsync(delReq);
      return true;
    }

  }
}
