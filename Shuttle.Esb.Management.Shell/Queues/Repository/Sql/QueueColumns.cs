using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Esb.Management.Shell
{
    public static class QueueColumns
    {
		public static readonly MappedColumn<string> Uri = new MappedColumn<string>("Uri", DbType.AnsiString, 130);
    }
}
