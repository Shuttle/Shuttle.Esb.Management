using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb.Management.Shell
{
	public enum ShowQueueButtonPosition
	{
		NotShown = 0,
		Left = 1,
		Right = 2
	}

	public partial class QueueHierarchyView : UserControl, IQueueHierarchyView
	{
		private readonly Form _queueForm = new Form();
		private readonly TreeView _queueTree = new TreeView();
		private readonly Timer _timer = new Timer { Interval = 100 };

		public QueueHierarchyView()
		{
			InitializeComponent();

			_timer.Tick += delegate
			{
				_timer.Stop();

				SelectedQueueUri.Focus();
			};

			_queueTree.Dock = DockStyle.Fill;
			_queueTree.Location = new Point(0, 20);
			_queueTree.Name = "QueueTree";
			_queueTree.Size = new Size(390, 250);
			_queueTree.TabIndex = 0;
			_queueTree.LostFocus += queueTree_LostFocus;
			_queueTree.NodeMouseClick += queueTree_NodeMouseClick;
			_queueTree.KeyDown += queueTree_KeyDown;

			_queueForm.Controls.Add(_queueTree);
			_queueForm.FormBorderStyle = FormBorderStyle.None;
			_queueForm.StartPosition = FormStartPosition.Manual;
			_queueForm.ShowInTaskbar = false;
			_queueForm.BackColor = SystemColors.Control;
			_queueForm.Deactivate += delegate
				{
					_timer.Start();
				};

			ShowQueuesButton.Click += ShowQueuesButtonClick;
		}

		private void ShowQueuesButtonClick(object sender, EventArgs e)
		{
			ShowQueueForm();
		}

		private ShowQueueButtonPosition showQueueButtonPosition;

		public ShowQueueButtonPosition ShowQueueButtonPosition
		{
			get { return showQueueButtonPosition; }
			set
			{
				showQueueButtonPosition = value;

				switch (showQueueButtonPosition)
				{
					case ShowQueueButtonPosition.Left:
						{
							ShowQueuesButton.Visible = true;
							ShowQueuesButton.Dock = DockStyle.Left;

							break;
						}
					case ShowQueueButtonPosition.Right:
						{
							ShowQueuesButton.Visible = true;
							ShowQueuesButton.Dock = DockStyle.Right;

							break;
						}
					default:
						{
							ShowQueuesButton.Visible = false;
							break;
						}
				}
			}
		}

		public event EventHandler<QueueSelectedEventArgs> QueueSelected = delegate { };

		public void AddQueue(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			var scheme = _queueTree.Nodes.ContainsKey(uri.Scheme)
							 ? _queueTree.Nodes[uri.Scheme]
							 : _queueTree.Nodes.Add(uri.Scheme, uri.Scheme);

			var host = scheme.Nodes.ContainsKey(uri.Host)
						   ? scheme.Nodes[uri.Host]
						   : scheme.Nodes.Add(uri.Host, uri.Host);

			var localPath = uri.LocalPath.Replace("/", string.Empty);

			var key = Key(uri);

			if (!host.Nodes.ContainsKey(key))
			{
				var userName = string.Empty;

				if (!string.IsNullOrEmpty(uri.UserInfo))
				{
					var colonPosition = uri.UserInfo.IndexOf(":", StringComparison.OrdinalIgnoreCase);

					userName = colonPosition > -1 ? uri.UserInfo.Substring(0, colonPosition) : uri.UserInfo;
				}

				host.Nodes.Add(key, string.Concat(localPath, string.IsNullOrEmpty(userName) ? string.Empty : string.Format(" [{0}]", userName))).Tag = uri;
			}

			_queueTree.Sort();
			_queueTree.ExpandAll();
		}

		private static string Key(Uri uri)
		{
			return uri.ToString().ToLower();
		}

		public void AddQueue(string uri)
		{
			AddQueue(new Uri(uri));
		}

		public void Clear()
		{
			_queueTree.Nodes.Clear();
		}

		public bool ContainsQueue(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return _queueTree.Nodes.Find(uri.ToString(), true).Length > 0;
		}

		public bool ContainsQueue(string uri)
		{
			return ContainsQueue(new Uri(uri));
		}

		public bool RemoveQueue(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			var node = FindQueueNode(uri);

			if (node != null)
			{
				_queueTree.Nodes.Remove(node);

				NormalizeTree();
			}

			return node != null;
		}

		private void NormalizeTree()
		{
			var node = FindChildlessNode(_queueTree.Nodes);

			while (node != null)
			{
				_queueTree.Nodes.Remove(node);

				node = FindChildlessNode(_queueTree.Nodes);
			}
		}

		private static TreeNode FindChildlessNode(TreeNodeCollection nodes)
		{
			return nodes.Cast<TreeNode>().FirstOrDefault(node => node.Nodes.Count == 0);
		}

		public override string Text
		{
			get { return SelectedQueueUri.Text; }
			set { SelectedQueueUri.Text = value; }
		}

		public bool RemoveQueue(string uri)
		{
			return RemoveQueue(new Uri(uri));
		}

		private void queueTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			NodeSelected(e.Node);
		}

		private void NodeSelected(TreeNode node)
		{
			if (node == null || node.Tag == null)
			{
				return;
			}

			SelectedQueueUri.Text = node.Tag.ToString();

			HideQueueForm();

			QueueSelected.Invoke(this, new QueueSelectedEventArgs((Uri)node.Tag));
		}

		private void queueTree_KeyDown(object sender, KeyEventArgs e)
		{
			e.OnF4(HideQueueForm);
			e.OnEscape(HideQueueForm);
			e.OnEnterPressed(() => NodeSelected(_queueTree.SelectedNode));
		}

		private void queueTree_LostFocus(object sender, EventArgs e)
		{
			HideQueueForm();
		}

		private void SelectedQueueUri_Click(object sender, EventArgs e)
		{
			HideQueueForm();
		}

		private void ShowQueueForm()
		{
			if (_queueForm.Visible)
			{
				return;
			}

			var rectangle = RectangleToScreen(ClientRectangle);

			_queueForm.Location = rectangle.Y + SelectedQueueUri.Height + _queueForm.Height >
								 Screen.FromHandle(_queueForm.Handle).Bounds.Height
									 ? new Point(rectangle.X, rectangle.Y - _queueForm.Height)
									 : new Point(rectangle.X, rectangle.Y + SelectedQueueUri.Height);

			_queueForm.Width = rectangle.Width;
			_queueForm.Show();
			_queueForm.BringToFront();
		}

		private void HideQueueForm()
		{
			_queueForm.Hide();
		}

		private void ToggleQueueForm()
		{
			if (_queueForm.Visible)
			{
				HideQueueForm();
			}
			else
			{
				ShowQueueForm();
			}
		}

		private void SelectedQueueUri_KeyUp(object sender, KeyEventArgs e)
		{
			e.OnEnterPressed(() =>
								 {
									 Uri uri;

									 if (Uri.TryCreate(SelectedQueueUri.Text, UriKind.Absolute, out uri))
									 {
										 QueueSelected.Invoke(this, new QueueSelectedEventArgs(uri));
									 }
								 });
			e.OnKeyDown(ShowQueueForm);
			e.OnF4(ToggleQueueForm);
		}

		private TreeNode FindQueueNode(Uri uri)
		{
			var nodes = _queueTree.Nodes.Find(Key(uri), true);

			return nodes.Length > 0
					   ? nodes[0]
					   : null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

	}
}