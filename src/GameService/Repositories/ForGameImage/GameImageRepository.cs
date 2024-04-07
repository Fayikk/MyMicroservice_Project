using GameService.Data;
using GameService.DTOs;
using GameService.Entities;
using GameService.Services;

namespace GameService.Repositories.ForGameImage
{
    public class GameImageRepository : IGameImageRepository
    {
        private readonly IFileService _fileService;
        private readonly GameDatabaseContext _context;
        public GameImageRepository(IFileService fileService, GameDatabaseContext context)
        {
            _context = context;
            _fileService = fileService; 
        }

        public async Task<bool> CreateGameImage(GameImageDTO imageDTO)
        {
            string filePath = await _fileService.UploadImage(imageDTO.file);

            GameImage game = new()
            {
                GameId = imageDTO.GameId,
                ImageUrl = filePath,
            };

            await _context.GameImages.AddAsync(game);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}