using Example.Mobile.Infrastructure.Serialization;
using Utf8Json;

namespace Example.Mobile.Serialization.UTF8Json.Command
{
    internal sealed class CommandSerializer : ICommandSerializer
    {
        public string Serialize<T>(T data)
        {
            return JsonSerializer.ToJsonString(data);
        }
    }
}
