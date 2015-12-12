using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    public interface IMessageManagementPresenter : IManagementModulePresenter
    {
	    void Acknowledge();
        void ReturnToSourceQueue();
		void Move();
		void MoveAll();
		void Copy();
		void CopyAll();
	    void ReturnAllToSourceQueue();
	    void GetMessage();
        void ReleaseMessage();
        void StopIgnoring();
        void RefreshQueues();
        void MessageSelected(MessageManagementPresenter.ReceivedMessageItem receivedMessageItem);
        void MessageSelectionCleared();
    }
}