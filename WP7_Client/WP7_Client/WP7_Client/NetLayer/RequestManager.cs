using System.Windows;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessLibrary.Commands.RegisterCommand;
using AliveChessLibrary.GameObjects.Buildings;

namespace WP7_Client.NetLayer
{
    public static class RequestManager
    {
        public static void SendAuthorization(string login, string password)
        {
            var request = new AuthorizeRequest {Login = login, Password = password};
            Send(request);
        }

        public static void SendGetMap()
        {
            Send(new GetMapRequest());
        }

        public static void SendRegistration(string login, string password)
        {
            Send(new RegisterRequest {Login = login, Password = password});
        }

        public static void SendGetGameState()
        { 
            Send(new GetGameStateRequest());
        }

        public static void SendMoveKing(int x, int y)
        {
            Send(new MoveKingRequest{X = x, Y = y});
        }

        public static void SendGetObjects()
        {
            Send(new GetObjectsRequest());
        }

        public static void SendComeInCaste(Castle castle)
        {
            Send(new ComeInCastleRequest{CastleId = castle.Id});
        }

        private static void Send(ICommand command)
        {
            ((App)Application.Current).Transport.Send(command);
        }
    }
}
