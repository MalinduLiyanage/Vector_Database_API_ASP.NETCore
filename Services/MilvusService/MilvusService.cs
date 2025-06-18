using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Milvus.Client;
using Milvus_Vector_Database_API.DTOs.Requests;
using Milvus_Vector_Database_API.DTOs.Responses;
using Milvus_Vector_Database_API.Settings;
using StackExchange.Redis;

namespace Milvus_Vector_Database_API.Services.MilvusService
{
    public class MilvusService : IMilvusService
    {
        private readonly ILogger<MilvusService> logger;
        private readonly MilvusSettings settings;
        private MilvusClient? client;

        public MilvusService(IOptions<MilvusSettings> settings, ILogger<MilvusService> logger)
        {
            this.logger = logger;
            this.settings = settings.Value;
        }

        public async Task<BaseResponse> ANNSearchAsync(ANNSearchRequest request)
        {
            if (client == null)
            {
                client = new MilvusClient(host: settings.Host, database: settings.Database, username: settings.Username, password: settings.Password);
                await client.GetCollection(request.CollectionName).LoadAsync();
            }

            try
            {

                await client.GetCollection(request.CollectionName).LoadAsync();

                ReadOnlyMemory<float>[] vectors = new ReadOnlyMemory<float>[] { request.QueryVector.ToArray() };

                SearchResults result = await client.GetCollection(request.CollectionName).SearchAsync(
                    vectorFieldName: request.FieldName,
                    vectors: vectors,
                    metricType: SimilarityMetricType.L2,
                    limit: request.TopK,
                    cancellationToken: default
                );

                List<object> searchResults = new List<object>();

                IReadOnlyList<long>? ids = result.Ids.LongIds;
                IReadOnlyList<float> scores = result.Scores;

                for (int i = 0; i < ids?.Count; i++)
                {
                    searchResults.Add(new
                    {
                        Record_Id = ids[i],
                        Score = scores[i]
                    });
                }

                await client.GetCollection(request.CollectionName).ReleaseAsync();

                return new BaseResponse(HttpStatusCode.OK, searchResults, "ANN search completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ANN search failed for collection {CollectionName}", request.CollectionName);
                Task collection = client.GetCollection(request.CollectionName).ReleaseAsync();
                return new BaseResponse(HttpStatusCode.InternalServerError, "No Data", $"ANN search failed: {ex.Message}");
            }
        }

    }
}
