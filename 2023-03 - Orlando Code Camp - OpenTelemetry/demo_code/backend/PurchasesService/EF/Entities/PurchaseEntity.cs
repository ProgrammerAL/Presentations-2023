using Microsoft.EntityFrameworkCore;

namespace ProgrammerAl.Presentations.OTel.PurchasesService.EF.Entities;

public class PurchaseEntity
{
    [Key]
    public int Id { get; set; }
    public required DateTime CreatedUtc { get; set; }
    public required int ProductId { get; set; }
    public string? UserId { get; set; }

    public ProductEntity? Product { get; set; }
}
