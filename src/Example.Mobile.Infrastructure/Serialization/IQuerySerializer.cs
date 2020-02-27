namespace Example.Mobile.Infrastructure.Serialization
{
    public interface IQuerySerializer
    {
        string Serialize<T>(T data);
    }
}
