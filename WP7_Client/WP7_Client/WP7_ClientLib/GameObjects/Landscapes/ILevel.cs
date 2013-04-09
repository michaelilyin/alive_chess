using AliveChessLibrary.Interaction;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    public interface ILevel : IStage
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        new int Id { get; set; }

        /// <summary>
        /// карта
        /// </summary>
        Map Map { get; set; }

        /// <summary>
        /// добавить диалог
        /// </summary>
        void AddDispute(IDispute dispute);

        /// <summary>
        /// удалить диалог
        /// </summary>
        void RemoveDispute(IDispute dispute);

        /// <summary>
        /// добавить битву
        /// </summary>
        void AddBattle(Battle battle);

        /// <summary>
        /// удалить битву
        /// </summary>
        void RemoveBattle(Battle battle);
    }
}