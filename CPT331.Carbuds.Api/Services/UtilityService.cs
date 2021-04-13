using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CPT331.Carbuds.Api.Services
{

  public interface IUtilityService
  {
    Dictionary<string, AttributeValue> ToDynamoAttributeValueDictionary<T>(T obj);
    T ToObjectFromDynamoResult<T>(Dictionary<string, AttributeValue> dynamoObj) where T : new();
    DateTime ConvertFromEpochTime(double timestamp);
    long ConvertToEpochTime(DateTime date);
  }

  public class UtilityService: IUtilityService
  {
    public Dictionary<string, AttributeValue> ToDynamoAttributeValueDictionary<T>(T obj)
    {

      Dictionary<string, AttributeValue> objectProps = new Dictionary<string, AttributeValue>();
      PropertyInfo[] props = obj.GetType().GetProperties();

      foreach (var prop in props)
      {
        AttributeValue propertyValue;
        switch (prop.PropertyType.Name.ToLower())
        {
          case "string":
            propertyValue = new AttributeValue()
            {
              S = (string)prop.GetValue(obj)
            };
            break;
          //todo - make sure other nullables don't fall under this weird nullable 1 thing
          case "nullable`1":
            if (prop.GetValue(obj) != null)
            {
              propertyValue = new AttributeValue()
              {
                N = prop.GetValue(obj).ToString()
              };
            }
            else
            {
              propertyValue = null;
            }
            break;
          case "int":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "int32":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "int64":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "long":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "float":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "decimal":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "bool":
            propertyValue = new AttributeValue()
            {
              BOOL = (bool)prop.GetValue(obj)
            };
            break;
          case "boolean":
            propertyValue = new AttributeValue()
            {
              BOOL = (bool)prop.GetValue(obj)
            };
            break;
          case "datetime":
            propertyValue = new AttributeValue()
            {
              S = ((DateTime)prop.GetValue(obj)).ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            };
            break;
          default:
            propertyValue = new AttributeValue()
            {
              S = (string)prop.GetValue(obj)
            };
            break;
        }

        if (propertyValue != null)
        {
          objectProps.Add(prop.Name, propertyValue);
        }
      }

      return objectProps;
    }

    public T ToObjectFromDynamoResult<T>(Dictionary<string, AttributeValue> dynamoObj) where T : new()
    {
      var returnObj = new T();
      Dictionary<string, AttributeValue> objectProps = new Dictionary<string, AttributeValue>();

      PropertyInfo[] props = returnObj.GetType().GetProperties();

      foreach (var prop in props)
      {
        var dynamoObjValue = dynamoObj.Where(d => d.Key.ToLower() == prop.Name.ToLower()).FirstOrDefault().Value;
        if (dynamoObjValue != null)
        {
          switch (prop.PropertyType.Name.ToLower())
          {
            case "string":
              prop.SetValue(returnObj, dynamoObjValue.S);
              break;
            case "int":
              prop.SetValue(returnObj, Convert.ToInt32(dynamoObjValue.N));
              break;
            case "int32":
              prop.SetValue(returnObj, Convert.ToInt32(dynamoObjValue.N));
              break;
            case "int64":
              prop.SetValue(returnObj, Convert.ToInt64(dynamoObjValue.N));
              break;
            case "long":
              prop.SetValue(returnObj, Convert.ToInt64(dynamoObjValue.N));
              break;
            case "nullable`1":
              prop.SetValue(returnObj, Convert.ToInt64(dynamoObjValue.N));
              break;
            case "double":
              prop.SetValue(returnObj, Convert.ToDouble(dynamoObjValue.S));
              break;
            case "decimal":
              prop.SetValue(returnObj, Convert.ToDecimal(dynamoObjValue.N));
              break;
            case "boolean":
              prop.SetValue(returnObj, dynamoObjValue.BOOL);
              break;
            case "datetime":
              prop.SetValue(returnObj, DateTime.Parse(dynamoObjValue.S));
              break;
          }
        }
      }

      return returnObj;
    }

    public DateTime ConvertFromEpochTime(double timestamp)
    {
      DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      return origin.AddSeconds(timestamp);
    }

    public long ConvertToEpochTime(DateTime date)
    {
      DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      TimeSpan diff = date.ToUniversalTime() - origin;
      return (long)Math.Floor(diff.TotalMilliseconds);
    }
  }


}