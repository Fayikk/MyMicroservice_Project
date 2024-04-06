using System.Security.Claims;
using AutoMapper;
using BasketService.Base;
using BasketService.Model;
using Contracts;
using DiscountService;
using DiscountService.Services;
using MassTransit;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BasketService.Repository;



public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _db;
    public string connectionString;
    private readonly IHttpContextAccessor _contextAccessor;
    public string UserId;
    public IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;
    private readonly GrpcDiscountClient _discountClient;
    public BasketRepository(IConfiguration configuration,GrpcDiscountClient discountClient,IMapper mapper,IHttpContextAccessor contextAccessor,IPublishEndpoint publishEndpoint)
    {
        connectionString = configuration.GetValue<string>("RedisDatabase");
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
        _db = redis.GetDatabase();
        _contextAccessor = contextAccessor;
        UserId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _discountClient = discountClient;
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


    public async Task<ResponseModel<bool>> Checkout()
    {
        List<Checkout> checkouts = new List<Checkout>();
        ResponseModel<bool> responseModel = new ResponseModel<bool>();
         var response = await _db.ListRangeAsync(UserId);
       foreach (var item in response)
       {
           Checkout _checkout = new Checkout();
            var objResult = JsonConvert.DeserializeObject<BasketModel>(item);
            _checkout.GameName = objResult.GameName;
            _checkout.GameAuthor = objResult.GameAuthor;
            _checkout.GameId = objResult.GameId;
            _checkout.Price = objResult.Price;
            _checkout.GameDescription = objResult.GameDescription;
            _checkout.UserId = Guid.Parse(UserId);
            checkouts.Add(_checkout);
       }
        if (checkouts.Count > 0)
        {
            responseModel.isSuccess = true;
            foreach (var item in checkouts)
            {

                try
                {
                        await _publishEndpoint.Publish(_mapper.Map<CheckoutBasketModel>(item));

                }
                catch (System.Exception ex)
                {


                    throw ex;
                }

                
            }

            await _db.KeyDeleteAsync(UserId);


            return responseModel;
        }
            responseModel.isSuccess = false;
            return responseModel;
    }

    public async Task<ResponseModel<bool>> ImplementCoupon(long index,string couponCode)
    {
        ResponseModel<bool> responseModel = new ResponseModel<bool>();
        var discount = _discountClient.GetDiscount(couponCode);

        if (discount != null)
        {
               var response = await _db.ListGetByIndexAsync(UserId,index);
               var deserializeObj = JsonConvert.DeserializeObject<BasketModel>(response);
               deserializeObj.Price = deserializeObj.Price - (deserializeObj.Price*discount.DiscountAmount)/100;
                var SerializeObject = JsonConvert.SerializeObject(deserializeObj);
               await _db.ListSetByIndexAsync(UserId,index,SerializeObject);
               responseModel.isSuccess = true;
               return responseModel;
        }
         responseModel.isSuccess = false;
               return responseModel;
    }
}