namespace AliveChessLibrary.Interfaces
{
    /// <summary>
    /// идентификатор пользователя
    /// </summary>
    public interface IIdentity
    {
        string Login { get; set; }

        string Password { get; set; }
    }
}
