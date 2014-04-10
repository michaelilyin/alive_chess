using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using Assets.CommonScripts.Utils;
using GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BigMap.Scripts.MapGUI
{
    class CastleGUI : MonoBehaviour
    {
        private Castle _castle;

        private Rect _buildingsWindow;
        private Rect _troopsWindow;
        private Rect _tooltip;

        private bool _active;

        void Start()
        {
            _active = false;
            _buildingsWindow = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 200, 200, 400);
            _troopsWindow = new Rect(Screen.width / 2 + 50, Screen.height / 2 - 200, 200, 400);
            _tooltip = new Rect(Screen.width - 150, 0, 150, 150);
        }

        public void Show()
        {
            _active = true;
            _castle = GameCore.Instance.World.Player.King.CurrentCastle;
            GameCore.Instance.Network.CastleCommandController.StartCastleUpdate();
        }

        public void Hide()
        {
            _active = false;
            GameCore.Instance.Network.CastleCommandController.StopCastleUpdate();
        }

        void Update()
        {
            if (GameCore.Instance.World.Player.KingInKastle)
            {
                if (!_active)
                    Show();
            }
            else
            {
                if (_active)
                    Hide();
            }
        }

        void OnGUI()
        {
            if (_active)
            {
                _buildingsWindow = GUILayout.Window((int)GUIIdentifers.BuildingsWindow, _buildingsWindow, BuildingsWindow, "Buildings");
                _troopsWindow = GUILayout.Window((int)GUIIdentifers.TroopsWindow, _troopsWindow, TroopsWindow, "Troops");
            }
        }

        private void ProcessBuildingControl(InnerBuildingType type)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(NamesConverter.GetNameByType(type));
            CreationRequirements reqirements = _castle.BuildingManager.GetCreationRequirements(type);
            string req;
            if (reqirements == null)
                req = "Calculating requirements";
            else
                req = reqirements.TextView();
            if (_castle.HasBuilding(type))
            {
                if (GUILayout.Button("Destroy"))
                {
                    GameCore.Instance.Network.CastleCommandController.SendDestroyBuildingRequest(type);
                }
            }
            else if (_castle.BuildingManager.HasInQueue(type))
            {
                if (GUILayout.Button("Cancel"))
                {
                    GameCore.Instance.Network.CastleCommandController.SendDestroyBuildingRequest(type);
                }
            }
            else
            {
                if (reqirements != null && reqirements.RequiredBuildings.All(building => _castle.HasBuilding(building))
                    && GameCore.Instance.World.Player.King.ResourceStore.HasEnoughResources(reqirements.Resources))
                {
                    if (GUILayout.Button(new GUIContent("Create", req)))
                    {
                        GameCore.Instance.Network.CastleCommandController.SendCreateBuildingRequest(type);
                    }
                }
                else
                {
                    GUILayout.Button(new GUIContent("Forbidden", req));
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ProcessUnitControl(UnitType type)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("{0}: {1}", NamesConverter.GetNameByType(type), _castle.Army.GetUnitQuantity(type)));
            if (_castle.KingInside)
            {
                if (_castle.Army.HasUnits(type))
                {
                    if (GUILayout.Button("Get"))
                    {

                    }
                }
                if (GameCore.Instance.World.Player.King.Army.HasUnits(type))
                {
                    if (GUILayout.Button("Leave"))
                    {

                    }
                }
            }
            else
            {
                GUILayout.Label("King is not in the castle");
            }
            GUILayout.EndHorizontal();
        }

        private void BuildingsWindow(int id)
        {
            GUILayout.BeginVertical();
            ProcessBuildingControl(InnerBuildingType.Quarters);
            ProcessBuildingControl(InnerBuildingType.TrainingGround);
            ProcessBuildingControl(InnerBuildingType.Hospital);
            ProcessBuildingControl(InnerBuildingType.Forge);
            ProcessBuildingControl(InnerBuildingType.Fortress);
            ProcessBuildingControl(InnerBuildingType.Stabling);
            ProcessBuildingControl(InnerBuildingType.Workshop);
            GUILayout.BeginHorizontal();
            GUILayout.Label(GUI.tooltip);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void TroopsWindow(int id)
        {
            GUILayout.BeginVertical();
            ProcessUnitControl(UnitType.Bishop);
            ProcessUnitControl(UnitType.Knight);
            ProcessUnitControl(UnitType.Pawn);
            ProcessUnitControl(UnitType.Queen);
            ProcessUnitControl(UnitType.Rook);
            GUILayout.EndVertical();

            if (GUILayout.Button("Exit"))
            {
                GameCore.Instance.Network.CastleCommandController.SendLeaveCastleRequest();
            }
        }


    }
}
