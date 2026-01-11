using CS.ApiGateway.OrdersMicroservice.Data;
using CS.ApiGateway.OrdersMicroservice.Enpoints;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrdersDbContext") ?? throw new InvalidOperationException("Connection string 'OrdersDbContext' not found.")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"); });
}

app.UseHttpsRedirection();

app.MapOrderEndpoints();

app.MapOrderLineEndpoints();

app.Run();
