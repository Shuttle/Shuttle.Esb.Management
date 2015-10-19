using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public class SubscriptionManagementPresenter : ManagementModulePresenter, ISubscriptionManagementPresenter
	{
		private readonly IDatabaseContextFactory _databaseContextFactory;
		private readonly IDatabaseGateway _databaseGateway;
		private readonly IReflectionService _reflectionService;

		private readonly ISubscriptionManagementView _subscriptionManagementView;
		private readonly ISubscriptionQuery _subscriptionQuery;

		public SubscriptionManagementPresenter(IDatabaseGateway databaseGateway,
			IDatabaseContextFactory databaseContextFactory,
			ISubscriptionQuery subscriptionQuery, IReflectionService reflectionService)
		{
			_subscriptionManagementView = new SubscriptionManagementView(this);

			Guard.AgainstNull(databaseGateway, "databaseGateway");
			Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
			Guard.AgainstNull(subscriptionQuery, "subscriptionQuery");
			Guard.AgainstNull(reflectionService, "reflectionService");

			_databaseGateway = databaseGateway;
			_databaseContextFactory = databaseContextFactory;
			_subscriptionQuery = subscriptionQuery;
			_reflectionService = reflectionService;
		}

		public void CheckAllSubscriptions()
		{
			_subscriptionManagementView.CheckAllSubscriptions();
		}

		public void InvertSubscriptionChecks()
		{
			_subscriptionManagementView.InvertSubscriptionChecks();
		}

		public void RefreshSubscriptions()
		{
			var dataStoreName = _subscriptionManagementView.DataStoreValue;

			if (string.IsNullOrEmpty(dataStoreName))
			{
				Log.Warning(ManagementResources.NoDataStoreSelected);

				return;
			}

			var inboxWorkQueueUriValue = _subscriptionManagementView.InboxWorkQueueUriValue;

			if (string.IsNullOrEmpty(inboxWorkQueueUriValue))
			{
				Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

				return;
			}

			QueueTask("RefreshSubscriptions",
				() =>
				{
					_subscriptionManagementView.ClearSubscriptions();

					using (_databaseContextFactory.Create(dataStoreName))
						foreach (DataRow row in
							_subscriptionQuery.MessageTypes(inboxWorkQueueUriValue).Rows)
						{
							_subscriptionManagementView.AddSubscription(SubscriptionColumns.MessageType.MapFrom(row));
						}
				});
		}

		public override void OnViewReady()
		{
			base.OnViewReady();

			RefreshDataStores();
			RefreshSubscribers();
		}

		public void RefreshDataStores()
		{
			QueueTask("RefreshDataStores",
				() => _subscriptionManagementView.PopulateDataStores(ManagementConfiguration.DataStoreRepository().All()));
		}

		public void AddSubscriptions()
		{
			var dataStoreName = _subscriptionManagementView.DataStoreValue;

			if (string.IsNullOrEmpty(dataStoreName))
			{
				Log.Warning(ManagementResources.NoDataStoreSelected);

				return;
			}

			var inboxWorkQueueUri = _subscriptionManagementView.InboxWorkQueueUriValue;

			if (string.IsNullOrEmpty(inboxWorkQueueUri))
			{
				Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

				return;
			}

			QueueTask("AddSubscriptions",
				() =>
				{
					using (_databaseContextFactory.Create(dataStoreName))
					{
						foreach (var messageType in _subscriptionManagementView.SelectedMessageTypes)
						{
							if (_subscriptionQuery.Contains(inboxWorkQueueUri, messageType))
							{
								continue;
							}

							_subscriptionQuery.Add(inboxWorkQueueUri, messageType);
						}
					}
				});

			RefreshSubscriptions();
		}

		public void CheckAllEventMessageTypes()
		{
			_subscriptionManagementView.CheckAllEventMessageTypes();
		}

		public void InvertEventMessageTypeChecks()
		{
			_subscriptionManagementView.InvertEventMessageTypeChecks();
		}

		public void GetAssemblyEventMessageTypes()
		{
			_subscriptionManagementView.GetAssemblyFileName();
		}

		public void ShowAssemblyTypes(string fileName)
		{
			QueueTask("ShowAssemblyTypes",
				() =>
					_subscriptionManagementView.PopulateEventTypes(
						_reflectionService.GetTypes(_reflectionService.GetAssembly(fileName))));
		}

		public void RefreshSubscribers()
		{
			var dataStoreName = _subscriptionManagementView.DataStoreValue;

			QueueTask("RefreshSubscribers",
				() =>
				{
					var uris = new List<string>();

					if (!string.IsNullOrEmpty(dataStoreName))
					{
						using (_databaseContextFactory.Create(dataStoreName))
						{
							uris.AddRange(from DataRow row in _subscriptionQuery.AllUris().Rows
								select SubscriptionColumns.InboxWorkQueueUri.MapFrom(row));
						}
					}

					uris.AddRange(from Queue queue in ManagementConfiguration.QueueRepository().All()
						where !uris.Contains(queue.Uri)
						select queue.Uri);

					uris.Sort();

					_subscriptionManagementView.PopulateSubscriberUris(uris);
				});

			RefreshSubscriptions();
		}

		public void RemoveSubscriptions()
		{
			var dataStoreName = _subscriptionManagementView.DataStoreValue;

			if (string.IsNullOrEmpty(dataStoreName))
			{
				Log.Warning(ManagementResources.NoDataStoreSelected);

				return;
			}

			var inboxWorkQueueUri = _subscriptionManagementView.InboxWorkQueueUriValue;

			if (string.IsNullOrEmpty(inboxWorkQueueUri))
			{
				Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

				return;
			}

			if (
				MessageBox.Show(string.Format(ManagementResources.ConfirmRemoval, SubscriptionResources.TextSubscriptions),
					ManagementResources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
				DialogResult.Yes)
			{
				return;
			}

			QueueTask("RemoveSubscriptions",
				() =>
				{
					using (_databaseContextFactory.Create(dataStoreName))
					{
						foreach (var messageType in _subscriptionManagementView.SelectedSubscriptions)
						{
							_subscriptionQuery.Remove(inboxWorkQueueUri, messageType);
						}
					}
				}
				);

			RefreshSubscriptions();
		}

		public void DataStoreChanged()
		{
			using (_databaseContextFactory.Create(_subscriptionManagementView.DataStoreValue))
			{
				if (!_subscriptionQuery.HasSubscriptionStructures())
				{
					Log.Error(
						string.Format(
							"Data store '{0}' does not contain the required structures for subscription handling.  Please execute the relevant creation script against the data store.",
							_subscriptionManagementView.DataStoreValue));
				}
			}

			RefreshSubscribers();
		}

		public override string Text
		{
			get { return SubscriptionResources.TextSubscriptions; }
		}

		public override Image Image
		{
			get { return SubscriptionResources.ImageSubscriptions; }
		}

		public override UserControl ViewUserControl
		{
			get
			{
				var control = (UserControl) _subscriptionManagementView;

				control.Enabled = ManagementConfiguration.HasDataStoreRepository;

				return control;
			}
		}
	}
}