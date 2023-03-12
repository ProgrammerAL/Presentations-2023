using Microsoft.AspNetCore.Mvc;

using ProgrammerAl.Presentations.OTel.PurchasesService.EF.Repositories;

namespace ProgrammerAl.Presentations.OTel.PurchasesService.Controllers;

[ApiController]
[Route("[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly IPurchasesRepository _purchasesRepository;

    public PurchasesController(IPurchasesRepository purchasesRepository)
    {
        _purchasesRepository = purchasesRepository;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreatePurchaseAsync([FromBody] CreatePurchaseRequest? request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        await _purchasesRepository.CreatePurchaseAsync(request.ProductId, request.UserId);
        return Ok();
    }

    [HttpPost("get-user-purchases/{userId}")]
    public async Task<ActionResult> GEtPurchaseAsync([FromRoute] string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest();
        }

        var result = await _purchasesRepository.GetUserPurchasesAsync(userId);

        var responsePurchases = result.Select(x => new UserPurchasesResponse.UserPurchase(x.PurchasedDateUtc, x.ProductName)).ToImmutableArray();
        var response = new UserPurchasesResponse(responsePurchases);
        return Ok(response);
    }

    public record CreatePurchaseRequest(int ProductId, string? UserId);
    public record UserPurchasesResponse(ImmutableArray<UserPurchasesResponse.UserPurchase> Purchases)
    { 
        public record UserPurchase(DateTime PurchasedDateUtc, string ProductName);
    }
}