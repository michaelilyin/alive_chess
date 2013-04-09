namespace AliveChessPluginLibrary
{
    /// <summary>
    /// контекст сервера. Содержит коннесторы (объекты для вызыва
    /// методов логики сервера из плагинов) и интерфейсы модулей логики
    /// </summary>
    public interface IAliveChessServerContext
    {
        IChatConnector Chat { get; set; }
    }
}