using AliveChessLibrary.Interaction;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Landscapes
{
    public interface ILevel : IStage
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// карта
        /// </summary>
        Map Map { get; set; }

        /// <summary>
        /// добавить диалог
        /// </summary>
        /// <param name="d"></param>
        void AddDispute(IDispute dispute);

        /// <summary>
        /// удалить диалог
        /// </summary>
        /// <param name="dispute"></param>
        void RemoveDispute(IDispute dispute);

        /// <summary>
        /// добавить битву
        /// </summary>
        /// <param name="b"></param>
        void AddBattle(Battle battle);

        /// <summary>
        /// удалить битву
        /// </summary>
        /// <param name="b"></param>
        void RemoveBattle(Battle battle);
    }
}
