using System.Threading.Tasks;

namespace NServiceBus.HostBuilder
{
    public interface IProvideEndpointInstance
    {
        Task<IEndpointInstance> EndpointInstance { get; }
    }

    public class EndpointInstanceProvider : IProvideEndpointInstance
    {
        private readonly TaskCompletionSource<IEndpointInstance> _endpointTask = new TaskCompletionSource<IEndpointInstance>();

        public Task<IEndpointInstance> EndpointInstance => _endpointTask.Task;

        public void SetEndpointInstance(IEndpointInstance endpoint)
        {
            _endpointTask.SetResult(endpoint);
        }
    }
}