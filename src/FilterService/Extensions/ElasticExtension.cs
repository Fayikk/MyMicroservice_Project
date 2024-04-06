using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace FilterService.Extensions;


public static class ElasticExtension
{
     // "Url":"http://localhost:9200",
    // "Username":"elastic",
    // "Password":"Admin123*"
    public static void AddElastic(this IServiceCollection services,IConfiguration configuration)
    {
        var userName = configuration.GetSection("Elastic")["Username"];
        var password = configuration.GetSection("Elastic")["Password"];
        var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]))
            .Authentication(new BasicAuthentication("elastic"!,"Admin123*"!));
        
        var client = new ElasticsearchClient(settings);
        services.AddSingleton(client);
    }
}