using AutoMapper;
using Contracts;
using Elastic.Clients.Elasticsearch;
using FilterService.Models;
using MassTransit;

namespace FilterService.Consumer;


public class GameCreatedFilterConsumer : IConsumer<GameCreated>
{
    private readonly IMapper _mapper;
    private readonly ElasticsearchClient _elasticClient;
    public string indexName;
    public GameCreatedFilterConsumer(IMapper mapper,ElasticsearchClient elasticClient,IConfiguration configuration)
    {
        _elasticClient = elasticClient;
        _mapper = mapper;
        indexName = configuration.GetValue<string>("indexName");
    }

    public async Task Consume(ConsumeContext<GameCreated> context)
    {
        Console.WriteLine("Consuming Filter Service For Created Game ----> " + context.Message.GameName);
        var objDTO = _mapper.Map<GameFilterItem>(context.Message);
        objDTO.GameId = context.Message.Id.ToString();

        var elasticSearch = await _elasticClient.IndexAsync(objDTO,x=>x.Index(indexName));
        if (!elasticSearch.IsValidResponse)
        {
            Console.WriteLine(elasticSearch);
            Console.WriteLine("Consuming process is not valid");
        }
    }
}