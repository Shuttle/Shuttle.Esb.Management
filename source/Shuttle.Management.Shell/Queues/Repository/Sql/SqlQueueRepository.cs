using System.Collections.Generic;
using Shuttle.Core.Data;

namespace Shuttle.Management.Shell
{
	public class SqlQueueRepository : IQueueRepository
	{
		private static readonly DataSource DataSource = new DataSource("SqlQueueRepository", new SqlDbDataParameterFactory());

		private readonly IDatabaseConnectionFactory databaseConnectionFactory;
		private readonly IDatabaseGateway databaseGateway;
		private readonly IDataRepository<Queue> dataRepository;

		public SqlQueueRepository()
		{
			databaseConnectionFactory = DatabaseConnectionFactory.Default();
			databaseGateway = DatabaseGateway.Default();
			dataRepository = new DataRepository<Queue>(databaseGateway, new QueueMapper());
		}

		public IEnumerable<Queue> All()
		{
			using (databaseConnectionFactory.Create(DataSource))
			{
				return dataRepository.FetchAllUsing(DataSource, QueueTableAccess.All());
			}
		}

		public void Save(Queue queue)
		{
			if (Contains(queue.Uri))
			{
				return;
			}

			using (databaseConnectionFactory.Create(DataSource))
			{
				databaseGateway.ExecuteUsing(DataSource, QueueTableAccess.Add(queue));
			}
		}

		public void Remove(string uri)
		{
			using (databaseConnectionFactory.Create(DataSource))
			{
				databaseGateway.ExecuteUsing(DataSource, QueueTableAccess.Remove(uri));
			}
		}

		public bool Contains(string uri)
		{
			using (databaseConnectionFactory.Create(DataSource))
			{
				return databaseGateway.GetScalarUsing<int>(DataSource, QueueTableAccess.Contains(uri)) == 1;
			}
		}

		public Queue Get(string uri)
		{
			using (databaseConnectionFactory.Create(DataSource))
			{
				return dataRepository.FetchItemUsing(DataSource, QueueTableAccess.Get(uri));
			}
		}
	}
}