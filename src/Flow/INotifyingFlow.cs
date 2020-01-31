using System.Threading.Tasks;

namespace Flow
{
    public interface INotifyingFlow<T, TFilteringError> : IFlow<T, TFilteringError>, IEventSource<T, TFilteringError>
    {
        
    }
}