namespace Milvus_Vector_Database_API.DTOs.Requests
{
    public class ANNSearchRequest
    {
        public required List<float> QueryVector { get; set; }
        public required int TopK { get; set; }
        public required string CollectionName { get; set; }
        public required string FieldName { get; set; }
    }
}
