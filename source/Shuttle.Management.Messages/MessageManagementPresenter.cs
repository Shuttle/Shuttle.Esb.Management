using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
	public class MessageManagementPresenter : ManagementModulePresenter, IMessageManagementPresenter
	{
		private class SelectedMessage
		{
			public SelectedMessage(string sourceQueueUri, ReceivedMessage receivedMessage, TransportMessage transportMessage)
			{
				SourceQueueUri = sourceQueueUri;
				ReceivedMessage = receivedMessage;
				TransportMessage = transportMessage;
			}

			public string SourceQueueUri { get; private set; }
			public ReceivedMessage ReceivedMessage { get; private set; }
			public TransportMessage TransportMessage { get; private set; }
		}

		private readonly IMessageConfiguration _messageConfiguration;
		private readonly IQueueManager _queueManager;
		private readonly ISerializer _serializer;
		private readonly IMessageManagementView _view;

		private SelectedMessage _selectedMessage;
		private readonly ILog _log;

		public MessageManagementPresenter()
		{
			_log = Log.For(this);

			_view = new MessageManagementView(this);

			_queueManager = QueueManager.Default();

			_messageConfiguration = new MessageConfiguration();

			_serializer = _messageConfiguration.GetSerializer();
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

		public void Acknowledge()
		{
			if (!HasSelectedMessage)
			{
				_log.Error(MessageResources.NoMessageSelected);

				return;
			}

			if (MessageBox.Show(MessageResources.ConfirmMessageDeletion,
								MessageResources.Confirmation,
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			QueueTask("AcknowledgeMessage",
						  () =>
						  {
							  var queue = _queueManager.GetQueue(_selectedMessage.SourceQueueUri);

							  if (queue == null)
							  {
								  _log.Error(MessageResources.SourceQueueUriReader);

								  return;
							  }

							  queue.Acknowledge(_selectedMessage.ReceivedMessage.AcknowledgementToken);

							  _selectedMessage = null;
						  });

			GetMessage();
		}

		public void Move()
		{
			if (!HasSelectedMessage)
			{
				_log.Error(MessageResources.NoMessageSelected);

				return;
			}

			var destinationQueueUriValue = _view.DestinationQueueUriValue;

			QueueTask("Move",
					  () =>
					  {
						  var destination = _queueManager.GetQueue(destinationQueueUriValue);
						  var source = _queueManager.GetQueue(_selectedMessage.SourceQueueUri);

						  destination.Enqueue(_selectedMessage.TransportMessage.MessageId, _selectedMessage.ReceivedMessage.Stream);
						  source.Acknowledge(_selectedMessage.ReceivedMessage.AcknowledgementToken);

						  _log.Information(string.Format(MessageResources.EnqueuedMessage, _selectedMessage.TransportMessage.MessageId, destinationQueueUriValue));
						  _log.Information(string.Format(MessageResources.RemovedMessage, _selectedMessage.TransportMessage.MessageId, _selectedMessage.SourceQueueUri));

						  GetMessage();
					  });
		}

		public void MoveAll()
		{
			var sourceQueueUriValue = _view.SourceQueueUriValue;
			var destinationQueueUriValue = _view.DestinationQueueUriValue;

			if (MessageBox.Show(string.Format(MessageResources.ConfirmMoveAll, sourceQueueUriValue, destinationQueueUriValue),
								MessageResources.Confirmation,
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			QueueTask("MoveAll",
					  () =>
					  {
						  var destination = _queueManager.GetQueue(destinationQueueUriValue);
						  var source = _queueManager.GetQueue(sourceQueueUriValue);
						  var totalMessagesMoved = 0;
						  ReceivedMessage receivedMessage = null;

						  do
						  {
							  receivedMessage = source.GetMessage();

							  if (receivedMessage == null)
							  {
								  continue;
							  }

							  var transportMessage = (TransportMessage)_serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

							  destination.Enqueue(transportMessage.MessageId, receivedMessage.Stream);
							  source.Acknowledge(receivedMessage.AcknowledgementToken);

							  totalMessagesMoved++;
						  } while (receivedMessage != null);

						  _log.Information(string.Format(MessageResources.MoveAllComplete, totalMessagesMoved, sourceQueueUriValue, destinationQueueUriValue));

						  GetMessage();
					  });
		}

		public void ReturnToSourceQueue()
		{
			var sourceQueueUriValue = _view.SourceQueueUriValue;

			QueueTask("ReturnToSourceQueue",
					  () =>
					  {
						  // TODO: FIX

						  //var source = _queueManager.GetQueue(sourceQueueUriValue);

						  //foreach (var message in _view.SelectedMessages)
						  //{
						  //	message.StopIgnoring();
						  //	message.FailureMessages.Clear();

						  //	var destination = _queueManager.GetQueue(message.RecipientInboxWorkQueueUri);

						  //	using (var scope = new TransactionScope())
						  //	{
						  //		if (source.Acknowledge(message.MessageId))
						  //		{
						  //			_log.Information(string.Format(MessageResources.RemovedMessage,
						  //										  message.MessageId, sourceQueueUriValue));

						  //			destination.Enqueue(message.MessageId, _serializer.Serialize(message));

						  //			_log.Information(string.Format(MessageResources.EnqueuedMessage,
						  //										  message.MessageId,
						  //										  message.RecipientInboxWorkQueueUri));
						  //		}
						  //		else
						  //		{
						  //			_log.Warning(string.Format(MessageResources.CouldNotRemoveMessage,
						  //									  message.MessageId, sourceQueueUriValue));
						  //		}

						  //		scope.Complete();
						  //	}
						  //}
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

		public void StopIgnoring()
		{
			var sourceQueueUriValue = _view.SourceQueueUriValue;

			QueueTask("StopIgnoring",
					  () =>
					  {
						  // TODO: FIX

						  //var source = _queueManager.GetQueue(sourceQueueUriValue);

						  //foreach (var message in _view.SelectedMessages)
						  //{
						  //	message.StopIgnoring();

						  //	using (var scope = new TransactionScope())
						  //	{
						  //		if (source.Acknowledge(message.MessageId))
						  //		{
						  //			source.Enqueue(message.MessageId, _serializer.Serialize(message));
						  //		}

						  //		scope.Complete();
						  //	}

						  //	_log.Information(string.Format(MessageResources.StoppedIgnoringMessage,
						  //								  message.MessageId));
						  //}
					  });
		}

		public void ReturnAllToSourceQueue()
		{
			throw new NotImplementedException();
		}

		public void GetMessage()
		{
			ReleaseMessage();

			var sourceQueueUri = _view.SourceQueueUriValue;

			QueueTask("GetMessage",
					  () =>
					  {
						  var queue = _queueManager.GetQueue(sourceQueueUri);

						  if (queue == null)
						  {
							  _log.Error(MessageResources.SourceQueueUriReader);

							  return;
						  }

						  TransportMessage transportMessage = null;
						  object message = null;
						  var receivedMessage = queue.GetMessage();

						  if (receivedMessage == null)
						  {
							  _view.ClearMessageView();

							  return;
						  }

						  transportMessage = (TransportMessage) _serializer.Deserialize(typeof (TransportMessage), receivedMessage.Stream);

						  _selectedMessage = new SelectedMessage(sourceQueueUri, receivedMessage, transportMessage);

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
				);
		}

		private void ReleaseMessage()
		{
			if (!HasSelectedMessage)
			{
				return;
			}

			QueueTask("ReleaseMessage",
						  () =>
						  {
							  var queue = _queueManager.GetQueue(_selectedMessage.SourceQueueUri);

							  if (queue == null)
							  {
								  _log.Error(MessageResources.SourceQueueUriReader);

								  return;
							  }

							  queue.Release(_selectedMessage.ReceivedMessage.AcknowledgementToken);

							  _selectedMessage = null;
						  });
		}

		protected bool HasSelectedMessage
		{
			get { return _selectedMessage != null; }
		}
	}
}