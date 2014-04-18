using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;

namespace Shuttle.Management.Shell
{
	public class ManagementShellPresenter : IManagementShellPresenter
	{
		private readonly IManagementShellView _view;
		private readonly TaskQueue _taskQueue;
		private readonly IManagementConfiguration _managementConfiguration;
		private readonly List<IManagementModulePresenter> _presenters = new List<IManagementModulePresenter>();

		public ManagementShellPresenter(IManagementShellView view)
		{
			_view = view;

			_taskQueue = new TaskQueue();

			ActionAppender.Register(view.LogMessage);

			_managementConfiguration = new ManagementConfiguration();

			_managementConfiguration.Initialize();
		}

		public void Dispose()
		{
			foreach (var presenter in _presenters)
			{
				presenter.AttemptDispose();
			}

			_taskQueue.Dispose();
		}

		public void OnViewReady()
		{
			var reflectionService = new ReflectionService();

			var moduleTypes = reflectionService.GetTypes<IManagementModule>();

			foreach (var type in moduleTypes)
			{
				if (!type.HasDefaultConstructor())
				{
					Log.Warning(string.Format(ManagementResources.ManagementModuleInitializerHasNoDefaultConstructor, type.FullName));
				}
				else
				{
				    var module = ((IManagementModule) Activator.CreateInstance(type));

				    module.Configure(_managementConfiguration);

                    foreach (ManagementModulePresenter presenter in module.Presenters)
				    {
                        presenter.TaskQueue = _taskQueue;
                        presenter.ManagementConfiguration = _managementConfiguration;

                        _view.AddManagementModulePresenter(presenter);

                        _presenters.Add(presenter);
                    }
                }
			}
		}
	}
}