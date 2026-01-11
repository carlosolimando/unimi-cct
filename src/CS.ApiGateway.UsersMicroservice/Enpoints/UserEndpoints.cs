using CS.ApiGateway.Core.Models;
using CS.ApiGateway.UsersMicroservice.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CS.ApiGateway.UsersMicroservice.Enpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags(nameof(User));

        group.MapGet("/", async (UsersDbContext db) =>
        {
            return await db.User.ToListAsync();
        })
        .WithName("GetAllUsers");

        group.MapGet("/{id}", async Task<Results<Ok<User>, NotFound>> (int id, UsersDbContext db) =>
        {
            return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, User user, UsersDbContext db) =>
        {
            var affected = await db.User
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, user.Id)
                    .SetProperty(m => m.FirstName, user.FirstName)
                    .SetProperty(m => m.LastName, user.LastName)
                    .SetProperty(m => m.UserName, user.UserName)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUser");
        
        group.MapPost("/", async (User user, ClaimsPrincipal claimsPrincipal, UsersDbContext db) =>
        {
            var claimsDictionary = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
            var userFromClaims = new User
            {
                FirstName = claimsDictionary["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"],
                LastName = claimsDictionary["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"],
                UserName = claimsDictionary["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
            };

            db.User.Add(userFromClaims);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/users/{userFromClaims.Id}", userFromClaims);
        })
        .WithName("CreateUser")
        .RequireAuthorization();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, UsersDbContext db) =>
        {
            var affected = await db.User
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser");
    }
    public static void MapBasketItemEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/basketitems").WithTags(nameof(BasketItem));

        group.MapGet("/", async (UsersDbContext db) =>
        {
            return await db.BasketItem.ToListAsync();
        })
        .WithName("GetAllBasketItems");

        group.MapGet("/{id}", async Task<Results<Ok<BasketItem>, NotFound>> (int id, UsersDbContext db) =>
        {
            return await db.BasketItem.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is BasketItem model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBasketItemById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, BasketItem basketItem, UsersDbContext db) =>
        {
            var affected = await db.BasketItem
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, basketItem.Id)
                  .SetProperty(m => m.UserId, basketItem.UserId)
                  .SetProperty(m => m.ProductId, basketItem.ProductId)
                  .SetProperty(m => m.ProductName, basketItem.ProductName)
                  .SetProperty(m => m.Quantity, basketItem.Quantity)
                  .SetProperty(m => m.Price, basketItem.Price)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBasketItem");

        group.MapPost("/", async (BasketItem basketItem, UsersDbContext db) =>
        {
            db.BasketItem.Add(basketItem);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/basketitems/{basketItem.Id}", basketItem);
        })
        .WithName("CreateBasketItem");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, UsersDbContext db) =>
        {
            var affected = await db.BasketItem
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBasketItem");
    }
}
