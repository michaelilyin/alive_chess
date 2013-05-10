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
using AliveChessServer.LogicLayer.RequestExecutors.StatisticExecutors;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer
{
    public class MainExecutor
    {
        private CommandPool _commandPool;
        private AliveChessLogger _logger;
        private ProtoBufferTransport _transport;
        private static IDictionary<Command, IExecutor> _executors;

        public MainExecutor(CommandPool commands, ProtoBufferTransport transport,
            AliveChessLogger logger, GameLogic gameLogic)
        {
            this._logger = logger;
            this._commandPool = commands;
            this._transport = transport;
         
            Debug.Assert(gameLogic.Environment != null);
            Debug.Assert(gameLogic.PlayerManager != null);

            _executors = new Dictionary<Command, IExecutor>();
            //
            _executors.Add(Command.DownloadBattlefildRequest, new DownloadBattlefildExecutor(gameLogic));

            _executors.Add(Command.MoveUnitRequest, new PlayerMoveRequestExecutor(gameLogic));

            CreateAuthorizeExecutors(gameLogic, transport);
            CreateBigMapExecutors(gameLogic);
            CreateDialogExecutors(gameLogic, transport);
            CreateEmpireExecutors(gameLogic);
            CreateCastleExecutors(gameLogic);
            CreateStatisticExecutors(gameLogic);
        }

        public void Execute()
        {
            while (true)
            {
                if (_commandPool.Count > 0)
                {
                    Message msg = _commandPool.Dequeue();

                    //if (msg.Command.Id == Command.TEST)
                    //    continue;

                    CallExecutor(msg.Command.Id, msg);
                }

                Thread.Sleep(5);
            }
        }

        private static void CallExecutor(Command type, Message msg)
        {/*
#if DEBUG
            DebugConsole.WriteLine("MainExecutor", "Executor <" + executorName + "> Command: " + msg.Command.GetType().Name);
#endif*/
            _executors[type].Execute(msg);
        }

        private void CreateBigMapExecutors(GameLogic gameLogic)
        {
            // обработчик запроса карты
            _executors.Add(Command.GetMapRequest, new GetMapExecutor());

            // обработчик запроса перемещения короля
            _executors.Add(Command.MoveKingRequest, new MoveKingExecutor());

            // получение объектов на карте
            _executors.Add(Command.GetObjectsRequest, new GetObjectsExecutor(gameLogic));

            // вход в замок
            _executors.Add(Command.ComeInCastleRequest, new ComeInCastleExecutor());

            // начать диалог с другим королем
            _executors.Add(Command.ContactKingRequest, new ContactKingExecutor());

            // возврат на карту
            _executors.Add(Command.BigMapRequest, new BigMapExecutor());

            // начать диалог с владельцем замка
            _executors.Add(Command.ContactCastleRequest, new ContactCastleExecutor());

            // запросить захват замка
            _executors.Add(Command.CaptureCastleRequest, new CaptureCastleExecutor());

            // получение карты юнити
            _executors.Add(Command.GetUnityMapRequest, new GetUnityMapExecutor());

            // получение начального состояния игры
            _executors.Add(Command.GetGameStateRequest, new GetGameStateExecutor(_logger));

            // проверка пути
            _executors.Add(Command.VerifyPathRequest, new VerifyPathExecutor(_logger, gameLogic.PlayerManager));

            // получение короля по идентификатору
            _executors.Add(Command.GetKingRequest, new GetKingExecutor());

            // запросить захват шахты
            _executors.Add(Command.CaptureMineRequest, new CaptureMineExecutor(gameLogic));
        }

        private void CreateCastleExecutors(GameLogic gameLogic)
        {

            // выход из замка
            _executors.Add(Command.LeaveCastleRequest, new LeaveCastleExecutor(gameLogic));
            //
            _executors.Add(Command.GetBuildingCostRequest, new GetBuildingCostExecutor(gameLogic));
            //
            _executors.Add(Command.CreateBuildingRequest, new CreateBuildingExecutor(gameLogic));
            //
            _executors.Add(Command.CreateUnitRequest, new CreateUnitExecutor(gameLogic));
            //
            _executors.Add(Command.CollectUnitsRequest, new CollectUnitsExequtor(gameLogic));
            //
            _executors.Add(Command.GetBuildingsRequest, new GetBuildingsExecutor(gameLogic));
            //
            _executors.Add(Command.GetCastleArmyRequest, new GetCastleArmyExecutor(gameLogic));
            //
            _executors.Add(Command.GetKingArmyRequest, new GetKingArmyExecutor(gameLogic));
            _executors.Add(Command.DestroyBuildingRequest, new DestroyBuildingExecutor(gameLogic));
            _executors.Add(Command.GetBuildingQueueRequest, new GetBuildingQueueExecutor(gameLogic));
            
        }

        private void CreateEmpireExecutors(GameLogic gameLogic)
        {
            // отправка информации о союзе или империи
            _executors.Add(Command.GetAlianceInfoRequest, new GetAlianceInfoExecutor(gameLogic));

            // отправка запроса об установлении налога
            _executors.Add(Command.EmbedTaxRateRequest, new EmbedTaxRateExecutor(gameLogic));

            // отправка запроса об исключении короля из империи
            _executors.Add(Command.ExcludeKingFromEmpireRequest, new ExcludeKingFromEmpireExecutor(gameLogic));

            // отправка запроса помощи фигурами
            _executors.Add(Command.GetHelpFigureRequest, new GetHelpFigureExecutor(gameLogic));

            // отправка запроса помощи ресурсами
            _executors.Add(Command.GetHelpResourceRequest, new GetHelpResourceExecutor(gameLogic));

            // отправка запроса о включении короля в империю
            _executors.Add(Command.IncludeKingInEmpireRequest, new IncludeKingInEmpireExecutor(gameLogic));

            // отправка запроса присоединения к империи или союзу
            _executors.Add(Command.JoinToAlianceRequest, new JoinToAlianceExecutor(gameLogic));

            // обработчик для пересылка фигур между игроками
            _executors.Add(Command.SendFigureHelpMessage, new SendFigureHelpExecutor(gameLogic));

            // обработчик для пересылка ресурсов между игроками
            _executors.Add(Command.SendResourceHelpMessage, new SendResourceHelpExecutor(gameLogic));

            // отправка запроса начала импичмента
            _executors.Add(Command.StartImpeachmentRequest, new StartImpeachmentExecutor(gameLogic));

            // отправка запроса начала голосования
            _executors.Add(Command.StartVoteRequest, new StartVoteExecutor(gameLogic));

            // получение информации о всех союзах и империях на уровне
            _executors.Add(Command.GetAliancesInfoRequest, new GetAliancesInfoExecutor(gameLogic));

            // обработка сообщения голосования
            _executors.Add(Command.VoteBallotMessage, new VoteFactExecutor(gameLogic));

            // обработка запроса о выходе из союза
            _executors.Add(Command.ExitFromAlianceRequest, new ExitFromAlianceExecutor(gameLogic));

            // обработка запроса начала переговоров между лидерами
            _executors.Add(Command.StartNegotiateRequest, new StartNegotiateExecutor(gameLogic));
        }

        private void CreateDialogExecutors(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            // обработчик для маршрутизации сообщений диалога битвы
            _executors.Add(Command.BattleDialogMessage, new MessageBattleExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога торговли
            _executors.Add(Command.MarketDialogMessage, new MessageMarketExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога капитуляции
            _executors.Add(Command.CapitulateDialogMessage, new MessageCapitulateExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога откупа
            _executors.Add(Command.PayOffDialogMessage, new MessagePayOffExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога создания союза
            _executors.Add(Command.CreateUnionDialogMessage, new MessageCreateUnionExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога захвата замка
            _executors.Add(Command.CaptureCastleDialogMessage, new MessageCastleExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений начала войны между империями
            _executors.Add(Command.WarDialogMessage, new MessageWarExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога заключения мира между империями
            _executors.Add(Command.PeaceDialogMessage, new MessagePeaceExecutor(gameLogic, transport));

            // обработчик для маршрутизации сообщений диалога объединения империй
            _executors.Add(Command.JoinEmperiesDialogMessage, new MessageJoinEmperiesExecutor(gameLogic, transport));
        }

        private void CreateStatisticExecutors(GameLogic gameLogic)
        {
            // получение статистики
            _executors.Add(Command.GetStatisticRequest, new GetStatisticExecutor(gameLogic));

            // получение доступных карт
            _executors.Add(Command.GetAvailableMapsRequest, new GetAvailableMapsExecutor(gameLogic));
        }

        private void CreateAuthorizeExecutors(GameLogic gameLogic, ProtoBufferTransport transport)
        {
            // обработчик авторизации пользователей
            _executors.Add(Command.AuthorizeRequest, new AuthorizeExecutor(gameLogic, transport));

            // выйти из игры
            _executors.Add(Command.ExitFromGameRequest, new ExitFromGameExecutor(gameLogic, transport));

            // обработчик регистрации пользователей
            _executors.Add(Command.RegisterRequest, new RegisterExecutor(gameLogic, transport));
        }
    }
}
