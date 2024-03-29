using DiscountService.Models;
using DiscountService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountService.Controllers;


[ApiController]
[Route("[controller]")]
public class DiscountController : ControllerBase
{

    private readonly IDiscountRepository _repository;
    public DiscountController(IDiscountRepository repository)
    {
        _repository = repository;
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateDiscount(DiscountModel model)
    {
        var response = await _repository.CreateDiscount(model);
        return Ok(response);
    }
}