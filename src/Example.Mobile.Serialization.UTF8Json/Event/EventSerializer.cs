using Example.Mobile.Infrastructure.Serialization;
using Utf8Json;

namespace Example.Mobile.Serialization.UTF8Json.Event
{
    internal sealed class EventSerializer : IEventSerializer
    {
        public string Serialize<T>(T data)
        {
            return JsonSerializer.ToJsonString(data);
        }
    }
}
