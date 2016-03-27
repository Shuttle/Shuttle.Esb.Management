using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Esb.Management.Subscriptions
{
    public static class SubscriptionColumns
    {
        public static readonly MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri", DbType.AnsiString, 130);
        public static readonly MappedColumn<string> MessageType = new MappedColumn<string>("MessageType", DbType.AnsiString, 250);
	}
}