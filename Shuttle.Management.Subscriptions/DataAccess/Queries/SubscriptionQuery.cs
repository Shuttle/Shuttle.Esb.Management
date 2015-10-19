using System.Data;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Subscriptions
{
	public class SubscriptionQuery : ISubscriptionQuery
	{
		private readonly IDatabaseGateway _databaseGateway;
		private readonly ISubscriptionQueryFactory _queryFactory;

		public SubscriptionQuery(IDatabaseGateway databaseGateway, ISubscriptionQueryFactory queryFactory)
		{
			Guard.AgainstNull(databaseGateway, "databaseGateway");
			Guard.AgainstNull(queryFactory, "queryFactory");

			_databaseGateway = databaseGateway;
			_queryFactory = queryFactory;
		}

		public DataTable All()
		{
			return _databaseGateway.GetDataTableFor(_queryFactory.All());
		}

		public DataTable AllUris()
		{
			return _databaseGateway.GetDataTableFor(_queryFactory.AllInboxWorkQueueUris());
		}

		public DataTable MessageTypes(string inboxWorkQueueUri)
		{
			return _databaseGateway.GetDataTableFor(_queryFactory.MessageTypes(inboxWorkQueueUri));
		}

		public bool HasSubscriptionStructures()
		{
			return _databaseGateway.GetScalarUsing<int>(_queryFactory.HasSubscriptionStructures()) == 1;
		}

		public void Remove(string inboxWorkQueueUri, string messageType)
		{
			_databaseGateway.ExecuteUsing(_queryFactory.Remove(inboxWorkQueueUri, messageType));
		}

		public bool Contains(string inboxWorkQueueUri, string messageType)
		{
			return _databaseGateway.GetScalarUsing<int>(_queryFactory.Contains(inboxWorkQueueUri, messageType)) == 1;
		}

		public void Add(string inboxWorkQueueUri, string messageType)
		{
			_databaseGateway.ExecuteUsing(_queryFactory.Add(inboxWorkQueueUri, messageType));
		}
	}
}