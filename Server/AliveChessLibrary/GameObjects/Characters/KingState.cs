namespace AliveChessLibrary.GameObjects.Characters
{
    public enum KingState
    {
        BigMap    = 0, // король ходит по карте
        Battle    = 1, // король с кем то сражается
        Trade     = 2, // король торгует
        Castle    = 3, // король сидит в замке
        Dispute   = 4, // король вступил в переговоры
        GetInfo   = 5, // король получает информмацию
        Negotiate = 6, // переговоры лидеров разных империй
    }
}
