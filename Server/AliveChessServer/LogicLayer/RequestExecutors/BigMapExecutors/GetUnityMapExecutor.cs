using System.IO;
using AliveChessLibrary.Commands.BigMapCommand;
using AliveChessServer.NetLayer;

namespace AliveChessServer.LogicLayer.RequestExecutors.BigMapExecutors
{
    public class GetUnityMapExecutor : IExecutor
    {
        private const string Location = @"..\Maps\";

        public void Execute(Message msg)
        {
            GetUnityMapRequest request = (GetUnityMapRequest)msg.Command;
            string name = Path.Combine(Location, request.Name);

            FileStream fileStream = File.Open(name, FileMode.Open);
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            memoryStream.Write(buffer, 0, buffer.Length);
            GetUnityMapResponse response = new GetUnityMapResponse(memoryStream.GetBuffer());
            msg.Sender.Messenger.SendNonSerializedMessage(response);
         
            fileStream.Close();
            memoryStream.Close();
        }
    }
}
