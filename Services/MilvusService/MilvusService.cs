using System.Net;
using Microsoft.Extensions.Options;
using Milvus.Client;
using Milvus_Vector_Database_API.DTOs.Responses;
using StackExchange.Redis;

namespace Milvus_Vector_Database_API.Services.MilvusService
{
    public class MilvusService : IMilvusService
    {
        private readonly IConfiguration settings;
        private readonly ILogger<MilvusService> logger;
        private MilvusClient? client;
        private readonly bool disposed = false;

        private readonly string host;
        private readonly int port;
        private readonly string database;
        private readonly string username;
        private readonly string password;
        private readonly int connectTimeout;
        private readonly string serverVersion;

        public MilvusService(IConfiguration configuration, ILogger<MilvusService> logger)
        {
            this.logger = logger;
            this.host = configuration["MilvusSettings:Host"] ?? "localhost";
            this.port = int.TryParse(configuration["MilvusSettings:Port"], out var port) ? port : 19530;
            this.database = configuration["MilvusSettings:Database"] ?? "default";
            this.username = configuration["MilvusSettings:Username"] ?? string.Empty;
            this.password = configuration["MilvusSettings:Password"] ?? string.Empty;
            this.connectTimeout = int.TryParse(configuration["MilvusSettings:ConnectTimeout"], out var timeout) ? timeout : 30000;
            this.serverVersion = configuration["MilvusSettings:ServerVersion"] ?? "2.3.0";
        }

        public async Task<BaseResponse> ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (client != null)
                {
                    await DisconnectAsync();
                }

                client = new MilvusClient(host, port, ssl: false);

                var collections = await client.ListCollectionsAsync(cancellationToken: cancellationToken);

                logger.LogInformation("Successfully connected to Milvus at {Host}:{Port}", host, port);

                return new BaseResponse(HttpStatusCode.OK, collections, "Connected successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to connect to Milvus at {Host}:{Port}", host, port);
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
            catch
            {
                return new BaseResponse(HttpStatusCode.OK, "No Data", "Client is not connected.");
            }
        }
    }
}
