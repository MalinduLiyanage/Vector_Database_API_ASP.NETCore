using System.Net;
using Microsoft.Extensions.Options;
using Milvus.Client;
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

        public async Task<BaseResponse> ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (client != null)
                {
                    await DisconnectAsync();
                }

                client = new MilvusClient(settings.Host, settings.Port, ssl: false);

                var collections = await client.ListCollectionsAsync(cancellationToken: cancellationToken);

                logger.LogInformation("Successfully connected to Milvus at {Host}:{Port}", settings.Host, settings.Port);

                return new BaseResponse(HttpStatusCode.OK, collections, "Connected successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to connect to Milvus at {Host}:{Port}", settings.Host, settings.Port);
                return new BaseResponse(HttpStatusCode.InternalServerError, "No Data", $"Connection failed: {ex.Message}");
            }
        }

        public async Task<BaseResponse> DisconnectAsync()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
                logger.LogInformation("Disconnected from Milvus");

                return new BaseResponse(HttpStatusCode.OK, "No Data", "Disconnected successfully.");
            }

            return new BaseResponse(HttpStatusCode.BadRequest, "No Data", "No active connection to disconnect.");
        }

        public async Task<BaseResponse> IsConnectedAsync()
        {
            if (client == null)
            {
                return new BaseResponse(HttpStatusCode.OK, "No Data", "Client is not connected.");
            }

            try
            {
                await client.ListCollectionsAsync();
                return new BaseResponse(HttpStatusCode.OK, "No Data", "Client is connected.");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Client connection check failed.");
                return new BaseResponse(HttpStatusCode.OK, "No Data", "Client is not connected.");
            }
        }

    }
}
