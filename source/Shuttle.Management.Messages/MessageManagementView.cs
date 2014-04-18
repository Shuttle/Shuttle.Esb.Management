using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
	public partial class MessageManagementView : UserControl, IMessageManagementView
	{
		private readonly MessageManagementPresenter presenter;

		public MessageManagementView(MessageManagementPresenter presenter)
		{
			InitializeComponent();

			this.presenter = presenter;

			MessageToolStrip.AddItem(MessageResources.TextAcknowledge,
			                         ManagementResources.ImageRemove,
			                         delegate { presenter.Acknowledge(); });
			MessageToolStrip.AddItem(MessageResources.TextStopIgnoring,
									 MessageResources.ImageStopIgnoring,
									 delegate { presenter.StopIgnoring(); });
			MessageToolStrip.AddItem(MessageResources.TextMove,
									 MessageResources.ImageArrowDown,
			                         delegate { presenter.Move(); });
			MessageToolStrip.AddItem(MessageResources.TextMoveAll,
									 MessageResources.ImageArrowDown,
			                         delegate { presenter.MoveAll(); });
			MessageToolStrip.AddItem(MessageResources.TextReturnToSourceQueue,
			                         MessageResources.ImageArrowBack,
			                         delegate { presenter.ReturnToSourceQueue(); });
			MessageToolStrip.AddItem(MessageResources.TextReturnAllToSourceQueue,
			                         MessageResources.ImageArrowBack,
			                         delegate { presenter.ReturnAllToSourceQueue(); });
			MessageToolStrip.AddItem(MessageResources.TextGetMessage,
			                         MessageResources.ImageMessage,
			                         delegate { presenter.GetMessage(); });
			MessageToolStrip.AddItem(MessageResources.TextRefreshQueues,
			                         ManagementResources.ImageQueues,
			                         delegate { presenter.RefreshQueues(); });
		}

		protected override void OnLoad(EventArgs e)
		{
			presenter.OnViewReady();
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

		private void SourceQueueUri_QueueSelected(object sender, QueueSelectedEventArgs e)
		{
			this.Invoke(() => presenter.GetMessage());
		}
	}
}