using AutoMapper;
using GameService.Data;
using GameService.DTOs;
using GameService.Entities;
using Grpc.Core;

namespace GameService.Services
{
    public class GrpcMyGameService : GrpcMyGame.GrpcMyGameBase    
    {
        private readonly GameDatabaseContext _context;
        private readonly IMapper _mapper;
        public GrpcMyGameService(GameDatabaseContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<GrpcMyGameResponse> GetMyGame(GetMyGameRequest request, ServerCallContext context)
        {
            Console.WriteLine("trigger -----> Grpc protobuf for GetMyGame");

            MyGameDTO myGameDTO = new(){
                UserId = Guid.Parse(request.UserId),
                GameId = Guid.Parse(request.GameId) 
            };
            var objDTO = _mapper.Map<MyGame>(myGameDTO);
            await _context.MyGames.AddAsync(objDTO);
            if (await _context.SaveChangesAsync() > 0)
            {
                Console.WriteLine("inner savechanges");
               var response = new GrpcMyGameResponse
               {
                        MyGame = new GrpcMyGameModel {
                                UserId = request.UserId,
                                GameId = request.GameId
                        }
               };
               return response;

            }
         return null;

        }


    }
}