using System.Collections.Generic;
using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Messages
{
    public class MessageManagementModule : IManagementModule
    {
        public void Configure(IManagementConfiguration managementConfiguration)
        {
        }

        public IEnumerable<IManagementModulePresenter> Presenters
        {
            get
            {
                return new List<IManagementModulePresenter>
                           {
                               new MessageManagementPresenter()
                           };
            }
        }
    }
}