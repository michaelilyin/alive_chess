using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using AliveChess.GameLayer.PresentationLayer;
using AliveChessLibrary.Commands.CastleCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace AliveChess.GameLayer.LogicLayer
{
    public class CastleCommandController
    {
        private GameCore _gameCore;

        private Castle _castle;

        public Castle Castle
        {
            get { return _castle; }
            set { _castle = value; }
        }

        private CastleScene _castleScene;

        public CastleScene CastleScene
        {
            get { return _castleScene; }
            set { _castleScene = value; }
        }

        public CastleCommandController(GameCore gameCore)
        {
            _gameCore = gameCore;
        }

        public void SendGetListBuildingsInCastleRequest()
        {
            GetListBuildingsInCastleRequest request = new GetListBuildingsInCastleRequest();
            _gameCore.Network.Send(request);
        }

        public void ReceiveGetListBuildingsInCastleResponce(GetListBuildingsInCastleResponse response)
        {
            IList<InnerBuilding> buildings = (response.List);
            _castleScene.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(_castleScene.ShowGetListBuildingsInCastleResult));
            _castle.InnerBuildings = CustomConverter.ListToEntitySet(response.List);
            foreach (var innerBuilding in _castle.InnerBuildings)
            {
                System.Windows.MessageBox.Show(innerBuilding.InnerBuildingType.ToString());
            }
        }
    }
}
