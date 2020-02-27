using Xamarin.Essentials;

namespace Example.Mobile.Infrastructure
{
    internal class ConnectivityProvider : IConnectivityProvider
    {
        public NetworkAccess GetAccess()
        {
            return Connectivity.NetworkAccess;
        }
    }
}
