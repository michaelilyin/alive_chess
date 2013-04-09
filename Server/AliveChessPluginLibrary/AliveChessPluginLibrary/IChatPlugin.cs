using System;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Interfaces;

namespace AliveChessPluginLibrary
{
    public interface IChatPlugin : IAliveChessPlugin
    {
        /// <summary>
        /// через этот метод чат принимает сообщения исходящие от сетевого
        /// модуля
        /// </summary>
        /// <param name="package">пакет байт</param>
        /// <param name="from">отправитель</param>
        void Receive(IBytePackage package, IConnectionInfo from);

        /// <summary>
        /// через этот метод чат принимает сообщения исходящие от сетевого
        /// модуля
        /// </summary>
        /// <param name="command">команда</param>
        /// <param name="from">отправитель</param>
        void Receive(ICommand command, IConnectionInfo from);

        /// <summary>
        /// обработка сообщения чата. Вызывается из серверной части,
        /// работает в отдельном потоке.
        /// Никаких бесконечных циклов быть не должно!
        /// </summary>
        void Execute();

        /// <summary>
        /// получние чатом сообщения об ошибке
        /// </summary>
        /// <param name="exception"></param>
        void Receive(Exception exception);
    }
}