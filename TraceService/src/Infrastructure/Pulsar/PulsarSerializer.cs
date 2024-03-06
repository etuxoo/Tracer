using Newtonsoft.Json;
using TraceService.Application.Interfaces.Pulsar;

namespace TraceService.Infrastructure.Pulsar
{
    public class PulsarSerializer : ISerializer
    {
        public string Serialize<T>(T value) where T : class
        {
            return JsonConvert.SerializeObject(value);
        }

        public T? Deserialize<T>(string value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public byte[] SerializeBytes<T>(T value) where T : class
        {
            return System.Text.Encoding.UTF8.GetBytes(this.Serialize(value));
        }

        public T? DeserializeBytes<T>(byte[] value) where T : class
        {
            return this.Deserialize<T>(System.Text.Encoding.UTF8.GetString(value));
        }
    }
}
