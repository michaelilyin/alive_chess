namespace AliveChessLibrary.Interfaces
{
    public enum UserRole
    {
        Player        = 0,
        Administrator = 1,
        Moderator     = 2
    }

    /// <summary>
    /// интерфейс пользователя
    /// </summary>
    public interface IUser : IIdentity
    {
        /// <summary>
        /// уровень
        /// </summary>
        IStage Stage { get; }

        /// <summary>
        /// союз либо империя
        /// </summary>
        ICommunity Community { get; }

        /// <summary>
        /// сетевое соединение
        /// </summary>
        IConnectionInfo Connection { get; }

        /// <summary>
        /// роль пользователя
        /// </summary>
        UserRole Role { get; }
    }
}
