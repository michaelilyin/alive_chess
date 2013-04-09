using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using WindowsMobileClientAliveChess.GameLayer;
using WindowsMobileClientAliveChess.NetLayer.Transport;

namespace WindowsMobileClientAliveChess.NetLayer.Main
{
    public class ClientApplication
    {
        private Socket socket;
        private IPHostEntry remoteHostEntry;
        private IPAddress remoteAddress;
        private IPEndPoint remoteEndPoint;
        private MainExecutor executor;
        private CommandPool commands;
        private SocketTransport transport;

        private Thread decodeThread;
        private Thread receiveThread;
        private Thread executeThread;

        private Game game;
        private string address;
        private bool started;

        static ClientApplication client;

        private ClientApplication(Game game, string address)
        {
            this.game = game;
            this.address = address;
        }

        private void Init()
        {
            remoteAddress = IPAddress.Parse(address);
            remoteEndPoint = new IPEndPoint(remoteAddress, 22000);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            commands = new CommandPool();

            transport = new SocketTransport(socket, commands);
            executor = new MainExecutor(commands, transport, game);

            decodeThread = new Thread(new ThreadStart(transport.Decode));
            decodeThread.Priority = ThreadPriority.Normal;
            decodeThread.IsBackground = true;

            executeThread = new Thread(new ThreadStart(executor.Execute));
            executeThread.Priority = ThreadPriority.Normal;
            executeThread.IsBackground = true;

            receiveThread = new Thread(new ThreadStart(transport.Receive));
            receiveThread.Priority = ThreadPriority.Highest;
            receiveThread.IsBackground = true;
        }

        public void StartClient()
        {
            Init();
            if (!socket.Connected)
            {
                socket.Connect(remoteEndPoint);

                decodeThread.Start();
                receiveThread.Start();
                executeThread.Start();
            }
            started = true;
        }

        public void StopClient()
        {
            decodeThread.Abort();
            receiveThread.Abort();
            executeThread.Abort();
            if (socket.Connected)
                socket.Close();
            socket = null;
            started = false;
        }

        public bool isStarted()
        {
            return started;
        }

        public static void CreateClient(Game game, string address)
        {
            if (client == null)
                client = new ClientApplication(game, address);
        }

        public bool IsConnected
        {
            get { return socket.Connected; }
        }

        public MainExecutor Executor
        {
            get { return executor; }
        }

        public SocketTransport Transport
        {
            get { return transport; }
        }

        public static ClientApplication Instance
        {
            get { return client; }
        }
    }
}
