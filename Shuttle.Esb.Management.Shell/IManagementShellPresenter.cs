using System;

namespace Shuttle.Esb.Management.Shell
{
    public interface IManagementShellPresenter : IDisposable
    {
        void OnViewReady();
    }
}