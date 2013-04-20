using System.Threading;

namespace AliveChessServer.LogicLayer.AI.Services
{
    /// <summary>
    /// предоставление сервисов всем остальным слоям модуля ИИ
    /// </summary>
    public class AIExecutor
    {
        public void Execute()
        {
            while (true)
            {                
                Thread.Sleep(5);
            }
        }
    }
}
