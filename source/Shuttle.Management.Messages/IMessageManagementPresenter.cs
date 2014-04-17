namespace Shuttle.Management.Messages
{
    public interface IMessageManagementPresenter
    {
	    void Acknowledge();
        void Move();
        void ReturnToSourceQueue();
	    void MoveAll();
	    void ReturnAllToSourceQueue();
	    void GetMessage();
    }
}