using Microsoft.AspNetCore.Mvc;

using ProgrammerAl.Presentations.OTel.PurchasesService.EF.Repositories;

namespace ProgrammerAl.Presentations.OTel.PurchasesService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsRepository _productsRepository;

    public ProductsController(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreatePurchaseAsync([FromBody] CreateProductRequest? request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        await _productsRepository.CreateProduct(request.Name);
        return Ok();
    }

    public record CreateProductRequest(string Name);
}