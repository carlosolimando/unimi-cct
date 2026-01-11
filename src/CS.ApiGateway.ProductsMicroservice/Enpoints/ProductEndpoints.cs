using CS.ApiGateway.ProductsMicroservice.Data;
using CS.ApiGateway.ProductsMicroservice.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace CS.ApiGateway.ProductsMicroservice.Enpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products").WithTags(nameof(Product));

        group.MapGet("/", async (ProductsDbContext db) =>
        {
            return await db.Product.ToListAsync();
        })
        .WithName("GetAllProducts");

        group.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (int id, ProductsDbContext db) =>
        {
            return await db.Product.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Product model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProductById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Product product, ProductsDbContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, product.Id)
                    .SetProperty(m => m.Name, product.Name)
                    .SetProperty(m => m.Description, product.Description)
                    .SetProperty(m => m.Category, product.Category)
                    .SetProperty(m => m.Price, product.Price)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProduct");

        group.MapPost("/", async (Product product, ProductsDbContext db) =>
        {
            db.Product.Add(product);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/products/{product.Id}", product);
        })
        .WithName("CreateProduct");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ProductsDbContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProduct");
    }
}
