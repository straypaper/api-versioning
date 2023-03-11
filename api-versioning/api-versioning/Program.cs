using api_versioning;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    // https://referbruv.com/blog/versioning-apis-in-aspnet-core-explained-strategies-and-implementations/
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc().AddApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            Console.WriteLine(description.GroupName);
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName); 
        }
    });
}

//app.UseApiVersioning();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();