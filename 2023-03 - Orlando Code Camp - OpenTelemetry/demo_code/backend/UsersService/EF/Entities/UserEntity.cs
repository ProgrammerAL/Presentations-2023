namespace ProgrammerAl.Presentations.OTel.UsersService.EF.Entities;

public class UserEntity
{
    public required Guid EntityId { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedUtc { get; set; }
    public required bool Enabled { get; set; }
}
