using Milvus_Vector_Database_API.DTOs.Responses;

namespace Milvus_Vector_Database_API.Services.MilvusService
{
    public interface IMilvusService
    {
        Task<BaseResponse> ConnectAsync(CancellationToken cancellationToken = default);
        Task<BaseResponse> DisconnectAsync();
        Task<BaseResponse> IsConnectedAsync();
    }
}
