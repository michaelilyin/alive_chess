using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using AliveChessLibrary.Entities;
using AliveChessLibrary.Entities.Characters;
using AliveChessLibrary.Entities.Landscapes;
using AliveChessLibrary.Entities.Abstract;
using AliveChessClient.GameLayer.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessClient.GameLayer.AliveChessGraphics;
using AliveChessLibrary.Entities.Resources;
using AliveChessClient.GameLayer.Controls;

namespace AliveChessClient.GameLayer
{
    /// <summary>
    /// Base game class. Contains all game's objects, 
    /// game scenes (controls) and realize actions of player
    /// </summary>
    public class GameContext
    {
        private GameForm gameForm;
        private StartForm startForm;

        private Player player;

        private Map map;
        private Battle battle;
        private Dispute dispute;

        public GameContext()
        {
            this.player = new Player();
           
            this.gameForm = new GameForm(this);
            this.startForm = new StartForm(this);
        }

        public void Run()
        {
            this.BigMap.Initialize();
            this.GameForm.StartBigMap();
            this.GameForm.ShowDialog();
        }

        public void Stop()
        {
            //
        }


        public Map Map
        {
            get { return map; }
            set { map = value; }
        }

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public Battle Battle
        {
            get { return battle; }
            set { battle = value; }
        }

        public Dispute Dispute
        {
            get { return dispute; }
            set { dispute = value; }
        }

        public GameForm GameForm
        {
            get { return gameForm; }
        }

        public StartForm StartForm
        {
            get { return startForm; }
        }

        public BigMapControl BigMap
        {
            get { return gameForm.BigMapControl; }
        }

        public MainDisputeControl DisputeControl
        {
            get { return gameForm.MainDisputControl; }
        }

        public CastleControl CastleControl
        {
            get { return gameForm.CastleControl; }
        }
    }
}
