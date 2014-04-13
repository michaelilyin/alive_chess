using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BigMap.Scripts.MapGUI.Windows
{
    class TransferTroopsWindow
    {
        private bool _show;
        private Rect _transferTroopsWindow;

        private Castle _castle;
        private King _king;
        private bool _toCastle;
        private UnitType _unitType;

        private int currentCount;

        public TransferTroopsWindow()
        {
            _show = false;
            _transferTroopsWindow = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100);
        }

        public void Show(Castle castle, King king, UnitType type, bool toCastle)
        {
            _show = true;
            _castle = castle;
            _king = king;
            _toCastle = toCastle;
            _unitType = type;
            currentCount = 0;
        }

        public void Hide()
        {
            _show = false;
        }

        public void Draw()
        {
            if (_show)
            {
                _transferTroopsWindow = GUILayout.Window((int)GUIIdentifers.TransferWindow, _transferTroopsWindow, TransferTroops, "Transfer troops");
            }
        }

        private void TransferTroops(int id)
        {
            int max = _toCastle ? _king.Army.GetUnitQuantity(_unitType) : _castle.Army.GetUnitQuantity(_unitType);
            GUILayout.BeginVertical();
            GUILayout.Label(currentCount.ToString());
            GUILayout.BeginHorizontal();
            GUILayout.Label("0");
            currentCount = (int)GUILayout.HorizontalSlider((float)currentCount, (float)0, (float)max);
            GUILayout.Label(max.ToString());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("5"))
                currentCount = 5 > max ? max : 5;
            if (GUILayout.Button("10"))
                currentCount = 10 > max ? max : 10;
            if (GUILayout.Button("20"))
                currentCount = 20 > max ? max : 20;
            if (GUILayout.Button("50"))
                currentCount = 50 > max ? max : 50;
            if (GUILayout.Button("100"))
                currentCount = 100 > max ? max : 100;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Transfer"))
            {
                Dictionary<UnitType, int> units = new Dictionary<UnitType, int>();
                units[_unitType] = currentCount;
                if (_toCastle)
                {
                    GameCore.Instance.Network.CastleCommandController.SendLeaveUnitsRequest(units);
                }
                else
                {
                    GameCore.Instance.Network.CastleCommandController.SendCollectUnitsRequest(units);
                }
                _show = false;
            }
            if (GUILayout.Button("Cancel"))
            {
                _show = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
