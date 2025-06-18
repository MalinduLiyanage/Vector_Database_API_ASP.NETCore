namespace Milvus_Vector_Database_API.DTOs.Requests
{
    public class HybridSearchRequest
    {
        public required List<List<float>> QueryVectors { get; set; }
        public required List<float> Weights { get; set; }
        public required string CollectionName { get; set; }
        public required string VectorField { get; set; }
        public required int PerQueryLimit { get; set; }
        public required int CombinedLimit { get; set; }
        public required string? Filter { get; set; }
        public required List<string>? OutputFields { get; set; }
        public required string MetricType { get; set; }
        public required string ConsistencyLevel { get; set; }
    }
}
