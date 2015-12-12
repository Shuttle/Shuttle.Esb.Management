using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;
using Shuttle.Management.Shell;

namespace Shuttle.Management
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Log.Assign(new Log4NetLog(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)));

			Application.ThreadException += ThreadException;

			Log.Debug("log4net configured.");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ManagementShellView());
		}

		private static void ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Log.Error(e.Exception.AllMessages());

			var reflection = e.Exception as ReflectionTypeLoadException;

			if (reflection != null)
			{
				foreach (var exception in reflection.LoaderExceptions)
				{
					Log.Debug(string.Format("- '{0}'.", exception.AllMessages()));
				}
			}
		}
	}
}