using Amazon.DynamoDBv2;
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
        Task<ParkingLocation> GetParkingLocation(string parkingLocationId);
        Task<List<ParkingLocation>> ListAvailableParkingLocations();
        Task<bool> AddUpdateParkingLocation(ParkingLocation record);
        Task<bool> DeleteParkingLocation(string parkingLocationUuid);
    }

    public class ParkingLocationService : IParkingLocationService
    {
        private IAmazonDynamoDB _dynamoDb;
        private IConfiguration _config;
        private IUtilityService _utils;
        private IParkingAllocationService _plService;

        // constructor
        public ParkingLocationService(IAmazonDynamoDB dynamoDb, IConfiguration config, IUtilityService utils, IParkingAllocationService plService)
        {
            _dynamoDb = dynamoDb;
            _config = config;
            _utils = utils;
            _plService = plService;
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

        public async Task<ParkingLocation> GetParkingLocation(string parkingLocationId)
        {
            QueryRequest query = new QueryRequest()
            {
                TableName = _config.GetValue<string>("DynamoDb:Tablenames:ParkingLocations"),
                ReturnConsumedCapacity = "TOTAL",
                KeyConditionExpression = "#Uuid = :v_Uuid",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {
                        ":v_Uuid",
                        new AttributeValue
                        {
                            S = parkingLocationId
                        }
                    }
                },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                  {
                    "#Uuid" , "Uuid"
                  }
                }
            };

            var queryResult = await _dynamoDb.QueryAsync(query);
            return queryResult.Items.Any() ? _utils.ToObjectFromDynamoResult<ParkingLocation>(queryResult.Items.First()) : null;
        }

        public async Task<List<ParkingLocation>> ListAvailableParkingLocations()
        {
            var allParkingLocations = await ListAllParkingLocations();
            var allAllocations = await _plService.ListAllParkingAllocations();

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
