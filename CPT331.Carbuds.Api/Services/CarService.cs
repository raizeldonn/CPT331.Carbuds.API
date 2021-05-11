using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPT331.Carbuds.Api.Models.Car;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using CPT331.Carbuds.Api.Models.ParkingLocation;

namespace CPT331.Carbuds.Api.Services
{
  public interface ICarService
  {
    Task<List<Car>> ListAllCars();
    Task<bool> AddUpdateCar(Car record);
    Task<Car> GetCar(string Uuid);
    Task<bool> DeleteCar(string carUuid);
  }

  public class CarService: ICarService
  {
    private IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;
    private IUtilityService _utils;
    private IParkingAllocationService _plService;

    public CarService(IAmazonDynamoDB dynamoDb, IConfiguration config, IUtilityService utils, IParkingAllocationService plService)
    {
      _dynamoDb = dynamoDb;
      _config = config;
      _utils = utils;
      _plService = plService;
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

      //todo - wrap this in a check to see if the record has changed before doing the allocation update process.
      var existingParkingAllocation = await _plService.GetParkingAllocationByCar(record.Uuid);
      if(existingParkingAllocation != null)
      {
        var existingAllocationDeleted = await _plService.DeleteParkingAllocation(existingParkingAllocation);
      }

      ParkingAllocation newAlloc = new ParkingAllocation()
      {
        CarUuid = record.Uuid,
        LocationUuid = record.Location
      };
      var newParkingLocation = await _plService.AddEditParkingAllocation(newAlloc);

      return true;
    }

    public async Task<Car> GetCar(string Uuid)
    {
        Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
        {
        { "Uuid", new AttributeValue { S = Uuid } },
        };
        GetItemRequest itemReq = new GetItemRequest()
        {
            TableName = _config.GetValue<string>("DynamoDb:Tablenames:Cars"),
            Key = key
        };
        var dbResult = await _dynamoDb.GetItemAsync(itemReq);
        var car = _utils.ToObjectFromDynamoResult<Car>(dbResult.Item);
        return car;
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
