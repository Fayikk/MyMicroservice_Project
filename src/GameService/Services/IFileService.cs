using GameService.DTOs;

namespace GameService.Services;


public interface IFileService
{
    Task<string> UploadVideo(IFormFile File);
    Task UploadImage();
}