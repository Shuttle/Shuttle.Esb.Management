using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Esb;
using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Messages
{
    public partial class MessageManagementView : UserControl, IMessageManagementView
    {
        private readonly IMessageManagementPresenter _presenter;

        public MessageManagementView(IMessageManagementPresenter presenter)
        {
            InitializeComponent();

            _presenter = presenter;

            MessageToolStrip.AddItem(MessageResources.TextAcknowledge,
                ManagementResources.ImageRemove,
                delegate { presenter.Acknowledge(); });
            MessageToolStrip.AddItem(MessageResources.TextRelease,
                MessageResources.ImageMessageRelease,
                delegate { presenter.ReleaseMessage(); });
            MessageToolStrip.AddItem(MessageResources.TextStopIgnoring,
                MessageResources.ImageStopIgnoring,
                delegate { presenter.StopIgnoring(); });
            MessageToolStrip.AddItem(MessageResources.TextMove,
                MessageResources.ImageMessageMove,
                delegate { presenter.Move(); });
            MessageToolStrip.AddItem(MessageResources.TextMoveAll,
                MessageResources.ImageMessageMove,
                delegate { presenter.MoveAll(); });
            MessageToolStrip.AddItem(MessageResources.TextCopy,
                MessageResources.ImageMessageCopy,
                delegate { presenter.Copy(); });
            MessageToolStrip.AddItem(MessageResources.TextCopyAll,
                MessageResources.ImageMessageCopy,
                delegate { presenter.CopyAll(); });
            MessageToolStrip.AddItem(MessageResources.TextReturnToSourceQueue,
                MessageResources.ImageArrowBack,
                delegate { presenter.ReturnToSourceQueue(); });
            MessageToolStrip.AddItem(MessageResources.TextReturnAllToSourceQueue,
                MessageResources.ImageArrowBack,
                delegate { presenter.ReturnAllToSourceQueue(); });
            MessageToolStrip.AddItem(MessageResources.TextGetMessage,
                MessageResources.ImageMessage,
                delegate { presenter.GetMessage(); });
            MessageToolStrip.AddItem(ManagementResources.TextCheckAll,
                ManagementResources.ImageCheck,
                delegate { CheckAll(); });
            MessageToolStrip.AddItem(ManagementResources.TextInvertChecks,
                ManagementResources.ImageCheck,
                delegate { InvertChecks(); });
            MessageToolStrip.AddItem(MessageResources.TextRefreshQueues,
                ManagementResources.ImageQueues,
                delegate { presenter.RefreshQueues(); });
        }

        public string SourceQueueUriValue
        {
            get { return SourceQueueUri.Text; }
            set { SourceQueueUri.Text = value; }
        }

        public string DestinationQueueUriValue
        {
            get { return DestinationQueueUri.Text; }
            set { DestinationQueueUri.Text = value; }
        }

        public List<MessageManagementPresenter.ReceivedMessageItem> GetCheckedMessages()
        {
            var result = new List<MessageManagementPresenter.ReceivedMessageItem>();

            this.Invoke(() =>
            {
                result.AddRange(from ListViewItem checkedItem in Messages.CheckedItems
                    select (MessageManagementPresenter.ReceivedMessageItem) checkedItem.Tag);
            });

            return result;
        }

        public void PopulateQueues(IEnumerable<Queue> queues)
        {
            this.Invoke(() =>
            {
                SourceQueueUri.Clear();
                DestinationQueueUri.Clear();

                foreach (var queue in queues)
                {
                    SourceQueueUri.AddQueue(queue.Uri);
                    DestinationQueueUri.AddQueue(queue.Uri);
                }
            });
        }

        public void ShowMessage(TransportMessage transportMessage, object message)
        {
            this.Invoke(() => MessageView.Show(transportMessage, message));
        }

        public void ClearMessageView()
        {
            this.Invoke(() => MessageView.Clear());
        }

        public void AddReceivedMessageItem(MessageManagementPresenter.ReceivedMessageItem receivedMessageItem)
        {
            this.Invoke(() =>
            {
                var item = Messages.Items.Add(receivedMessageItem.TransportMessage.MessageId.ToString(), "Message");

                item.SubItems.Add(receivedMessageItem.TransportMessage.AssemblyQualifiedName);

                item.Tag = receivedMessageItem;

                item.Selected = true;
            });
        }

        public void RemoveCheckedItems()
        {
            this.Invoke(() =>
            {
                var itemsToRemove = Messages.CheckedItems.Cast<ListViewItem>().ToList();

                foreach (var item in itemsToRemove)
                {
                    Messages.Items.Remove(item);
                }
            });
        }

        public void RemoveAllItems()
        {
            this.Invoke(() => { Messages.Items.Clear(); });
        }

        private void InvertChecks()
        {
            foreach (ListViewItem item in Messages.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void CheckAll()
        {
            foreach (ListViewItem item in Messages.Items)
            {
                item.Checked = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            _presenter.OnViewReady();
        }

        private void SourceQueueUri_QueueSelected(object sender, QueueSelectedEventArgs e)
        {
            this.Invoke(() => _presenter.GetMessage());
        }

        private void Messages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Messages.SelectedItems.Count == 0)
            {
                _presenter.MessageSelectionCleared();

                return;
            }

            _presenter.MessageSelected(Messages.SelectedItems[0].Tag as MessageManagementPresenter.ReceivedMessageItem);
        }
    }
}