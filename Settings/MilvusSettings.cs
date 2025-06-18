namespace Milvus_Vector_Database_API.Settings
{
    public class MilvusSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Database { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required int ConnectTimeout { get; set; }
        public required string ServerVersion { get; set; }
    }
}
