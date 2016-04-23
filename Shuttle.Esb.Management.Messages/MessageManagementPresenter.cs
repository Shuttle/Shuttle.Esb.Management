using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;
using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Messages
{
    public class MessageManagementPresenter : ManagementModulePresenter, IMessageManagementPresenter, IDisposable
    {
        private readonly ILog _log;

        private readonly IQueueManager _queueManager;
        private readonly ISerializer _serializer;
        private readonly IMessageManagementView _view;

        public MessageManagementPresenter()
        {
            _log = Log.For(this);

            _view = new MessageManagementView(this);

            _queueManager = new QueueManager();

            _queueManager.ScanForQueueFactories();

            var messageConfiguration = new MessageConfiguration();

            _serializer = messageConfiguration.GetSerializer();
        }

        public override string Text
        {
            get { return MessageResources.TextMessages; }
        }

        public override Image Image
        {
            get { return MessageResources.ImageMessage; }
        }

        public override UserControl ViewUserControl
        {
            get { return (UserControl)_view; }
        }

        protected bool HasCheckedMessages
        {
            get { return _view.GetCheckedMessages().Count > 0; }
        }

        public void Dispose()
        {
            if (_queueManager != null)
            {
                _queueManager.AttemptDispose();
            }
        }

        public void Acknowledge()
        {
            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            if (MessageBox.Show(MessageResources.ConfirmMessageDeletion,
                MessageResources.Confirmation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClearMessageView();

            QueueTask("Acknowledge",
                () =>
                {
                    foreach (var receivedMessageItem in _view.GetCheckedMessages())
                    {
                        receivedMessageItem.Queue.Acknowledge(receivedMessageItem.ReceivedMessage.AcknowledgementToken);
                    }

                    RemoveCheckedMessages();
                });
        }

        public void Move()
        {
            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            ClearMessageView();

            var destinationQueueUriValue = _view.DestinationQueueUriValue;

            QueueTask("Move",
                () =>
                {
                    foreach (var receivedMessageItem in _view.GetCheckedMessages())
                    {
                        var destination = _queueManager.GetQueue(destinationQueueUriValue);

                        destination.Enqueue(receivedMessageItem.TransportMessage,
                            receivedMessageItem.ReceivedMessage.Stream);
                        receivedMessageItem.Queue.Acknowledge(receivedMessageItem.ReceivedMessage.AcknowledgementToken);

                        _log.Information(string.Format(MessageResources.EnqueuedMessage,
                            receivedMessageItem.TransportMessage.MessageId, destinationQueueUriValue));
                        _log.Information(string.Format(MessageResources.RemovedMessage,
                            receivedMessageItem.TransportMessage.MessageId,
                            receivedMessageItem.Queue));
                    }

                    RemoveCheckedMessages();
                });
        }

        public void MoveAll()
        {
            var sourceQueueUriValue = _view.SourceQueueUriValue;
            var destinationQueueUriValue = _view.DestinationQueueUriValue;

            if (MessageBox.Show(
                string.Format(MessageResources.ConfirmMoveAll, sourceQueueUriValue, destinationQueueUriValue),
                MessageResources.Confirmation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClearMessageView();

            ReleaseMessage();

            QueueTask("MoveAll",
                () =>
                {
                    var destination = _queueManager.GetQueue(destinationQueueUriValue);
                    var source = _queueManager.GetQueue(sourceQueueUriValue);
                    var totalMessagesMoved = 0;
                    ReceivedMessage receivedMessage;

                    do
                    {
                        receivedMessage = source.GetMessage();

                        if (receivedMessage == null)
                        {
                            continue;
                        }

                        var transportMessage =
                            (TransportMessage)
                                _serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

                        destination.Enqueue(transportMessage, receivedMessage.Stream);
                        source.Acknowledge(receivedMessage.AcknowledgementToken);

                        totalMessagesMoved++;
                    } while (receivedMessage != null);

                    RemoveAllMessages();

                    _log.Information(string.Format(MessageResources.MoveAllComplete, totalMessagesMoved,
                        sourceQueueUriValue,
                        destinationQueueUriValue));
                });
        }

        private void RemoveAllMessages()
        {
            _view.RemoveAllItems();
        }

        public void Copy()
        {
            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            var destinationQueueUriValue = _view.DestinationQueueUriValue;

            QueueTask("Copy",
                () =>
                {
                    foreach (var receivedMessageItem in _view.GetCheckedMessages())
                    {
                        _queueManager.GetQueue(destinationQueueUriValue)
                            .Enqueue(receivedMessageItem.TransportMessage,
                                receivedMessageItem.ReceivedMessage.Stream);

                        _log.Information(string.Format(MessageResources.EnqueuedMessage,
                            receivedMessageItem.TransportMessage.MessageId, destinationQueueUriValue));
                    }

                    RemoveCheckedMessages();
                });
        }

        public void CopyAll()
        {
            var sourceQueueUriValue = _view.SourceQueueUriValue;
            var destinationQueueUriValue = _view.DestinationQueueUriValue;

            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            if (MessageBox.Show(
                string.Format(MessageResources.ConfirmCopyAll, sourceQueueUriValue, destinationQueueUriValue),
                MessageResources.Confirmation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClearMessageView();

            QueueTask("CopyAll",
                () =>
                {
                    var destination = _queueManager.GetQueue(destinationQueueUriValue);
                    var source = _queueManager.GetQueue(sourceQueueUriValue);
                    var totalMessagesCopied = 0;
                    var startMessageId = Guid.Empty;

                    ReceivedMessage receivedMessage;
                    TransportMessage transportMessage = null;

                    do
                    {
                        receivedMessage = source.GetMessage();

                        if (receivedMessage == null)
                        {
                            continue;
                        }

                        transportMessage =
                            (TransportMessage)
                                _serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

                        if (!transportMessage.MessageId.Equals(startMessageId))
                        {
                            if (startMessageId.Equals(Guid.Empty))
                            {
                                startMessageId = transportMessage.MessageId;
                            }

                            destination.Enqueue(transportMessage, receivedMessage.Stream);
                            source.Release(receivedMessage.AcknowledgementToken);

                            totalMessagesCopied++;
                        }
                    } while (receivedMessage != null && !startMessageId.Equals(transportMessage.MessageId));

                    RemoveAllMessages();

                    _log.Information(string.Format(MessageResources.CopyAllComplete, totalMessagesCopied,
                        sourceQueueUriValue,
                        destinationQueueUriValue));
                });
        }

        public void ReturnToSourceQueue()
        {
            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            ClearMessageView();

            QueueTask("ReturnToSourceQueue",
                () =>
                {
                    foreach (var receivedMessageItem in _view.GetCheckedMessages())
                    {
                        var destination =
                            _queueManager.GetQueue(receivedMessageItem.TransportMessage.RecipientInboxWorkQueueUri);

                        receivedMessageItem.TransportMessage.StopIgnoring();
                        receivedMessageItem.TransportMessage.FailureMessages.Clear();

                        destination.Enqueue(receivedMessageItem.TransportMessage,
                            _serializer.Serialize(receivedMessageItem.TransportMessage));
                        receivedMessageItem.Queue.Acknowledge(receivedMessageItem.ReceivedMessage.AcknowledgementToken);

                        _log.Information(string.Format(MessageResources.RemovedMessage,
                            receivedMessageItem.TransportMessage.MessageId,
                            receivedMessageItem.Queue));
                        _log.Information(string.Format(MessageResources.EnqueuedMessage,
                            receivedMessageItem.TransportMessage.MessageId,
                            receivedMessageItem.TransportMessage.RecipientInboxWorkQueueUri));
                    }

                    RemoveCheckedMessages();
                });
        }

        public void ReturnAllToSourceQueue()
        {
            var sourceQueueUriValue = _view.SourceQueueUriValue;

            if (MessageBox.Show(string.Format(MessageResources.ConfirmReturnAllToSourceQueue, sourceQueueUriValue),
                MessageResources.Confirmation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ReleaseMessage();

            ClearMessageView();

            QueueTask("MoveAll",
                () =>
                {
                    var source = _queueManager.GetQueue(sourceQueueUriValue);
                    var totalMessagesReturned = 0;
                    ReceivedMessage receivedMessage;

                    do
                    {
                        receivedMessage = source.GetMessage();

                        if (receivedMessage == null)
                        {
                            continue;
                        }

                        var transportMessage =
                            (TransportMessage)
                                _serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

                        var destination = _queueManager.GetQueue(transportMessage.RecipientInboxWorkQueueUri);

                        destination.Enqueue(transportMessage, receivedMessage.Stream);
                        source.Acknowledge(receivedMessage.AcknowledgementToken);

                        totalMessagesReturned++;
                    } while (receivedMessage != null);

                    RemoveAllMessages();

                    _log.Information(string.Format(MessageResources.ReturnAllToSourceQueueComplete,
                        totalMessagesReturned, sourceQueueUriValue));
                });
        }

        public void GetMessage()
        {
            var sourceQueueUri = _view.SourceQueueUriValue;

            ClearMessageView();

            QueueTask("GetMessage",
                () =>
                {
                    var queue = _queueManager.GetQueue(sourceQueueUri);

                    if (queue == null)
                    {
                        _log.Error(MessageResources.SourceQueueUriReader);

                        return;
                    }

                    var receivedMessage = queue.GetMessage();

                    if (receivedMessage == null)
                    {
                        return;
                    }

                    var transportMessage =
                        (TransportMessage)_serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

                    _view.AddReceivedMessageItem(new ReceivedMessageItem(queue, receivedMessage, transportMessage));
                });
        }

        public override void OnViewReady()
        {
            RefreshQueues();
        }

        public void RefreshQueues()
        {
            QueueTask("RefreshQueues", () => _view.PopulateQueues(ManagementConfiguration.QueueRepository().All()));
        }

        public void MessageSelected(ReceivedMessageItem receivedMessageItem)
        {
            object message = null;
            var transportMessage = receivedMessageItem.TransportMessage;

            try
            {
                var type = Type.GetType(transportMessage.AssemblyQualifiedName, true, true);

                var canDisplayMessage = true;

                if (transportMessage.CompressionEnabled())
                {
                    _log.Warning(string.Format(MessageResources.MessageCompressed, transportMessage.MessageId));
                    canDisplayMessage = false;
                }

                if (transportMessage.EncryptionEnabled())
                {
                    _log.Warning(string.Format(MessageResources.MessageEncrypted, transportMessage.MessageId));
                    canDisplayMessage = false;
                }

                if (canDisplayMessage)
                {
                    using (var stream = new MemoryStream(transportMessage.Message))
                    {
                        message = _serializer.Deserialize(type, stream);
                    }

                    if (!type.AssemblyQualifiedName.Equals(transportMessage.AssemblyQualifiedName))
                    {
                        _log.Warning(string.Format(MessageResources.MessageTypeMismatch,
                            transportMessage.AssemblyQualifiedName,
                            type.AssemblyQualifiedName));
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Warning(string.Format(MessageResources.CannotObtainMessageType,
                    transportMessage.AssemblyQualifiedName));
                _log.Error(ex.Message);
            }

            _view.ShowMessage(transportMessage, message);
        }

        public void MessageSelectionCleared()
        {
            ClearMessageView();
        }

        public void StopIgnoring()
        {
            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            ClearMessageView();

            QueueTask("StopIgnoring",
                () =>
                {
                    foreach (var receivedMessageItem in _view.GetCheckedMessages())
                    {
                        receivedMessageItem.TransportMessage.StopIgnoring();

                        receivedMessageItem.Queue.Enqueue(receivedMessageItem.TransportMessage,
                            _serializer.Serialize(receivedMessageItem.TransportMessage));
                        receivedMessageItem.Queue.Acknowledge(receivedMessageItem.ReceivedMessage.AcknowledgementToken);
                    }

                    RemoveCheckedMessages();
                });
        }

        private void ClearMessageView()
        {
            _view.ClearMessageView();
        }

        public void ReleaseMessage()
        {
            if (!HasCheckedMessages)
            {
                _log.Information(MessageResources.NoMessageChecked);

                return;
            }

            ClearMessageView();

            QueueTask("ReleaseMessage",
                () =>
                {
                    foreach (var receivedMessageItem in _view.GetCheckedMessages())
                    {
                        receivedMessageItem.Queue.Release(receivedMessageItem.ReceivedMessage.AcknowledgementToken);
                    }

                    RemoveCheckedMessages();
                });
        }

        private void RemoveCheckedMessages()
        {
            _view.RemoveCheckedItems();
        }

        public class ReceivedMessageItem
        {
            public ReceivedMessageItem(IQueue queue, ReceivedMessage receivedMessage, TransportMessage transportMessage)
            {
                Guard.AgainstNull(queue, "queue");

                Queue = queue;
                ReceivedMessage = receivedMessage;
                TransportMessage = transportMessage;
            }

            public IQueue Queue { get; private set; }
            public ReceivedMessage ReceivedMessage { get; private set; }
            public TransportMessage TransportMessage { get; private set; }
        }
    }
}