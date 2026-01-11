using CS.ApiGateway.UsersMicroservice.Data;
using CS.ApiGateway.UsersMicroservice.Enpoints;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDbContext") ?? throw new InvalidOperationException("Connection string 'UsersDbContext' not found.")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"); });
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.MapBasketItemEndpoints();

app.Run();
