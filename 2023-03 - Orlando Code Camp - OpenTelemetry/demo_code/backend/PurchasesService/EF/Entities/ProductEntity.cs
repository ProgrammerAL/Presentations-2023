namespace ProgrammerAl.Presentations.OTel.PurchasesService.EF.Entities;

public class ProductEntity
{
    [Key]
    public int Id { get; set; }
    public required DateTime CreatedUtc { get; set; }
    public required bool Enabled { get; set; }
    public required string Name { get; set; }
}
