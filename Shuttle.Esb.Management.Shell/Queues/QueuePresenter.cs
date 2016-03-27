using System.Drawing;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb.Management.Shell
{
	public class QueuePresenter : ManagementModulePresenter, IQueuePresenter
	{
		private readonly IQueueView view;

		public QueuePresenter()
		{
			view = new QueueView(this);
		}

		public override string Text
		{
			get { return ManagementResources.TextQueues; }
		}

		public override Image Image
		{
			get { return ManagementResources.ImageQueues; }
		}

		public override UserControl ViewUserControl
		{
			get
			{
				var control = (UserControl) view;

				control.Enabled = ManagementConfiguration.HasQueueRepository;

				return control;
			}
		}

		public void Remove()
		{
			if (!view.HasSelectedQueues)
			{
				Log.Warning(string.Format(ManagementResources.NoItemsSelected, ManagementResources.TextQueues));

				return;
			}

			if (MessageBox.Show(string.Format(ManagementResources.ConfirmRemoval, ManagementResources.TextQueues), ManagementResources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			QueueTask("Remove",
			          () =>
			          	{
			          		foreach (var queue in view.SelectedQueues())
			          		{
			          			ManagementConfiguration.QueueRepository().Remove(queue.Uri);
			          		}

			          	});

			Refresh();
		}

		public void CheckAll()
		{
			view.CheckAll();
		}

		public void InvertChecks()
		{
			view.InvertChecks();
		}

		public void Save()
		{
			if (string.IsNullOrEmpty(view.UriValue))
			{
				Log.Error(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.Uri));

				return;
			}

			QueueTask("AddQueue",
			          () => ManagementConfiguration.QueueRepository().Save(new Queue
			                                                               	{
			                                                               		Uri = view.UriValue
			                                                               	}));

			Refresh();
		}

		public void Refresh()
		{
			QueueTask("Refresh",
				() => view.RefreshQueues(ManagementConfiguration.QueueRepository().All()));
		}

		public void QueueSelected()
		{
			var uri = view.GetSelectedQueueUri();

			if (string.IsNullOrEmpty(uri))
			{
				return;
			}

			view.UriValue = uri;
		}

		public override void OnViewReady()
		{
			base.OnViewReady();

			Refresh();
		}
	}
}