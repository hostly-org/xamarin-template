namespace Example.Mobile.Infrastructure.Serialization
{
    public interface ICommandSerializer
    {
        string Serialize<T>(T data);
    }
}
