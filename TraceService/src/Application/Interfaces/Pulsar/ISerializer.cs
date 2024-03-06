namespace TraceService.Application.Interfaces.Pulsar
{
    public interface ISerializer
    {
        public string Serialize<T>(T value) where T : class;
        public T? Deserialize<T>(string value) where T : class;
        public byte[] SerializeBytes<T>(T value) where T : class;
        public T? DeserializeBytes<T>(byte[] value) where T : class;
    }
}
