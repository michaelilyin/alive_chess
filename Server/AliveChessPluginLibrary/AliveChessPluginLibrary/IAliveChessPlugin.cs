namespace AliveChessPluginLibrary
{
    /// <summary>
    /// основной интерфейс плагина
    /// </summary>
    public interface IAliveChessPlugin
    {
        string Name { get; }

        void AfterCreate(IAliveChessPluginContext context);

        void BeforeDestroy();
    }
}