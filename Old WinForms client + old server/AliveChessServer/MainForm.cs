using System;
using System.Windows.Forms;
using AliveChessLibrary.GameObjects;
using AliveChessServer.LogicLayer;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer
{
    public partial class MainForm : Form
    {
        private CommandPool _commands;
        private GameWorld _environment;
        private PlayerManager _playerManager;
        private AliveChessLogger _logger;
        private ServerApplication _serverAppl;
        private LogicEntryPoint _logicEntryPoint;
        private GameData _context;

        public MainForm()
        {
            InitializeComponent();

            this._commands = new CommandPool();
            this._context = new GameData();
            this._logger = new AliveChessLogger(this);
            this._environment = new GameWorld(_context);
            this._playerManager = new PlayerManager(_environment);
            this._serverAppl = new ServerApplication(this, _playerManager, _commands, _logger);
            this._logicEntryPoint = new LogicEntryPoint(_environment, _commands, _serverAppl.Transport,
                _logger, _playerManager);
        }

        private void StartServer()
        {
            this._serverAppl.Start();
            this._logicEntryPoint.StartGame();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartServer();
            button1.Enabled = false;
        }

        public void AddChatMessage(string message)
        {
            listBox1.Items.Add(DateTime.Now.ToString() + ": " + message);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //_logicEntryPoint.Environment.DbLoader.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartServer();
            //this._logicEntryPoint.StartGame();
            button1.Enabled = false;
            button2.Enabled = false;
            AliveChessStarter starter = new AliveChessStarter();
            starter.InitializeBots(_environment, _playerManager);
        }
    }
}
