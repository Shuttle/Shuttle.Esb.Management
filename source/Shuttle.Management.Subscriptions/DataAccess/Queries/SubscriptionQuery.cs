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

		public DataTable All(DataSource source)
		{
			return _databaseGateway.GetDataTableFor(source, _queryFactory.All());
		}

		public DataTable AllUris(DataSource source)
		{
			return _databaseGateway.GetDataTableFor(source, _queryFactory.AllInboxWorkQueueUris());
		}

		public DataTable MessageTypes(DataSource source, string inboxWorkQueueUri)
		{
			return _databaseGateway.GetDataTableFor(source, _queryFactory.MessageTypes(inboxWorkQueueUri));
		}

		public bool HasSubscriptionStructures(DataSource source)
		{
			return _databaseGateway.GetScalarUsing<int>(source, _queryFactory.HasSubscriptionStructures()) == 1;
		}

		public void Remove(DataSource source, string inboxWorkQueueUri, string messageType)
		{
			_databaseGateway.ExecuteUsing(source, _queryFactory.Remove(inboxWorkQueueUri, messageType));
		}

		public bool Contains(DataSource source, string inboxWorkQueueUri, string messageType)
		{
			return _databaseGateway.GetScalarUsing<int>(source, _queryFactory.Contains(inboxWorkQueueUri, messageType)) == 1;
		}

		public void Add(DataSource source, string inboxWorkQueueUri, string messageType)
		{
			_databaseGateway.ExecuteUsing(source, _queryFactory.Add(inboxWorkQueueUri, messageType));
		}
	}
}