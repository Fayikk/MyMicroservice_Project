using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.DTOs;
using GameService.Repositories.ForGameImage;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameImageController : ControllerBase
    {
        private readonly IGameImageRepository _gameImage;
        public GameImageController(IGameImageRepository gameImage)
        {
            _gameImage = gameImage; 
        }

        [HttpPost]
        public async Task<ActionResult> CreateImage([FromForm]GameImageDTO model)
        {
            var result = await _gameImage.CreateGameImage(model);
            return Ok(result);
        }
    }
}