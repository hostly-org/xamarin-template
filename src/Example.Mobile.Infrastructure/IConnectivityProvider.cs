using Xamarin.Essentials;

namespace Example.Mobile.Infrastructure
{
    public interface IConnectivityProvider
    {
        NetworkAccess GetAccess();
    }
}
