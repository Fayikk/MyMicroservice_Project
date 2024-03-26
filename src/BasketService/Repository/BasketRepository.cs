using System.Security.Claims;
using BasketService.Base;
using BasketService.Model;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BasketService.Repository;



public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _db;
    public string connectionString;
    private readonly IHttpContextAccessor _contextAccessor;
    public string UserId;
    public BasketRepository(IConfiguration configuration,IHttpContextAccessor contextAccessor)
    {
        connectionString = configuration.GetValue<string>("RedisDatabase");
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
        _db = redis.GetDatabase();
        _contextAccessor = contextAccessor;
        UserId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<ResponseModel<bool>> AddBasket(BasketModel model)
    {
        ResponseModel<bool> responseModel = new ResponseModel<bool>();
        if (model is not null)
        {
            var convertType = JsonConvert.SerializeObject(model);
             await _db.ListRightPushAsync(UserId,convertType);
            responseModel.isSuccess = true;
            return responseModel;
        }
        return responseModel;
    }

    public async Task<ResponseModel<BasketModel>> GetBasketItem(long index)
    {
        ResponseModel<BasketModel> responseModel = new ResponseModel<BasketModel>();
        var response = await _db.ListGetByIndexAsync(UserId,index);
        var objResult = JsonConvert.DeserializeObject<BasketModel>(response);
        responseModel.isSuccess = true;
        responseModel.Data = objResult;
        return responseModel;
    }

    public async Task<ResponseModel<List<BasketModel>>> GetBasketItems()
    {
             ResponseModel<List<BasketModel>> responseModel = new ResponseModel<List<BasketModel>>();

        if (!string.IsNullOrEmpty(UserId))
        {
       var response = await _db.ListRangeAsync(UserId);
       List<BasketModel> basketModel = new List<BasketModel>();
       foreach (var item in response)
       {
            var objResult = JsonConvert.DeserializeObject<BasketModel>(item);
            basketModel.Add(objResult);
       }
       if (basketModel.Count > 0)
       {
        
            responseModel.Data = basketModel;
            responseModel.isSuccess = true;
            return responseModel;
       }
        responseModel.isSuccess = false;
            return responseModel;
        }
        responseModel.isSuccess = false;
        responseModel.Message = "Please before login your account";
        return responseModel;
    }

    public async Task<ResponseModel<bool>> RemoveBasketItem(long index)
    {
        ResponseModel<bool> responseModel = new ResponseModel<bool>();
        var willDeletedItem = await _db.ListGetByIndexAsync(UserId,index);
        await _db.ListRemoveAsync(UserId,willDeletedItem);
        responseModel.isSuccess = true;
        return responseModel;

    }
}