using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.Network network = null;
            Log.SetLevel(LogLevel.ALL);
            try
            {
                network = new Network.Network();
                network.Connect("player", "pw");
                Console.ReadKey();
                if (network != null)
                    network.Disconnect();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Fatal error!");
            }
        }
    }
}
