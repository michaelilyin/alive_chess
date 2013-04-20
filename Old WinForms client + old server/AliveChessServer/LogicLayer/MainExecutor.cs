using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AliveChessLibrary.Commands;
using AliveChessServer.LogicLayer.RequestExecutors;
using AliveChessServer.LogicLayer.RequestExecutors.BattleExecutors;
using AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors;
using AliveChessServer.LogicLayer.RequestExecutors.CastleExecutors;
using AliveChessServer.LogicLayer.RequestExecutors.DialogExecutors;
using AliveChessServer.LogicLayer.RequestExecutors.EmpireExecutors;
using AliveChessServer.LogicLayer.RequestExecutors.RegisterExecutors;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer
{
    public class MainExecutor
    {
        private CommandPool _commandPool;
        private ProtoBufferTransport _transport;
        private static IDictionary<string, IExecutor> _executors;

        public MainExecutor(CommandPool commands, ProtoBufferTransport transport,
            AliveChessLogger logger, LogicEntryPoint gameLogic)
        {
            this._commandPool = commands;
            this._transport = transport;
         
            Debug.Assert(gameLogic.Environment != null);
            Debug.Assert(gameLogic.PlayerManager != null);

            _executors = new Dictionary<string, IExecutor>();

            // обработчик авторизации пользователей
            _executors.Add(ExecutorTypes.AuthorizeRequestExecutor.ToString(), new AuthorizeExecutor(gameLogic, transport));

            // покидание замка
            _executors.Add(ExecutorTypes.LeaveCastleRequestExecutor.ToString(), new LeaveCastleExecutor(gameLogic));

            // 
            //_executors.Add(ExecutorTypes.CaptureMineRequestExecutor.ToString(), new CaptureMineExecutor(gameLogic, queryManager));

            //
           // _executors.Add(ExecutorTypes.GetResourceRequestExecutor.ToString(), new GetResourceExecutor(gameLogic, queryManager));

            // выйти из игры
            _executors.Add(ExecutorTypes.ExitFromGameRequestExecutor.ToString(), new ExitFromGameExecutor(gameLogic, transport));

            // обработчик регистрации пользователей
            _executors.Add(ExecutorTypes.RegisterRequestExecutor.ToString(), new ExitFromGameExecutor(gameLogic, transport));

            //
            _executors.Add(ExecutorTypes.GetRecBuildingsRequestExecutor.ToString(), new GetRecBuildingsExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.BuildingInCastleRequestExecutor.ToString(), new BuildingInCastleRequestExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.BuyFigureRequestExecutor.ToString(), new BuyFigureExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.GetArmyCastleToKingRequestExecutor.ToString(), new GetArmyCastleToKingExequtor(gameLogic));
            //
            _executors.Add(ExecutorTypes.GetInnerBuildingsRequestExecutor.ToString(), new GetInnerBuildingsExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.GetListBuildingsInCastleRequestExecutor.ToString(), new GetListBuildingsInCastleExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.ShowArmyCastleRequestExecutor.ToString(), new ShowArmyCastleExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.ShowArmyKingRequestExecutor.ToString(), new ShowArmyKingExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.DownloadBattlefildRequestExecutor.ToString(), new DownloadBattlefildExecutor(gameLogic));
            //
            _executors.Add(ExecutorTypes.PlayerMoveRequestExecutor.ToString(), new PlayerMoveRequestExecutor(gameLogic));

            InitializeBigMapExecutors(gameLogic);
            InitializeDialogExecutors(gameLogic, transport);
            InitializeEmpireExecutors(gameLogic);
        }

        public void Execute()
        {
            while (true)
            {
                if (_commandPool.Count > 0)
                {
                    Message msg = _commandPool.Dequeue();

                    if (msg.Command.Id == Command.TEST)
                        continue;

                    string identifier = ((Command)msg.Command.Id).ToString();
                    CallExecutor(String.Concat(identifier, "Executor"), msg);
                }

                Thread.Sleep(5);
            }
        }

        private static void CallExecutor(string executorName, Message msg)
        {
            _executors[executorName].Execute(msg);
        }

        /// <summary>
        /// инициализация обработчиков большой карты
        /// </summary>
        /// <param name="gameLogic"></param>
        /// <param name="queryManager"></param>
        private void InitializeBigMapExecutors(LogicEntryPoint gameLogic)
        {
            // обработчик запроса карты
            _executors.Add(ExecutorTypes.GetMapRequestExecutor.ToString(), new GetMapExecutor(gameLogic));

            // обработчик запроса перемещения короля
            _executors.Add(ExecutorTypes.MoveKingRequestExecutor.ToString(), new MoveKingExecutor(gameLogic));

            // получение объектов на карте
            _executors.Add(ExecutorTypes.GetObjectsRequestExecutor.ToString(), new GetObjectsExecutor(gameLogic));

            // вход в замок
            _executors.Add(ExecutorTypes.ComeInCastleRequestExecutor.ToString(), new ComeInCastleExecutor(gameLogic));

            // начать диалог с другим королем
            _executors.Add(ExecutorTypes.ContactKingRequestExecutor.ToString(), new ContactKingExecutor(gameLogic));

            // возврат на карту
            _executors.Add(ExecutorTypes.BigMapRequestExecutor.ToString(), new BigMapExecutor(gameLogic));

            // начать диалог с владельцем замка
            _executors.Add(ExecutorTypes.ContactCastleRequestExecutor.ToString(), new ContactCastleExecutor(gameLogic));

            // запросить захват замка
            _executors.Add(ExecutorTypes.CaptureCastleRequestExecutor.ToString(), new CaptureCastleExecutor(gameLogic));

        }

        /// <summary>
        /// инициализация обработчиков управления империей
        /// </summary>
        /// <param name="gameLogic"></param>
        /// <param name="queryManager"></param>
        private void InitializeEmpireExecutors(LogicEntryPoint gameLogic)
        {
            // отправка информации о союзе или империи
            _executors.Add(ExecutorTypes.GetAlianceInfoRequestExecutor.ToString(), new GetAlianceInfoExecutor(gameLogic));

            // отправка запроса об установлении налога
            _executors.Add(ExecutorTypes.EmbedTaxRateRequestExecutor.ToString(), new EmbedTaxRateExecutor(gameLogic));

            // отправка запроса об исключении короля из империи
            _executors.Add(ExecutorTypes.ExcludeKingFromEmpireRequestExecutor.ToString(), new ExcludeKingFromEmpireExecutor(gameLogic));

            // отправка запроса помощи фигурами
            _executors.Add(ExecutorTypes.GetHelpFigureRequestExecutor.ToString(), new GetHelpFigureExecutor(gameLogic));

            // отправка запроса помощи ресурсами
            _executors.Add(ExecutorTypes.GetHelpResourceRequestExecutor.ToString(), new GetHelpResourceExecutor(gameLogic));

            // отправка запроса о включении короля в империю
            _executors.Add(ExecutorTypes.IncludeKingInEmpireRequestExecutor.ToString(), new IncludeKingInEmpireExecutor(gameLogic));

            // отправка запроса присоединения к империи или союзу
            _executors.Add(ExecutorTypes.JoinToAlianceRequestExecutor.ToString(), new JoinToAlianceExecutor(gameLogic));

            // обработчик для пересылка фигур между игроками
            _executors.Add(ExecutorTypes.SendFigureHelpMessageExecutor.ToString(), new SendFigureHelpExecutor(gameLogic));

            // обработчик для пересылка ресурсов между игроками
            _executors.Add(ExecutorTypes.SendResourceHelpMessageExecutor.ToString(), new SendResourceHelpExecutor(gameLogic));

            // отправка запроса начала импичмента
            _executors.Add(ExecutorTypes.StartImpeachmentRequestExecutor.ToString(), new StartImpeachmentExecutor(gameLogic));

            // отправка запроса начала голосования
            _executors.Add(ExecutorTypes.StartVoteRequestExecutor.ToString(), new StartVoteExecutor(gameLogic));

            // получение информации о всех союзах и империях на уровне
            _executors.Add(ExecutorTypes.GetAliancesInfoRequestExecutor.ToString(), new GetAliancesInfoExecutor(gameLogic));

            // обработка сообщения голосования
            _executors.Add(ExecutorTypes.VoteBallotMessageExecutor.ToString(), new VoteFactExecutor(gameLogic));

            // обработка запроса о выходе из союза
            _executors.Add(ExecutorTypes.ExitFromAlianceRequestExecutor.ToString(), new ExitFromAlianceExecutor(gameLogic));

            // обработка запроса начала переговоров между лидерами
            _executors.Add(ExecutorTypes.StartNegotiateRequestExecutor.ToString(), new StartNegotiateExecutor(gameLogic));
        }

        /// <summary>
        /// инициализация обработчиков системных диалогов
        /// </summary>
        /// <param name="gameLogic"></param>
        /// <param name="queryManager"></param>
        private void InitializeDialogExecutors(LogicEntryPoint gameLogic, ProtoBufferTransport transport)
        {
            // обработчик для маршрутизации сообщений диалога битвы
            _executors.Add(ExecutorTypes.BattleDialogMessageExecutor.ToString(), new MessageBattleExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога торговли
            _executors.Add(ExecutorTypes.MarketDialogMessageExecutor.ToString(), new MessageMarketExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога капитуляции
            _executors.Add(ExecutorTypes.CapitulateDialogMessageExecutor.ToString(), new MessageCapitulateExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога откупа
            _executors.Add(ExecutorTypes.PayOffDialogMessageExecutor.ToString(), new MessagePayOffExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога создания союза
            _executors.Add(ExecutorTypes.CreateUnionDialogMessageExecutor.ToString(), new MessageCreateUnionExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога захвата замка
            _executors.Add(ExecutorTypes.CaptureCastleDialogMessageExecutor.ToString(), new MessageCastleExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений начала войны между империями
            _executors.Add(ExecutorTypes.WarDialogMessageExecutor.ToString(), new MessageWarExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога заключения мира между империями
            _executors.Add(ExecutorTypes.PeaceDialogMessageExecutor.ToString(), new MessagePeaceExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога объединения империй
            _executors.Add(ExecutorTypes.JoinEmperiesDialogMessageExecutor.ToString(), new MessageJoinEmperiesExecutor(gameLogic, transport));
        }
    }
}
