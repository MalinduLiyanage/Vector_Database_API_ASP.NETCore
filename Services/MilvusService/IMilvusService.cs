using Milvus_Vector_Database_API.DTOs.Requests;
using Milvus_Vector_Database_API.DTOs.Responses;

namespace Milvus_Vector_Database_API.Services.MilvusService
{
    public interface IMilvusService
    {
        Task<BaseResponse> ANNSearchAsync(ANNSearchRequest request);
    }
}
