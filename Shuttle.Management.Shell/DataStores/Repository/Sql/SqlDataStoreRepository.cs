using System.Collections.Generic;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
    public class SqlDataStoreRepository : IDataStoreRepository
    {
		private readonly string _connectionStringName = ConfigurationItem<string>.ReadSetting("SqlDataStoreRepositoryConnectionStringName","SqlDataStoreRepository").GetValue();

        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly IDataRepository<DataStore> _dataRepository;

        public SqlDataStoreRepository()
        {
            _databaseContextFactory = DatabaseContextFactory.Default();
            _databaseGateway = new DatabaseGateway();
            _dataRepository = new DataRepository<DataStore>(_databaseGateway, new DataStoreMapper());
        }

        public IEnumerable<DataStore> All()
        {
            using (_databaseContextFactory.Create(_connectionStringName))
            {
                return _dataRepository.FetchAllUsing(DataStoreTableAccess.All());
            }
        }

        public void Save(DataStore dataStore)
        {
            if (Contains(dataStore.Name))
            {
                Remove(dataStore.Name);
            }

            using (_databaseContextFactory.Create(_connectionStringName))
            {
                _databaseGateway.ExecuteUsing(DataStoreTableAccess.Add(dataStore));
            }
        }

        public void Remove(string name)
        {
            using (_databaseContextFactory.Create(_connectionStringName))
            {
                _databaseGateway.ExecuteUsing(DataStoreTableAccess.Remove(name));
            }
        }

        public bool Contains(string name)
        {
            using (_databaseContextFactory.Create(_connectionStringName))
            {
                return _databaseGateway.GetScalarUsing<int>(DataStoreTableAccess.Contains(name)) == 1;
            }
        }

        public DataStore Get(string name)
        {
            using (_databaseContextFactory.Create(_connectionStringName))
            {
                return _dataRepository.FetchItemUsing(DataStoreTableAccess.Get(name));
            }
        }
    }
}