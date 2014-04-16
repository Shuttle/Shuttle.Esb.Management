using System;
using System.Drawing;
using System.IO;
using System.Transactions;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    public class MessageManagementPresenter : ManagementModulePresenter, IMessageManagementPresenter
    {
        private readonly IMessageConfiguration _messageConfiguration;
        private readonly IQueueManager _queueManager;
        private readonly ISerializer _serializer;
        private readonly IMessageManagementView _view;

	    private string _sourceQueueUri;
	    private ReceivedMessage _receivedMessage;
	    private TransportMessage _transportMessage;

	    public MessageManagementPresenter()
        {
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
            get { return (UserControl) _view; }
        }

        public void RefreshQueue()
        {
            var sourceQueueUriValue = _view.SourceQueueUriValue;
            
            QueueTask("RefreshQueue",
                      () =>
                          {
	                          var reader = _queueManager.GetQueue(sourceQueueUriValue) as IQueueReader;

                              if (reader == null)
                              {
                                  Log.Error(MessageResources.SourceQueueUriReader);

                                  return;
                              }
                          }
                );
        }

        public void Remove()
        {
			//if (!_view.HasSelectedMessages)
			//{
			//	Log.Error(MessageResources.NoMessagesSelected);

			//	return;
			//}

			//if (MessageBox.Show(MessageResources.ConfirmMessageDeletion,
			//					MessageResources.Confirmation,
			//					MessageBoxButtons.YesNo,
			//					MessageBoxIcon.Question) != DialogResult.Yes)
			//{
			//	return;
			//}

			//var sourceQueueUriValue = _view.SourceQueueUriValue;

			//QueueTask("Remove",
			//		  () =>
			//			  {
			//				  var queue = _queueManager.GetQueue(sourceQueueUriValue);

			//				  foreach (var message in _view.SelectedMessages)
			//				  {
			//					  queue.GetMessage();
			//					  queue.Acknowledge(message.MessageId);
			//				  }
			//			  });

            RefreshQueue();
        }

        public void Move()
        {
			//if (!_view.HasSelectedMessages)
			//{
			//	Log.Error(MessageResources.NoMessagesSelected);

			//	return;
			//}

            var sourceQueueUriValue = _view.SourceQueueUriValue;
            var destinationQueueUriValue = _view.DestinationQueueUriValue;

            QueueTask("Move",
                      () =>
                          {
							  // TODO: FIX

							  //var source = _queueManager.GetQueue(sourceQueueUriValue);
							  //var destination = _queueManager.GetQueue(destinationQueueUriValue);

							  //foreach (var message in _view.SelectedMessages)
							  //{
							  //	using (var scope = new TransactionScope())
							  //	{
							  //		if (source.Remove(message.MessageId))
							  //		{
							  //			Log.Information(string.Format(MessageResources.RemovedMessage,
							  //										  message.MessageId, sourceQueueUriValue));

							  //			destination.Enqueue(message.MessageId, _serializer.Serialize(message));

							  //			Log.Information(string.Format(MessageResources.EnqueuedMessage,
							  //										  message.MessageId, destinationQueueUriValue));
							  //		}
							  //		else
							  //		{
							  //			Log.Warning(string.Format(MessageResources.CouldNotRemoveMessage,
							  //									  message.MessageId, sourceQueueUriValue));
							  //		}

							  //		scope.Complete();
							  //	}
							  //}
                          })
                ;

            RefreshQueue();
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
							  //		if (source.Remove(message.MessageId))
							  //		{
							  //			Log.Information(string.Format(MessageResources.RemovedMessage,
							  //										  message.MessageId, sourceQueueUriValue));

							  //			destination.Enqueue(message.MessageId, _serializer.Serialize(message));

							  //			Log.Information(string.Format(MessageResources.EnqueuedMessage,
							  //										  message.MessageId,
							  //										  message.RecipientInboxWorkQueueUri));
							  //		}
							  //		else
							  //		{
							  //			Log.Warning(string.Format(MessageResources.CouldNotRemoveMessage,
							  //									  message.MessageId, sourceQueueUriValue));
							  //		}

							  //		scope.Complete();
							  //	}
							  //}
                          });

            RefreshQueue();
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
							  //		if (source.Remove(message.MessageId))
							  //		{
							  //			source.Enqueue(message.MessageId, _serializer.Serialize(message));
							  //		}

							  //		scope.Complete();
							  //	}

							  //	Log.Information(string.Format(MessageResources.StoppedIgnoringMessage,
							  //								  message.MessageId));
							  //}
                          });

            RefreshQueue();
        }

	    public void MoveAll()
	    {
		    throw new NotImplementedException();
	    }

	    public void ReturnAllToSourceQueue()
	    {
		    throw new NotImplementedException();
	    }

	    public void GetMessage()
	    {
		    if (HasSelectedMessage)
		    {
			    QueueTask("ReleaseMessage",
			              () =>
				              {
								  var queue = _queueManager.GetQueue(_sourceQueueUri);

								  if (queue == null)
								  {
									  Log.Error(MessageResources.SourceQueueUriReader);

									  return;
								  }

								  queue.Release(_receivedMessage.AcknowledgementToken);
				              });
		    }

		    _sourceQueueUri = _view.SourceQueueUriValue;

			QueueTask("GetMessage",
					  () =>
					  {
						  var queue = _queueManager.GetQueue(_sourceQueueUri);

						  if (queue == null)
						  {
							  Log.Error(MessageResources.SourceQueueUriReader);

							  return;
						  }

						  _receivedMessage = queue.GetMessage();

						  _transportMessage = (TransportMessage)_serializer.Deserialize(typeof(TransportMessage), _receivedMessage.Stream);

						  object message = null;

						  try
						  {
							  var type = Type.GetType(_transportMessage.AssemblyQualifiedName, true, true);

							  var canDisplayMessage = true;

							  if (_transportMessage.CompressionEnabled())
							  {
								  Log.Warning(string.Format(MessageResources.MessageCompressed, _transportMessage.MessageId));
								  canDisplayMessage = false;
							  }

							  if (_transportMessage.EncryptionEnabled())
							  {
								  Log.Warning(string.Format(MessageResources.MessageEncrypted, _transportMessage.MessageId));
								  canDisplayMessage = false;
							  }

							  if (canDisplayMessage)
							  {
								  using (var stream = new MemoryStream(_transportMessage.Message))
								  {
									  message = _serializer.Deserialize(type, stream);
								  }

								  if (!type.AssemblyQualifiedName.Equals(_transportMessage.AssemblyQualifiedName))
								  {
									  Log.Warning(string.Format(MessageResources.MessageTypeMismatch,
																_transportMessage.AssemblyQualifiedName,
																type.AssemblyQualifiedName));
								  }
							  }
						  }
						  catch (Exception ex)
						  {
							  Log.Warning(string.Format(MessageResources.CannotObtainMessageType,
														_transportMessage.AssemblyQualifiedName));
							  Log.Error(ex.Message);
						  }

						  _view.ShowMessage(_transportMessage, message);
					  }
				);
		}

	    protected bool HasSelectedMessage
	    {
		    get { return _receivedMessage != null; }
	    }
    }
}