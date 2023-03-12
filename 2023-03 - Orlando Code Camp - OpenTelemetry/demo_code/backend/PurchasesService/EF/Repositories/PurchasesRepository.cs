using Microsoft.EntityFrameworkCore;

using ProgrammerAl.Presentations.OTel.PurchasesService.EF.Entities;
using ProgrammerAl.Presentations.OTel.PurchasesService.EF.QueryResults;

namespace ProgrammerAl.Presentations.OTel.PurchasesService.EF.Repositories;

public interface IPurchasesRepository
{
    ValueTask CreatePurchaseAsync(int productId, string? userId);
    ValueTask<ImmutableArray<UserPurchase>> GetUserPurchasesAsync(string userId);
}

public class PurchasesRepository : IPurchasesRepository
{
    private readonly IDbContextFactory<PurchasesServiceDbContext> _contextFactory;

    public PurchasesRepository(IDbContextFactory<PurchasesServiceDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask CreatePurchaseAsync(int productId, string? userId)
    {
        using var activity = ActivitySources.PurchasesServiceSource.StartActivity("CreatePurchase");

        using var context = await _contextFactory.CreateDbContextAsync();
        _ = await context.Purchases.AddAsync(new PurchaseEntity
        {
            CreatedUtc = DateTime.UtcNow,
            ProductId = productId,
            UserId = userId
        });

        _ = await context.SaveChangesAsync();
    }

    public async ValueTask<ImmutableArray<UserPurchase>> GetUserPurchasesAsync(string userId)
    {
        using var activity = ActivitySources.PurchasesServiceSource.StartActivity("GetUserPurchases");

        using var context = await _contextFactory.CreateDbContextAsync();
        var products = await context.Purchases
            .Where(x => string.Equals(x.UserId, userId, StringComparison.OrdinalIgnoreCase))
            .Select(x => new { PurchaseDate = x.CreatedUtc, ProductName = x.Product!.Name })
            .ToListAsync();

        return products.Select(x => new UserPurchase(PurchasedDateUtc: x.PurchaseDate, ProductName: x.ProductName))
            .ToImmutableArray();
    }
}
