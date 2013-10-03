using System;
using Shuttle.Core.Data;

namespace Shuttle.Management.Shell
{
    public static class DataStoreTableAccess
    {
        public const string TableName = "DataStore";

        public static IQuery Get(string name)
        {
            return RawQuery.CreateFrom("select Name, ConnectionString, ProviderName from [{0}] where Name = @Name", TableName)
                .AddParameterValue(DataStoreColumns.Name, name);
        }

        public static IQuery Add(DataStore dataStore)
        {
            return RawQuery.CreateFrom("insert into [{0}] (Name, ConnectionString, ProviderName) values (@Name, @ConnectionString, @ProviderName)", TableName)
                .AddParameterValue(DataStoreColumns.Name, dataStore.Name)
                .AddParameterValue(DataStoreColumns.ConnectionString, dataStore.ConnectionString)
                .AddParameterValue(DataStoreColumns.ProviderName, dataStore.ProviderName);
        }

        public static IQuery Remove(string name)
        {
            return RawQuery.CreateFrom("delete from [{0}] where Name = @Name", TableName)
                .AddParameterValue(DataStoreColumns.Name, name);
        }

        public static IQuery All()
        {
            return RawQuery.CreateFrom("select Name, ConnectionString, ProviderName from [{0}] order by Name", TableName);
        }

        public static IQuery Contains(string name)
        {
            return RawQuery.CreateFrom("if exists (select null from [{0}] where Name = @Name) select 1 else select 0", TableName)
                .AddParameterValue(DataStoreColumns.Name, name);
        }
    }
}