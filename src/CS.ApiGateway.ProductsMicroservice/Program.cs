using CS.ApiGateway.ProductsMicroservice.Data;
using CS.ApiGateway.ProductsMicroservice.Enpoints;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProductsDbContext") ?? throw new InvalidOperationException("Connection string 'ProductsDbContext' not found.")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"); });
}

app.UseHttpsRedirection();

app.MapProductEndpoints();

app.Run();
