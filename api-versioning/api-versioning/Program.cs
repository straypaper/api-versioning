using api_versioning;
using api_versioning.Handlers;
using api_versioning.Requests;
using Asp.Versioning;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    // https://referbruv.com/blog/versioning-apis-in-aspnet-core-explained-strategies-and-implementations/
    opt.ApiVersionReader = new QueryStringApiVersionReader(); //?api-version=2.0
    //opt.ApiVersionReader = new UrlSegmentApiVersionReader(); //v2.0/ update route to [Route("v{version:apiVersion}/[controller]")]
    //opt.ApiVersionReader = new HeaderApiVersionReader("api-version"); //api-version: 2.0
    //opt.ApiVersionReader = new MediaTypeApiVersionReader();  //Content-Type: application/json;v=2.0
}).AddMvc().AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen( options => options.OperationFilter<SwaggerDefaultValues>() );

builder.Services.AddScoped<IRequestHandler<WeatherForecastRequest<IEnumerable<api_versioning.Contracts.v1.WeatherForecast>>, IEnumerable<api_versioning.Contracts.v1.WeatherForecast>>, WeatherForecastHandler>();
builder.Services.AddScoped<IRequestHandler<WeatherForecastRequest<IEnumerable<api_versioning.Contracts.v2.WeatherForecast>>, IEnumerable<api_versioning.Contracts.v2.WeatherForecast>>, WeatherForecastHandler>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach ( var description in app.DescribeApiVersions() )
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName );
        }
        
        // var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        // foreach (var description in provider.ApiVersionDescriptions)
        // {
        //     Console.WriteLine(description.ApiVersion);
        //     options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName); 
        // }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();