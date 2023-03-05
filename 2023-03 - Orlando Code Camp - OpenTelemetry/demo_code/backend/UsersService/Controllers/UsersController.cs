using Microsoft.AspNetCore.Mvc;

using ProgrammerAl.Presentations.OTel.UsersService.EF.Repositories;

namespace ProgrammerAl.Presentations.OTel.UsersService.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpPost("create/{name}")]
    public async Task<ActionResult<Guid>> CreateUserAsync([FromRoute] string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest();
        }

        return await _usersRepository.CreateUserAsync(name);
    }

    [HttpGet("disable/{id}")]
    public async Task<ActionResult> DisableUserAsync([FromRoute] string? id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }

        await _usersRepository.DisableUserAsync(guidId);
        return NoContent();
    }

    [HttpGet("exists/{id}")]
    public async Task<ActionResult<bool>> GetDoesUserExistAsync([FromRoute] string? id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            return BadRequest();
        }

        return await _usersRepository.GetDoesUserExistAsync(guidId);
    }
}