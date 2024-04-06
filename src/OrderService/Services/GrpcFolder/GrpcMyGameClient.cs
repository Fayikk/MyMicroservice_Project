using GameService;
using Grpc.Net.Client;

namespace OrderService.Services.GrpcFolder
{
    public class GrpcMyGameClient
    {
        private readonly IConfiguration _configuration;
        public GrpcMyGameClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SaveMyGame(string UserId,string GameId)
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcGame"]);
            var client = new GrpcMyGame.GrpcMyGameClient(channel);
            var request = new GetMyGameRequest{
                UserId = UserId,
                GameId = GameId
            };
            try
            {
                var response = client.GetMyGame(request);
                if (!string.IsNullOrEmpty(response.MyGame.GameId) && !string.IsNullOrEmpty(response.MyGame.GameId) )
                {
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                
                throw ex;
            }
        }
    }
}