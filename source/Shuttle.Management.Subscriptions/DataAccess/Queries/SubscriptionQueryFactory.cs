using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
	public class SubscriptionQueryFactory : ISubscriptionQueryFactory
	{
		public IQuery All()
		{
			return RawQuery.Create(@"
select 
	[MessageType],
	[InboxWorkQueueUri]
from
	[SubscriberMessageType]
order by
	[InboxWorkQueueUri]
");
		}

		public IQuery AllInboxWorkQueueUris()
		{
			return RawQuery.Create("select distinct InboxWorkQueueUri from [SubscriberMessageType]");
		}

		public IQuery MessageTypes(string inboxWorkQueueUri)
		{
			return RawQuery.Create(@"
select 
	[MessageType]
from
	[SubscriberMessageType]
where
	[InboxWorkQueueUri] = @InboxWorkQueueUri
order by
	[MessageType]
")
			               .AddParameterValue(SubscriptionColumns.InboxWorkQueueUri, inboxWorkQueueUri);
		}

		public IQuery HasSubscriptionStructures()
		{
			return
				RawQuery.Create(
					"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'SubscriberMessageType') select 1 ELSE select 0");
		}

		public IQuery Add(string inboxWorkQueueUri, string messageType)
		{
			return RawQuery.Create(@"
insert into [SubscriberMessageType]
(
	MessageType,
	InboxWorkQueueUri
)
values
(
	@MessageType,
	@InboxWorkQueueUri
)
")
			               .AddParameterValue(SubscriptionColumns.MessageType, messageType)
			               .AddParameterValue(SubscriptionColumns.InboxWorkQueueUri, inboxWorkQueueUri);
		}

		public IQuery Remove(string inboxWorkQueueUri, string messageType)
		{
			return RawQuery.Create(@"
delete from  [SubscriberMessageType]
where
	MessageType = @MessageType
and
	InboxWorkQueueUri = @InboxWorkQueueUri
")
			               .AddParameterValue(SubscriptionColumns.MessageType, messageType)
			               .AddParameterValue(SubscriptionColumns.InboxWorkQueueUri, inboxWorkQueueUri);
		}

		public IQuery Contains(string inboxWorkQueueUri, string messageType)
		{
			return
				RawQuery.Create(
					"if exists(select null from [SubscriberMessageType] where MessageType = @MessageType and InboxWorkQueueUri = @InboxWorkQueueUri) select 1 else select 0")
				        .AddParameterValue(SubscriptionColumns.MessageType, messageType)
				        .AddParameterValue(SubscriptionColumns.InboxWorkQueueUri, inboxWorkQueueUri);
		}
	}
}