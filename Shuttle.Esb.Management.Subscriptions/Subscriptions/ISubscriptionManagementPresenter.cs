using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Subscriptions
{
	public interface ISubscriptionManagementPresenter : IManagementModulePresenter
	{
		void CheckAllSubscriptions();
		void InvertSubscriptionChecks();
		void RefreshSubscriptions();
		void RefreshSubscribers();
		void RemoveSubscriptions();
		void DataStoreChanged();
		void RefreshDataStores();
		void AddSubscriptions();
		void CheckAllEventMessageTypes();
		void InvertEventMessageTypeChecks();
	    void GetAssemblyEventMessageTypes();
	    void ShowAssemblyTypes(string fileName);
	}
}