using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.GameObjects.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameModel.ResponseHandlers
{
    class ComeInCastleResponseHandler : Network.CommandHandler
    {
        public override void Handle(AliveChessLibrary.Commands.ICommand command)
        {
            ComeInCastleResponse responce = (ComeInCastleResponse)command;
            Castle castle = GameCore.Instance.World.Map.SearchCastleById(responce.CastleId);
            if (castle != null)
            {
                GameCore.Instance.World.Player.King.ComeInCastle(castle);
                GameCore.Instance.World.Player.KingInKastle = true;
                Logger.Log.Debug("Enter in caslte");
            }
        }
    }
}
