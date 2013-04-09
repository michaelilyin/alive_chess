namespace AliveChessLibrary.Commands
{
    public enum CommandType
    {
        None             = 0, // не определено
        BigMapCommand    = 1, // команды большой карты
        DialogCommand    = 2, // команды диалога игроков
        BattleCommand    = 3, // команды карты боя
        CastleCommand    = 4, //команды управления замком
        TradingCommand   = 5, // команды для торговли
        RegisterCommand  = 6, // команды входа в игру и выхода
        EmpireCommand    = 7, // команды управления империей
        StatisticCommand = 8, // команды статистики
        ErrorCommand     = 9, // команды ошибок
        ChatCommand      = 10 // команды чата
    }
}
