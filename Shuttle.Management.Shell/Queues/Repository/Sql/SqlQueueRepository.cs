using System.Collections.Generic;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class SqlQueueRepository : IQueueRepository
	{
		private readonly string _connectionStringName =
			ConfigurationItem<string>.ReadSetting("SqlQueueRepositoryConnectionStringName",
				"SqlQueueRepository").GetValue();

		private readonly IDatabaseContextFactory _databaseContextFactory;
		private readonly IDatabaseGateway _databaseGateway;
		private readonly IDataRepository<Queue> _dataRepository;

		public SqlQueueRepository()
		{
			_databaseContextFactory = DatabaseContextFactory.Default();
			_databaseGateway = new DatabaseGateway();
			_dataRepository = new DataRepository<Queue>(_databaseGateway, new QueueMapper());
		}

		public IEnumerable<Queue> All()
		{
			using (_databaseContextFactory.Create(_connectionStringName))
			{
				return _dataRepository.FetchAllUsing(QueueTableAccess.All());
			}
		}

		public void Save(Queue queue)
		{
			if (Contains(queue.Uri))
			{
				return;
			}

			using (_databaseContextFactory.Create(_connectionStringName))
			{
				_databaseGateway.ExecuteUsing(QueueTableAccess.Add(queue));
			}
		}

		public void Remove(string uri)
		{
			using (_databaseContextFactory.Create(_connectionStringName))
			{
				_databaseGateway.ExecuteUsing(QueueTableAccess.Remove(uri));
			}
		}

		public bool Contains(string uri)
		{
			using (_databaseContextFactory.Create(_connectionStringName))
			{
				return _databaseGateway.GetScalarUsing<int>(QueueTableAccess.Contains(uri)) == 1;
			}
		}

		public Queue Get(string uri)
		{
			using (_databaseContextFactory.Create(_connectionStringName))
			{
				return _dataRepository.FetchItemUsing(QueueTableAccess.Get(uri));
			}
		}
	}
}