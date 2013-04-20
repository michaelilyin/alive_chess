using AliveChessServer.LogicLayer.AI;
using AliveChessServer.LogicLayer.AI.DecisionLayer;
using AliveChessServer.LogicLayer.Environment;
using AliveChessServer.LogicLayer.UsersManagement;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer
{
    public class AliveChessStarter
    {
        public void InitializeBots(GameWorld environment, PlayerManager playerManager)
        {
            Level level = environment.LevelManager.EasyLevel;
            Animat animat = playerManager.AddAnimat(level);
            level.Animat = animat;
            animat.Level = level;
         
            BotKing bot1 = playerManager.AddBotKing(animat);
            bot1.Steering.WanderOn();

            animat.AddBot(bot1);
            animat.TeachBots();
            animat.Map.AddKing(bot1);

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
