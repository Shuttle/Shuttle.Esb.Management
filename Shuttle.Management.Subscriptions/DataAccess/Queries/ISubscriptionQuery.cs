using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
    public interface ISubscriptionQuery
    {
        DataTable All(DataSource source);
		DataTable AllUris(DataSource source);
		DataTable MessageTypes(DataSource source, string inboxWorkQueueUri);
		bool HasSubscriptionStructures(DataSource source);
	    void Remove(DataSource source, string inboxWorkQueueUri, string messageType);
	    bool Contains(DataSource source, string inboxWorkQueueUri, string messageType);
	    void Add(DataSource source, string inboxWorkQueueUri, string messageType);
    }
}