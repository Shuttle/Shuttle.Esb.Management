using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Shell
{
    public static class DataStoreColumns
    {
        public static readonly MappedColumn<string> Name = new MappedColumn<string>("Name", DbType.AnsiString, 64).AsIdentifier();
        public static readonly MappedColumn<string> ConnectionString = new MappedColumn<string>("ConnectionString", DbType.AnsiString, 1024);
        public static readonly MappedColumn<string> ProviderName = new MappedColumn<string>("ProviderName", DbType.AnsiString, 512);
    }
}