using System;
using System.Collections.Generic;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.DialogCommand;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Landscapes;
using AliveChessLibrary.Interaction;
using AliveChessServer.DBLayer;
using AliveChessServer.LogicLayer.UsersManagement;

namespace AliveChessServer.LogicLayer.Environment
{
    public class DisputeRoutine : ITimeRoutine
    {
        private delegate void DisputeAction(King respondent, Dialog dispute);
        private delegate void NegotiateAction(King respondent, Negotiate negotiate);

        private Level _level;
        private PlayerManager _playerManager;
        private GameWorld _environment;
        private List<IDispute> _dialogs;
        private object _sync = new object();
        private Dictionary<DialogTheme, DisputeAction> _dialogActions;
        private Dictionary<NegotiateTheme, NegotiateAction> _negotiateActions;
        private TimeManager _timeManager;

        private TimeSpan _timeWithoutAnswer = TimeSpan.FromSeconds(5);

        public DisputeRoutine(Level level, TimeManager timeManager)
        {
            this._level = level;
            //this._environment = environment;
            this._dialogs = new List<IDispute>();
            this._timeManager = timeManager;
            this._dialogActions = new Dictionary<DialogTheme, DisputeAction>();
            this._negotiateActions = new Dictionary<NegotiateTheme, NegotiateAction>();

            #region Dialog Default Actions. Sent to reveiver

            // тема переговоров еще не определена, возвращаем короля на карту
            this._dialogActions.Add(DialogTheme.NoTheme, delegate(King king, Dialog dispute)
            {
                king.Interaction = null;
                king.State = KingState.BigMap;
                Remove(king.Player.Level, dispute);
                king.Player.Messenger.SendNetworkMessage(new BigMapResponse());
            });

            // король предлагает свою капитуляцию
            this._dialogActions.Add(DialogTheme.Battle, delegate(King king, Dialog dispute) 
            {
                dispute.State = DialogState.Offer;
                dispute.Theme = DialogTheme.Capitulation;
                king.Player.Messenger.SendNetworkMessage(new CapitulateDialogMessage(dispute.Id, dispute.State));
            });

            // король согласен на капитуляцию
            this._dialogActions.Add(DialogTheme.Capitulation, delegate(King king, Dialog dispute)
            {
                king.Interaction = null;
                dispute.State = DialogState.Agree;
                Remove(king.Player.Level, dispute);
                king.Player.Messenger.SendNetworkMessage(new CapitulateDialogMessage(dispute.Id, dispute.State));
            });

            // король согласен на откуп
            this._dialogActions.Add(DialogTheme.PayOff, delegate(King king, Dialog dispute)
            {
                king.Interaction = null;
                dispute.State = DialogState.Agree;
                Remove(king.Player.Level, dispute);
                king.Player.Messenger.SendNetworkMessage(new PayOffDialogMessage(dispute.Id, dispute.State));
            });

            // король отказывается создать союз
            this._dialogActions.Add(DialogTheme.CreateUnion, delegate(King king, Dialog dispute)
            {
                king.Interaction = null;
                dispute.State = DialogState.Refuse;
                Remove(king.Player.Level, dispute);
                king.Player.Messenger.SendNetworkMessage(new CreateUnionDialogMessage(dispute.Id, dispute.State));
            });

            // король отказывается защищать замок
            this._dialogActions.Add(DialogTheme.CaptureCastle, delegate(King king, Dialog dispute)
            {
                king.Interaction = null;
                Remove(king.Player.Level, dispute);
                dispute.State = DialogState.Refuse;
                king.Player.Messenger.SendNetworkMessage(new CaptureCastleDialogMessage(dispute.Id, dispute.State));
            });

            // король отказывается торговать
            this._dialogActions.Add(DialogTheme.Trade, delegate(King king, Dialog dispute)
            {
                king.Interaction = null;
                dispute.State = DialogState.Refuse;
                Remove(king.Player.Level, dispute);
                king.Player.Messenger.SendNetworkMessage(new MarketDialogMessage(dispute.Id, dispute.State));
            });

            #endregion

            #region Negotiate Default Actions. Sent to reveiver

            this._negotiateActions.Add(NegotiateTheme.War, delegate(King king, Negotiate negotiate)
            {
                king.Interaction = null;
                negotiate.State = DialogState.Refuse;
                Remove(king.Player.Level, negotiate);
                king.Player.Messenger.SendNetworkMessage(new WarDialogMessage(negotiate.Id, negotiate.State));
            });

            #endregion
        }

        public void Update(GameTime time)
        {
            if (time.Elapsed > TimeSpan.FromMilliseconds(1000))
            {
                foreach (IDispute d in NextDispute())
                {
                    if (d.Elapsed >= this._timeWithoutAnswer)
                    {
                        HandleDispute(d);
                        d.Elapsed = TimeSpan.Zero;
                    }
                }

                time.SavePreviousTimestamp();
            }
        }

        private IEnumerable<IDispute> NextDispute()
        {
            for (int i = 0; i < _dialogs.Count; i++)
                yield return _dialogs[i];
        }

        public void Add(ILevel level, IDispute dispute)
        {
            level.AddDispute(dispute);
            this._dialogs.Add(dispute);
        }

        public void Remove(ILevel level, IDispute dispute)
        {
            level.RemoveDispute(dispute);
            this._dialogs.Remove(dispute);
        }

        public King GetOpponent(IDispute dispute, King player)
        {
            return player.Id == dispute.Organizator.Id ? dispute.Respondent : dispute.Organizator;
        }

        public Dialog CreateDispute(King first, King second, bool step)
        {
            Dialog dispute = new Dialog();
            dispute.YouStep = step;
            dispute.Id = GuidGenerator.Instance.GetInt();
            dispute.Organizator = first;
            dispute.Respondent = second;
            dispute.State = DialogState.NoState;
            dispute.Theme = DialogTheme.NoTheme;
            return dispute;
        }

        public Negotiate CreateNegotiate(King first, King second, bool step)
        {
            Negotiate negotiate = new Negotiate();
            negotiate.YouStep = step;
            negotiate.Id = GuidGenerator.Instance.GetInt();
            negotiate.Organizator = first;
            negotiate.Respondent = second;
            negotiate.Theme = NegotiateTheme.No;
            return negotiate;
        }

        public IDispute GetDisputeById(int id)
        {
            return _dialogs.Find(x => x.Id == id);
        }

        public bool CanStartDialog(King respondent)
        {
            return respondent.State == KingState.BigMap && !respondent.Sleep;
        }

        public void TerminateDispute(IDispute dispute)
        {
            dispute.Respondent.Interaction = null;
            dispute.Organizator.Interaction = null;

            // удаляем переговоры
            Remove(dispute.Respondent.Player.Level, dispute);

            // отменяем переговоры
            if (!dispute.Respondent.Player.Bot)
                dispute.Respondent.Player.Messenger.SendNetworkMessage(new BigMapResponse());
            //else dispute.Respondent.Player.Messenger.SendAIMessage(null);
            if (!dispute.Organizator.Player.Bot)
                dispute.Organizator.Player.Messenger.SendNetworkMessage(new BigMapResponse());
            //else dispute.Organizator.Player.Messenger.SendAIMessage(null);

            // возвращаем короля на карту
            dispute.Respondent.State = KingState.BigMap;
            dispute.Organizator.State = KingState.BigMap;

        }

        private void HandleDispute(IDispute dispute)
        {
            if (dispute.InteractionType == InteractionType.Dialog)
                HandleDispute(dispute as Dialog);
            else if (dispute.InteractionType == InteractionType.Negotiate)
                HandleDispute(dispute as Negotiate);
        }

        private void HandleDispute(Dialog dispute)
        {
            DialogTheme theme = dispute.Theme;

            King k_respondent = dispute.YouStep ? dispute.Respondent : dispute.Organizator;
            King k_organizator = dispute.YouStep ? dispute.Organizator : dispute.Respondent;
           
            if (theme != DialogTheme.NoTheme)
            {
                // оключаем возможность посылать сообщения
                if (!k_organizator.Player.Bot)
                    k_organizator.Player.Messenger.SendNetworkMessage(new DeactivateDialogMessage());
                //else asking.Messenger.SendAIMessage(new DeactivateDialogMessage());
            }
            else // send message to sender
            {
                dispute.Respondent.Interaction = null;
                dispute.Organizator.Interaction = null;

                // удаляем переговоры
                Remove(k_organizator.Player.Level, dispute);

                // отменяем переговоры
                if (!k_organizator.Player.Bot)
                    k_organizator.Player.Messenger.SendNetworkMessage(new BigMapResponse());
               // else 

                // возвращаем короля на карту
                if(dispute.YouStep) 
                    dispute.Organizator.State = KingState.BigMap;
                else dispute.Respondent.State = KingState.BigMap;
            }

            dispute.YouStep = !dispute.YouStep;

            // посылаем ответ собеседнику
            if (!k_respondent.Player.Bot)
                _dialogActions[theme].Invoke(k_respondent, dispute);
        }

        private void HandleDispute(Negotiate negotiate)
        {
            NegotiateTheme theme = negotiate.Theme;

            IPlayer respondent = negotiate.Respondent.Player;
            IPlayer organizator = negotiate.Organizator.Player;

            //PlayerInfo organizator = _playerManager.GetPlayerInfoById(negotiate.Organizator.Id);
            //PlayerInfo respondent = _playerManager.GetPlayerInfoById(negotiate.Respondent.Id);

            King k_respondent = negotiate.YouStep ? negotiate.Respondent : negotiate.Organizator;
            King k_organizator = negotiate.YouStep ? negotiate.Organizator : negotiate.Respondent;

            if (theme != NegotiateTheme.No)
            {
                // оключаем возможность посылать сообщения
                if (!k_organizator.Player.Bot)
                    k_organizator.Player.Messenger.SendNetworkMessage(new DeactivateDialogMessage());
                else
                {
                    //
                }
            }
            else
            {
                negotiate.Respondent.Interaction = null;
                negotiate.Organizator.Interaction = null;

                // удаляем переговоры
                Remove(k_organizator.Player.Level, negotiate);

                // отменяем переговоры
                if (!k_organizator.Player.Bot)
                    k_organizator.Player.Messenger.SendNetworkMessage(new BigMapResponse());

                // возвращаем короля на карту
                if (negotiate.YouStep)
                    negotiate.Organizator.State = KingState.BigMap;
                else negotiate.Respondent.State = KingState.BigMap;
            }

            negotiate.YouStep = !negotiate.YouStep;

            // посылаем ответ собеседнику
            if (!k_respondent.Player.Bot)
                _negotiateActions[theme].Invoke(k_respondent, negotiate);
        }

        public PlayerManager PlayerManager
        {
            get { return _playerManager; }
            set { _playerManager = value; }
        }
    }
}
