using System;
using System.Collections.Generic;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public interface ISubscriptionManagementView
	{
		void PopulateSubscriberUris(IEnumerable<string> uris);
		void ClearSubscriptions();
		string InboxWorkQueueUriValue { get; }
		void AddSubscription(string messageType);
		List<string> SelectedMessageTypes { get; }
		IEnumerable<string> SelectedSubscriptions { get; }
		void AddSubscription(string inboxWorkQueueUri, string messageType);
		void CheckAllSubscriptions();
		void InvertSubscriptionChecks();
		string DataStoreValue { get; }
		void PopulateDataStores(IEnumerable<DataStore> list);
		void PopulateEventTypes(IEnumerable<Type> list);
		void CheckAllEventMessageTypes();
		void InvertEventMessageTypeChecks();
		void GetAssemblyFileName();
	}
}