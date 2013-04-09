using AliveChessLibrary.GameObjects.Abstract;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessLibrary.Statistics;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Characters
{
    /// <summary>
    /// игрок
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// добавить короля
        /// </summary>
        /// <param name="king"></param>
        void AddKing(King king);

        /// <summary>
        /// удалить короля
        /// </summary>
        /// <param name="king"></param>
        void RemoveKing(King king);

        /// <summary>
        /// добавить область видимости
        /// </summary>
        /// <param name="space"></param>
        void AddVisibleSector(IVisibleSpace space);

        /// <summary>
        /// удалить область видимости
        /// </summary>
        /// <param name="space"></param>
        void RemoveVisibleSector(IVisibleSpace space);

        /// <summary>
        /// идентификатор
        /// </summary>
        int Id { get; }

        /// <summary>
        /// карта
        /// </summary>
        Map Map { get; set; }

        /// <summary>
        /// атрибут ИИ
        /// </summary>
        bool Bot { get; }

        /// <summary>
        /// атрибут готовности
        /// </summary>
        bool Ready { get; set; }

        /// <summary>
        /// идетификатор уровня
        /// </summary>
        int LevelId { get; set; }

        /// <summary>
        /// уровень
        /// </summary>
        ILevel Level { get; set; }

        /// <summary>
        /// атрибут супер пользователя
        /// </summary>
        bool IsSuperUser { get; set; }

        /// <summary>
        /// отправитель сообщений
        /// </summary>
        IMessenger Messenger { get; set; }

        /// <summary>
        /// статистика
        /// </summary>
        Statistic Statistics { get; set; }

        /// <summary>
        /// область видимости
        /// </summary>
        IVisibleSpace VisibleSpace { get; }

        /// <summary>
        /// объединение игроков
        /// </summary>
        ICommunity Community { get; set; }
    }
}
