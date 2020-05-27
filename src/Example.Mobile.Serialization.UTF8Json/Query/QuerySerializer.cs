using Example.Mobile.Infrastructure.Serialization;
using Utf8Json;

namespace Example.Mobile.Serialization.UTF8Json.Query
{
    internal sealed class QuerySerializer : IQuerySerializer
    {
        public string Serialize<T>(T data)
        {
            return JsonSerializer.ToJsonString(data);
        }
    }
}
