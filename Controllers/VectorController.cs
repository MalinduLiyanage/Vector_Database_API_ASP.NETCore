using System.Net;
using Microsoft.AspNetCore.Mvc;
using Milvus_Vector_Database_API.DTOs.Requests;
using Milvus_Vector_Database_API.DTOs.Responses;
using Milvus_Vector_Database_API.Services.MilvusService;

namespace Milvus_Vector_Database_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VectorController : ControllerBase
    {
        private readonly IMilvusService milvusService;

        public VectorController(IMilvusService milvusService)
        {
            this.milvusService = milvusService;
        }

        [HttpPost("annsearch")]
        public async Task<BaseResponse> ANNSearch(ANNSearchRequest request)
        {
            return await milvusService.ANNSearchAsync(request);
        }
    }
}
