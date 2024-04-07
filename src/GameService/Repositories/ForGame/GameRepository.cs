using System.Security.Claims;
using AutoMapper;
using Contracts;
using GameService.Base;
using GameService.Data;
using GameService.DTOs;
using GameService.Entities;
using GameService.Repositories;
using GameService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace GameService.GameRepository;

public class GameRepository : IGameRepository
{
    private GameDatabaseContext _context;
    private IMapper _mapper;
    private IFileService _fileService;
    private BaseResponseModel _responseModel;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHttpContextAccessor _httpContextAccesor;
    private string UserId;
    public GameRepository(GameDatabaseContext context,IHttpContextAccessor httpContextAccessor,IMapper mapper,IFileService fileService,BaseResponseModel responseModel,IPublishEndpoint publishEndpoint)
    {
        _responseModel = responseModel;
        _context = context;
        _fileService = fileService;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _httpContextAccesor = httpContextAccessor;
         UserId = _httpContextAccesor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<BaseResponseModel> CreateGame(GameDTO game)
    {
       if (game.File.Length > 0)
       {
            string videoUrl = await _fileService.UploadVideo(game.File);
            var objDTO = _mapper.Map<Game>(game);
            objDTO.VideoUrl = videoUrl;
            objDTO.UserId = UserId;
            await _context.Games.AddAsync(objDTO);
                await _publishEndpoint.Publish(_mapper.Map<GameCreated>(objDTO));

            if (await _context.SaveChangesAsync() > 0)
            {
                
                _responseModel.IsSuccess = true;
                _responseModel.Message = "Created Game Successfully";
                _responseModel.Data = objDTO;
                return _responseModel;   
            }
       }
    
                _responseModel.IsSuccess = false;
                return _responseModel;
    }

    public async Task<BaseResponseModel> GetAllGames()
    {
       List<Game> games = await _context.Games.Include(x=>x.Category).Include(x=>x.GameImages).ToListAsync();
       if (games is not null)
       {
            _responseModel.Data = games;
            _responseModel.IsSuccess = true;
            return _responseModel;
       }
       
            _responseModel.IsSuccess = false;
            return _responseModel;
    }

    public async Task<BaseResponseModel> GetGamesByCategory(Guid categoryId)
    {
       List<Game> games = await _context.Games.Include(x=>x.GameImages).Where(x=>x.CategoryId == categoryId).ToListAsync();
       if (games is not null)
       {
            _responseModel.Data = games;
            _responseModel.IsSuccess = true;
            return _responseModel;
       }
       
            _responseModel.IsSuccess = false;
            return _responseModel;
    }

    public async Task<BaseResponseModel> RemoveGame(Guid gameId)
    {
        Game game = await _context.Games.FindAsync(gameId);
        if (game is not null)
        {
            _context.Games.Remove(game);
            await _publishEndpoint.Publish<GameDeleted>(new {Id = gameId.ToString()});
            if (await _context.SaveChangesAsync() > 0)
            {
                _responseModel.IsSuccess = true;
                _responseModel.Data = game;
                return _responseModel;
            }

        }
        _responseModel.IsSuccess = false;
            return _responseModel;
    }

    public async Task<BaseResponseModel> UpdateGame(UpdateGameDTO game, Guid gameId)
    {
        Game gameObj = await _context.Games.FindAsync(gameId);
        if (gameObj is not null)
        {
            gameObj.Price = game.Price;
            gameObj.RecommendedSystemRequirement = game.RecommendedSystemRequirement;
            gameObj.MinimumSystemRequirement = game.MinimumSystemRequirement;
            gameObj.GameAuthor = game.GameAuthor;
            gameObj.GameName = game.GameName;
            gameObj.GameDescription = game.GameDescription;
            await _publishEndpoint.Publish(_mapper.Map<GameUpdated>(gameObj));
            if (await _context.SaveChangesAsync() > 0)
            {
                  _responseModel.IsSuccess = true;
                _responseModel.Data = gameObj;
                return _responseModel;
            }
        }

        _responseModel.IsSuccess = false;
        return _responseModel;
    }
}