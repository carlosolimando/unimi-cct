using CS.ApiGateway.Core.Models;
using CS.ApiGateway.OrdersMicroservice.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace CS.ApiGateway.OrdersMicroservice.Enpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/orders").WithTags(nameof(Order));

        group.MapGet("/", async (OrdersDbContext db) =>
        {
            return await db.Order.ToListAsync();
        })
        .WithName("GetAllOrders");

        group.MapGet("/{id}", async Task<Results<Ok<Order>, NotFound>> (int id, OrdersDbContext db) =>
        {
            return await db.Order.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Order model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrderById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Order order, OrdersDbContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, order.Id)
                    .SetProperty(m => m.Code, order.Code)
                    .SetProperty(m => m.User, order.User)
                    .SetProperty(m => m.TotalAmount, order.TotalAmount)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrder");

        group.MapPost("/", async (Order order, OrdersDbContext db) =>
        {
            db.Order.Add(order);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/orders/{order.Id}", order);
        })
        .WithName("CreateOrder");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, OrdersDbContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrder");
    }
    public static void MapOrderLineEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/orderlines").WithTags(nameof(OrderLine));

        group.MapGet("/", async (OrdersDbContext db) =>
        {
            return await db.OrderLine.ToListAsync();
        })
        .WithName("GetAllOrderLines");

        group.MapGet("/{id}", async Task<Results<Ok<OrderLine>, NotFound>> (int id, OrdersDbContext db) =>
        {
            return await db.OrderLine.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is OrderLine model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrderLineById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, OrderLine orderLine, OrdersDbContext db) =>
        {
            var affected = await db.OrderLine
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, orderLine.Id)
                  .SetProperty(m => m.LineNumber, orderLine.LineNumber)
                  .SetProperty(m => m.OrderId, orderLine.OrderId)
                  .SetProperty(m => m.Product, orderLine.Product)
                  .SetProperty(m => m.Quantity, orderLine.Quantity)
                  .SetProperty(m => m.Amount, orderLine.Amount)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrderLine");

        group.MapPost("/", async (OrderLine orderLine, OrdersDbContext db) =>
        {
            db.OrderLine.Add(orderLine);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/orderlines/{orderLine.Id}", orderLine);
        })
        .WithName("CreateOrderLine");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, OrdersDbContext db) =>
        {
            var affected = await db.OrderLine
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrderLine");
    }
}
