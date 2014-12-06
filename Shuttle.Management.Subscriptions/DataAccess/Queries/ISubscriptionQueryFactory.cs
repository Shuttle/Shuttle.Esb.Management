using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
	public interface ISubscriptionQueryFactory
	{
		IQuery All();
		IQuery AllInboxWorkQueueUris();
		IQuery MessageTypes(string inboxWorkQueueUri);
		IQuery HasSubscriptionStructures();
		IQuery Add(string inboxWorkQueueUri, string messageType);
		IQuery Remove(string inboxWorkQueueUri, string messageType);
		IQuery Contains(string inboxWorkQueueUri, string messageType);
	}
}