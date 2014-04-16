namespace Shuttle.Management.Messages
{
    public interface IMessageManagementPresenter
    {
        void RefreshQueue();
        void Remove();
        void Move();
        void ReturnToSourceQueue();
	    void MoveAll();
	    void ReturnAllToSourceQueue();
	    void GetMessage();
    }
}