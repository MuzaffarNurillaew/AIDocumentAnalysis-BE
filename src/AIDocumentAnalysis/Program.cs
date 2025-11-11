using System.Text.Json;

using AIDocumentAnalysis.Extensions;

using FastEndpoints.Swagger;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.AllowSynchronousIO = false;
});


builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddSwaggerDocument();
builder.Services.AddFastEndpoints().AddOpenApiDocument().AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(options =>
{
    options.Errors.ResponseBuilder = (errors, _, _) => errors.ToResponse();
    options.Errors.StatusCode = StatusCodes.Status422UnprocessableEntity;
    options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

app.UseOpenApi();
app.UseSwaggerUi(x => x.ConfigureDefaults());

await app.RunAsync();

public partial class Program
{
}