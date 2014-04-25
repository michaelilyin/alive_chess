using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using Assets.BigMap.Scripts.MapGUI.Windows;
using Assets.CommonScripts.Utils;
using GameModel;
using GameModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BigMap.Scripts.MapGUI
{
    class CastleGUI : MonoBehaviour
    {
        public GUISkin skin;

        public AudioSource OpenSound;
        public AudioSource ClickSound;
        public AudioSource ErrorSound;

        private Castle _castle;

        private Rect _buildingsWindow;
        private Rect _troopsWindow;
        private Rect _exitButton;
        private Rect _tooltip;


        private bool _active;
        private MessageWindow _messageWindow;
        private Vector2 _buildingQueueScroll;
        private Vector2 _troopsQueueScroll;

        private TransferTroopsWindow _transferTroopsWindow;

        void Start()
        {
            _buildingQueueScroll = Vector2.zero;
            _troopsQueueScroll = Vector2.zero;
            _active = false;
            _buildingsWindow = new Rect(Screen.width / 2 - 400, Screen.height / 2 - 200, 350, 450);
            _troopsWindow = new Rect(Screen.width / 2 + 50, Screen.height / 2 - 200, 400, 400);
            _tooltip = new Rect(Screen.width - 150, 0, 150, 150);
            _exitButton = new Rect(Screen.width / 2 - 50, Screen.height / 2 + 250, 100, 30);
            _messageWindow = GetComponent<MessageWindow>();
            _transferTroopsWindow = new TransferTroopsWindow();
            _transferTroopsWindow.ClickSound = ClickSound;
            _transferTroopsWindow.OpenSound = OpenSound;
        }

        public void Show()
        {
            OpenSound.Play();
            _active = true;
            _castle = GameCore.Instance.World.Player.King.CurrentCastle;
            GameCore.Instance.Network.CastleCommandController.StartCastleUpdate();
            if (!_castle.KingInside)
                _messageWindow.AddMessage("King is not in the castle");
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
                GUI.skin = skin;
                _buildingsWindow = GUILayout.Window((int)GUIIdentifers.BuildingsWindow, _buildingsWindow, BuildingsWindow, "Buildings");
                _troopsWindow = GUILayout.Window((int)GUIIdentifers.TroopsWindow, _troopsWindow, TroopsWindow, "Troops");
                if (GUI.Button(_exitButton, "Exit"))
                {
                    ClickSound.Play();
                    GameCore.Instance.Network.CastleCommandController.SendLeaveCastleRequest();
                }
                _transferTroopsWindow.Draw();
            }
        }

        private void ProcessBuildingControl(InnerBuildingType type)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(NamesConverter.GetNameByType(type));
            CreationRequirements reqirements = _castle.BuildingManager.GetCreationRequirements(type);
            if (_castle.HasBuilding(type))
            {
                if (GUILayout.Button("Destroy"))
                {
                    GameCore.Instance.Network.CastleCommandController.SendDestroyBuildingRequest(type);
                    ClickSound.Play();
                }
            }
            else if (_castle.BuildingManager.HasInQueue(type))
            {
                if (GUILayout.Button("Cancel"))
                {
                    GameCore.Instance.Network.CastleCommandController.SendDestroyBuildingRequest(type);
                    ClickSound.Play();
                }
            }
            else
            {
                if (GUILayout.Button(new GUIContent("Create")))
                {
                    if (reqirements != null && reqirements.RequiredBuildings.All(building => _castle.HasBuilding(building))
                        && GameCore.Instance.World.Player.King.ResourceStore.HasEnoughResources(reqirements.Resources))
                    {
                        GameCore.Instance.Network.CastleCommandController.SendCreateBuildingRequest(type);
                        ClickSound.Play();
                    }
                    else
                    {
                        ErrorSound.Play();
                        string req;
                        if (reqirements == null)
                            req = "Calculating requirements";
                        else
                            req = reqirements.TextView();
                        _messageWindow.AddMessage(req);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ProcessUnitControl(UnitType type)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("{0}: {1}", NamesConverter.GetNameByType(type), _castle.Army.GetUnitQuantity(type)));
            CreationRequirements reqirements = _castle.RecruitingManager.GetCreationRequirements(type);
            if (GUILayout.Button("+"))
            {
                if (reqirements != null && reqirements.RequiredBuildings.All(building => _castle.HasBuilding(building))
                    && GameCore.Instance.World.Player.King.ResourceStore.HasEnoughResources(reqirements.Resources))
                {
                    ClickSound.Play();
                    GameCore.Instance.Network.CastleCommandController.SendCreateUnitRequest(type);
                }
                else
                {
                    ErrorSound.Play();
                    string req;
                    if (reqirements == null)
                        req = "Calculating requirements";
                    else
                        req = reqirements.TextView();
                    _messageWindow.AddMessage(req);
                }
            }
            if (_castle.RecruitingManager.HasInQueue(type))
                if (GUILayout.Button("x"))
                {
                    ClickSound.Play();
                    GameCore.Instance.Network.CastleCommandController.SendCancelUnitRecruitingRequest(type);
                }
            if (_castle.KingInside)
            {
                if (GUILayout.Button("<"))
                {
                    if (GameCore.Instance.World.Player.King.Army.HasUnits(type))
                    {
                        OpenSound.Play();
                        _transferTroopsWindow.Show(_castle, GameCore.Instance.World.Player.King, type, true);
                    }
                    else
                    {
                        ErrorSound.Play();
                        _messageWindow.AddMessage(String.Format("The king have not {0}", NamesConverter.GetNameByType(type)));
                    }
                }
                if (GUILayout.Button(">"))
                {
                    if (_castle.Army.HasUnits(type))
                    {
                        OpenSound.Play();
                        _transferTroopsWindow.Show(_castle, GameCore.Instance.World.Player.King, type, false);
                    }
                    else
                    {
                        ErrorSound.Play();
                        _messageWindow.AddMessage(String.Format("In the castle no {0}", NamesConverter.GetNameByType(type)));
                    }
                }
                GUILayout.Label(String.Format("King: {0}", GameCore.Instance.World.Player.King.Army.GetUnitQuantity(type)));
            }
            GUILayout.EndHorizontal();
        }

        private void BuildingsWindow(int id)
        {
            GUI.skin = skin;
            GUILayout.BeginVertical();
            ProcessBuildingControl(InnerBuildingType.Quarters);
            ProcessBuildingControl(InnerBuildingType.TrainingGround);
            ProcessBuildingControl(InnerBuildingType.Hospital);
            ProcessBuildingControl(InnerBuildingType.Forge);
            ProcessBuildingControl(InnerBuildingType.Fortress);
            ProcessBuildingControl(InnerBuildingType.Stabling);
            ProcessBuildingControl(InnerBuildingType.Workshop);
            _buildingQueueScroll = GUILayout.BeginScrollView(_buildingQueueScroll);
            foreach (var building in _castle.BuildingManager.GetProductionQueueCopy())
            {
                GUILayout.Label(String.Format("{0}:{1}%", NamesConverter.GetNameByType(building.Type), (int)building.TimeToPercent()));
            }
            GUILayout.EndScrollView();
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
            GUILayout.Label(GUI.tooltip);
            _troopsQueueScroll = GUILayout.BeginScrollView(_troopsQueueScroll);
            foreach (var unit in _castle.RecruitingManager.GetProductionQueueCopy())
            {
                GUILayout.Label(String.Format("{0}:{1}%", NamesConverter.GetNameByType(unit.Type), (int)unit.TimeToPercent()));
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();           
        }


    }
}
