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
    }

    public class ParkingAllocationService: IParkingAllocationService
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
        
        
    }
}
