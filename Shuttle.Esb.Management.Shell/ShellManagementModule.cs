﻿using System.Collections.Generic;

namespace Shuttle.Esb.Management.Shell
{
    public class ShellManagementModule : IManagementModule
    {
        public void Configure(IManagementConfiguration managementConfiguration)
        {
        }

        public IEnumerable<IManagementModulePresenter> Presenters
        {
            get { return new IManagementModulePresenter[] { new DataStorePresenter(), new QueuePresenter() }; }
        }
    }
}