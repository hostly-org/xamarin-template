using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Example.Mobile.Hosting
{
    public class XamarinBaseLifetime :  IHostLifetime
    {
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
