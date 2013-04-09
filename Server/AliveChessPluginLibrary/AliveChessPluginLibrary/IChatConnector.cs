using System.Collections.Generic;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Interfaces;

namespace AliveChessPluginLibrary
{
    /// <summary>
    /// коннектор для чата
    /// </summary>
    public interface IChatConnector : IConnector
    {
        /// <summary>
        /// запрос пользователя
        /// </summary>
        /// <param name="name">идентификатор пользователя</param>
        /// <returns></returns>
        IUser RequestUser(int playerId);

        /// <summary>
        /// авторизация
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ProvideCredentials(string login, string password);

        /// <summary>
        /// предоставления списка союзников
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<IUser> RequestAllies(IUser user);

        /// <summary>
        /// посылка сообщения
        /// </summary>
        /// <param name="info"></param>
        /// <param name="array"></param>
        void SendMessage(IConnectionInfo info, byte[] array);

        /// <summary>
        /// посылка сообщения
        /// </summary>
        void SendMessage<T>(IConnectionInfo info, T command) where T : ICommand;

        /// <summary>
        /// список сообществ
        /// </summary>
        /// <param name="stage">уровень</param>
        /// <returns></returns>
        List<ICommunity> RequestCommunities(IStage stage);

        /// <summary>
        /// список сообществ
        /// </summary>
        List<ICommunity> RequestCommunities();
    }
}