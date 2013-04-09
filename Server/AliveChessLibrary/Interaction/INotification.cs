namespace AliveChessLibrary.Interaction
{
    public interface INotification
    {
        uint ReceiverId { get; set; }
        NotificationType Type { get; set; }
    }
}
