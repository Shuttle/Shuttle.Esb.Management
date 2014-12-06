namespace Shuttle.Management.Messages
{
    public interface IMessageManagementPresenter
    {
	    void Acknowledge();
        void ReturnToSourceQueue();
		void Move();
		void MoveAll();
		void Copy();
		void CopyAll();
	    void ReturnAllToSourceQueue();
	    void GetMessage();
    }
}