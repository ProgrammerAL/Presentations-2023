using Microsoft.EntityFrameworkCore;

using OpenTelemetry.Trace;

using ProgrammerAl.Presentations.OTel.UsersService.EF.Entities;

namespace ProgrammerAl.Presentations.OTel.UsersService.EF.Repositories;

public interface IUsersRepository
{
    ValueTask<Guid> CreateUserAsync(string name);
    ValueTask DisableUserAsync(Guid id);
    ValueTask<bool> GetDoesUserExistAsync(Guid id);
}

public class UsersRepository : IUsersRepository
{
    private readonly Tracer _tracer;
    private readonly IDbContextFactory<UsersServiceCosmosContext> _contextFactory;

    public UsersRepository(IDbContextFactory<UsersServiceCosmosContext> contextFactory, Tracer tracer)
    {
        _contextFactory = contextFactory;
        _tracer = tracer;
    }

    public async ValueTask<Guid> CreateUserAsync(string name)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        using (var span = _tracer.StartActiveSpan("create-user"))
        {
            var newUser = new UserEntity
            {
                EntityId = Guid.NewGuid(),
                Name = name,
                CreatedUtc = DateTime.UtcNow,
                Enabled = true
            };

            _ = span.SetAttribute("user-id", newUser.EntityId.ToString());
            _ = await context.Users.AddAsync(newUser);
            _ = await context.SaveChangesAsync();

            return newUser.EntityId;
        }
    }

    public async ValueTask DisableUserAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        using (var span = _tracer.StartActiveSpan("disable-user"))
        {
            _ = span.SetAttribute("user-id", id.ToString());
            var user = await context.Users.AsTracking().SingleAsync(x => x.EntityId == id);
            if (user.Enabled)
            {
                user.Enabled = false;
                _ = await context.SaveChangesAsync();
            }
        };
    }

    public async ValueTask<bool> GetDoesUserExistAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        using (var span = _tracer.StartActiveSpan("disable-user"))
        {
            _ = span.SetAttribute("user-id", id.ToString());
            var user = await context.Users.FirstOrDefaultAsync(x => x.EntityId == id);
            return user is object && user.Enabled;
        };
    }
}
