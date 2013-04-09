namespace AliveChessLibrary.Interfaces
{
    /// <summary>
    /// пакет байт
    /// </summary>
    public interface IBytePackage
    {
        /// <summary>
        /// размер тела команды (количество байт)
        /// </summary>
        int CommandSize { get; set; }

        /// <summary>
        /// иня команды
        /// </summary>
        string CommandName { get; set; }

        /// <summary>
        /// тип команды (чат, большая карта, бой и т. д.)
        /// </summary>
        string CommandType { get; set; }

        /// <summary>
        /// тело команды
        /// </summary>
        byte[] CommandBody { get; set; }
    }
}
