using BasketService.Model;
using BasketService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> AddBasketItem(BasketModel model)
    {
        var response = await _basketRepository.AddBasket(model);
        return Ok(response);
    }

    [HttpGet("BasketItems")]

    public async Task<ActionResult> GetListItems()
    {
        var response = await _basketRepository.GetBasketItems();
        return Ok(response);
    }

    [HttpGet("BasketItem/{index}")]
    [Authorize]

    public async Task<ActionResult> GetBasketItem([FromRoute]long index)
    {
        var response = await _basketRepository.GetBasketItem(index);
        return Ok(response);
    }

    [HttpDelete("{index}")]
    [Authorize]

    public async Task<ActionResult> RemoveBasketItem([FromRoute]long index)
    {
        var response = await _basketRepository.RemoveBasketItem(index);
        return Ok(response);
    }
}