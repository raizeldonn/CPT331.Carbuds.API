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
        // get a list of parking locations from database
        Task<List<ParkingLocation>> ListAllParkingLocations();
        // add parking location in database
        Task<bool> AddUpdateParkingLocation(ParkingLocation record);
    }

    public class ParkingLocationService: IParkingLocationService
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

        // method to add or update a parking location to database
        public async Task<bool> AddUpdateParkingLocation(ParkingLocation record)
        {
            // store the uuid of the passed record
            var parkingUuid = record.Uuid;
            var parkingLocationExists = false;

            // scan the parking locations table for locations and store in scanReq variable
            ScanRequest scanReq = new ScanRequest()
            {
                TableName = _config.GetValue<string>("DynamoDb:Tablenames:ParkingLocationsTable"),
            };

            // check if the parking location uuid already exists in database
            var dbResult = await _dynamoDb.ScanAsync(scanReq);
            foreach (var item in dbResult.Items)
            {
                if(_utils.ToObjectFromDynamoResult<ParkingLocation>(item).Uuid == parkingUuid)
                {   // if yes update parkingLocationExists to true
                    parkingLocationExists = true;
                }
            }

            // add the parking location to the database if the location does not already exist
            if (parkingLocationExists == false)
            {
                var putReq = new PutItemRequest()
                {
                    TableName = _config.GetValue<string>("DynamoDb:Tablenames:ParkingLocations"),
                    Item = _utils.ToDynamoAttributeValueDictionary<ParkingLocation>(record)
                };
                var response = await _dynamoDb.PutItemAsync(putReq);
                return true;
            }

            return false;
        }

    }
}

