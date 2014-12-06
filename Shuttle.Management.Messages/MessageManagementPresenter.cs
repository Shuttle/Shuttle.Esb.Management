using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
	public class MessageManagementPresenter : ManagementModulePresenter, IMessageManagementPresenter, IDisposable
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

			_queueManager = new QueueManager();

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
			get { return (UserControl) _view; }
		}

		public void Acknowledge()
		{
			if (!HasSelectedMessage)
			{
				_log.Information(MessageResources.NoMessageSelected);

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

							  GetMessage();
				          });
		}

		public void Move()
		{
			if (!HasSelectedMessage)
			{
				_log.Information(MessageResources.NoMessageSelected);

				return;
			}

			ClearMessageView();

			var destinationQueueUriValue = _view.DestinationQueueUriValue;

			QueueTask("Move",
			          () =>
				          {
					          var destination = _queueManager.GetQueue(destinationQueueUriValue);
					          var source = _queueManager.GetQueue(_selectedMessage.SourceQueueUri);

					          destination.Enqueue(_selectedMessage.TransportMessage.MessageId, _selectedMessage.ReceivedMessage.Stream);
					          source.Acknowledge(_selectedMessage.ReceivedMessage.AcknowledgementToken);

					          _log.Information(string.Format(MessageResources.EnqueuedMessage,
					                                         _selectedMessage.TransportMessage.MessageId, destinationQueueUriValue));
					          _log.Information(string.Format(MessageResources.RemovedMessage,
					                                         _selectedMessage.TransportMessage.MessageId,
					                                         _selectedMessage.SourceQueueUri));

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

			ClearMessageView();

			ReleaseMessage();

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

						          var transportMessage =
							          (TransportMessage) _serializer.Deserialize(typeof (TransportMessage), receivedMessage.Stream);

						          destination.Enqueue(transportMessage.MessageId, receivedMessage.Stream);
						          source.Acknowledge(receivedMessage.AcknowledgementToken);

						          totalMessagesMoved++;
					          } while (receivedMessage != null);

					          _log.Information(string.Format(MessageResources.MoveAllComplete, totalMessagesMoved, sourceQueueUriValue,
					                                         destinationQueueUriValue));

					          GetMessage();
				          });
		}

		public void Copy()
		{
			if (!HasSelectedMessage)
			{
				_log.Information(MessageResources.NoMessageSelected);

				return;
			}

			var destinationQueueUriValue = _view.DestinationQueueUriValue;

			QueueTask("Copy",
					  () =>
					  {
						  _queueManager.GetQueue(destinationQueueUriValue).Enqueue(_selectedMessage.TransportMessage.MessageId, _selectedMessage.ReceivedMessage.Stream);

						  _log.Information(string.Format(MessageResources.EnqueuedMessage,
														 _selectedMessage.TransportMessage.MessageId, destinationQueueUriValue));
					  });
		}

		public void CopyAll()
		{
			var sourceQueueUriValue = _view.SourceQueueUriValue;
			var destinationQueueUriValue = _view.DestinationQueueUriValue;

			if (!HasSelectedMessage)
			{
				_log.Information(MessageResources.NoMessageSelected);

				return;
			}

			if (MessageBox.Show(string.Format(MessageResources.ConfirmCopyAll, sourceQueueUriValue, destinationQueueUriValue),
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
						  var startMessageId = _selectedMessage.TransportMessage.MessageId;

						  source.Release(_selectedMessage.ReceivedMessage.AcknowledgementToken);

						  ReceivedMessage receivedMessage;
						  TransportMessage transportMessage = null;

						  do
						  {
							  receivedMessage = source.GetMessage();

							  if (receivedMessage == null)
							  {
								  continue;
							  }

							  transportMessage = (TransportMessage)_serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

							  destination.Enqueue(transportMessage.MessageId, receivedMessage.Stream);
							  source.Release(receivedMessage.AcknowledgementToken);

							  totalMessagesCopied++;
						  } while (receivedMessage != null && !startMessageId.Equals(transportMessage.MessageId));

						  _log.Information(string.Format(MessageResources.CopyAllComplete, totalMessagesCopied, sourceQueueUriValue,
														 destinationQueueUriValue));

						  GetMessage();
					  });
		}

		public void ReturnToSourceQueue()
		{
			if (!HasSelectedMessage)
			{
				_log.Information(MessageResources.NoMessageSelected);

				return;
			}

			ClearMessageView();

			QueueTask("ReturnToSourceQueue",
			          () =>
				          {
					          var source = _queueManager.GetQueue(_selectedMessage.SourceQueueUri);
					          var destination = _queueManager.GetQueue(_selectedMessage.TransportMessage.RecipientInboxWorkQueueUri);

					          _selectedMessage.TransportMessage.StopIgnoring();
					          _selectedMessage.TransportMessage.FailureMessages.Clear();

					          destination.Enqueue(_selectedMessage.TransportMessage.MessageId,
					                              _serializer.Serialize(_selectedMessage.TransportMessage));
					          source.Acknowledge(_selectedMessage.ReceivedMessage.AcknowledgementToken);

					          _log.Information(string.Format(MessageResources.RemovedMessage,
					                                         _selectedMessage.TransportMessage.MessageId,
					                                         _selectedMessage.SourceQueueUri));
					          _log.Information(string.Format(MessageResources.EnqueuedMessage,
					                                         _selectedMessage.TransportMessage.MessageId,
					                                         _selectedMessage.TransportMessage.RecipientInboxWorkQueueUri));

							  GetMessage();
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
						  ReceivedMessage receivedMessage = null;

						  do
						  {
							  receivedMessage = source.GetMessage();

							  if (receivedMessage == null)
							  {
								  continue;
							  }

							  var transportMessage =
								  (TransportMessage)_serializer.Deserialize(typeof(TransportMessage), receivedMessage.Stream);

							  var destination = _queueManager.GetQueue(transportMessage.RecipientInboxWorkQueueUri);

							  destination.Enqueue(transportMessage.MessageId, receivedMessage.Stream);
							  source.Acknowledge(receivedMessage.AcknowledgementToken);

							  totalMessagesReturned++;
						  } while (receivedMessage != null);

						  _log.Information(string.Format(MessageResources.ReturnAllToSourceQueueComplete, totalMessagesReturned, sourceQueueUriValue));

						  GetMessage();
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
			if (!HasSelectedMessage)
			{
				_log.Information(MessageResources.NoMessageSelected);

				return;
			}

			ClearMessageView();

			QueueTask("StopIgnoring",
			          () =>
				          {
					          var source = _queueManager.GetQueue(_selectedMessage.SourceQueueUri);

					          _selectedMessage.TransportMessage.StopIgnoring();

					          source.Enqueue(_selectedMessage.TransportMessage.MessageId,
					                         _serializer.Serialize(_selectedMessage.TransportMessage));
					          source.Acknowledge(_selectedMessage.ReceivedMessage.AcknowledgementToken);

					          GetMessage();
				          });
		}

		private void ClearMessageView()
		{
			_view.ClearMessageView();
		}

		public void GetMessage()
		{
			ReleaseMessage();

			var sourceQueueUri = _view.SourceQueueUriValue;

			ClearMessageView();

			QueueTask("GetMessage",
			          () =>
				          {
							  _selectedMessage = null;
							  
							  var queue = _queueManager.GetQueue(sourceQueueUri);

					          if (queue == null)
					          {
						          _log.Error(MessageResources.SourceQueueUriReader);

						          return;
					          }

					          object message = null;
					          var receivedMessage = queue.GetMessage();

					          if (receivedMessage == null)
					          {
								  return;
					          }

					          var transportMessage =
						          (TransportMessage) _serializer.Deserialize(typeof (TransportMessage), receivedMessage.Stream);

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
				_log.Information(MessageResources.NoMessageSelected);

				return;
			}

			ClearMessageView();

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
				          });
		}

		protected bool HasSelectedMessage
		{
			get { return _selectedMessage != null; }
		}

		public void Dispose()
		{
			if (_queueManager != null)
			{
				_queueManager.AttemptDispose();
			}
		}

	}
}