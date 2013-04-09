namespace AliveChessLibrary.GameObjects.Abstract
{
    public enum PointTypes
    {
        None         = 0,
        King         = 1, // король
        Castle       = 2, // замок
        Mine         = 3, // шахта
        Resource     = 4, // ресурс
        SingleObject = 5, // оьъект, занимающий одну ячейку
        MultyObject  = 6, // объект, занимающий несколько ячеек
        Landscape    = 7, // тип местности
    }
}
