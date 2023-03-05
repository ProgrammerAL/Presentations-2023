using Microsoft.EntityFrameworkCore;

using ProgrammerAl.Presentations.OTel.Shared.EF;
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
    private readonly EfQueryRunner<UsersServiceCosmosContext> _queryRunner;

    public UsersRepository(EfQueryRunner<UsersServiceCosmosContext> queryRunner)
    {
        _queryRunner = queryRunner;
    }

    public async ValueTask<Guid> CreateUserAsync(string name)
    {
        return await _queryRunner.RunAsync("create-user", async (context, span) =>
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
        });
    }

    public async ValueTask DisableUserAsync(Guid id)
    {
        await _queryRunner.RunAsync("disable-user", async (context, span) =>
        {
            _ = span.SetAttribute("user-id", id.ToString());
            var user = await context.Users.AsTracking().SingleAsync(x => x.EntityId == id);
            if (user.Enabled)
            {
                user.Enabled = false;
                _ = await context.SaveChangesAsync();
            }
        });
    }

    public async ValueTask<bool> GetDoesUserExistAsync(Guid id)
    {
        return await _queryRunner.RunAsync("does-user-exist", async (context, span) =>
        {
            _ = span.SetAttribute("user-id", id.ToString());
            var user = await context.Users.FirstOrDefaultAsync(x => x.EntityId == id);
            return user is object && user.Enabled;
        });
    }
}
