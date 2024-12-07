using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WebApplication2.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ConsumesAttribute("application/json"));// Request body content type 
    options.Filters.Add(new ProducesAttribute("application/json"));// Response body content type 
}).AddXmlSerializerFormatters();


// Configuring Db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString"));
});



// adding versioning as parameter

var apiVersioningBuilder = builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader(); //Reads version number from request url at "apiVersion" constraint

    //config.ApiVersionReader = new QueryStringApiVersionReader(); //Reads version number from request query string called "version". Eg:version=1.0

    //config.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Reads version number from request header called "api-version". Eg: api-version: 1.0

    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});


apiVersioningBuilder.AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV"; //v1
    options.SubstituteApiVersionInUrl = true;
});
var app = builder.Build();



//Swagger
builder.Services.AddEndpointsApiExplorer(); // Generates description for all endpoints (it enables the swagger to read details(http method, url,attributes) of our endpoints )

builder.Services.AddSwaggerGen(options =>  // it configures the swagger to generate documentation for API's endpoints
{ 
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });

    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() {Title="Cities Web API", Version="2.0" });
});

   



// Configure the HTTP request pipeline.

app.UseHsts();  

app.UseHttpsRedirection();

app.UseSwagger(); // creates end points for swagger.json

app.UseSwaggerUI( options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});  //creates swagger UI for testing all web Api endpoints/action methods

app.UseAuthorization();

app.MapControllers();



app.Run();
