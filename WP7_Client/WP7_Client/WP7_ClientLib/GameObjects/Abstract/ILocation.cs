namespace AliveChessLibrary.GameObjects.Abstract
{
    /// <summary>
    /// область, характеризуемая стоимостью прохождения
    /// </summary>
    public interface ILocation : IPosition<int>
    {
        /// <summary>
        /// стоимость прохождения
        /// </summary>
        float WayCost { get; set; }
    }
}
