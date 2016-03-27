using System.Collections.Generic;
using Shuttle.Esb;
using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Messages
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