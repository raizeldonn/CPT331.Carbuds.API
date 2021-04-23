﻿using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPT331.Carbuds.Api.Models.ParkingLocation;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;

namespace CPT331.Carbuds.Api.Services
{
  public interface IParkingLocationService
  {
    Task<List<ParkingLocation>> ListAllParkingLocations();
    Task<List<ParkingAllocation>> ListAllParkingAllocations();
    Task<List<ParkingLocation>> ListAvailableParkingLocations();
    Task<bool> AddUpdateParkingLocation(ParkingLocation record);
    Task<bool> DeleteParkingLocation(string parkingLocationUuid);
  }

  public class ParkingLocationService : IParkingLocationService
  {
    private IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;
    private IUtilityService _utils;

    // constructor
    public ParkingLocationService(IAmazonDynamoDB dynamoDb, IConfiguration config, IUtilityService utils)
    {
      _dynamoDb = dynamoDb;
      _config = config;
      _utils = utils;
    }

    // method to retrieve a list of parking locations from database
    public async Task<List<ParkingLocation>> ListAllParkingLocations()
    {
      var parkingLocationList = new List<ParkingLocation>();
      ScanRequest scanReq = new ScanRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:ParkingLocations")
      };

      var dbResult = await _dynamoDb.ScanAsync(scanReq);
      foreach (var item in dbResult.Items)
      {
        parkingLocationList.Add(_utils.ToObjectFromDynamoResult<ParkingLocation>(item));
      }

      return parkingLocationList;

    }

    public async Task<List<ParkingAllocation>> ListAllParkingAllocations()
    {
      var allocationList = new List<ParkingAllocation>();
      ScanRequest scanReq = new ScanRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:CarParkingAllocations")
      };
      var dbResult = await _dynamoDb.ScanAsync(scanReq);

      foreach(var item in dbResult.Items)
      {
        allocationList.Add(_utils.ToObjectFromDynamoResult<ParkingAllocation>(item));
      }
      return allocationList;
    }

    public async Task<List<ParkingLocation>> ListAvailableParkingLocations()
    {
      var allParkingLocations = await ListAllParkingLocations();
      var allAllocations = await ListAllParkingAllocations();

      return allParkingLocations.Except(allParkingLocations.Join(allAllocations, p => p.Uuid, a => a.LocationUuid, (p, a) => p)).ToList();     
    }
    
    public async Task<bool> AddUpdateParkingLocation(ParkingLocation record)
    {
      var putReq = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:ParkingLocations"),
        Item = _utils.ToDynamoAttributeValueDictionary<ParkingLocation>(record)
      };
      var response = await _dynamoDb.PutItemAsync(putReq);
      return true;
    }

    public async Task<bool> DeleteParkingLocation(string parkingLocationUuid)
    {
      var delReq = new DeleteItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDb:Tablenames:ParkingLocations"),
        Key = new Dictionary<string, AttributeValue>() {
            { "Uuid", new AttributeValue { S = parkingLocationUuid } },
        }
      };

      var response = await _dynamoDb.DeleteItemAsync(delReq);
      return true;
    }
  }
}
