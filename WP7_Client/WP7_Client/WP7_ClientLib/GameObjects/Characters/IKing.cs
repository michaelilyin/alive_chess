using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.Statistics;
using AliveChessLibrary.Interfaces;

namespace AliveChessLibrary.GameObjects.Characters
{
    /// <summary>
    /// Король
    /// </summary>
    public interface IKing : IMovable
    {
        /// <summary>
        /// обновить состояние
        /// </summary>
        void Update();

        /// <summary>
        /// покинуть замок
        /// </summary>
        void LeaveCastle();

        /// <summary>
        /// искать ближний замок
        /// </summary>
        /// <returns></returns>
        Castle SearchCastle();

        /// <summary>
        /// зайти в замок
        /// </summary>
        /// <param name="castle"></param>
        void ComeInCastle(Castle castle);

        /// <summary>
        /// проверить нахождение внутри замка
        /// </summary>
        /// <param name="castle"></param>
        /// <returns></returns>
        bool InsideCastle(Castle castle);

        /// <summary>
        /// назначить начальный замок
        /// </summary>
        /// <param name="castle"></param>
        void AttachStartCastle(Castle castle);

        /// <summary>
        /// имя
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// опыт
        /// </summary>
        int Experience { get; set; }

        /// <summary>
        /// воинское звание
        /// </summary>
        int MilitaryRank { get; set; }

        /// <summary>
        /// ссылка на игрока
        /// </summary>
        IPlayer Player { get; set; }

        /// <summary>
        /// объект для оценки различных коэффициентов
        /// </summary>
        IEvaluator Evaluator { get; set; }

        /// <summary>
        /// объединение
        /// </summary>
        ICommunity Community { get; }
    }
}
