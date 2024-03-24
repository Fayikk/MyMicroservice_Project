using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FilterService.Models;

namespace FilterService.Services;


public class FilterGameService : IFilterGameService
{
    private readonly ElasticsearchClient _elasticSearch;
    public string indexName;
    public FilterGameService(ElasticsearchClient elasticSearch,IConfiguration configuration)
    {
        _elasticSearch = elasticSearch;
        indexName = configuration.GetValue<string>("indexName");
    }

    public async Task<List<GameFilterItem>> SearchAsync(GameFilterItem gameFilterItem)
    {
        List<Action<QueryDescriptor<GameFilterItem>>> listQuery = new();
        if (gameFilterItem is null)
        {
            listQuery.Add(q=>q.MatchAll());
            return await CalculateResultSet(listQuery);
        }
        if (!string.IsNullOrEmpty(gameFilterItem.Price.ToString()) && gameFilterItem.Price != 0)
        {
            listQuery.Add((q) => q.Range(m=>m.NumberRange(f=>f.Field(a=>a.Price).Gte(Convert.ToDouble(gameFilterItem.Price)))));
        }
          if (!string.IsNullOrEmpty(gameFilterItem.Price.ToString()) && gameFilterItem.Price != 0)
        {
            listQuery.Add((q) => q.Range(m=>m.NumberRange(f=>f.Field(a=>a.Price).Lte(Convert.ToDouble(gameFilterItem.Price)))));
        }
        if (!string.IsNullOrEmpty(gameFilterItem.MinimumSystemRequirement))
        {
            string searchValue = "*"+gameFilterItem.MinimumSystemRequirement+"*";
            listQuery.Add((q) => q.Wildcard(m=>m.Field(f=>f.MinimumSystemRequirement).Value(searchValue)));
        }
        if (!string.IsNullOrEmpty(gameFilterItem.RecommendedSystemRequirement))
        {
            string searchValue = "*"+gameFilterItem.RecommendedSystemRequirement+"*";
            listQuery.Add((q) => q.Wildcard(m=>m.Field(f=>f.RecommendedSystemRequirement).Value(searchValue)));
        }
        if (!listQuery.Any())
        {
            listQuery.Add(q=>q.MatchAll());
        }

        return await CalculateResultSet(listQuery);
    }

    private async Task<List<GameFilterItem>> CalculateResultSet(List<Action<QueryDescriptor<GameFilterItem>>> listQuery)
    {
        var result = await _elasticSearch.SearchAsync<GameFilterItem>(x=>x.Index(indexName).Query(a=>a.Bool(b=>b.Must(listQuery.ToArray()))));
        return result.Documents.ToList();
    }
}