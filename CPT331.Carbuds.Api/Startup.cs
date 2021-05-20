using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPT331.Carbuds.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CPT331.Carbuds.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public static IConfiguration Configuration { get; private set; }
    public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.AddCors(options =>
      {
        options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder.WithOrigins("*"));
      });


      services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();
      services.AddAWSService<Amazon.CognitoIdentityProvider.IAmazonCognitoIdentityProvider>();

      services.AddSingleton<IUtilityService, UtilityService>();
      services.AddSingleton<IAuthService, AuthService>();
      services.AddSingleton<IUserService, UserService>();
      services.AddSingleton<IParkingAllocationService, ParkingAllocationService>();
      services.AddSingleton<ICarService, CarService>();
      services.AddSingleton<IParkingLocationService, ParkingLocationService>();
      services.AddSingleton<IBookingService, BookingService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();
      app.UseCors(MyAllowSpecificOrigins);

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapGet("/", async context =>
              {
            await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
          });
      });
    }
  }
}
