namespace AliveChessLibrary.Interfaces
{
    /// <summary>
    /// объединение игроков (союз, империя)
    /// </summary>
    public interface ICommunity
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// показатель военной мощи
        /// </summary>
        /// <returns></returns>
        double RequestPower();

        /// <summary>
        /// показатель богатства
        /// </summary>
        /// <returns></returns>
        double RequestWealth();
    }
}
