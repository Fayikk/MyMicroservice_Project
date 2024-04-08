using GameService.DTOs;
using GameService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private IGameRepository _gameRepository;
    public GameController(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateGame([FromForm]GameDTO game)
    {
        var response = await _gameRepository.CreateGame(game);
        return Ok(response);
    }

    [HttpDelete("{gameId}")]
    public async Task<ActionResult> RemoveGame([FromRoute]Guid gameId)
    {
        var response = await _gameRepository.RemoveGame(gameId);
        return Ok(response);
    }


    [HttpGet]
    public async Task<ActionResult> GetAllGames()
    {
        var response = await _gameRepository.GetAllGames();
        return Ok(response);
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult> GetGamesByCategoryId([FromRoute]Guid categoryId)
    {
        var response = await _gameRepository.GetGamesByCategory(categoryId);
        return Ok(response);
    }

    [HttpPut("{gameId}")]
    public async Task<ActionResult> UpdateGame(UpdateGameDTO model,[FromRoute]Guid gameId)
    {
        var response = await _gameRepository.UpdateGame(model,gameId);
        return Ok(response);
    }

    [HttpGet("game/{gameId}")]
    public async Task<ActionResult> GetGameById([FromRoute] Guid gameId)
    {
        var response = await _gameRepository.GetGameById(gameId);
        return Ok(response);
    }

}