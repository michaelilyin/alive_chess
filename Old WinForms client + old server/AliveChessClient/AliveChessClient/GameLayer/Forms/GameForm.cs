using System;
using System.Windows.Forms;
using AliveChessClient.GameLayer.Controls;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;

namespace AliveChessClient.GameLayer.Forms
{
    public partial class GameForm : Form
    {
        private BigMapControl bigMapControl;
        private CastleControl castleControl;
        private CastleInfoControl castleInfoControl;
        private KingInfoControl startControl;

        private MainDisputeControl mainDisputeControl;
        private SimpleDisputeControl simpleDisputeControl;
        private BattleDisputeControl battleDisputeControl;
        private BattleControl batleControl;
        private AlianceControl alianceControl;
        private LeaderControl leaderControl;
        private NegotiateControl negotiateControl;

        private Control currentControl;
      
        public GameForm(Game context)
        {
            InitializeComponent();

            this.bigMapControl = new BigMapControl(context);
            Controls.Add(bigMapControl);
            this.mainDisputeControl = new MainDisputeControl(context);
            Controls.Add(mainDisputeControl);
            this.castleControl = new CastleControl(context);
            Controls.Add(castleControl);
            this.simpleDisputeControl = new SimpleDisputeControl(context);
            Controls.Add(simpleDisputeControl);
            this.battleDisputeControl = new BattleDisputeControl(context);
            Controls.Add(battleDisputeControl);
            this.castleInfoControl = new CastleInfoControl(context);
            Controls.Add(castleInfoControl);
            this.startControl = new KingInfoControl();
            Controls.Add(startControl);
            this.batleControl = new BattleControl(context);
            Controls.Add(batleControl);
            this.alianceControl = new AlianceControl(context);
            Controls.Add(alianceControl);
            this.leaderControl = new LeaderControl(context);
            Controls.Add(leaderControl);
            this.negotiateControl = new NegotiateControl(context);
            Controls.Add(negotiateControl);

            startControl.Hide();
            bigMapControl.Hide();
            castleControl.Hide();
            castleInfoControl.Hide();
            mainDisputeControl.Hide();
            simpleDisputeControl.Hide();
            battleDisputeControl.Hide();
            batleControl.Hide();
            alianceControl.Hide();
            leaderControl.Hide();
            negotiateControl.Hide();

            currentControl = bigMapControl;
        }

        public void StartBigMap()
        {
            currentControl.Hide();
            currentControl = bigMapControl;
            bigMapControl.Show();
        }
        
        public void StartBatle()
        {
            currentControl.Hide();
            currentControl = batleControl;
            batleControl.Show();//buildControl.Show();
        }
        
        public void StartMainDialog()
        {
            currentControl.Hide();
            mainDisputeControl.Initialize();
            currentControl = mainDisputeControl;
            mainDisputeControl.Show();
        }

        public void StartNegotiateDialog()
        {
            currentControl.Hide();
            negotiateControl.Initialize();
            currentControl = negotiateControl;
            negotiateControl.Show();
        }

        public void StartAlianceDialog()
        {
            currentControl.Hide();
            alianceControl.Initialize();
            currentControl = alianceControl;
            alianceControl.Show();
        }

        public void StartLeaderDialog()
        {
            currentControl.Hide();
            leaderControl.Initialize();
            currentControl = leaderControl;
            leaderControl.Show();
        }

        public void StartBattleDialog()
        {
            currentControl.Hide();
            battleDisputeControl.Initialize();
            currentControl = battleDisputeControl;
            battleDisputeControl.Show();
        }

        public void StartSimpleTradeDialog()
        {
            currentControl.Hide();
            simpleDisputeControl.InitializeTrade();
            currentControl = simpleDisputeControl;
            simpleDisputeControl.Show();
        }

        public void StartSimplePayOffDialog()
        {
            currentControl.Hide();
            simpleDisputeControl.InitializePayOff();
            currentControl = simpleDisputeControl;
            simpleDisputeControl.Show();
        }

        public void StartSimpleJoiningDialog()
        {
            currentControl.Hide();
            simpleDisputeControl.InitializeJoinEmperies();
            currentControl = simpleDisputeControl;
            simpleDisputeControl.Show();
        }

        public void StartEmpireDialog()
        {
            currentControl.Hide();
            simpleDisputeControl.InitializeEmpire();
            currentControl = simpleDisputeControl;
            simpleDisputeControl.Show();
        }

        public void StartSimpleCapitulateDialog()
        {
            currentControl.Hide();
            simpleDisputeControl.InitializeCapitulate();
            currentControl = simpleDisputeControl;
            simpleDisputeControl.Show();
        }

        public void StartSimpleCaptureCastleDialog()
        {
            currentControl.Hide();
            simpleDisputeControl.InitializeCaptureCastle();
            currentControl = simpleDisputeControl;
            simpleDisputeControl.Show();
        }

        public void StartCastle(Castle castle)
        {
            currentControl.Hide();
            castleControl.DrawScene();
            currentControl = castleControl;
            castleControl.Show();
        }

        public void StartKingInfoDialog(King args)
        {
            currentControl.Hide();
            startControl.Initialize(args);
            currentControl = startControl;
            startControl.Show();
        }

        public void StartCastleInfoControl(Castle args)
        {
            currentControl.Hide();
            castleInfoControl.Initialize(args);
            currentControl = castleInfoControl;
            castleInfoControl.Show();
        }

        public void StartBattle(Battle battle)
        {
        }

        public BigMapControl BigMapControl
        {
            get { return bigMapControl; }
        }

        public CastleControl CastleControl
        {
            get { return castleControl; }
        }

        public MainDisputeControl MainDisputControl
        {
            get { return mainDisputeControl; }
        }

        public BattleDisputeControl BattleDisputeControl
        {
            get { return battleDisputeControl; }
        }

        public CastleInfoControl CastleInfoControl
        {
            get { return castleInfoControl; }
        }

        public NegotiateControl NegotiateControl
        {
            get { return negotiateControl; }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryManager.SendExit();
        }

        public BattleControl BattleControl
        {
            get { return batleControl; }
        }

        public AlianceControl AlianceControl
        {
            get { return alianceControl; }
        }

        public LeaderControl LeaderControl
        {
            get { return leaderControl; }
        }
    }
}
