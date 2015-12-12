using System.Collections.Generic;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    public interface IMessageManagementView
    {
        string SourceQueueUriValue { get; set; }
        string DestinationQueueUriValue { get; set; }
        List<MessageManagementPresenter.ReceivedMessageItem> GetCheckedMessages();
        void PopulateQueues(IEnumerable<Queue> queues);
        void ShowMessage(TransportMessage transportMessage, object message);
        void ClearMessageView();
        void AddReceivedMessageItem(MessageManagementPresenter.ReceivedMessageItem receivedMessageItem);
        void RemoveCheckedItems();
        void RemoveAllItems();
    }
}