#pragma warning disable IDE0058 // Expression value is never used

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Diagnostics;
using ProgrammerAl.Presentations.OTel.UsersService.EF.Entities;

namespace ProgrammerAl.Presentations.OTel.UsersService.EF;
public class UsersServiceCosmosContext : DbContext
{
    public static readonly ImmutableArray<Microsoft.Extensions.Logging.EventId> LoggingEventIds = new[]
    {
        CosmosEventId.ExecutedReadItem,
        CosmosEventId.ExecutedReadNext,
        CosmosEventId.ExecutedCreateItem,
        CosmosEventId.ExecutedReplaceItem,
        CosmosEventId.ExecutedDeleteItem
    }.ToImmutableArray();

    [NotNull]
    public DbSet<UserEntity>? Users { get; private set; }


    public UsersServiceCosmosContext(DbContextOptions<UsersServiceCosmosContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .ToContainer(nameof(Users))
            .HasPartitionKey(x => x.EntityId)
            .HasNoDiscriminator()
            .HasKey(x => x.EntityId);

        base.OnModelCreating(modelBuilder);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
