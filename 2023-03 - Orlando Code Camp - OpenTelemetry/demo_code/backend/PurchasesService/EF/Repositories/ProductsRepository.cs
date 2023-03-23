using Microsoft.EntityFrameworkCore;

using ProgrammerAl.Presentations.OTel.PurchasesService.EF.Entities;

namespace ProgrammerAl.Presentations.OTel.PurchasesService.EF.Repositories;

public interface IProductsRepository
{
    ValueTask CreateProduct(string name);
}

public class ProductsRepository : IProductsRepository
{
    private readonly IDbContextFactory<PurchasesServiceDbContext> _contextFactory;

    public ProductsRepository(IDbContextFactory<PurchasesServiceDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask CreateProduct(string name)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        using (var activity = ActivitySources.PurchasesServiceSource.StartActivity("CreateProduct"))
        {
            _ = await context.Products.AddAsync(new ProductEntity
            {
                CreatedUtc = DateTime.UtcNow,
                Enabled = true,
                Name = name
            });

            _ = await context.SaveChangesAsync();
        }
    }
}
