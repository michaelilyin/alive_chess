using System.Configuration;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer
{
    public class AliveChessStarter
    {
        private GameLogic _logic;
        private ServerApplication _server;

        public AliveChessStarter(GameLogic logic, ServerApplication server)
        {
            this._logic = logic;
            this._server = server;
        }

        public void Start()
        {
            //ConfigHandler handler = ConfigurationManager.GetSection("ChessSection/SysModules") as ConfigHandler;
            //if (handler.Network.Enabled)
            _server.Start();
            //if (handler.Logic.Enabled)
            //{
            _logic.Environment.Initialize();
            //if (handler.AI.Enabled)
            InitializeBots(_logic.Environment, _logic.PlayerManager);
            _logic.StartGame();
            //}
            //if(handler.Executing.Enabled)
            _logic.StartExecuting();
        }

        private void InitConfiguration()
        {
            ConfigurationSectionGroup configurationSectionGroup = new ConfigurationSectionGroup();
            ExeConfigurationFileMap m = new ExeConfigurationFileMap();
            m.ExeConfigFilename = "AliveChessServer.exe.config";
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(m, ConfigurationUserLevel.None);
            configuration.SectionGroups.Add("ChessSection", configurationSectionGroup);
            ConfigHandler handler = new ConfigHandler();
            ConfigElement element = new ConfigElement();
            element.Enabled = true;
            handler.Network = element;
            configurationSectionGroup.Sections.Add("SysModules", handler);
            configuration.AppSettings.SectionInformation.ForceSave = true;
            configuration.Save();
        }

        public void InitializeBots(GameWorld environment, PlayerManager playerManager)
        {
            //Level level = environment.GlobalLevelManager.GetLevelByType(LevelTypes.Easy);
            //Animat animat = playerManager.AddAnimat(level);
            //level.Animat = animat;
            //animat.Level = level;
         
            //BotKing bot1 = playerManager.AddBotKing(animat);
            //bot1.Steering.WanderOn();

            //animat.AddBot(bot1);
            //animat.TeachBots();
            //animat.Map.AddKing(bot1);

            //BotKing bot2 = playerManager.AddBotKing(animat);
            //bot2.Steering.WanderOn();
            //BotKing bot3 = playerManager.AddBotKing(animat);
            //bot3.Steering.WanderOn();
            //BotKing bot4 = playerManager.AddBotKing(animat);
            //bot4.Steering.WanderOn();

            //AliveChessDataContext c = new AliveChessDataContext(@"Data Source=ИГОРЬ-ПК\SQLEXPRESS;Initial Catalog=alive_chess;Integrated Security=True");
            //MapPoint point = new MapPoint();
            //point.DbId = Guid.NewGuid();
            //BotKing b = new BotKing(point);
            //b.Name = "fdsf";

            //c.Kings.InsertOnSubmit(b);
            //c.SubmitChanges();
        }
    }
}
