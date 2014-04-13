using System.Collections.Generic;
using Shuttle.Core.Data;

namespace Shuttle.Management.Shell
{
    public class SqlDataStoreRepository : IDataStoreRepository
    {
		private static readonly DataSource DataSource = new DataSource("SqlDataStoreRepository", new SqlDbDataParameterFactory());

        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private readonly IDatabaseGateway databaseGateway;
        private readonly IDataRepository<DataStore> dataRepository;

        public SqlDataStoreRepository()
        {
            databaseConnectionFactory = DatabaseConnectionFactory.Default();
            databaseGateway = DatabaseGateway.Default();
            dataRepository = new DataRepository<DataStore>(databaseGateway, new DataStoreMapper());
        }

        public IEnumerable<DataStore> All()
        {
            using (databaseConnectionFactory.Create(DataSource))
            {
                return dataRepository.FetchAllUsing(DataSource, DataStoreTableAccess.All());
            }
        }

        public void Save(DataStore dataStore)
        {
            if (Contains(dataStore.Name))
            {
                Remove(dataStore.Name);
            }

            using (databaseConnectionFactory.Create(DataSource))
            {
                databaseGateway.ExecuteUsing(DataSource, DataStoreTableAccess.Add(dataStore));
            }
        }

        public void Remove(string name)
        {
            using (databaseConnectionFactory.Create(DataSource))
            {
                databaseGateway.ExecuteUsing(DataSource, DataStoreTableAccess.Remove(name));
            }
        }

        public bool Contains(string name)
        {
            using (databaseConnectionFactory.Create(DataSource))
            {
                return databaseGateway.GetScalarUsing<int>(DataSource, DataStoreTableAccess.Contains(name)) == 1;
            }
        }

        public DataStore Get(string name)
        {
            using (databaseConnectionFactory.Create(DataSource))
            {
                return dataRepository.FetchItemUsing(DataSource, DataStoreTableAccess.Get(name));
            }
        }
    }
}