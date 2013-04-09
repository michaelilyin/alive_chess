namespace AliveChessPluginLibrary
{
    /// <summary>
    /// контекст плагина. Плагин его использует для получения
    /// доступа к контексту сервера
    /// </summary>
    public interface IAliveChessPluginContext
    {
        string PluginDir { get; }

        IAliveChessServerContext Server { get; }
    }
}