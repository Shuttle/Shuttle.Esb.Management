using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Shell
{
    public class DataStoreMapper : IDataRowMapper<DataStore>
    {
        public MappedRow<DataStore> Map(DataRow row)
        {
            return new MappedRow<DataStore>(row, new DataStore
                                                             {
                                                                 Name = DataStoreColumns.Name.MapFrom(row),
                                                                 ConnectionString = DataStoreColumns.ConnectionString.MapFrom(row),
                                                                 ProviderName = DataStoreColumns.ProviderName.MapFrom(row)
                                                             });
        }
    }
}