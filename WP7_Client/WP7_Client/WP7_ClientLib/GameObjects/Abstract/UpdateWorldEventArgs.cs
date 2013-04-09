using System;
using AliveChessLibrary.GameObjects.Landscapes;

namespace AliveChessLibrary.GameObjects.Abstract
{
    public class UpdateWorldEventArgs : EventArgs
    {
        private Map _map;
        private FPosition _location;
        private UpdateType _updateType;

        public UpdateWorldEventArgs(Map map, FPosition location, UpdateType updateType)
        {
            this._map = map;
            this._location = location;
            this._updateType = updateType;
        }

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public FPosition Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public UpdateType UpdateType
        {
            get { return _updateType; }
            set { _updateType = value; }
        }
    }
}
