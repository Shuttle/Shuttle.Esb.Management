using Shuttle.Core.Data;

namespace Shuttle.Management.Shell
{
	public static class QueueTableAccess
	{
		public const string TableName = "Queue";

		public static IQuery All()
		{
			return RawQuery.CreateFrom("select Uri from [{0}] order by Uri", TableName);
		}

		public static IQuery Get(string uri)
		{
			return RawQuery.CreateFrom("select Uri from [{0}] where Uri = @Uri", TableName)
				.AddParameterValue(QueueColumns.Uri, uri);
		}

		public static IQuery Add(Queue queue)
		{
			return RawQuery.CreateFrom("insert into [{0}] (Uri) values (@Uri)", TableName)
				.AddParameterValue(QueueColumns.Uri, queue.Uri);
		}

		public static IQuery Remove(string uri)
		{
			return RawQuery.CreateFrom("delete from [{0}] where Uri = @Uri", TableName)
				.AddParameterValue(QueueColumns.Uri, uri);
		}

	    public static IQuery Contains(string uri)
	    {
			return RawQuery.CreateFrom("if exists (select null from [{0}] where Uri = @Uri) select 1 else select 0", TableName)
				.AddParameterValue(QueueColumns.Uri, uri);
		}
	}
}