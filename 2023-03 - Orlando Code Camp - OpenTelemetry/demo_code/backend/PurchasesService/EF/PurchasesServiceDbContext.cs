#pragma warning disable IDE0058 // Expression value is never used

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProgrammerAl.Presentations.OTel.PurchasesService.EF.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ProgrammerAl.Presentations.OTel.PurchasesService.EF;

public class PurchasesServiceDbContext : DbContext
{
    public static readonly ImmutableArray<Microsoft.Extensions.Logging.EventId> LoggingEventIds = new[]
    {
        RelationalEventId.CommandExecuted,
    }.ToImmutableArray();

    [NotNull]
    public DbSet<PurchaseEntity>? Purchases { get; private set; }

    [NotNull]
    public DbSet<ProductEntity>? Products { get; private set; }

    public PurchasesServiceDbContext(DbContextOptions<PurchasesServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseEntity>()
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseEntity>();

        base.OnModelCreating(modelBuilder);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
