using System;

namespace Shuttle.Esb.Management.Shell
{
    public class QueueSelectedEventArgs : EventArgs
    {
        public QueueSelectedEventArgs(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; private set; }
    }
}