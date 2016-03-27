using System.Data;

namespace Shuttle.Esb.Management.Subscriptions
{
	public interface ISubscriptionQuery
	{
		DataTable All();
		DataTable AllUris();
		DataTable MessageTypes(string inboxWorkQueueUri);
		bool HasSubscriptionStructures();
		void Remove(string inboxWorkQueueUri, string messageType);
		bool Contains(string inboxWorkQueueUri, string messageType);
		void Add(string inboxWorkQueueUri, string messageType);
	}
}