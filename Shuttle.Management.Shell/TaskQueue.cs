using System;
using System.Collections.Generic;
using System.Threading;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class TaskQueue : IDisposable, IThreadState
	{
		private readonly object _padlock = new object();
		private readonly Queue<QueuedTask> _tasks = new Queue<QueuedTask>();
		private readonly Thread _thread;
		private volatile bool _active;
		private readonly ILog _log;

		public TaskQueue()
		{
			_log = Log.For(this);

			_log.Debug("Starting TaskQueue.");

			_active = true;

			_thread = new Thread(ProcessQueuedActions);

			_thread.Start();

			while (!_thread.IsAlive)
			{
			}

			_log.Debug("TaskQueue Started.");
		}

		public void QueueTask(string name, Action action)
		{
			lock (_padlock)
			{
				_log.Information(string.Format(ManagementResources.TaskQueued, name));

				_tasks.Enqueue(new QueuedTask(name, action));
			}
		}

		private void ProcessQueuedActions()
		{
			while (_active)
			{
				QueuedTask task = null;

				lock (_padlock)
				{
					if (_tasks.Count > 0)
					{
						task = _tasks.Dequeue();
					}
				}

				if (task != null)
				{
					_log.Information(string.Format(ManagementResources.RunningTask, task.Name));

                    try
                    {
                        task.Action.Invoke();
                    }
                    catch (Exception exception)
                    {
                        _log.Error(exception.AllMessages());
                    }

					_log.Information(string.Format(ManagementResources.TaskCompleted, task.Name));
				}

				if (task == null)
				{
					ThreadSleep.While(250, this);
				}
			}
		}

		internal class QueuedTask
		{
			public QueuedTask(string name, Action action)
			{
				Name = name;
				Action = action;
			}

			public string Name { get; private set; }
			public Action Action { get; private set; }
		}

		public void Dispose()
		{
			_log.Debug("Deactivating TaskQueue.");

			_active = false;

			_log.Debug("Joining UI thread.");

			_thread.Join(TimeSpan.FromSeconds(5));

			_log.Debug("TaskQueue disposing.");
		}

		public bool Active
		{
			get { return _active; }
		}
	}
}