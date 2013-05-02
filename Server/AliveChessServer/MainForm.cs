using System;
using System.Configuration;
using System.Windows.Forms;
using AliveChessPluginLibrary;
using AliveChessServer.LogicLayer;
using AliveChessServer.Properties;

namespace AliveChessServer
{
    public partial class MainForm : Form
    {
        private PluginPool _pluginPool;
        private AliveChessServerContext _server;
        private AliveChessPluginLoader _pluginLoader;
        private AliveChessLogger _logger;

        private IAliveChessPlugin _chatPlugin;

        private string _ipAddress;

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }
        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public MainForm()
        {
#if DEBUG
            AliveChessLibrary.DebugConsole.AllocConsole();
            AliveChessLibrary.DebugConsole.WriteLine(this, "HELLO");
#endif
            InitializeComponent();
        }

        //Запуск
        private void button1_Click(object sender, EventArgs e)
        {
            AliveChessStarter starter = new AliveChessStarter(_server.Logic, _server.Network);
            starter.Start();
            buttonStart.Enabled = false;
        }

        public void AddChatMessage(string message)
        {
            listBoxMessageLog.Items.Add(DateTime.Now + ": " + message);
        }

        //Загрузить чат
        private void button2_Click(object sender, EventArgs e)
        {
            _pluginLoader.RegisterChat(_server);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxDatabase.SelectedIndex = 0;
        }

        //Инициализация
        private void button3_Click(object sender, EventArgs e)
        {
            _ipAddress = textBoxIP.Text;

            switch (comboBoxDatabase.SelectedIndex)
            {
                case 0: // Connect to local cache
                    this._connectionString =
                        Settings.Default.Client_alive_chessConnectionString;
                    break;
                case 1: // Connect ro real database
                    this._connectionString =
                        ConfigurationManager.ConnectionStrings["AliveChess"].ConnectionString;
                    break;
            }

            _pluginPool = new PluginPool();
            _logger = new AliveChessLogger(this);
            _server = new AliveChessServerContext(this, _pluginPool, _logger);
            _pluginLoader = new AliveChessPluginLoader(_pluginPool, _logger);

            buttonInit.Enabled = false;
            buttonStart.Enabled = true;
            button2.Enabled = true;
        }

        //Синхронизация
        private void buttonSync_Click(object sender, EventArgs e)
        {
            //AliveChessCacheSyncAgent syncAgent = new AliveChessCacheSyncAgent();
            //Microsoft.Synchronization.Data.SyncStatistics syncStats = syncAgent.Synchronize();
        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
